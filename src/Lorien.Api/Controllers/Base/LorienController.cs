using Lorien.Api.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Lorien.Api.Controllers.Base
{
    [ApiController]
    [ServiceFilter(typeof(ApiExceptionFilterAttribute))]
    public class LorienBaseController : ControllerBase
    {
    }
}
