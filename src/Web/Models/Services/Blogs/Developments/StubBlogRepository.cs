using Markdig;
using Web.Models.Data.Blogs;

namespace Web.Models.Services.Blogs.Developments;

public class StubBlogRepository : IBlogRepository
{
    private readonly string _renderedBody;
    public StubBlogRepository()
    {
        var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        var markdownContent = File.ReadAllText("./DummyBlogContent.md");
        _renderedBody = Markdig.Markdown.ToHtml(markdownContent, pipeline);
    }

    public Task<Blog> GetByIdAsync(int id)
    {
        return Task.FromResult(
            new Blog(id, $"Blog {id}", new[] { "tech", "random"}, _renderedBody, DateTimeOffset.Now, DateTimeOffset.Now ));
    }

    public Task<IEnumerable<Blog>> GetAsync(int count = 5, int offset = 0)
    {
        return Task.FromResult(Enumerable
            .Range(offset, count)
            .Select(x => new Blog(x, $"Blog {x}", new[] { "Tech", "Random" }, _renderedBody, DateTimeOffset.Now,
                DateTimeOffset.Now)));
    }

    public Task CreateAsync(string markdownContent, string[] tags)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(long id, string markdownContent, string[] tags)
    {
        throw new NotImplementedException();
    }
}