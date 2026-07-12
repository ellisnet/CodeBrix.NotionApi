using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CodeBrix.NotionApi;

/// <summary>
/// High-level authoring conveniences over <see cref="INotionClient"/> for the common "build a page from
/// scratch" workflow: create a child page, append arbitrarily many blocks (respecting Notion's
/// 100-children-per-request cap), upload a local file in one call, archive a page, and read every child
/// block back through pagination.
/// </summary>
public static class NotionAuthoringExtensions
{
    /// <summary>The maximum number of child blocks Notion accepts in a single append request.</summary>
    public const int MaxChildrenPerAppend = 100;

    /// <summary>
    /// Create a new page whose parent is another page, with a plain-text title. Body content is added
    /// afterwards via <see cref="AppendChildrenBatchedAsync"/>.
    /// </summary>
    public static Task<Page> CreateChildPageAsync(
        this INotionClient client,
        string parentPageId,
        string title,
        CancellationToken cancellationToken = default)
    {
        var parameters = new PagesCreateParameters
        {
            Parent = new PageParentRequest { PageId = parentPageId },
            Properties = new Dictionary<string, PropertyValue>
            {
                ["title"] = new TitlePropertyValue { Title = NotionText.PlainBase(title) }
            }
        };

        return client.Pages.CreateAsync(parameters, cancellationToken);
    }

    /// <summary>
    /// Append any number of blocks to a page or block, automatically splitting into requests of at most
    /// <see cref="MaxChildrenPerAppend"/> children. Batches are sent sequentially so block order is
    /// preserved, with an optional pause between batches to stay under Notion's rate limit.
    /// </summary>
    /// <param name="client">The Notion client.</param>
    /// <param name="parentBlockId">The parent page or block id to append under.</param>
    /// <param name="blocks">The blocks to append, in order.</param>
    /// <param name="throttleMs">Milliseconds to pause between batches (0 to disable). Defaults to ~3 req/s pacing.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    public static async Task AppendChildrenBatchedAsync(
        this INotionClient client,
        string parentBlockId,
        IReadOnlyList<IBlockObjectRequest> blocks,
        int throttleMs = 350,
        CancellationToken cancellationToken = default)
    {
        for (var i = 0; i < blocks.Count; i += MaxChildrenPerAppend)
        {
            var batch = blocks.Skip(i).Take(MaxChildrenPerAppend).ToList();
            await client.Blocks.AppendChildrenAsync(
                new BlockAppendChildrenRequest { BlockId = parentBlockId, Children = batch },
                cancellationToken);

            if (i + MaxChildrenPerAppend < blocks.Count && throttleMs > 0)
            {
                await Task.Delay(throttleMs, cancellationToken);
            }
        }
    }

    /// <summary>
    /// Upload a local file to Notion using the single-part File Upload flow (Create then Send) and return
    /// the resulting <c>file_upload</c> id, ready to pass to <see cref="NotionBlocks.ImageUpload"/> (or any
    /// other file-bearing block). The content type is inferred from the file extension when not supplied.
    /// </summary>
    public static async Task<string> UploadFileAsync(
        this INotionClient client,
        string filePath,
        string contentType = null,
        CancellationToken cancellationToken = default)
    {
        var fileName = Path.GetFileName(filePath);
        if (string.IsNullOrEmpty(contentType))
        {
            contentType = GuessContentType(filePath);
        }

        var created = await client.FileUploads.CreateAsync(
            new CreateFileUploadRequest
            {
                Mode = FileUploadMode.SinglePart,
                FileName = fileName,
                ContentType = contentType
            },
            cancellationToken);

        using (var stream = File.OpenRead(filePath))
        {
            var sent = await client.FileUploads.SendAsync(
                SendFileUploadRequest.Create(created.Id, new FileData { FileName = fileName, Data = stream, ContentType = contentType }),
                cancellationToken);

            return sent.Id;
        }
    }

    /// <summary>Move a page to the trash (the API equivalent of archiving), via <c>in_trash = true</c>.</summary>
    public static Task<Page> ArchivePageAsync(this INotionClient client, string pageId, CancellationToken cancellationToken = default) =>
        client.Pages.UpdateAsync(pageId, new PagesUpdateParameters { InTrash = true }, cancellationToken);

    /// <summary>Retrieve every child block of a page or block, following pagination to the end.</summary>
    public static async Task<List<IBlock>> RetrieveAllChildrenAsync(
        this INotionClient client,
        string blockId,
        CancellationToken cancellationToken = default)
    {
        var all = new List<IBlock>();
        string cursor = null;
        do
        {
            var page = await client.Blocks.RetrieveChildrenAsync(
                new BlockRetrieveChildrenRequest { BlockId = blockId, StartCursor = cursor, PageSize = 100 },
                cancellationToken);
            all.AddRange(page.Results);
            cursor = page.HasMore ? page.NextCursor : null;
        }
        while (cursor != null);

        return all;
    }

    /// <summary>Best-effort MIME type from a file extension, for the File Upload API.</summary>
    public static string GuessContentType(string path)
    {
        var ext = Path.GetExtension(path).ToLowerInvariant();
        switch (ext)
        {
            case ".jpg":
            case ".jpeg":
                return "image/jpeg";
            case ".png":
                return "image/png";
            case ".gif":
                return "image/gif";
            case ".webp":
                return "image/webp";
            case ".svg":
                return "image/svg+xml";
            case ".pdf":
                return "application/pdf";
            default:
                return "application/octet-stream";
        }
    }
}
