using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{    
    [ApiController]
    public class ErrorsController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Error()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
            //var statusCode = exception.Error.GetType().Name;            
            return Problem(detail : exception.Error.Message);
        }
    }
}
