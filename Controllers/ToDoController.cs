using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using React__User_Control__API.Modells;
using System.Security.Cryptography;


[Route("api/[controller]")]
public class ToDoController : Controller
{

    [HttpPost("todos")]

    public IActionResult ToDo([FromBody] Todo model) {

        if (ModelState.IsValid) {
            ToDoResult res = Program.DB.InsertToDO(model.Name, model.Description, model.Type, model.Startdate, model.Enddate);
            if (res.success) {

                return Ok("ToDo erstellt");
            }
            else return BadRequest(res.error);
        }
        else return BadRequest(ModelState);
        
    }

    [HttpGet("todos")]
    public IActionResult GetToDos() {
        
        ToDoResult res = Program.DB.GetToDos()
        
        return Ok(todos);
    }
}
