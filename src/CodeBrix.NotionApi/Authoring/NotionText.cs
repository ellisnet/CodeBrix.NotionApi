using System;
using System.Collections.Generic;

namespace CodeBrix.NotionApi;

/// <summary>
/// Convenience factory for Notion rich-text runs. Builds <see cref="RichTextText"/> values with optional
/// bold/italic/code annotations and links, and splits long strings so no single run exceeds Notion's
/// 2,000-character-per-rich-text-element limit (a limit the raw API enforces but does not help you meet).
/// </summary>
public static class NotionText
{
    /// <summary>The maximum length Notion allows for a single rich-text element's text content.</summary>
    public const int MaxRunLength = 2000;

    /// <summary>
    /// Build a single rich-text run. Does not split — pass content of at most <see cref="MaxRunLength"/>
    /// characters, or use <see cref="Split(string, bool, bool, bool, string)"/> for arbitrary length.
    /// </summary>
    /// <param name="content">The text content.</param>
    /// <param name="bold">Apply the bold annotation.</param>
    /// <param name="italic">Apply the italic annotation.</param>
    /// <param name="code">Apply the inline-code annotation.</param>
    /// <param name="linkUrl">Optional URL to turn the run into a link.</param>
    public static RichTextText Run(string content, bool bold = false, bool italic = false, bool code = false, string linkUrl = null)
    {
        var text = new Text { Content = content ?? string.Empty };
        if (!string.IsNullOrEmpty(linkUrl))
        {
            text.Link = new Link { Url = linkUrl };
        }

        var run = new RichTextText { Text = text };
        if (bold || italic || code)
        {
            run.Annotations = new Annotations { IsBold = bold, IsItalic = italic, IsCode = code };
        }

        return run;
    }

    /// <summary>
    /// Build one or more rich-text runs from <paramref name="content"/>, splitting on word boundaries so
    /// that no run exceeds <see cref="MaxRunLength"/>. All runs carry the same annotations/link.
    /// </summary>
    public static List<RichTextText> Split(string content, bool bold = false, bool italic = false, bool code = false, string linkUrl = null)
    {
        var runs = new List<RichTextText>();
        if (string.IsNullOrEmpty(content))
        {
            return runs;
        }

        if (content.Length <= MaxRunLength)
        {
            runs.Add(Run(content, bold, italic, code, linkUrl));
            return runs;
        }

        var start = 0;
        while (start < content.Length)
        {
            var length = Math.Min(MaxRunLength, content.Length - start);
            if (start + length < content.Length)
            {
                // Back up to the last space so a word is not split across two runs.
                var lastSpace = content.LastIndexOf(' ', start + length - 1, length);
                if (lastSpace > start)
                {
                    length = lastSpace - start + 1;
                }
            }

            runs.Add(Run(content.Substring(start, length), bold, italic, code, linkUrl));
            start += length;
        }

        return runs;
    }

    /// <summary>
    /// Same as <see cref="Split(string, bool, bool, bool, string)"/> but typed as <see cref="RichTextBase"/>,
    /// which is what block rich-text properties expect.
    /// </summary>
    public static List<RichTextBase> SplitBase(string content, bool bold = false, bool italic = false, bool code = false, string linkUrl = null)
    {
        var typed = Split(content, bold, italic, code, linkUrl);
        var list = new List<RichTextBase>(typed.Count);
        foreach (var run in typed)
        {
            list.Add(run);
        }

        return list;
    }

    /// <summary>Plain, unannotated text as one or more runs (split to respect <see cref="MaxRunLength"/>).</summary>
    public static List<RichTextText> Plain(string text) => Split(text);

    /// <summary>Plain, unannotated text as <see cref="RichTextBase"/> runs, ready for a block's rich-text property.</summary>
    public static List<RichTextBase> PlainBase(string text) => SplitBase(text);
}
