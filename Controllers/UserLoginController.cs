using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using React__User_Control__API.Modells;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
namespace React__User_Control__API.Controllers
{
    [Route("api/[controller]")]
    public class UserLoginController : Controller {
        [HttpPost("login")]
        public IActionResult CheckLogin([FromBody]NetUserLogin value) {

            ToDoResult result = Program.DB.CheckEmail(value.Email);

            if (!result.success) return BadRequest("Email wird nicht verwendet");


            ToDoResult result1 = Program.DB.CheckLogin(value.Email, value.Password);

            if (!result1.success)  return BadRequest("Falsche eingaben für Login");

            ToDoResult getid = Program.DB.GetID(value.Email);
            return Ok(getid.result);
            
        }
        public class NetUserLogin {
            [Required] public string Email { get; set; }
            [Required] public string Password { get; set; }

            public NetUserLogin(string email, string password) {
                this.Email = email;
                this.Password = password;
            }

            public NetUserLogin() { }

        }
    }
}
   
    

