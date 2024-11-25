using Npgsql;

string connectionString = ConfigurationHelper.GetConnectionString("DefaultConnection");

// connect to the postgresql server
await using var conn = new NpgsqlConnection(connectionString);
await conn.OpenAsync();

Console.WriteLine($"The postgresql version: {conn.PostgreSqlVersion}");
