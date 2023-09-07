using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nathan.Models;
using Nathan.Repositories;
using Nathan.ViewModels;
using System.Security.Claims;

namespace Nathan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BooksController : ControllerBase
    {
        private readonly IRepository _repsository;
        private readonly UserManager<UserModel> _userManager;

        public BooksController(IRepository repository, UserManager<UserModel> userManager)
        {
            _repsository = repository;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("AddBook/{authorId}")]
        public async Task<ActionResult> AddBook(string authorId, BookViewModel bookAdd)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Username not found.");
            }

            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var userId = user.Id;

            try
            {
                var existingAuthor = await _repsository.GetAuthorAsync(authorId);
                if (existingAuthor == null) return NotFound("The author does not exist");


                var book = new Book
                {
                    CreatedBy = userId,
                    Author = existingAuthor.AuthorId.ToString(),
                    Publisher = bookAdd.publisher,
                    CopiesSold = bookAdd.copiesSold,
                    BookName = bookAdd.bookName,
                    DatePublished = bookAdd.datePublished
                };

                _repsository.Add(book);
                await _repsository.SaveChangesAsync();

                return Ok(book);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetAllBooks")]
        public async Task<IActionResult> GetAllBooks()
        {
            try
            {
                var books = await _repsository.GetAllBooksAsync();

                var bookReportList = new List<object>();

                foreach (var book in books)
                {
                    var author = await _repsository.GetAuthorAsync(book.Author);

                    if (author == null) return NotFound(book.Author);

                    var BookData = new
                    {
                        bookname=book.BookName,
                        bookAuthorName=author.AuthorName,
                        ownsAuthor=true
                    };

                    bookReportList.Add(BookData);
                }

                return Ok(bookReportList);
            }
            catch
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetAuthorBooks/{authorId}")]
        public async Task<IActionResult> GetAuthorBooks(string authorId)
        {
            try
            {
                var result = await _repsository.GetBooksByAuthor(authorId);
                if (result == null) return NotFound("Author does not exist");

                var returnList=new List<object>();

                foreach (var book in result)
                {
                    var list = new
                    {
                        bookName=book.BookName,
                        datePublished=book.DatePublished
                    };
                    returnList.Add(list);
                }

                return Ok(returnList);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }


        [HttpGet]
        [Route("GetAuthorBook/{authorId}/{bookId}")]
        public async Task<IActionResult> GetAuthorBook(string authorId, string bookId)
        {
            try
            {
                var result = await _repsository.GetBookByAuthor(authorId,bookId);
                if (result == null) return NotFound("Author or book does not exist");

                var user = await _repsository.GetCreator(result.CreatedBy);
                if (user == null) return NotFound("User does not exist");

                var returnData = new
                {
                    bookName=result.BookName,
                    datePublished=result.DatePublished,
                    publisher=result.Publisher,
                    copiesSold=result.CopiesSold,
                    creatorName=user.UserName,
                };

               return Ok(returnData);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }
    }
}
