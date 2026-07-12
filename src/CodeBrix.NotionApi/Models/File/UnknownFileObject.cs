namespace CodeBrix.NotionApi;

/// <summary>
/// Catch-all type deserialized when a <see cref="FileObject"/> payload carries a discriminator value
/// that has no [JsonKnownType] mapping (for example a value newly introduced by the Notion API).
/// </summary>
public class UnknownFileObject : FileObject
{
}
