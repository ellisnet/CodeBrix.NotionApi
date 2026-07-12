using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class SyncedBlockUpdateBlock : UpdateBlock
{
    [JsonPropertyName("synced_block")]
    public Info SyncedBlock { get; set; }

    public class Info
    {
        [JsonPropertyName("synced_from")]
        public SyncedFromBlockId SyncedFrom { get; set; }

        public class SyncedFromBlockId
        {
            [JsonPropertyName("block_id")]
            public string BlockId { get; set; }
        }
    }
}
