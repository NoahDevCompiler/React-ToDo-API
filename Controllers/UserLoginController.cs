using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using React__User_Control__API.Modells;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
namespace React__User_Control__API.Controllers
{
    [Route("api/[controller]")]
    public class UserLoginController : ControllerBase {
        [HttpPost("login")]
        public IActionResult CheckLogin(NetUserLogin value) {

            ToDoResult result = Program.DB.CheckEmail(value.Email);
            ToDoResult result1 = Program.DB.CheckLogin(value.Password, value.Email);

            if (!result.success) {
                return BadRequest("Email wird nicht verwendet");
            }
            if (!result1.success) {
                return BadRequest("Falsche eingaben für Login");
            } else {
                return Ok("Korrekte Login eingaben");
            }

        }
        public class NetUserLogin
        {
            [Required] public string Email { get; }
            [Required] public string Password { get; }

            public NetUserLogin(string email, string password) {
                this.Email = email;
                this.Password = password;
            }

        }
    }
}
   
    

