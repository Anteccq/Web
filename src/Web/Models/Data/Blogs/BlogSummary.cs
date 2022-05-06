namespace Web.Models.Data.Blogs;

public record BlogSummary(long Id, string Title, string[] Tags, DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt);