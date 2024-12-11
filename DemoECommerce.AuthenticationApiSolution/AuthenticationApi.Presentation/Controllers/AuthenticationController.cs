using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using ECommerce.SharedLibrary.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]


    public class AuthenticationController(IUser userInterface) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<Response>> Register(AppUserDTO user) 
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var result = await userInterface.Register(user);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response>> Login(LoginDTO login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await userInterface.Login(login);
            return result.Flag ? Ok(result) : BadRequest(result);

        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ActionResult<GetUserDTO>> GetUser(int id)
        {
            if(id < 0) 
            {
                return BadRequest("User Id is invalid");
            }
            var user = await userInterface.GetUser(id);
            return user is not null ? Ok(user) : NotFound();
        }
    }
}
 