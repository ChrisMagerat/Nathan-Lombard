using Microsoft.EntityFrameworkCore;
using Nathan.Context;
using Nathan.Models;

namespace Nathan.Repositories
{
    public class Repository : IRepository
    {
        private readonly LibraryContext _libraryContext;

        public Repository(LibraryContext pedalProDbContext)
        {
            _libraryContext = pedalProDbContext;
        }

        
        public void Add<T>(T entity) where T : class
        {
            _libraryContext.Add(entity);
        }

        public void AddRange<T>(IEnumerable<T> entities) where T : class
        {
            _libraryContext.AddRange(entities);
        }

        public void Delete<T>(T entity) where T : class
        {
            _libraryContext.Remove(entity);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _libraryContext.SaveChangesAsync() > 0;
        }


        //Author
        public async Task<Author[]> GetAllAuthorAsync()
        {
            IQueryable<Author> query = _libraryContext.Authors;
            return await query.ToArrayAsync();
        }

        public async Task<Author> GetAuthorAsync(string authorId)
        {
            IQueryable<Author> query = _libraryContext.Authors.Where(c => c.AuthorId.ToString() == authorId);
            return await query.FirstOrDefaultAsync();
        }
    }
}
