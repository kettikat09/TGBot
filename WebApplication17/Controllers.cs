using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api/[controller]")]
public class KursovaController : ControllerBase
{
    private readonly Client _client;

    public KursovaController(Client client)
    {
        _client = client;
    }

    [HttpGet("horoscope")]
    [SwaggerOperation(Summary = "Отримати результат")]
    [SwaggerResponse(200, "Успішний запит", typeof(string))]
    public async Task<IActionResult> Get([FromQuery] string zodiac)
    {
        var model = new ModelSwagger
        {
            Zodiac = zodiac
        };


        var json = await _client.Swagger(model);

        return Ok(json);
        Console.WriteLine(model);
    }

}