using DrinkFood_API.Model;
using DrinkFood_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrinkFood_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrinkFoodController : BaseController
    {
        [Inject] private readonly DrinkFoodService _drinkFoodService;

        public DrinkFoodController(IServiceProvider provider) : base()
        {
            provider.Inject(this);
        }

        /// <summary>
        /// 取得店家品項
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpGet("GetDrinkFoodList/{StoreID}")]
        public IActionResult GetDrinkFoodList(Guid StoreID)
        {
            var Response = _drinkFoodService.GetDrinkFoodList(StoreID);
            if (_drinkFoodService.SimpleResult.Success)
            {
                return Json(new ResponseData<object?>(Response, Response.Count));
            }
            else
            {
                return Json(new ResponseModel(_drinkFoodService.SimpleResult));
            }
        }

    }
}
