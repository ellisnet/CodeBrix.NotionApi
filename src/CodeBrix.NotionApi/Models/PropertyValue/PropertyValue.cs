using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using CodeBrix.Json.Extensions.Polymorphism;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

/// <summary>
///     An object describing the identifier, type, and value of a page property.
/// </summary>
[JsonConverter(typeof(FallbackTypeConverterFactory))]
[JsonDiscriminator("type")]
[JsonKnownType(typeof(CheckboxPropertyValue), PropertyValueType.CheckboxValue)]
[JsonKnownType(typeof(CreatedByPropertyValue), PropertyValueType.CreatedByValue)]
[JsonKnownType(typeof(CreatedTimePropertyValue), PropertyValueType.CreatedTimeValue)]
[JsonKnownType(typeof(DatePropertyValue), PropertyValueType.DateValue)]
[JsonKnownType(typeof(EmailPropertyValue), PropertyValueType.EmailValue)]
[JsonKnownType(typeof(FilesPropertyValue), PropertyValueType.FilesValue)]
[JsonKnownType(typeof(FormulaPropertyValue), PropertyValueType.FormulaValue)]
[JsonKnownType(typeof(LastEditedByPropertyValue), PropertyValueType.LastEditedByValue)]
[JsonKnownType(typeof(LastEditedTimePropertyValue), PropertyValueType.LastEditedTimeValue)]
[JsonKnownType(typeof(MultiSelectPropertyValue), PropertyValueType.MultiSelectValue)]
[JsonKnownType(typeof(NumberPropertyValue), PropertyValueType.NumberValue)]
[JsonKnownType(typeof(PeoplePropertyValue), PropertyValueType.PeopleValue)]
[JsonKnownType(typeof(PhoneNumberPropertyValue), PropertyValueType.PhoneNumberValue)]
[JsonKnownType(typeof(RelationPropertyValue), PropertyValueType.RelationValue)]
[JsonKnownType(typeof(RichTextPropertyValue), PropertyValueType.RichTextValue)]
[JsonKnownType(typeof(RollupPropertyValue), PropertyValueType.RollupValue)]
[JsonKnownType(typeof(SelectPropertyValue), PropertyValueType.SelectValue)]
[JsonKnownType(typeof(StatusPropertyValue), PropertyValueType.StatusValue)]
[JsonKnownType(typeof(TitlePropertyValue), PropertyValueType.TitleValue)]
[JsonKnownType(typeof(UrlPropertyValue), PropertyValueType.UrlValue)]
[JsonKnownType(typeof(UniqueIdPropertyValue), PropertyValueType.UniqueIdValue)]
[JsonKnownType(typeof(ButtonPropertyValue), PropertyValueType.ButtonValue)]
[JsonKnownType(typeof(VerificationPropertyValue), PropertyValueType.VerificationValue)]
[JsonKnownType(typeof(PlacePropertyValue), PropertyValueType.PlaceValue)]
[JsonFallbackType(typeof(UnknownPropertyValue))]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
public class PropertyValue
{
    /// <summary>
    ///     Underlying identifier of the property.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    public virtual PropertyValueType Type { get; }
}
