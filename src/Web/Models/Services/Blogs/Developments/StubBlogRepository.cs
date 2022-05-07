using Markdig;
using Web.Models.Data.Blogs;

namespace Web.Models.Services.Blogs.Developments;

public class StubBlogRepository : IBlogRepository
{
    private readonly string _renderedBody;
    private readonly Blog _embeddedBlog;
    public StubBlogRepository()
    {
        var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        var markdownContent = File.ReadAllText("./DummyBlogContent.md");
        _renderedBody = Markdown.ToHtml(markdownContent, pipeline);
        _embeddedBlog = new Blog(0, "ASP .NET Core MVC で個人ブログを作った", new[] { "Tech", "ASP.NET Core", "C#" }, "Anteccq Blog with ASP .NET Core",
            _renderedBody, DateTimeOffset.Parse("2022/05/07"), DateTimeOffset.Parse("2022/05/07"));
    }

    public Task<Blog> GetByIdAsync(int id)
    {
        return Task.FromResult(_embeddedBlog);
    }

    public Task<IEnumerable<Blog>> GetAsync(int count = 5, int offset = 0)
    {
        IEnumerable<Blog> blogs = new List<Blog>
        {
            _embeddedBlog
        };

        return Task.FromResult(blogs);
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