namespace CodeBrix.NotionApi; //was previously: Notion.Client;

// ABSTRACT ON PURPOSE. System.Text.Json serializes the DECLARED type and does not inherit a base
// class's [JsonConverter], so a CONCRETE RichTextBaseInput made every IEnumerable<RichTextBaseInput>
// member (block updates, database/data-source titles, comment bodies) serialize as RichTextBase only
// -- silently dropping the "text" / "equation" / "mention" payload. Making the empty base abstract
// puts it on RuntimeTypeConverterFactory's runtime-type-writing path, which is exactly the treatment
// the Filter base class already gets (see MAINTAINER-README, "Port decisions worth knowing").
public abstract class RichTextBaseInput : RichTextBase
{
}
