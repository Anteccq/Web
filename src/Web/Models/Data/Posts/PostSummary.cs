namespace Web.Models.Data.Posts;

public record PostSummary(string Name, string Title, string SummaryContent, string[] Tags, DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt);