using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{    
    // Fångar upp eventuella fel
    [ApiController]
    public class ErrorsController : ControllerBase
    {        
        [ApiExplorerSettings(IgnoreApi = true)] // Förhindra fel med Swagger
        [Route("/error")]
        public IActionResult Error()
        {
            // Hämta aktuellt fel
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
            // Om det inte finns några detaljer om felet
            if (exception is null) return Problem();
            // Om detaljer finns, skicka med dessa
            return Problem(detail : exception.Error.Message);
        }
    }
}
