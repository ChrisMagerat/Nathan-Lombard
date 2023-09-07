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

        //general
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



        //Book
        public async Task<Book[]> GetAllBooksAsync()
        {
            IQueryable<Book> query = _libraryContext.Books;
            return await query.ToArrayAsync();
        }

        public async Task<Book> GetBookAsync(string bookId)
        {
            IQueryable<Book> query = _libraryContext.Books.Where(c => c.BookId.ToString() == bookId);
            return await query.FirstOrDefaultAsync();
        }


        //Other books
        public async Task<Book[]> GetBooksByAuthor(string authourId)
        {
            IQueryable<Book> query = _libraryContext.Books.Where(c => c.Author == authourId);
            return await query.ToArrayAsync();
        }


        public async Task<Book> GetBookByAuthor(string authourId, string bookId)
        {
            IQueryable<Book> query = _libraryContext.Books.Where(c => c.Author == authourId && c.BookId.ToString()==bookId);
            return await query.FirstOrDefaultAsync();
        }



        //User
        public async Task<UserModel> GetCreator(string userId)
        {
            IQueryable<UserModel> query = _libraryContext.Users.Where(c => c.Id == userId);
            return await query.FirstOrDefaultAsync();
        }
    }
}
