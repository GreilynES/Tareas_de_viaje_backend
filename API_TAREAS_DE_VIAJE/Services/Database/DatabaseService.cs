using Microsoft.Data.SqlClient;

namespace API_TAREAS_DE_VIAJE.Services.Database
{
    public class DatabaseService
    {
        private readonly string _connectionString;
        private readonly ILogger<DatabaseService> _logger;

        public DatabaseService(IConfiguration configuration, ILogger<DatabaseService> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string no configurada");
            _logger = logger;
        }

        public async Task<SqlConnection> GetOpenConnectionAsync()
        {
            try
            {
                var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                return connection;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al abrir conexión a la base de datos");
                throw;
            }
        }
    }
}