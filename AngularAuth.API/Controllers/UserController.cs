using AngularAuth.API.Context;
using AngularAuth.API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularAuth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UserController(AppDbContext appDbContext)
        {
            this._context = appDbContext; 
        }

        [HttpPost("authenticate")]

        public async Task<IActionResult> AuthenticateUser([FromBody]User user)
        {

            if (user == null)
            {
                return BadRequest();
            }

            //check user
            var userData=await this._context.Users.FirstOrDefaultAsync(x=>x.Username == user.Username);
            if (user == null) {
                return NotFound(new { Message= " User Not Found"});
            }

            //check the password

            var userDataWithPassword = await this._context.Users.FirstOrDefaultAsync(x =>x.Username==user.Username && x.Password == user.Password);
            if (userDataWithPassword == null)
            {
                return NotFound(new { Message = "Incorrect Password" });
            }


            //return the token
            return Ok(new { Message = "Login is successful" });
        }




        [HttpPost("register")]

        public async Task<IActionResult> RegisterUser([FromBody]User user)
        {

            if(user == null) { 
            return BadRequest();    
            }

            if(string.IsNullOrEmpty(user.Username))
            {
                return BadRequest(new {Message= "Username Cannot be null" });
            }

            if (string.IsNullOrEmpty(user.Firstname))
            {
                return BadRequest(new { Message = "Firstname Cannot be null" });
            }

            if (string.IsNullOrEmpty(user.Lastname))
            {
                return BadRequest(new { Message = "Lastname Cannot be null" });
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                return BadRequest(new { Message = "Email Cannot be null" });
            }

            if (string.IsNullOrEmpty(user.Password))
            {
                return BadRequest(new { Message = "Password Cannot be null" });
            }

            if (string.IsNullOrEmpty(user.PhoneNumber))
            {
                return BadRequest(new { Message = "PhoneNumber Cannot be null" });
            }


            await this._context.Users.AddAsync(user);
            await this._context.SaveChangesAsync();
            return Ok(new
            {
                Message="Registration is successful"
            });

        }
    }
}
