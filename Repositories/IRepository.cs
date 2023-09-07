using Nathan.Models;

namespace Nathan.Repositories
{
    public interface IRepository
    {

        //general
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void AddRange<T>(IEnumerable<T> entities) where T : class;
        Task<bool> SaveChangesAsync();



        //Authors
        Task<Author[]> GetAllAuthorAsync();

        Task<Author> GetAuthorAsync(string authorId);


        //Books
        Task<Book[]> GetAllBooksAsync();

        Task<Book> GetBookAsync(string bookId);


        //Other books
        Task<Book[]> GetBooksByAuthor(string authourId);

        Task<Book> GetBookByAuthor(string authourId, string bookId);


        //User
        Task<UserModel> GetCreator(string userId);

    }
}
