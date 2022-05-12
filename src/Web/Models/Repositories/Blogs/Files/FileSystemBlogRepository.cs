using System.Collections.Concurrent;
using Markdig;
using Web.Models.Data.Blogs;

namespace Web.Models.Repositories.Blogs.Files;

public class FileSystemBlogRepository : IBlogRepository
{
    private const string DirectoryPath = "./Posts";

    private readonly ConcurrentDictionary<string, Blog> _cachedBlogs = new();

    private readonly ConcurrentBag<Blog> _cachedLatestBlogs = new();

    private readonly MarkdownPipeline _pipeline;

    public FileSystemBlogRepository()
    {
        _pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().DisableHtml().Build();
    }

    public async Task<Blog?> GetByIdAsync(string name)
    {
        if (_cachedBlogs.TryGetValue(name, out var value))
            return value;

        var path = Path.Combine(DirectoryPath, name + ".md");
        if (!File.Exists(path))
            return null;

        var blog = await GetBlogFromFileAsync(path);
        if (blog is not null)
            _cachedBlogs.TryAdd(name, blog);

        return blog;
    }

    public async Task<IEnumerable<Blog>> GetAsync(int count = 5, int offset = 0)
    {
        if (IsLatestParameter() && _cachedLatestBlogs.Any())
            return _cachedLatestBlogs.OrderByDescending(x => x.CreatedAt);

        var tasks = Directory.EnumerateFiles(DirectoryPath)
            .Where(x => Path.GetExtension(x) == ".md")
            .OrderByDescending(File.GetCreationTimeUtc)
            .Skip(offset)
            .Take(count)
            .Select(GetBlogFromFileAsync);

        var blogs = (await Task.WhenAll(tasks)).OfType<Blog>();

        if (!IsLatestParameter())
            return blogs;
        
        foreach (var blog in blogs)
            _cachedLatestBlogs.Add(blog);

        return _cachedLatestBlogs;

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

    private static async Task<(string? title, string? markdownContent, string[]? tags)> GetBlogElementsAsync(string filePath)
    {
        await using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        using var sr = new StreamReader(fs);

        var title = await sr.ReadLineAsync();
        var rawTagText = await sr.ReadLineAsync();
        _ = sr.ReadLineAsync();
        var markdownContent = await sr.ReadToEndAsync();

        return (title, markdownContent, rawTagText?.Split(',').Select(y => y.TrimStart(' ')).ToArray());
    }

    private async Task<Blog?> GetBlogFromFileAsync(string path)
    {
        var (title, markdownContent, tags) = await GetBlogElementsAsync(path);
        if (title is null || markdownContent is null || tags is null)
            return null;

        var fileName = Path.GetFileNameWithoutExtension(path);
        var text = Markdown.ToPlainText(markdownContent);
        var summary = text[..Math.Min(80, text.Length)].TrimEnd(' ') + "...";
        var renderedContent = Markdown.ToHtml(markdownContent, _pipeline);
        var createdAt = new DateTimeOffset(File.GetCreationTimeUtc(path), TimeSpan.Zero);
        var updatedAt = new DateTimeOffset(File.GetLastWriteTimeUtc(path));

        return new Blog(fileName, title, tags, summary, renderedContent, createdAt, updatedAt);
    }
}