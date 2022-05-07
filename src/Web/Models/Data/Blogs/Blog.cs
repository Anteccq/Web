namespace Web.Models.Data.Blogs;

public record Blog(long Id, string Title, string[] Tags, string SummaryContent, string RenderedContent, DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt);