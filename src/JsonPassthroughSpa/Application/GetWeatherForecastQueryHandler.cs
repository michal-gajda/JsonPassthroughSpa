namespace JsonPassthroughSpa.Application
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using JsonPassthroughSpa.Domain.Entities;
    using JsonPassthroughSpa.Interfaces;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public sealed class GetWeatherForecastQueryHandler : IRequestHandler<GetWeatherForecastQuery, IEnumerable<WeatherForecast>>
    {
        private readonly ILogger<GetWeatherForecastQueryHandler> logger;
        private readonly IDbService service;

        public GetWeatherForecastQueryHandler(ILogger<GetWeatherForecastQueryHandler> logger, IDbService service)
        {
            (this.logger, this.service) = (logger, service);
        }

        public async Task<IEnumerable<WeatherForecast>> Handle(GetWeatherForecastQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<WeatherForecast> result = await this.service.GetWeatherForecasts(cancellationToken);
            return result;
        }
    }
}
