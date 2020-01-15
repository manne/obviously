using System.Text.Json;

namespace Obviously.System.Text.Json.Tests
{
    public static class DefaultJsonSerializerOptions
    {
        static DefaultJsonSerializerOptions()
        {
            DefaultSettings = new JsonSerializerOptions();
            DefaultSettings.Converters.Add(new ImmutableConverter());
        }

        public static JsonSerializerOptions DefaultSettings { get; }
    }
}
