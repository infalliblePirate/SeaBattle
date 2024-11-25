using Microsoft.Extensions.Configuration;

public static class ConfigurationHelper {
    private static readonly IConfiguration _configuration;

    static ConfigurationHelper() {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional:false, reloadOnChange: true);
        _configuration = builder.Build();
    }

    public static string GetConnectionString(string name) {
        var connectionString = _configuration.GetConnectionString(name);
        if (string.IsNullOrEmpty(connectionString)) {
            throw new InvalidOperationException($"THe connection string '{name}' has not been initialized");
        }
        return connectionString;
    }
}