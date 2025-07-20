using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using University.Api.Filters;
using University.Core.DTOs;
using University.Core.Forms;
using University.Core.Services;

namespace University.Api.Controllers
{
    [Authorize]//since you put the attribute the system will need authorization everytime you want to do something
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ApiExceptionFilter))]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentsController(IStudentService studentService/*before here was empty*/)
        {
            /* var dbContext = new UniversityDbContext();
            var repo = new StudentRepository(dbContext);
            _studentService = new StudentService(repo); */

            _studentService = studentService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(StudentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ApiResponse GetById(int id)
        {
            var dto = _studentService.GetById(id);
            return new ApiResponse(dto);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<StudentDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ApiResponse GetAll()
        {
            var dto = _studentService.GetAll();
            return new ApiResponse(dto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ApiResponse Create([FromBody] CreateStudentForm form)
        {
            _studentService.Create(form);
            return new ApiResponse(HttpStatusCode.Created);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{id}")]

        public ApiResponse Update(int id, [FromBody] UpdateStudentForm form)
        {
            _studentService.Update(id, form);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id}")]
        public ApiResponse Delete(int id)
        {
            _studentService.Delete(id);
            return new ApiResponse(HttpStatusCode.OK);
        }
         
    }
}