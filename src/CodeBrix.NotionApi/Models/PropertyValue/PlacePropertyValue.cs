using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class PlacePropertyValue : PropertyValue
{
    public override PropertyValueType Type => PropertyValueType.Place;

    [JsonPropertyName("place")]
    public PlaceInfo Place { get; set; }

    public class PlaceInfo
    {
        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lon")]
        public double Lon { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("aws_place_id")]
        public string AwsPlaceId { get; set; }

        [JsonPropertyName("google_place_id")]
        public string GooglePlaceId { get; set; }
    }
}
