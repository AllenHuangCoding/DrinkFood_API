using DrinkFood_API.Model;
    using DrinkFood_API.Models;
using DrinkFood_API.Services;
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
        [ProducesResponseType(typeof(List<GroupDrinkFoodModel>), StatusCodes.Status200OK)]
        [HttpGet("GetDrinkFoodList/{StoreID}")]
        public IActionResult GetDrinkFoodList(Guid StoreID)
        {
            List<GroupDrinkFoodModel> Response = _drinkFoodService.GetDrinkFoodList(StoreID);
            return Json(new ResponseData<List<GroupDrinkFoodModel>>(Response, Response.Count));
        }

    }
}
