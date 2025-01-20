using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreApi.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiExtensions : ControllerBase
    {
        [HttpGet("GetHelloWorld")]
        public IActionResult GetHelloWorld()
        {
            return Ok("Hello , World!");
        }
    }
}