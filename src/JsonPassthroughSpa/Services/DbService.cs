namespace JsonPassthroughSpa.Services
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;
    using JsonPassthroughSpa.Domain.Entities;
    using JsonPassthroughSpa.Extensions;
    using JsonPassthroughSpa.Interfaces;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public sealed class DbService : IDbService
    {
        private readonly ILogger<DbService> logger;
        private readonly ConnectionStringsOptions options;

        public DbService(ILogger<DbService> logger, IOptions<ConnectionStringsOptions> options)
        {
            (this.logger, this.options) = (logger, options.Value);
        }

        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecasts(CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(this.options.DefaultConnection);
            await connection.OpenAsync(cancellationToken);
            using var command = connection.CreateCommand();

            command.CommandText = "[dbo].[GetWeatherForecasts]";
            command.CommandType = CommandType.StoredProcedure;

            var json = await command.ExecuteScalarAsync(cancellationToken) as string;
            var result = json.To<List<WeatherForecast>>();

            return await Task.FromResult(result);
        }

        public async Task AddWeatherForecast(string json, CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(this.options.DefaultConnection);
            await connection.OpenAsync(cancellationToken);
            using var command = connection.CreateCommand();

            command.CommandText = "[dbo].[AddWeatherForecast]";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@json", json);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}
