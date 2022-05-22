using Web.Models.Data.Posts;

namespace Web.Models.Repositories.Posts;

public interface IPostRepository
{
    public Task<Post?> GetByIdAsync(string name);
    public Task<IEnumerable<Post>> GetAsync(int count = 5, int offset = 0);
    public Task CreateAsync(string markdownContent, string[] tags);
    public Task UpdateAsync(string name, string markdownContent, string[] tags);
}