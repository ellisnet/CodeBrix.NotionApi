using System;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

/// <summary>
///     Date property value object.
/// </summary>
public class DatePropertyValue : PropertyValue
{
    public override PropertyValueType Type => PropertyValueType.Date;

    /// <summary>
    ///     Date
    /// </summary>
    [JsonPropertyName("date")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public Date Date { get; set; }
}

/// <summary>
///     Date value object.
/// </summary>
[JsonConverter(typeof(DateCustomConverter))]
public class Date
{
    /// <summary>
    ///     Start date with optional time.
    /// </summary>
    [JsonPropertyName("start")]
    public DateTimeOffset? Start { get; set; }

    /// <summary>
    ///     End date with optional time.
    /// </summary>
    [JsonPropertyName("end")]
    public DateTimeOffset? End { get; set; }

    /// <summary>
    ///     Optional time zone information for start and end. Possible values are extracted from the IANA database and they are
    ///     based on the time zones from Moment.js.
    /// </summary>
    [JsonPropertyName("time_zone")]
    public string TimeZone { get; set; }

    /// <summary>
    ///     Whether to include time
    /// </summary>
    [JsonIgnore]
    public bool IncludeTime { get; set; } = true;
}
