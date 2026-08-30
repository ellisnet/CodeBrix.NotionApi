using System.Collections.Generic;
using System.Text.Json.Serialization;
using CodeBrix.Json.Extensions.Polymorphism;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

[JsonConverter(typeof(FallbackTypeConverterFactory))]
[JsonDiscriminator("type")]
[JsonKnownType(typeof(TableViewConfiguration), ViewType.TableValue)]
[JsonKnownType(typeof(BoardViewConfiguration), ViewType.BoardValue)]
[JsonKnownType(typeof(CalendarViewConfiguration), ViewType.CalendarValue)]
[JsonKnownType(typeof(TimelineViewConfiguration), ViewType.TimelineValue)]
[JsonKnownType(typeof(GalleryViewConfiguration), ViewType.GalleryValue)]
[JsonKnownType(typeof(ListViewConfiguration), ViewType.ListValue)]
[JsonKnownType(typeof(MapViewConfiguration), ViewType.MapValue)]
[JsonKnownType(typeof(FormViewConfiguration), ViewType.FormValue)]
[JsonKnownType(typeof(ChartViewConfiguration), ViewType.ChartValue)]
[JsonKnownType(typeof(DashboardViewConfiguration), ViewType.DashboardValue)]
[JsonFallbackType(typeof(UnknownViewConfiguration))]
public abstract class ViewConfiguration
{
    [JsonPropertyName("type")]
    public abstract string Type { get; }
}

public class TableViewConfiguration : ViewConfiguration
{
    [JsonPropertyName("type")]
    public override string Type => ViewType.TableValue;

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}

public class BoardViewConfiguration : ViewConfiguration
{
    [JsonPropertyName("type")]
    public override string Type => ViewType.BoardValue;

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}

public class CalendarViewConfiguration : ViewConfiguration
{
    [JsonPropertyName("type")]
    public override string Type => ViewType.CalendarValue;

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}

public class TimelineViewConfiguration : ViewConfiguration
{
    [JsonPropertyName("type")]
    public override string Type => ViewType.TimelineValue;

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}

public class GalleryViewConfiguration : ViewConfiguration
{
    [JsonPropertyName("type")]
    public override string Type => ViewType.GalleryValue;

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}

public class ListViewConfiguration : ViewConfiguration
{
    [JsonPropertyName("type")]
    public override string Type => ViewType.ListValue;

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}

public class MapViewConfiguration : ViewConfiguration
{
    [JsonPropertyName("type")]
    public override string Type => ViewType.MapValue;

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}

public class FormViewConfiguration : ViewConfiguration
{
    [JsonPropertyName("type")]
    public override string Type => ViewType.FormValue;

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}

public class ChartViewConfiguration : ViewConfiguration
{
    [JsonPropertyName("type")]
    public override string Type => ViewType.ChartValue;

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}

public class DashboardViewConfiguration : ViewConfiguration
{
    [JsonPropertyName("type")]
    public override string Type => ViewType.DashboardValue;

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}

/// <summary>
/// Catch-all type deserialized when a <see cref="ViewConfiguration"/> payload carries a discriminator value
/// that has no [JsonKnownType] mapping (for example a view type newly introduced by the Notion API).
/// </summary>
public class UnknownViewConfiguration : ViewConfiguration
{
    [JsonPropertyName("type")]
    public override string Type => "unknown";

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}
