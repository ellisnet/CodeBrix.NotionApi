using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class FilesFilter : SinglePropertyFilter, IRollupSubPropertyFilter
{
    public FilesFilter(
        string propertyName,
        bool? isEmpty = null,
        bool? isNotEmpty = null)
    {
        Property = propertyName;
        Files = new Condition(isEmpty, isNotEmpty);
    }

    [JsonPropertyName("files")]
    public Condition Files { get; set; }

    public class Condition
    {
        public Condition(
            bool? isEmpty = null,
            bool? isNotEmpty = null)
        {
            IsEmpty = isEmpty;
            IsNotEmpty = isNotEmpty;
        }

        [JsonPropertyName("is_empty")]
        public bool? IsEmpty { get; set; }

        [JsonPropertyName("is_not_empty")]
        public bool? IsNotEmpty { get; set; }
    }
}
