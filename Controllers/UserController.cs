using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using React__User_Control__API.Modells;

namespace React__User_Control__API.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterViewModell model) {
            if (!ModelState.IsValid) return BadRequest("Invalid Data");

            ToDoResult _res = Program.DB.IsUserTaken(model.UserName);
            if (!_res.success) return BadRequest("DB Failed");
            if (!(bool)_res.result) return BadRequest("Benutzer Bereits vergeben");

            return Ok("Konto Erstellt");
        }
    }
}
