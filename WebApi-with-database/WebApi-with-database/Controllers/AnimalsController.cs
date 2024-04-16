using Microsoft.AspNetCore.Mvc;

namespace WebApi_with_database.Controllers;

[ApiController]
// [Route("/api/animals")]
[Route("api/[controller]")]
public class AnimalsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAnimals()
    {
        return Ok();
    }
    
}