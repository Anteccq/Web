using Web.Models.Data.Posts;

namespace Web.Models.ViewModels;

public record IndexViewModel(IEnumerable<PostSummary> Summaries);
