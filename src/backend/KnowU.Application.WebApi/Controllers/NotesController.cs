using Microsoft.AspNetCore.Mvc;

namespace KnowU.Application.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class NotesController : Controller
{
    [HttpPut(Name = "note")]
    public IActionResult AddNote([FromBody] string note)
    {
        return Ok(new { Message = "Note added successfully", Note = note });
    }
}
