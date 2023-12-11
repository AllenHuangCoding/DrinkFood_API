using CodeShare.Libs.BaseProject;
using DrinkFood_API.Models;
using DrinkFood_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DrinkFood_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : CheckTokenController
    {
        [Inject] private readonly HomeService _homeService;

        public HomeController(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
            SetHttpContextItem(IApiLogService.Module, "Home");
        }

        /// <summary>
        /// 首頁_開團資訊
        /// </summary>
        [ProducesResponseType(typeof(ResponseInfoCardModel), StatusCodes.Status200OK)]
        [HttpGet("GetInfoCard")]
        public IActionResult GetInfoCard()
        {
            var Response = _homeService.GetInfoCard();
            return Json(new ResponseData<ResponseInfoCardModel>(Response, 1));
        }

        /// <summary>
        /// 首頁_今日點餐
        /// </summary>
        [ProducesResponseType(typeof(List<ResponseTodayOrderModel>), StatusCodes.Status200OK)]
        [HttpGet("GetTodayOrder")]
        public IActionResult GetTodayOrder()
        {
            var Response = _homeService.GetTodayOrder();
            return Json(new ResponseData<List<ResponseTodayOrderModel>>(Response, Response.Count));
        }
    }
}
