using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class MeetingNotesBlock : Block
{
    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.MeetingNotes;

    [JsonPropertyName("meeting_notes")]
    public MeetingNotesBlockData MeetingNotes { get; set; }
}

public class MeetingNotesBlockData
{
    [JsonPropertyName("title")]
    public IEnumerable<RichTextBase> Title { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("children")]
    public MeetingNotesChildrenData Children { get; set; }

    [JsonPropertyName("calendar_event")]
    public MeetingNotesCalendarEventData CalendarEvent { get; set; }

    [JsonPropertyName("recording")]
    public MeetingNotesRecordingData Recording { get; set; }
}

public class MeetingNotesChildrenData
{
    [JsonPropertyName("summary_block_id")]
    public string SummaryBlockId { get; set; }

    [JsonPropertyName("notes_block_id")]
    public string NotesBlockId { get; set; }

    [JsonPropertyName("transcript_block_id")]
    public string TranscriptBlockId { get; set; }
}

public class MeetingNotesCalendarEventData
{
    [JsonPropertyName("start_time")]
    public string StartTime { get; set; }

    [JsonPropertyName("end_time")]
    public string EndTime { get; set; }

    [JsonPropertyName("attendees")]
    public IEnumerable<string> Attendees { get; set; }
}

public class MeetingNotesRecordingData
{
    [JsonPropertyName("start_time")]
    public string StartTime { get; set; }

    [JsonPropertyName("end_time")]
    public string EndTime { get; set; }
}
