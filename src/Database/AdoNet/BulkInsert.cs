using System.Data;
using Npgsql;
using NpgsqlTypes;

namespace HowProgrammingWorksOnDotNet.Database.AdoNet;

public class BulkInsert
{
    private static void WithConnection(Action<NpgsqlConnection> action)
    {
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder
        {
            Username = "postgres",
            Password = "password",
            Host = "localhost",
            Port = 5445,
            Database = "estore",
        };

        using var connection = new NpgsqlConnection(connectionStringBuilder.ConnectionString);
        connection.Open();
        action(connection);
    }

    private static void CreateTempTable(NpgsqlConnection connection)
    {
        using var createTmpTableCommand = connection.CreateCommand();
        createTmpTableCommand.CommandText =
            @"CREATE TEMP TABLE tmp_users (
                id SERIAL PRIMARY KEY,
                key UUID 
            );";
        createTmpTableCommand.ExecuteNonQuery();
    }

    [Fact]
    public void BulkInsert_RoundTrip_Users()
    {
        // Используется 1 скомпилированная команда для выполнения 25 подобных операций. 1 операция - 1 обращение к БД
        WithConnection(
            (connection) =>
            {
                CreateTempTable(connection);

                using var insertUserCommand = connection.CreateCommand();
                insertUserCommand.CommandText = @"INSERT INTO tmp_users (key) VALUES (@key)";

                var keyParameter = new NpgsqlParameter("@key", NpgsqlDbType.Uuid);
                insertUserCommand.Parameters.Add(keyParameter);

                var transaction = connection.BeginTransaction();
                insertUserCommand.Transaction = transaction;
                insertUserCommand.Prepare();

                try
                {
                    foreach (var key in Enumerable.Range(1, 25).Select(_ => Guid.NewGuid()))
                    {
                        keyParameter.Value = key;
                        insertUserCommand.ExecuteNonQuery();
                        // transaction.Save("savepoint-name"); - Сохранение "точки-возраста", "moniker-транзакции". Просто пример использования
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    // transaction.Rollback("savepoint-name");
                    throw new Exception("Bulk insert failed", ex);
                }
            }
        );
    }

    [Fact]
    public void BulkInsert_Batch_Users()
    {
        // Используется 25 команд для выполнения 25 подобных операций. Все операции выполняются в рамках пакета - 1 обращение к БД
        WithConnection(
            (connection) =>
            {
                CreateTempTable(connection);

                using var batch = connection.CreateBatch();
                var transaction = connection.BeginTransaction();

                try
                {
                    foreach (var key in Enumerable.Range(1, 25).Select(_ => Guid.NewGuid()))
                    {
                        var batchCommand = new NpgsqlBatchCommand(
                            "INSERT INTO tmp_users (key) VALUES (@key)"
                        );
                        batchCommand.Parameters.Add(
                            new NpgsqlParameter("@key", NpgsqlDbType.Uuid) { Value = key }
                        );
                        batch.BatchCommands.Add(batchCommand);
                    }

                    batch.Transaction = transaction;
                    batch.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Bulk insert failed", ex);
                }
            }
        );
    }

    [Fact]
    public void BulkInsert_Binary_Users()
    {
        WithConnection(connection =>
        {
            CreateTempTable(connection);
            using var importer = connection.BeginBinaryImport(
                "COPY tmp_users (key) FROM STDIN (FORMAT BINARY)"
            );
            foreach (var key in Enumerable.Range(1, 25).Select(_ => Guid.NewGuid()))
                importer.WriteRow(key);

            // Выполняй!
            importer.Complete();
        });
    }
}
