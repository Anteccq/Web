namespace Web.Models.Data.Blogs;

public record Blog(string Name, string Title, string[] Tags, string SummaryContent, string RenderedContent, DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt);