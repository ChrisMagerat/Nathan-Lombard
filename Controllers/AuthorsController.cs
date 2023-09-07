using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nathan.Models;
using System.Data;
using System.Security.Claims;
using Nathan.Repositories;
using Nathan.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Nathan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthorsController : ControllerBase
    {
        
            private readonly IRepository _repsository;
            private readonly UserManager<UserModel> _userManager;

            public AuthorsController(IRepository repository, UserManager<UserModel> userManager)
            {
                _repsository = repository;
                _userManager = userManager;
            }


            [HttpGet]
            [Route("GetAllAuthors")]
            public async Task<IActionResult> GetAllAuthors()
            {
                try
                {
                    var authors = await _repsository.GetAllAuthorAsync();

                    return Ok(authors);
                }
                catch
                {
                    return StatusCode(500, "Internal Server Error. Please contact support.");
                }
            }

            [HttpGet]
            [Route("GetAuthor/{authorId}")]
            public async Task<IActionResult> GetAuthor(string authorId)
            {
                try
                {
                    var result = await _repsository.GetAuthorAsync(authorId);
                    if (result == null) return NotFound("Author does not exist");
                    return Ok(result);
                }
                catch (Exception)
                {
                    return StatusCode(500, "Internal Server Error. Please contact support.");
                }
            }


            [HttpPost]
            [Route("AddAuthor")]
            public async Task<IActionResult> AddAuthor(AuthorViewModel authoradd)
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
                    var author = new Author
                    {
                        CreatedBy = userId,
                        AuthorName = authoradd.name,
                        ActiveFrom = authoradd.activeFrom,
                        ActiveTo = authoradd.activeTo
                    };

                    _repsository.Add(author);
                    await _repsository.SaveChangesAsync();

                    return Ok(author);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Internal Server Error. Please contact support.");
                }
            }


            
            [HttpPut]
            [Route("EditAuthor/{authorId}")]
            public async Task<ActionResult> EditAuthor(string authorId, AuthorViewModel authorEdit)
            {
                try
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


                    var existingAuthor = await _repsository.GetAuthorAsync(authorId);
                    if (existingAuthor == null) return NotFound("The author does not exist");


                    if (existingAuthor.CreatedBy != userId)
                    {
                        return BadRequest("You do not have access to edit this author");
                    }
                    else
                    {
                        existingAuthor.AuthorName = authorEdit.name;
                        existingAuthor.ActiveFrom = authorEdit.activeFrom;
                        existingAuthor.ActiveTo = authorEdit.activeTo;
                        existingAuthor.CreatedBy = userId;
                    }
                    await _repsository.SaveChangesAsync();

                    return Ok(existingAuthor);

                }
                catch (Exception)
                {
                    return StatusCode(500, "Internal Server Error. Please contact support.");
                }
            }
            


            [HttpDelete]
            [Route("DeleteAuthor/{authorId}")]
            public async Task<IActionResult> DeleteAuthor(string authorId)
            {
                try
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

                    var existingAuthor = await _repsository.GetAuthorAsync(authorId);
                    if (existingAuthor == null) return NotFound($"The author does not exist");


                    if (existingAuthor.CreatedBy != userId)
                    {
                        return BadRequest("You do not have access to delete this author");
                    }
                    else
                    {
                        _repsository.Delete(existingAuthor);
                        await _repsository.SaveChangesAsync();
                        return Ok(existingAuthor);

                    }
                }
                catch
                {
                    return StatusCode(500, "Internal Server Error. Please contact support.");
                }
                
            }
        }
    }

