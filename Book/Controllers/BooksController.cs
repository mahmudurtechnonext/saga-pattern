using AutoMapper;
using Book.Data;
using Book.DTOs;
using Book.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Book.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public BooksController(DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<IActionResult> BookList()
        {
            var books = await _context.Books!.ToListAsync();
            var mapBooks = _mapper.Map<List<BookViewModel>>(books);

            return Ok(mapBooks);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var book = await _context.Books!.FindAsync(id);

            var mapBook = _mapper.Map<BookViewModel>(book);

            return Ok(mapBook);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(BookDTO book)
        {
            await _context.AddAsync(_mapper.Map<Models.Book>(book));
            await _context.SaveChangesAsync();

            return Ok("Book created successfully!");
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(BookDTO book)
        {
            _context.Update(_mapper.Map<Models.Book>(book));
            await _context.SaveChangesAsync();

            return Ok("Book updated successfully!");
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books!.FindAsync(id);
            _context.Books.Remove(book!);
            await _context.SaveChangesAsync();

            return Ok("Book deleted successfully!");
        }
    }
}
