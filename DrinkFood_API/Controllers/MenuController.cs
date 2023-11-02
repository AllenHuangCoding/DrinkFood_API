using DrinkFood_API.Model;
using DrinkFood_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DrinkFood_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : BaseController
    {
        [Inject] private readonly MenuService _menuService;

        public MenuController(IServiceProvider provider) : base()
        {
            provider.Inject(this);
        }

        /// <summary>
        /// 品牌菜單 (含不同區域菜單)
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpGet("GetBrandMenuList/{BrandID}")]
        public IActionResult GetBrandMenuList(Guid StoreID)
        {
            var Response = _menuService.GetBrandMenuList(StoreID);
            if (_menuService.SimpleResult.Success)
            {
                return Json(new ResponseData<object?>(Response, Response.Count));
            }
            else
            {
                return Json(new ResponseModel(_menuService.SimpleResult));
            }
        }

        /// <summary>
        /// 店家菜單
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpGet("GetStoreMenuList/{StoreID}")]
        public IActionResult GetStoreMenuList(Guid StoreID)
        {
            var Response = _menuService.GetStoreMenu(StoreID);
            if (_menuService.SimpleResult.Success)
            {
                return Json(new ResponseData<object?>(Response, 1));
            }
            else
            {
                return Json(new ResponseModel(_menuService.SimpleResult));
            }
        }
    }
}
