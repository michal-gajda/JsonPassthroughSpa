namespace JsonPassthroughSpa.Extensions
{
    using System.Text.Json;

    public static class JsonExtensions
    {
        public static T To<T>(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(source);
        }

        public static string ToJson<T>(this T source)
            where T : class
        {
            if (source is null)
            {
                return string.Empty;
            }

            return JsonSerializer.Serialize(source);
        }
    }
}
