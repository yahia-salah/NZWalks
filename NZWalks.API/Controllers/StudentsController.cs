using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{
	// https://localhost:5001/api/students
	[Route("api/[controller]")]
	[ApiController]
	public class StudentsController : ControllerBase
	{
		private static string[] studentNames = new[]
		{
			"John", "Jane", "Jack", "Jill"
		};

		// GET: api/students
		[HttpGet]
		public IActionResult GetAllStudents()
		{
			return Ok(studentNames);
		}

		// GET: api/students/0
		[HttpGet("{id}")]
		public IActionResult GetStudentById(int id)
		{
			if (id >= 0 && id < studentNames.Length)
			{
				return Ok(studentNames[id]);
			}
			else
			{
				return NotFound();
			}
		}
	}
}
