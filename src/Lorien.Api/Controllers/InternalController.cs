using Lorien.Api.Controllers.Base;
using Lorien.Application.Services.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lorien.Api.Controllers
{
    [AllowAnonymous]
    //[ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]")]
    public class InternalController(IInternalService internalService) : LorienBaseController
    {
        [HttpGet("cache")]
        public async Task<IActionResult> GetCacheInfoAsync()
        {
            var cachedItemsResponse = await internalService.GetCachedItemsAsync();
            return Ok(cachedItemsResponse);
        }
    }
}
