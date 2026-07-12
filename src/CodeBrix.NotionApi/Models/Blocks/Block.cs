using System;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public abstract class Block : IBlock
{
    [JsonPropertyName("object")]
    public ObjectType Object => ObjectType.Block;

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    public virtual BlockType Type { get; set; }

    [JsonPropertyName("created_time")]
    public DateTime CreatedTime { get; set; }

    [JsonPropertyName("last_edited_time")]
    public DateTime LastEditedTime { get; set; }

    [JsonPropertyName("has_children")]
    public virtual bool HasChildren { get; set; }

    [JsonPropertyName("in_trash")]
    public bool InTrash { get; set; }

    [JsonPropertyName("created_by")]
    public PartialUser CreatedBy { get; set; }

    [JsonPropertyName("last_edited_by")]
    public PartialUser LastEditedBy { get; set; }

    /// <summary>
    ///     Information about the block's parent.
    /// </summary>
    [JsonPropertyName("parent")]
    public IParentOfBlock Parent { get; set; }
}
