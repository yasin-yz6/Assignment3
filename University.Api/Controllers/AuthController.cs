using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Mvc;
using University.Api.Filters;
using University.Api.Helpers;
using University.Core.Forms;
using University.Core.Services;

namespace University.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ApiExceptionFilter))]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtTokenHelper _jwtTokenHelper;
        public AuthController(IAuthService authService, IJwtTokenHelper jwtTokenHelper) //the constructor
        {
            _authService = authService;
            _jwtTokenHelper = jwtTokenHelper;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ApiResponse> Register([FromBody]RegisterForm form)
        {
            var dto = await _authService.Register(form);
            return new ApiResponse(dto, StatusCodes.Status200OK);
        }


        //----

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ApiResponse>/*will return ApiResponse*/ Login([FromBody] LoginForm form)
        {
            var user /*User*/ = await _authService.Login(form);
            var token = _jwtTokenHelper.GenerateToken(user /*User*/);

            return new ApiResponse(token, StatusCodes.Status200OK);//anoter way to return response!//because the fn. will return Task<ApiResponse>
            /*for example/ = [ProducesResponseType(typeof(StudentDTO), StatusCodes.Status200OK)]this one 
             applied to make the swagger know what response to give!!*/
        }


    }
}
