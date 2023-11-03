using DrinkFood_API.Model;
using DrinkFood_API.Models;
using DrinkFood_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrinkFood_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : BaseController
    {
        [Inject] private readonly StoreService _storeService;

        public StoreController(IServiceProvider provider) : base()
        {
            provider.Inject(this);
        }

        /// <summary>
        /// 店家清單
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpGet("GetStoreList")]
        public IActionResult GetStoreList([FromQuery] RequestStoreListModel Request)
        {
            var Response = _storeService.GetStoreList(Request);
            return Json(new ResponseData<object?>(Response, Response.Count));
        }
    }
}
