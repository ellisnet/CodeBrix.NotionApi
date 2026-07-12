using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class DateCustomConverter : JsonConverter<Date>
{
    private const string DateFormat = "yyyy-MM-dd";
    private const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ";

    public override Date Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonObject = JsonSerializer.Deserialize<DateJsonObject>(ref reader, options);

        if (jsonObject == null)
        {
            return null;
        }

        var date = new Date
        {
            Start = ParseDateTime(jsonObject.Start, out bool startIncludeTime),
            End = ParseDateTime(jsonObject.End, out bool endIncludeTime),
            TimeZone = jsonObject.TimeZone,
            IncludeTime = startIncludeTime || endIncludeTime,
        };

        return date;
    }

    public override void Write(Utf8JsonWriter writer, Date value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();

            return;
        }

        writer.WriteStartObject();

        if (value.Start.HasValue)
        {
            string startFormat = value.IncludeTime ? DateTimeFormat : DateFormat;
            writer.WritePropertyName("start");
            writer.WriteStringValue(value.Start.Value.ToUniversalTime().ToString(startFormat, CultureInfo.InvariantCulture));
        }

        if (value.End.HasValue)
        {
            string endFormat = value.IncludeTime ? DateTimeFormat : DateFormat;
            writer.WritePropertyName("end");
            writer.WriteStringValue(value.End.Value.ToUniversalTime().ToString(endFormat, CultureInfo.InvariantCulture));
        }

        if (!string.IsNullOrEmpty(value.TimeZone))
        {
            writer.WritePropertyName("time_zone");
            writer.WriteStringValue(value.TimeZone);
        }

        writer.WriteEndObject();
    }

    private static DateTimeOffset? ParseDateTime(string dateTimeString, out bool includeTime)
    {
        includeTime = false;

        if (string.IsNullOrEmpty(dateTimeString))
        {
            return null;
        }

        includeTime = dateTimeString.Contains("T") || dateTimeString.Contains(" ");

        return DateTimeOffset.Parse(dateTimeString, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
    }
}
