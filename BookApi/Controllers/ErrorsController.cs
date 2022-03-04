using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{    
    [ApiController]
    public class ErrorsController : ControllerBase
    {        
        [ApiExplorerSettings(IgnoreApi = true)] // Förhindra fel med Swagger
        [Route("/error")]
        public IActionResult Error()
        {
            // Skafffa tillgång till felet
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (exception is null) return Problem();
            //var statusCode = exception.Error.GetType().Name;
            return Problem(detail : exception.Error.Message);
        }
    }
}
