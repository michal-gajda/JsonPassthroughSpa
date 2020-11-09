namespace JsonPassthroughSpa.Domain.Entities
{
    using System;
    using System.Text.Json.Serialization;

    public sealed class WeatherForecast
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        [JsonPropertyName("temperatureInC")]
        public int TemperatureC { get; set; }
        [JsonPropertyName("temperatureInF")]
        public int TemperatureF => 32 + (int)(this.TemperatureC / 0.5556);
        [JsonPropertyName("summary")]
        public string Summary { get; set; }
    }
}
