using Web.Models.Data.Blogs;

namespace Web.Models.ViewModels;

public record IndexViewModel(IEnumerable<BlogSummary> Summaries);
