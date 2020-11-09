namespace JsonPassthroughSpa.Application
{
    using System.Collections.Generic;
    using JsonPassthroughSpa.Domain.Entities;
    using MediatR;

    public sealed class GetWeatherForecastQuery : IRequest<IEnumerable<WeatherForecast>>
    { }
}
