using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using CodeBrix.JsonPolymorphism;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

[JsonConverter(typeof(FallbackTypeConverterFactory))]
[JsonDiscriminator("type")]
[JsonKnownType(typeof(NumberPropertyItem), "number")]
[JsonKnownType(typeof(UrlPropertyItem), "url")]
[JsonKnownType(typeof(SelectPropertyItem), "select")]
[JsonKnownType(typeof(MultiSelectPropertyItem), "multi_select")]
[JsonKnownType(typeof(StatusPropertyItem), "status")]
[JsonKnownType(typeof(DatePropertyItem), "date")]
[JsonKnownType(typeof(EmailPropertyItem), "email")]
[JsonKnownType(typeof(PhoneNumberPropertyItem), "phone_number")]
[JsonKnownType(typeof(CheckboxPropertyItem), "checkbox")]
[JsonKnownType(typeof(FilesPropertyItem), "files")]
[JsonKnownType(typeof(CreatedByPropertyItem), "created_by")]
[JsonKnownType(typeof(CreatedTimePropertyItem), "created_time")]
[JsonKnownType(typeof(LastEditedByPropertyItem), "last_edited_by")]
[JsonKnownType(typeof(LastEditedTimePropertyItem), "last_edited_time")]
[JsonKnownType(typeof(FormulaPropertyItem), "formula")]
[JsonKnownType(typeof(TitlePropertyItem), "title")]
[JsonKnownType(typeof(RichTextPropertyItem), "rich_text")]
[JsonKnownType(typeof(PeoplePropertyItem), "people")]
[JsonKnownType(typeof(RelationPropertyItem), "relation")]
[JsonKnownType(typeof(RollupPropertyItem), "rollup")]
[JsonFallbackType(typeof(UnknownPropertyItem))]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public abstract class SimplePropertyItem : IPropertyItemObject
{
    [JsonPropertyName("object")]
    public string Object => "property_item";

    [JsonPropertyName("type")]
    public abstract string Type { get; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("next_url")]
    public string NextURL { get; set; }
}
