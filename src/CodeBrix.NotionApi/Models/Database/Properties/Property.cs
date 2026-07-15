using System.Text.Json.Serialization;
using CodeBrix.Json.Extensions.Polymorphism;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

[JsonConverter(typeof(FallbackTypeConverterFactory))]
[JsonDiscriminator("type")]
[JsonKnownType(typeof(CheckboxProperty), PropertyType.CheckboxValue)]
[JsonKnownType(typeof(CreatedByProperty), PropertyType.CreatedByValue)]
[JsonKnownType(typeof(CreatedTimeProperty), PropertyType.CreatedTimeValue)]
[JsonKnownType(typeof(DateProperty), PropertyType.DateValue)]
[JsonKnownType(typeof(EmailProperty), PropertyType.EmailValue)]
[JsonKnownType(typeof(FilesProperty), PropertyType.FilesValue)]
[JsonKnownType(typeof(FormulaProperty), PropertyType.FormulaValue)]
[JsonKnownType(typeof(LastEditedByProperty), PropertyType.LastEditedByValue)]
[JsonKnownType(typeof(LastEditedTimeProperty), PropertyType.LastEditedTimeValue)]
[JsonKnownType(typeof(MultiSelectProperty), PropertyType.MultiSelectValue)]
[JsonKnownType(typeof(NumberProperty), PropertyType.NumberValue)]
[JsonKnownType(typeof(PeopleProperty), PropertyType.PeopleValue)]
[JsonKnownType(typeof(PhoneNumberProperty), PropertyType.PhoneNumberValue)]
[JsonKnownType(typeof(RelationProperty), PropertyType.RelationValue)]
[JsonKnownType(typeof(RichTextProperty), PropertyType.RichTextValue)]
[JsonKnownType(typeof(RollupProperty), PropertyType.RollupValue)]
[JsonKnownType(typeof(SelectProperty), PropertyType.SelectValue)]
[JsonKnownType(typeof(StatusProperty), PropertyType.StatusValue)]
[JsonKnownType(typeof(TitleProperty), PropertyType.TitleValue)]
[JsonKnownType(typeof(UrlProperty), PropertyType.UrlValue)]
[JsonKnownType(typeof(UniqueIdProperty), PropertyType.UniqueIdValue)]
[JsonKnownType(typeof(ButtonProperty), PropertyType.ButtonValue)]
[JsonKnownType(typeof(VerificationProperty), PropertyType.VerificationValue)]
[JsonKnownType(typeof(PlaceProperty), PropertyType.PlaceValue)]
[JsonFallbackType(typeof(UnknownProperty))]
public class Property
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    public virtual PropertyType Type { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}
