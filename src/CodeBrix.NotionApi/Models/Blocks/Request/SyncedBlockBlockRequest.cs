using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class SyncedBlockBlockRequest : BlockObjectRequest, IColumnChildrenBlockRequest, INonColumnBlockRequest
{
    [JsonPropertyName("synced_block")]
    public Data SyncedBlock { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.SyncedBlock;

    public class Data
    {
        [JsonPropertyName("synced_from")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public SyncedFromBlockId SyncedFrom { get; set; }

        [JsonPropertyName("children")]
        public IEnumerable<ISyncedBlockChildrenRequest> Children { get; set; }

        public class SyncedFromBlockId
        {
            [JsonPropertyName("type")]
            public string Type { get; set; }

            [JsonPropertyName("block_id")]
            public string BlockId { get; set; }
        }
    }
}
