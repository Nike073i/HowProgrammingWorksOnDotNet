using System.Data;
using Npgsql;
using NpgsqlTypes;

namespace HowProgrammingWorksOnDotNet.Database.AdoNet;

public class Orm
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

    private static void CreateTmpTable(NpgsqlConnection connection)
    {
        using var createCommand = new NpgsqlCommand(
            @"CREATE TEMP TABLE tmp_users (
            id SERIAL PRIMARY KEY,
            key UUID 
        );",
            connection
        );
        createCommand.ExecuteNonQuery();
    }

    private static void InsertUsers(NpgsqlConnection connection)
    {
        using var transaction = connection.BeginTransaction();

        try
        {
            using var batch = new NpgsqlBatch(connection) { Transaction = transaction };

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

            batch.ExecuteNonQuery();
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception("Bulk insert failed", ex);
        }
    }

    private static void PrintTableSchema(DataTable dataTable)
    {
        Console.WriteLine($"Таблица: {dataTable.TableName}");
        Console.WriteLine($"Количество колонок: {dataTable.Columns.Count}");

        for (int i = 0; i < dataTable.Columns.Count; i++)
        {
            DataColumn col = dataTable.Columns[i];
            Console.WriteLine($"Колонка #{i + 1}:");
            Console.WriteLine($"  Имя: {col.ColumnName}");
            Console.WriteLine($"  Тип: {col.DataType.Name}");
            Console.WriteLine($"  .NET тип: {col.DataType.FullName}");
            Console.WriteLine($"  Nullable: {col.AllowDBNull}");
            Console.WriteLine($"  ReadOnly: {col.ReadOnly}");
            Console.WriteLine($"  AutoIncrement: {col.AutoIncrement}");
            Console.WriteLine($"  Unique: {col.Unique}");
            Console.WriteLine($"  DefaultValue: {col.DefaultValue}");
            Console.WriteLine($"  MaxLength: {col.MaxLength}");
            Console.WriteLine($"  Expression: {col.Expression}");
            Console.WriteLine();
        }
    }

    [Fact]
    public void PrintQueryResultSchema()
    {
        WithConnection(
            (connection) =>
            {
                CreateTmpTable(connection);
                using var readCommand = connection.CreateCommand();
                readCommand.CommandText = "SELECT * FROM tmp_users";

                using var reader = readCommand.ExecuteReader();

                //Таблица, где строки - это данные по колонкам, и по каждой колонке 24 фактора (колонки...) : ColumnName, ColumnOrdinal, ColumnSize, ..., DataTypeName
                DataTable schema = reader.GetSchemaTable()!;

                PrintTableSchema(schema);
            }
        );
    }
}
