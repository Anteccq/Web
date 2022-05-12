namespace Web.Models.Data.Blogs;

public record BlogSummary(string Name, string Title, string SummaryContent, string[] Tags, DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt);