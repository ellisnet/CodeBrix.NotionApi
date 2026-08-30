using System;
using System.Collections.Generic;
using System.Text.Json;
using CodeBrix.NotionApi;
using Xunit;

namespace CodeBrix.NotionApi.Tests; //was previously: Notion.UnitTests;

public class FilterTests
{
    private readonly SerializerSettingsSource _settingsSource = new(new ClientOptions());

    private string SerializeFilter(Filter filter)
    {
        return JsonSerializer.Serialize<object>(filter, _settingsSource.GetSerializerOptions());
    }

    [Fact]
    public void CompoundFilterTest()
    {
        var selectFilter = new SelectFilter("A select", "Option");
        var relationFilter = new RelationFilter("Link", "subtask#1");
        var dateFilter = new DateFilter("Due", pastMonth: new Dictionary<string, object>());

        var filterGroup = new List<Filter>
        {
            relationFilter,
            selectFilter
        };

        var complexFiler = new CompoundFilter(
            and: new List<Filter>
            {
                dateFilter,
                new CompoundFilter(filterGroup)
            }
        );

        Assert.Equal(
            "{\"and\":[{\"date\":{\"past_month\":{}},\"property\":\"Due\"},"
            + "{\"or\":[{\"relation\":{\"contains\":\"subtask#1\"},\"property\":\"Link\"}," +
            "{\"select\":{\"equals\":\"Option\"},\"property\":\"A select\"}]}]}",
            SerializeFilter(complexFiler)
        );
    }

    [Fact]
    public void CheckboxFilterTest()
    {
        var filter = new CheckboxFilter("Property name", false);

        Assert.Equal(
            "{\"checkbox\":{\"equals\":false},\"property\":\"Property name\"}",
            SerializeFilter(filter)
        );
    }

    [Fact]
    public void DateFilterTest_WithDateTime_UnspecifiedKind()
    {
        var filter = new DateFilter("When", onOrAfter: new DateTime(2042, 11, 29));

        Assert.Equal(
            "{\"date\":{\"on_or_after\":\"2042-11-29T00:00:00\"},\"property\":\"When\"}",
            SerializeFilter(filter)
        );
    }

    [Fact]
    public void DateFilterTest_WithDateTime_UtcKind()
    {
        var filter = new DateFilter("When", after: new DateTime(2042, 11, 29, 10, 30, 0, DateTimeKind.Utc));

        Assert.Equal(
            "{\"date\":{\"after\":\"2042-11-29T10:30:00Z\"},\"property\":\"When\"}",
            SerializeFilter(filter)
        );
    }

    [Fact]
    public void DateFilterTest_WithDateTimeOffset()
    {
        var offset = new DateTimeOffset(2042, 11, 29, 10, 30, 0, TimeSpan.FromHours(5));
        var filter = new DateFilter("When", before: offset);

        // RelativeDateValue carries the timestamp as a STRING, and System.Text.Json's default
        // JavaScriptEncoder escapes "+" inside string values as \u002B. That is still valid JSON
        // and decodes back to "+05:00" on the Notion side; upstream (Newtonsoft) emits a literal
        // "+" here. Assert what this library actually puts on the wire.
        Assert.Equal(
            "{\"date\":{\"before\":\"2042-11-29T10:30:00\\u002B05:00\"},\"property\":\"When\"}",
            SerializeFilter(filter)
        );
    }

    [Fact]
    public void DateFilterTest_WithDateTimeOffset_RoundTripsThroughJson()
    {
        var offset = new DateTimeOffset(2042, 11, 29, 10, 30, 0, TimeSpan.FromHours(5));
        var filter = new DateFilter("When", before: offset);

        // The \u002B escape above is transport-level only: parsing the payload back yields the
        // literal offset a Notion server would see.
        using var document = JsonDocument.Parse(SerializeFilter(filter));

        Assert.Equal(
            "2042-11-29T10:30:00+05:00",
            document.RootElement.GetProperty("date").GetProperty("before").GetString()
        );
    }

    [Fact]
    public void DateFilterTest_WithRelativeDateKeyword_Today()
    {
        var filter = new DateFilter("Due", onOrAfter: RelativeDateValue.Today);

        Assert.Equal(
            "{\"date\":{\"on_or_after\":\"today\"},\"property\":\"Due\"}",
            SerializeFilter(filter)
        );
    }

    [Fact]
    public void DateFilterTest_WithRelativeDateKeyword_Tomorrow()
    {
        var filter = new DateFilter("Due", before: RelativeDateValue.Tomorrow);

        Assert.Equal(
            "{\"date\":{\"before\":\"tomorrow\"},\"property\":\"Due\"}",
            SerializeFilter(filter)
        );
    }

    [Fact]
    public void DateFilterTest_WithRelativeDateKeyword_OneWeekFromNow()
    {
        var filter = new DateFilter("Due", onOrBefore: RelativeDateValue.OneWeekFromNow);

        Assert.Equal(
            "{\"date\":{\"on_or_before\":\"one_week_from_now\"},\"property\":\"Due\"}",
            SerializeFilter(filter)
        );
    }

    [Fact]
    public void DateFilterTest_WithRelativeDateKeyword_OneMonthAgo()
    {
        var filter = new DateFilter("Modified", equal: RelativeDateValue.OneMonthAgo);

        Assert.Equal(
            "{\"date\":{\"equals\":\"one_month_ago\"},\"property\":\"Modified\"}",
            SerializeFilter(filter)
        );
    }

    [Fact]
    public void DateFilterTest_WithRelativeDateKeyword_CustomString()
    {
        RelativeDateValue custom = "yesterday";
        var filter = new DateFilter("Due", after: custom);

        Assert.Equal(
            "{\"date\":{\"after\":\"yesterday\"},\"property\":\"Due\"}",
            SerializeFilter(filter)
        );
    }

    [Fact]
    public void FilesFilterTest()
    {
        var filter = new FilesFilter("Attachments", isNotEmpty: false);

        Assert.Equal(
            "{\"files\":{\"is_not_empty\":false},\"property\":\"Attachments\"}",
            SerializeFilter(filter)
        );
    }

    [Fact]
    public void FormulaFilterTest()
    {
        var filter = new FormulaFilter(
            "Some",
            number: new NumberFilter.Condition(isEmpty: true)
        );

        Assert.Equal(
            "{\"formula\":{\"number\":{\"is_empty\":true}},\"property\":\"Some\"}",
            SerializeFilter(filter)
        );
    }

    [Fact]
    public void MultiSelectFilterTest()
    {
        var filter = new MultiSelectFilter("category 1", doesNotContain: "tag");

        Assert.Equal(
            "{\"multi_select\":{\"does_not_contain\":\"tag\"},\"property\":\"category 1\"}",
            SerializeFilter(filter)
        );
    }

    [Fact]
    public void NumberFilterTest()
    {
        // This test was inherited from upstream disabled, with the note "Not sure if integer should
        // be serialized as a number with decimals". Settled 2026-08-30: System.Text.Json writes a
        // double with the shortest round-trippable form, so -54.0 goes out as "-54", where upstream's
        // Newtonsoft wrote "-54.0". Both were sent to the real Notion API against a number property
        // and both returned HTTP 200 with an identical result list -- JSON has a single number type,
        // so the two spellings are the same value. The System.Text.Json form is asserted here.
        var filter = new NumberFilter("sum", greaterThanOrEqualTo: -54);

        Assert.Equal(
            "{\"number\":{\"greater_than_or_equal_to\":-54},\"property\":\"sum\"}",
            SerializeFilter(filter)
        );
    }

    [Fact]
    public void NumberFilterTest_WithFractionalValue()
    {
        // A value that genuinely needs decimals keeps them.
        var filter = new NumberFilter("sum", greaterThanOrEqualTo: -54.5);

        Assert.Equal(
            "{\"number\":{\"greater_than_or_equal_to\":-54.5},\"property\":\"sum\"}",
            SerializeFilter(filter)
        );
    }

    [Fact]
    public void PeopleFilter()
    {
        var filter = new PeopleFilter("assignee PM", doesNotContain: "some-uuid");

        Assert.Equal(
            "{\"people\":{\"does_not_contain\":\"some-uuid\"},\"property\":\"assignee PM\"}",
            SerializeFilter(filter)
        );
    }

    [Fact]
    public void RichTextFilterTest()
    {
        var filter = new RichTextFilter("Some property", doesNotEqual: "Example text");

        Assert.Equal(
            "{\"rich_text\":{\"does_not_equal\":\"Example text\"},\"property\":\"Some property\"}",
            SerializeFilter(filter)
        );
    }

    private class SerializerSettingsSource : RestClient
    {
        public SerializerSettingsSource(ClientOptions options) : base(options)
        {
        }

        public JsonSerializerOptions GetSerializerOptions()
        {
            return DefaultSerializerOptions;
        }
    }
}
