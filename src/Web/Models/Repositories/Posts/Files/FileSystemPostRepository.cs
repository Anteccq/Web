using System.Collections.Concurrent;
using System.Globalization;
using Markdig;
using Web.Models.Data.Posts;

namespace Web.Models.Repositories.Posts.Files;

public class FileSystemPostRepository : IPostRepository
{
    private const string DirectoryPath = "./Posts";

    private readonly ConcurrentDictionary<string, Post> _cachedPosts = new();

    private readonly ConcurrentBag<Post> _cachedLatestPosts = new();

    private readonly MarkdownPipeline _pipeline;

    public FileSystemPostRepository()
    {
        _pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().DisableHtml().Build();
    }

    public async Task<Post?> GetByIdAsync(string name)
    {
        if (_cachedPosts.TryGetValue(name, out var value))
            return value;

        var path = Path.Combine(DirectoryPath, name + ".md");
        if (!File.Exists(path))
            return null;

        var blog = await GetBlogFromFileAsync(path);
        if (blog is not null)
            _cachedPosts.TryAdd(name, blog);

        return blog;
    }

    public async Task<IEnumerable<Post>> GetAsync(int count = 5, int offset = 0)
    {
        if (IsLatestParameter() && _cachedLatestPosts.Any())
            return _cachedLatestPosts;

        var tasks = Directory.EnumerateFiles(DirectoryPath)
            .Where(x => Path.GetExtension(x) == ".md")
            .OrderByDescending(File.GetCreationTimeUtc)
            .Skip(offset)
            .Take(count)
            .Select(GetBlogFromFileAsync);

        var posts = (await Task.WhenAll(tasks)).OfType<Post>();

        if (!IsLatestParameter())
            return posts;
        
        foreach (var post in posts)
            _cachedLatestPosts.Add(post);

        return _cachedLatestPosts;

        bool IsLatestParameter()
            => count == 5 && offset == 0;
    }

    public Task CreateAsync(string markdownContent, string[] tags)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(string name, string markdownContent, string[] tags)
    {
        throw new NotImplementedException();
    }

    private static async Task<(string? title, string? markdownContent, string[]? tags, string? createdTime, string? updatedTime)> GetBlogElementsAsync(string filePath)
    {
        await using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        using var sr = new StreamReader(fs);

        var title = await sr.ReadLineAsync();
        var rawTagText = await sr.ReadLineAsync();
        var createdTime = await sr.ReadLineAsync();
        var updateTime = await sr.ReadLineAsync();
        _ = sr.ReadLineAsync();
        var markdownContent = await sr.ReadToEndAsync();

        return (title, markdownContent, rawTagText?.Split(',').Select(y => y.TrimStart(' ')).ToArray(), createdTime, updateTime);
    }

    private async Task<Post?> GetBlogFromFileAsync(string path)
    {
        var (title, markdownContent, tags, createdTime, updatedTime) = await GetBlogElementsAsync(path);
        if (title is null || markdownContent is null || tags is null)
            return null;
        const string timeFormat = "yyyy/MM/dd";

        var fileName = Path.GetFileNameWithoutExtension(path);
        var text = Markdown.ToPlainText(markdownContent);
        var summary = text[..Math.Min(80, text.Length)].TrimEnd(' ') + "...";
        var renderedContent = Markdown.ToHtml(markdownContent, _pipeline);
        DateTimeOffset.TryParseExact(createdTime, timeFormat, null, DateTimeStyles.None, out var createdAt);
        var updatedAtParseResult = DateTimeOffset.TryParseExact(updatedTime, timeFormat, null, DateTimeStyles.None, out var updatedAt);

        return new Post(fileName, title, tags, summary, renderedContent, createdAt, updatedAtParseResult ? updatedAt : createdAt);
    }
}