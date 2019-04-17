using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Chameleon.Models;
using Chameleon.Mongo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Chameleon.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;
        private readonly ILogger _logger;

        public BooksController(BookService bookService, ILogger<BooksController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<List<Book>> Get()
        {
            var ipAddress = GetClientIp(HttpContext);
            _logger.LogInformation($"{ipAddress.ToString()} Get books");
            var request = HttpContext.Request;
            
            return _bookService.Get();
        }

        [HttpGet("{id:length(24)}", Name = "GetBook")]
        public ActionResult<Book> Get(string id)
        {
            var ipAddress = GetClientIp(HttpContext);
            _logger.LogInformation($"{ipAddress.ToString()} Get book by id: {id}");
            var book = _bookService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        public ActionResult<Book> Create(Book book)
        {

            _bookService.Create(book);

            return CreatedAtRoute("GetBook", new { id = book.Id.ToString() }, book);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Book bookIn)
        {
            var book = _bookService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _bookService.Update(id, bookIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var book = _bookService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _bookService.Remove(book.Id);

            return NoContent();
        }

        #region private methods

        private IPAddress GetClientIp(HttpContext context)
        {
            var remoteIpAddress = context.Request.HttpContext.Connection.RemoteIpAddress;
            return remoteIpAddress;
        }

        #endregion
    }
}