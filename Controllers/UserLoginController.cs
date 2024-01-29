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
            

            if (result.success){
                ToDoResult result1 = Program.DB.CheckLogin(value.Email, value.Password);

                if (!result1.success) {
                    return BadRequest("Falsche eingaben für Login");
                } else {
                    return Ok("Korrekte Login eingaben");
                }
            }

            else return BadRequest("Email wird nicht verwendet");
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
   
    

