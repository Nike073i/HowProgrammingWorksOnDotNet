using System.Data;
using System.Data.Common;
using Npgsql;

namespace HowProgrammingWorksOnDotNet.Database.AdoNet;

public class Query
{
    private void WithConnection(Action<IDbConnection, DbProviderFactory> action)
    {
        DbProviderFactory dbProviderFactory = NpgsqlFactory.Instance;
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder
        {
            Username = "postgres",
            Password = "password",
            Host = "localhost",
            Port = 5445,
            Database = "estore",
        };
        using var connection = dbProviderFactory.CreateConnection()!;

        // connection.ConnectionString = "User ID=postgres;Password=password;Host=localhost;Port=5445;Database=estore";
        connection.ConnectionString = connectionStringBuilder.ConnectionString;

        connection.Open();
        action(connection, dbProviderFactory);
    }

    [Fact]
    public void ReadByCursor()
    {
        WithConnection(
            (connection, _) =>
            {
                using var queryCommand = connection.CreateCommand();
                queryCommand.CommandText =
                    "SELECT * FROM (VALUES (1, 2, 3), (4, 5, 6)) AS t(field_1, field_2, field_3)";

                using var reader = queryCommand.ExecuteReader();
                for (int i = 1; reader.Read(); i++)
                {
                    Console.WriteLine($"Row - {i}:");
                    Console.WriteLine($"\t Column_1 - {reader["field_1"]}");
                    Console.WriteLine($"\t Column_2 - {reader[1]}"); // Или доступ по индексу
                    Console.WriteLine($"\t Column_3 - {reader[2]}");
                }
            }
        );
    }

    [Fact]
    public void ReadScalar()
    {
        WithConnection(
            (connection, _) =>
            {
                using var queryCommand = connection.CreateCommand();
                queryCommand.CommandText = "SELECT 159;";

                var scalar = queryCommand.ExecuteScalar();
                Console.WriteLine($"Scalar result - {scalar}");
            }
        );
    }

    [Fact]
    public void ActionWithoutResult()
    {
        WithConnection(
            (connection, provider) =>
            {
                using var mutationCommand = connection.CreateCommand();
                mutationCommand.CommandText = "CREATE TABLE IF NOT EXISTS Test(id SERIAL);";
                int rowsAffected = mutationCommand.ExecuteNonQuery();
                Console.WriteLine($"Mutation result - {rowsAffected}");
            }
        );
    }

    [Fact]
    public void MultipleQueryInOneCommand()
    {
        WithConnection(
            (connection, _) =>
            {
                using var command = connection.CreateCommand();
                command.CommandText =
                    @"
                SELECT 1 as number;
                SELECT * FROM (VALUES (2, 'hello'), (3, 'world')) AS t(id, message);
                SELECT 'result' as final;";

                using var reader = command.ExecuteReader();
                int resultSetIndex = 0;

                do
                {
                    resultSetIndex++;
                    Console.WriteLine($"Result Set {resultSetIndex}:");

                    int rowNumber = 0;
                    while (reader.Read())
                    {
                        rowNumber++;
                        Console.Write($"  Row {rowNumber}: ");

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var columnName = reader.GetName(i);
                            var value = reader.GetValue(i);
                            Console.Write($"{columnName} = {value}");
                            if (i < reader.FieldCount - 1)
                                Console.Write(" | ");
                        }
                        Console.WriteLine();
                    }

                    Console.WriteLine();
                } while (reader.NextResult());
            }
        );
    }
}
