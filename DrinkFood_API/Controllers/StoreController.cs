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
        /// 店家詳細資料
        /// </summary>
        [ProducesResponseType(typeof(ResponseStoreListModel), StatusCodes.Status200OK)]
        [HttpGet("GetStore/{StoreID}")]
        public IActionResult GetStore(Guid StoreID)
        {
            ResponseStoreListModel Response = _storeService.GetStore(StoreID);
            return Json(new ResponseData<ResponseStoreListModel>(Response, 1));
        }

        /// <summary>
        /// 店家清單
        /// </summary>
        [ProducesResponseType(typeof(List<ResponseStoreListModel>), StatusCodes.Status200OK)]
        [HttpGet("GetStoreList")]
        public IActionResult GetStoreList([FromQuery] RequestStoreListModel RequestData)
        {
            List<ResponseStoreListModel> Response = _storeService.GetStoreList(RequestData);
            return Json(new ResponseData<List<ResponseStoreListModel>>(Response, Response.Count));
        }
    }
}
