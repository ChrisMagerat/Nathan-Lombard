using Nathan.Models;

namespace Nathan.Repositories
{
    public interface IRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void AddRange<T>(IEnumerable<T> entities) where T : class;
        Task<bool> SaveChangesAsync();




        Task<Author[]> GetAllAuthorAsync();

        Task<Author> GetAuthorAsync(string authorId);

    }
}
