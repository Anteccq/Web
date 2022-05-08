using System.Collections.Concurrent;
using MessagePack;
using Web.Models.Data.Blogs;

namespace Web.Models.Services.Blogs.Files;

public class FileSystemBlogRepository : IBlogRepository
{
    private const string DirectoryPath = "./BinarizedPosts";

    private readonly ConcurrentDictionary<string, Blog> _cachedBlogs = new();
    public async Task<Blog?> GetByIdAsync(int id)
    {
        if (_cachedBlogs.TryGetValue(id.ToString(), out var value))
            return value;

        var path = Path.Combine(DirectoryPath, id.ToString());
        if (!File.Exists(path))
            return null;

        await using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        var content = await MessagePackSerializer.DeserializeAsync<Content>(fs);
        return ContentToBlog(content);
    }

    public async Task<IEnumerable<Blog>> GetAsync(int count = 5, int offset = 0)
    {
        var tasks = Directory.EnumerateFiles(DirectoryPath)
            .OrderByDescending(x => x)
            .Skip(offset)
            .Take(count)
            .Select(DeserializeFromFileAsync);

        return await Task.WhenAll(tasks.Select(x => x.AsTask()));
    }

    public Task CreateAsync(string markdownContent, string[] tags)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(long id, string markdownContent, string[] tags)
    {
        throw new NotImplementedException();
    }

    private async ValueTask<Blog> DeserializeFromFileAsync(string filePath)
    {
        var fileName = Path.GetFileNameWithoutExtension(filePath);
        if (_cachedBlogs.TryGetValue(fileName, out var value))
            return value;

        await using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        var content = await MessagePackSerializer.DeserializeAsync<Content>(fs);
        var blog = ContentToBlog(content);
        _cachedBlogs.TryAdd(fileName, blog);
        return blog;
    }

    private static Blog ContentToBlog(Content content)
        => new(content.Id, content.Title, content.Tags, content.Summary, content.RenderedContent,
            content.CreatedAt, content.UpdatedAt);

    [MessagePackObject]
    public class Content
    {
        [Key(0)]
        public long Id { get; set; }
        [Key(1)]
        public string Title { get; set; }
        [Key(2)]
        public string Summary { get; set; }
        [Key(3)]
        public string RenderedContent { get; set; }
        [Key(4)]
        public string[] Tags { get; set; }
        [Key(5)]
        public DateTimeOffset CreatedAt { get; set; }
        [Key(6)]
        public DateTimeOffset UpdatedAt { get; set; }
    }
}