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
        public IActionResult Register([FromBody] RegisterViewModell model) {

            UserHashed user = new UserHashed();

            if (!ModelState.IsValid) return BadRequest("Invalid Data");

            ToDoResult _res = Program.DB.IsUserTaken(model.UserName);
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
    }
}
