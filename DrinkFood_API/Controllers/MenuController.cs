using CodeShare.Libs.BaseProject;
using DataBase.View;
using DrinkFood_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DrinkFood_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : CheckTokenController
    {
        [Inject] private readonly MenuService _menuService;

        public MenuController(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }

        /// <summary>
        /// 品牌菜單 (含不同區域菜單)
        /// </summary>
        [ProducesResponseType(typeof(List<ViewMenu>), StatusCodes.Status200OK)]
        [HttpGet("GetBrandMenuList/{BrandID}")]
        public IActionResult GetBrandMenuList(Guid StoreID)
        {
            List<ViewMenu> Response = _menuService.GetBrandMenuList(StoreID);
            return Json(new ResponseData<List<ViewMenu>>(Response, Response.Count));
        }

        /// <summary>
        /// 店家菜單
        /// </summary>
        [ProducesResponseType(typeof(ViewMenu), StatusCodes.Status200OK)]
        [HttpGet("GetStoreMenuList/{StoreID}")]
        public IActionResult GetStoreMenuList(Guid StoreID)
        {
            ViewMenu Response = _menuService.GetStoreMenu(StoreID);
            return Json(new ResponseData<ViewMenu>(Response, 1));
        }
    }
}
