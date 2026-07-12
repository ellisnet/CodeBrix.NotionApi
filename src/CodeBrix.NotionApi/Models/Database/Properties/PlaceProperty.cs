using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class PlaceProperty : Property
{
    public override PropertyType Type => PropertyType.Place;

    [JsonPropertyName("place")]
    public Dictionary<string, object> Place { get; set; }
}
