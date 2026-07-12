using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class TranscriptionBlock : Block
{
    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.Transcription;

    [JsonPropertyName("transcription")]
    public TranscriptionBlockResponse Transcription { get; set; }
}

public class TranscriptionBlockResponse
{
    [JsonPropertyName("title")]
    public IEnumerable<RichTextBase> Title { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("children")]
    public TranscriptionChildrenResponse Children { get; set; }

    [JsonPropertyName("calendar_event")]
    public TranscriptionCalendarEventResponse CalendarEvent { get; set; }

    [JsonPropertyName("recording")]
    public TranscriptionRecordingResponse Recording { get; set; }
}

public class TranscriptionChildrenResponse
{
    [JsonPropertyName("summary_block_id")]
    public string SummaryBlockId { get; set; }

    [JsonPropertyName("notes_block_id")]
    public string NotesBlockId { get; set; }

    [JsonPropertyName("transcript_block_id")]
    public string TranscriptBlockId { get; set; }
}

public class TranscriptionCalendarEventResponse
{
    [JsonPropertyName("start_time")]
    public string StartTime { get; set; }

    [JsonPropertyName("end_time")]
    public string EndTime { get; set; }

    [JsonPropertyName("attendees")]
    public IEnumerable<string> Attendees { get; set; }
}

public class TranscriptionRecordingResponse
{
    [JsonPropertyName("start_time")]
    public string StartTime { get; set; }

    [JsonPropertyName("end_time")]
    public string EndTime { get; set; }
}
