using Lorien.Api.Controllers.Base;
using Lorien.Application.Services.Internal;
using Microsoft.AspNetCore.Mvc;

namespace Lorien.Api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]")]
    public class InternalController(IInternalService internalService) : LorienBaseController
    {
        [HttpGet("cache")]
        public async Task<IActionResult> GetCacheInfoAsync()
        {
            return Ok(await internalService.GetCachedItemsAsync());
        }
    }
}
