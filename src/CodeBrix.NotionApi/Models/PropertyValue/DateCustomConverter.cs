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

        // When the string has an explicit timezone (Z or +/-HH:mm), preserve it as-is so
        // the returned DateTimeOffset keeps the original offset. AssumeUniversal would
        // silently convert it to UTC, losing the offset and shifting the point-in-time
        // if Notion returns dates in the workspace's local timezone.
        // For strings without any timezone info, fall back to AssumeUniversal for UTC.
        var style = HasExplicitTimezone(dateTimeString)
            ? DateTimeStyles.None
            : DateTimeStyles.AssumeUniversal;

        return DateTimeOffset.Parse(dateTimeString, CultureInfo.InvariantCulture, style);
    }

    private static bool HasExplicitTimezone(string dateTimeString)
    {
        if (dateTimeString.EndsWith("Z", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        // A timezone designator (+/-) can only appear in the time portion, after T or a space.
        int sep = dateTimeString.IndexOf('T');

        if (sep < 0)
        {
            sep = dateTimeString.IndexOf(' ');
        }

        if (sep < 0)
        {
            // Date-only, so no timezone is possible.
            return false;
        }

        var timePart = dateTimeString.Substring(sep + 1);

        return timePart.Contains("+") || timePart.Contains("-");
    }
}
