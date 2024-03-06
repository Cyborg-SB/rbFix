using Microsoft.AspNetCore.Mvc;
using RbFix.Infrastructure.Configuration.Constants;

namespace RbFix.Controllers
{
    [ApiController]
    public class SessionManagementCotroller : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"};


        public SessionManagementCotroller()
        {
        }

        [HttpGet("gerenciar_sessoes/consulta")]
        public IEnumerable<object> GetAllSessionsAsync()
        {
            return Enumerable.Range(1, 5).Select(index => new
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("gerenciar_sessoes/consulta/{nome_sessao}")]
        public IActionResult GetSessionAsync([FromRoute(Name = "nome_sessao")] string sessionName)
        {
            return Ok(Enumerable.Range(1, 5).Select(index => new
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray());
        }

        [HttpPost("gerenciar_sessoes/registra")]
        public IActionResult RegistryAllSessionsAsync()
        {
            return Ok(Enumerable.Range(1, 5).Select(index => new
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray());
        }

        [HttpPost("gerenciar_sessoes/registra/{nome_sessao}")]
        public IActionResult RegisterSessionAsync([FromRoute(Name = "nome_sessao")] string sessionName)
        {
            return Ok(Enumerable.Range(1, 5).Select(index => new
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray());
        }

        [HttpPost("gerenciar_sessoes/remove_registro")]
        public IActionResult UnRegistryAllSessionsAsync()
        {
            return Ok(Enumerable.Range(1, 5).Select(index => new
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray());
        }

        [HttpPost("gerenciar_sessoes/remove_registro/{nome_sessao}")]
        public IActionResult UnRegisterSessionAsync([FromRoute(Name = "nome_sessao")] string sessionName)
        {
            return Ok(Enumerable.Range(1, 5).Select(index => new
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray());
        }


    }
}