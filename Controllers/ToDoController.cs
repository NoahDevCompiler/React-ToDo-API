using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using React__User_Control__API.Modells;
using System.Security.Cryptography;
using Newtonsoft.Json.Serialization;


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

    [HttpGet("gettodos")]
    public IActionResult GetToDo([FromQuery] bool completedOnly)
    {
        ToDoResult res = Program.DB.GetToDos(completedOnly);
        return Ok(res.result);
    }

    [HttpPut("updateStatus")]
    public IActionResult UpdateToDoStatus(int id, [FromBody] int isfinished)
    {
        try
        {
            ToDoResult res = Program.DB.UpdateCompletion(id, isfinished);
            if(res.success) {
                return Json("Aktualisiert");
            }
            else return BadRequest(res.error);
            
        }
        catch (Exception ex)
        {
            return BadRequest("Fehler beim aufrufen der Methode " + ex.Message);
        }
    }

    [HttpDelete("deleteTodo")]

    public IActionResult DeleteToDo(int id)
    {
        try
        {
            ToDoResult res = Program.DB.DeleteToDo(id);
            if(res.success) {
                return Json("ToDo von Datenbank gelöscht");
            }
            else return BadRequest(res.error);
        }
        catch (Exception ex) { 
            return BadRequest( "Fehler beim aufrufen der Methode" + ex.Message); 
        }

    }
}
