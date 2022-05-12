using Web.Models.Data.Blogs;

namespace Web.Models.Repositories.Blogs;

public interface IBlogRepository
{
    public Task<Blog?> GetByIdAsync(string name);
    public Task<IEnumerable<Blog>> GetAsync(int count = 5, int offset = 0);
    public Task CreateAsync(string markdownContent, string[] tags);
    public Task UpdateAsync(string name, string markdownContent, string[] tags);
}