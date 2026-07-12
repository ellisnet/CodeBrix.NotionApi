using System.Collections.Generic;
using System.Text.Json.Serialization;
using CodeBrix.JsonPolymorphism;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

[JsonConverter(typeof(FallbackTypeConverterFactory))]
[JsonDiscriminator("type")]
[JsonKnownType(typeof(CheckboxDataSourcePropertyConfig), DataSourcePropertyTypes.Checkbox)]
[JsonKnownType(typeof(CreatedByDataSourcePropertyConfig), DataSourcePropertyTypes.CreatedBy)]
[JsonKnownType(typeof(CreatedTimeDataSourcePropertyConfig), DataSourcePropertyTypes.CreatedTime)]
[JsonKnownType(typeof(DateDataSourcePropertyConfig), DataSourcePropertyTypes.Date)]
[JsonKnownType(typeof(EmailDataSourcePropertyConfig), DataSourcePropertyTypes.Email)]
[JsonKnownType(typeof(FilesDataSourcePropertyConfig), DataSourcePropertyTypes.Files)]
[JsonKnownType(typeof(FormulaDataSourcePropertyConfig), DataSourcePropertyTypes.Formula)]
[JsonKnownType(typeof(LastEditedByDataSourcePropertyConfig), DataSourcePropertyTypes.LastEditedBy)]
[JsonKnownType(typeof(LastEditedTimeDataSourcePropertyConfig), DataSourcePropertyTypes.LastEditedTime)]
[JsonKnownType(typeof(MultiSelectDataSourcePropertyConfig), DataSourcePropertyTypes.MultiSelect)]
[JsonKnownType(typeof(NumberDataSourcePropertyConfig), DataSourcePropertyTypes.Number)]
[JsonKnownType(typeof(PeopleDataSourcePropertyConfig), DataSourcePropertyTypes.People)]
[JsonKnownType(typeof(PhoneNumberDataSourcePropertyConfig), DataSourcePropertyTypes.PhoneNumber)]
[JsonKnownType(typeof(RelationDataSourcePropertyConfig), DataSourcePropertyTypes.Relation)]
[JsonKnownType(typeof(RichTextDataSourcePropertyConfig), DataSourcePropertyTypes.RichText)]
[JsonKnownType(typeof(RollupDataSourcePropertyConfig), DataSourcePropertyTypes.Rollup)]
[JsonKnownType(typeof(SelectDataSourcePropertyConfig), DataSourcePropertyTypes.Select)]
[JsonKnownType(typeof(StatusDataSourcePropertyConfig), DataSourcePropertyTypes.Status)]
[JsonKnownType(typeof(TitleDataSourcePropertyConfig), DataSourcePropertyTypes.Title)]
[JsonKnownType(typeof(UrlDataSourcePropertyConfig), DataSourcePropertyTypes.Url)]
[JsonKnownType(typeof(UniqueIdDataSourcePropertyConfig), DataSourcePropertyTypes.UniqueId)]
[JsonKnownType(typeof(ButtonDataSourcePropertyConfig), DataSourcePropertyTypes.Button)]
[JsonKnownType(typeof(PlaceDataSourcePropertyConfig), DataSourcePropertyTypes.Place)]
[JsonFallbackType(typeof(UnknownDataSourcePropertyConfig))]
public class DataSourcePropertyConfig
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    public virtual string Type { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}
