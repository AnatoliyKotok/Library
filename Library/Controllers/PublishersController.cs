using Library.ActionResults;
using Library.Data.Models;
using Library.Data.Services;
using Library.Data.ViewsModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly PublishersService _publisherService;

        public PublishersController(PublishersService publishersService)
        {
            _publisherService = publishersService;
        }

        [HttpGet("get-all-publishers")]
        public IActionResult GetAllPublishers(string sortBy, string searchString, int? pageNumber)
        {
            if (sortBy == "E")
            {
                throw new Exception();
            }
            var allPublishers = _publisherService.GetAllPublishers(sortBy, searchString, pageNumber);
            return Ok(allPublishers);
        }

        [HttpGet("get-publisher-by-id/{id}")]
        public CustomActionResult GetPublisherById(int id)
        {
            var _response = _publisherService.GetPublisherById(id);


            if (_response != null)
            {
                var _responceObj = new CustomActionResultVM()
                {
                    Publisher = _response
                };

                return new CustomActionResult(_responceObj);
            }
            else
            {
                var _responceObj = new CustomActionResultVM()
                {
                    Exception = new Exception("Publisher not found.")
                };

                return new CustomActionResult(_responceObj);
            }
        }

        [HttpGet("get-publisher-books-with-authors/{id}")]
        public IActionResult GetPublisherBooksWithAuthors(int id)
        {
            var _publisherData = _publisherService.GetPublisherData(id);
            if (_publisherData != null)
            {
                return Ok(_publisherData);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("add-publisher")]
        public IActionResult AddPublisher([FromBody] PublisherVM publisher)
        {
            var newPublisher = _publisherService.AddPublisher(publisher);
            return Created(nameof(AddPublisher), newPublisher);
        }

        [HttpPut("update-author-by-id/{id}")]
        public IActionResult UpdateAuthorById(int id, [FromBody] PublisherVM publisher)
        {
            var _publisher = _publisherService.UpdatePublisherById(id, publisher);
            return Ok(_publisher);
        }

        [HttpDelete("delete-publisher-by-id/{id}")]
        public IActionResult DeletePublisherById(int id)
        {
            try
            {
                _publisherService.DeletePublisherById(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
