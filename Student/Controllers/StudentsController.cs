using AutoMapper;
using Book.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student.Data;
using Student.DTOs;
using Student.ViewModels;

namespace Student.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public StudentsController(DBContext context, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet("")]
        public async Task<IActionResult> StudentList()
        {
            var students = await _context.Students!.ToListAsync();
            var mapStudents = _mapper.Map<List<StudentViewModel>>(students);

            return Ok(mapStudents);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var student = await _context.Students!.FindAsync(id);

            var mapStudent = _mapper.Map<StudentViewModel>(student);

            return Ok(mapStudent);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(StudentDTO student)
        {
            var mapStudent = _mapper.Map<Models.Student>(student);
            await _context.AddAsync(mapStudent);
            await _context.SaveChangesAsync();
            foreach (var book in student.Books!)
            {
                book.StudentId = mapStudent.Id;
            }
            var books = new CreateBooksEvent()
            {
                CorrelationId = Guid.NewGuid(),
                Books = student.Books.ToList()
            };
            await _publishEndpoint.Publish(books);

            return Ok("Student created successfully!");
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(StudentDTO student)
        {
            _context.Update(_mapper.Map<Models.Student>(student));
            await _context.SaveChangesAsync();

            return Ok("Student updated successfully!");
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Students!.FindAsync(id);
            _context.Students.Remove(student!);
            await _context.SaveChangesAsync();

            return Ok("Student deleted successfully!");
        }
    }
}
