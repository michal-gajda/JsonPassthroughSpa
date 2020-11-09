namespace JsonPassthroughSpa.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Threading;
    using JsonPassthroughSpa.Application;
    using JsonPassthroughSpa.Domain.Entities;
    using JsonPassthroughSpa.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using MediatR;

    [ApiController]
    [Route("[controller]")]
    public sealed class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> logger;
        private readonly IMediator mediator;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator)
        {
            (this.logger, this.mediator) = (logger, mediator);
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            try
            {
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                IEnumerable<WeatherForecast> result = await this.mediator.Send(new GetWeatherForecastQuery(), cts.Token);
                return result;
            }
            catch (OperationCanceledException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
            }

            return await Task.FromResult(new List<WeatherForecast>());
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Weather source)
        {
            try
            {
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                _ = await this.mediator.Send(new AddWeatherForecastCommand(source.Date, source.TemperatureC, source.Summary), cts.Token);
            }
            catch (OperationCanceledException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
            }

            return Ok();
        }
    }
}
