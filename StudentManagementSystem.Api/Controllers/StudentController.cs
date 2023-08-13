using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Domain.Common.Entities;
using StudentManagementSystem.Domain.Interfaces;
using StudentManagementSystem.Domain.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService) 
        { 
            _studentService = studentService;
        }

        // GET: api/<StudentController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _studentService.GetAllStudents());
        }

        // GET api/<StudentController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var student = await _studentService.GetStudent(id);

                if (student == null)
                {
                    return NotFound($"Student with ID {id} not found.");
                }

                return Ok(student);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // POST api/<StudentController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Student student)
        {
            return Ok(await _studentService.CreateStudent(student));
        }

        // PUT api/<StudentController>/5
        [HttpPut]
        public async Task<IActionResult> Put( [FromBody] Student student)
        {
            return Ok(await _studentService.UpdateStudent(student));
        }

        // DELETE api/<StudentController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _studentService.DeleteStudent(id));
        }
    }
}
