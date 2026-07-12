using System;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public interface IObjectModificationData
{
    /// <summary>
    ///     Date and time when this object was created.
    /// </summary>
    [JsonPropertyName("created_time")]
    DateTime CreatedTime { get; set; }

    /// <summary>
    ///     Date and time when this object was updated.
    /// </summary>
    [JsonPropertyName("last_edited_time")]
    DateTime LastEditedTime { get; set; }

    /// <summary>
    ///     User who created the object.
    /// </summary>
    [JsonPropertyName("created_by")]
    PartialUser CreatedBy { get; set; }

    /// <summary>
    ///     User who last modified the object.
    /// </summary>
    [JsonPropertyName("last_edited_by")]
    PartialUser LastEditedBy { get; set; }
}
