namespace JsonPassthroughSpa.Models
{
    using System;

    public sealed class Weather
    {
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public string Summary { get; set; }
    }
}