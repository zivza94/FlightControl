using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FlightControlWeb
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader,
            Type typeToConvert, JsonSerializerOptions options)
        {
            return Utiles.StringToDateTime(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer,
            DateTime value, JsonSerializerOptions options)
        {
            string time = Utiles.DateTimeToString(value);
            writer.WriteStringValue(time);
        }
    }
}
