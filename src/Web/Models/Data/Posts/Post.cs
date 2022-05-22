namespace Web.Models.Data.Posts;

public record Post(string Name, string Title, string[] Tags, string SummaryContent, string RenderedContent, DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt);