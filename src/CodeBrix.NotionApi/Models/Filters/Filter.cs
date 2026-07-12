using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

// Made abstract during the CodeBrix.NotionApi port: System.Text.Json serializes the DECLARED
// type, so a concrete empty base would serialize as "{}"; abstract bases are handled by
// RuntimeTypeConverterFactory, which serializes the runtime type like Newtonsoft.Json did.
public abstract class Filter
{
}

public class SinglePropertyFilter : Filter
{
    [JsonPropertyName("property")]
    public string Property { get; set; }
}

public class CompoundFilter : Filter
{
    public CompoundFilter(List<Filter> or = null, List<Filter> and = null)
    {
        Or = or;
        And = and;
    }

    [JsonPropertyName("or")]
    public List<Filter> Or { get; set; }

    [JsonPropertyName("and")]
    public List<Filter> And { get; set; }
}
