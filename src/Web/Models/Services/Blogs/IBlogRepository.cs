using Web.Models.Data.Blogs;

namespace Web.Models.Services.Blogs;

public interface IBlogRepository
{
    public Task<Blog?> GetByIdAsync(int id);
    public Task<IEnumerable<Blog>> GetAsync(int count = 5, int offset = 0);
    public Task CreateAsync(string markdownContent, string[] tags);
    public Task UpdateAsync(long id, string markdownContent, string[] tags);
}