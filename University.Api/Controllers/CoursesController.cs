using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using University.Api.Filters;
using University.Core.Forms;
using University.Core.Services;

namespace University.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ApiExceptionFilter))]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        public ApiResponse GetById(int id)
        {
            var dto = _courseService.GetById(id);
            return new ApiResponse(dto);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public ApiResponse GetAll()
        {
            var dto = _courseService.GetAll();
            return new ApiResponse(dto);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public ApiResponse Create([FromBody] CreateCourseForm form)
        {
            _courseService.Create(form);
            return new ApiResponse(HttpStatusCode.Created);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{id}")]
        public ApiResponse Update(int id, [FromBody] UpdateCourseForm form)
        {
            _courseService.Update(id, form);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id}")]
        public ApiResponse Delete(int id)
        {
            _courseService.Delete(id);
            return new ApiResponse(HttpStatusCode.OK);
        }
    }
}
