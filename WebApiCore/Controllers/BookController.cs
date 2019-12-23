using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public BookController(IRepositoryWrapper repositoryContext, ILoggerManager logger, IMapper mapper)
        {
            _repository = repositoryContext;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all books From DB
        /// </summary>
        [HttpGet]
        [Produces("application/json", "application/xml", Type = typeof(List<string>))]
        public async Task<IActionResult> GetBooks([FromQuery] BookParameters parameters)
        {
            try
            {
                //var books = await _repository.Book.GetBooksAsync(parameters);

                var books = await _repository.Book.GetBooksWithPaginationAsync(parameters);

                var metadata = new
                {
                    books.TotalCount,
                    books.PageSize,
                    books.CurrentPage,
                    books.TotalPages,
                    books.HasNext,
                    books.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                _logger.LogInfo($"Returned all books from database.");

                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetBooks action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get Book by id
        /// </summary>
        [HttpGet("{id}", Name = "BookById")]
        [Produces("application/json", "application/xml", Type = typeof(List<string>))]
        public async Task<IActionResult> GetBook(int id, [FromQuery] string fields)
        {
            try
            {
                var book = await _repository.Book.GetBookAsync(id, fields);

                if (book == default(Entity))
                {
                    _logger.LogInfo($"Book with id:{id} not found.");
                    return NotFound();
                }

                _logger.LogInfo($"Returned book with id:{id} from database.");

                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetBook action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Adding new book
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] BookDto book)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogInfo("Model is not valid");
                    return BadRequest();
                }

                var mappedBook = _mapper.Map<Book>(book);

                await _repository.Book.AddBookAsync(mappedBook);
                await _repository.SaveAsync();

                var createdBook = _mapper.Map<BookDto>(mappedBook);

                return CreatedAtRoute("BookById", new { id = mappedBook.Id }, createdBook);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside AddBook action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Update Book
        /// </summary>
        /// <param name="id">Book Id</param>
        /// <param name="book">BookDTO</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody]BookDto book)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogInfo("Model is not valid");
                    return BadRequest();
                }

                var dbBookEntity = _repository.Book.GetBook(id);

                if (dbBookEntity == null)
                {
                    _logger.LogInfo($"Book with id:{id} not found");
                    return NotFound();
                }

                _mapper.Map(book, dbBookEntity);

                _repository.Book.UpdateBook(dbBookEntity);

                _repository.Save();

                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateBook action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        /// <summary>
        /// Delete Book
        /// </summary>
        /// <param name="id">Book Id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            try
            {
                var book = _repository.Book.GetBook(id);

                if (book == null)
                {
                    _logger.LogInfo($"Book with id:{id} not found");
                    return NotFound();
                }

                _repository.Book.DeleteBook(book);

                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside AddBook action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
