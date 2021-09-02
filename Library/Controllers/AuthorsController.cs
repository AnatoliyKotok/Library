using Library.Data.Models;
using Library.Data.Services;
using Library.Data.ViewsModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {

        private AuthorService _authorsService;
        public AuthorsController(AuthorService authorsService)
        {
            _authorsService = authorsService;
        }

        [HttpGet("get-all-authors")]
        public IActionResult GetAllAuthors()
        {
            var _authors = _authorsService.GetAllAuthors();
            return Ok(_authors);
        }

        [HttpGet("get-author-by-id/{id}")]
        public IActionResult GetAuthorById(int id)
        {
            var _author = _authorsService.GetAuthorById(id);
            return Ok(_author);
        }

        //[HttpGet("get-author-with-books-by-id/{id}")]
        //public IActionResult GetAuthorWithBooks(int id)
        //{
        //    var response = _authorsService.GetAuthorWithBooks(id);
        //    return Ok(response);
        //}

        [HttpPost("add-author")]
        public IActionResult AddAuthor([FromBody] AuthorVM author)
        {
            _authorsService.AddAuthor(author);
            return Ok();
        }

        [HttpPut("update-author-by-id/{id}")]
        public IActionResult UpdateAuthorById(int id, [FromBody] AuthorVM author)
        {
            var _author = _authorsService.UpdateAuthorById(id, author);
            return Ok(_author);
        }

        [HttpDelete("delete-author-by-id/{id}")]
        public IActionResult DeletePublisherById(int id)
        {
            try
            {
                _authorsService.DeleteAuthorById(id);
                return Ok();
            }
            catch (Exception ex)
            {   
                return BadRequest(ex.Message);
            }
        }
    }
}
