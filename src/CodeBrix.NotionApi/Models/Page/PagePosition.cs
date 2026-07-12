using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

/// <summary>
/// Controls where a newly created page is positioned within its parent.
/// </summary>
public abstract class PagePosition
{
    [JsonPropertyName("type")]
    public abstract string Type { get; }
}

/// <summary>
/// Places the new page immediately after the specified block.
/// </summary>
public class AfterBlockPagePosition : PagePosition
{
    public override string Type => "after_block";

    [JsonPropertyName("after_block")]
    public AfterBlockReference AfterBlock { get; set; }
}

/// <summary>
/// The block after which the new page should be placed.
/// </summary>
public class AfterBlockReference
{
    /// <summary>
    /// The ID of the block.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }
}

/// <summary>
/// Places the new page at the start of the parent.
/// </summary>
public class PageStartPosition : PagePosition
{
    public override string Type => "page_start";
}

/// <summary>
/// Places the new page at the end of the parent.
/// </summary>
public class PageEndPosition : PagePosition
{
    public override string Type => "page_end";
}
