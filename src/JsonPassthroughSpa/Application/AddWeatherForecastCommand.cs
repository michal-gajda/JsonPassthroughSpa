namespace JsonPassthroughSpa.Application
{
    using System;
    using MediatR;

    public sealed class AddWeatherForecastCommand : IRequest
    {
        public AddWeatherForecastCommand(DateTime date, int temperatureC, string summary)
        {
            (this.Date, this.TemperatureC, this.Summary) = (date, temperatureC, summary);
        }

        public DateTime Date { get; }
        public int TemperatureC { get; }
        public string Summary { get; }
    }
}
