namespace JsonPassthroughSpa.Application
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JsonPassthroughSpa.Extensions;
    using JsonPassthroughSpa.Interfaces;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public sealed class AddWeatherForecastCommandHandler : IRequestHandler<AddWeatherForecastCommand>
    {
        private readonly ILogger<AddWeatherForecastCommandHandler> logger;
        private readonly IDbService service;

        public AddWeatherForecastCommandHandler(ILogger<AddWeatherForecastCommandHandler> logger, IDbService service)
        {
            (this.logger, this.service) = (logger, service);
        }

        public async Task<Unit> Handle(AddWeatherForecastCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var json = request.ToJson();
                await this.service.AddWeatherForecast(json, cancellationToken);
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, exception.Message);
            }

            return await Task.FromResult(Unit.Value);
        }
    }
}