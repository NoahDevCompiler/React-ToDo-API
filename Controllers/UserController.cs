using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using React__User_Control__API.Modells;
using System.Security.Cryptography;

namespace React__User_Control__API.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        
       
        [HttpPost("register")]     
        public IActionResult Register([FromBody] User model) {

            UserHashed user = new UserHashed();

            if (!ModelState.IsValid) return BadRequest("Invalid Data");

            ToDoResult _res = Program.DB.IsUserTaken(model.UserName, model.Email);
            if (!_res.success) return BadRequest("DB Failed");
            if ((bool)_res.result == true) return BadRequest("Benutzer Bereits vergeben");
            else
            {
                Auth.PasswordHash.CreatePassword(model.Password, out byte[] PasswordHashed, out byte[] PasswordSalt);

                
                user.PasswordHashed = PasswordHashed;
                user.UserName = model.UserName;
                user.PasswordSalt = PasswordSalt;

                string hashedPasswordBase64 = Convert.ToBase64String(user.PasswordHashed);
                string saltBase64 = Convert.ToBase64String(user.PasswordSalt);

                Program.DB.CreateUser(user.UserName, model.Email, hashedPasswordBase64, saltBase64);

                return Ok("Konto Erstellt");
            }
 
        }

        [HttpPost("login")]
        public IActionResult CheckLogin([FromBody] string email, [FromBody] string password) {

            ToDoResult result = Program.DB.CheckEmail(email);
            ToDoResult result1 = Program.DB.CheckLogin(password, email);

            if (!result.success) {
              return BadRequest("Email wird nicht verwendet");
            }
            if (!result1.success) {
                return BadRequest("Falsche eingaben für Login");
            } else {
                return Ok("Korrekte Login eingaben");
            }   
            
        }
    }


}
