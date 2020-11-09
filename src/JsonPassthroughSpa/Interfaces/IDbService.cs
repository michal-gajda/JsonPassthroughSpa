namespace JsonPassthroughSpa.Interfaces
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using JsonPassthroughSpa.Domain.Entities;

    public interface IDbService
    {
        Task<IEnumerable<WeatherForecast>> GetWeatherForecasts(CancellationToken cancellationToken);
        Task AddWeatherForecast(string json, CancellationToken cancellationToken);
    }
}
