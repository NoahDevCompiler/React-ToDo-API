using Microsoft.AspNetCore.Mvc;
using React__User_Control__API.Modells;

namespace React__User_Control__API.Controllers
{
    [Route("api/[controller]")]
    public class FetchUserData : ControllerBase
    {
        [HttpGet("UserName")]
        public IActionResult Username([FromQuery] int id) {
            ToDoResult res = Program.DB.GetUsername(id);
            return Ok(res.result);
        }
    }
}
