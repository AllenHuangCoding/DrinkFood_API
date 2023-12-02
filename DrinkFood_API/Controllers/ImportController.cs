using CodeShare.Libs.BaseProject;
using DrinkFood_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DrinkFood_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : CheckTokenController
    {
        [Inject] private readonly ImportService _importService;

        public ImportController(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }

        /// <summary>
        /// 讀取品牌店家內容
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpPost("ReadBrandStoreExcel")]
        public IActionResult ReadBrandStoreExcel(IFormFile File)
        {
            _importService.ReadBrandStoreExcel(File);
            return Ok();
        }


        /// <summary>
        /// 讀取食物品項內容
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpPost("ReadDrinkFoodExcel/{MenuID}")]
        public IActionResult ReadDrinkFoodExcel(Guid MenuID, IFormFile File)
        {
            _importService.ReadDrinkFoodExcel(new Guid("5471FA67-70FC-48B5-908F-15430112BE36"), File);
            return Ok();
        }


        /// <summary>
        /// 讀取訂單內容
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpPost("ReadOrderDetailExcel/{OrderID}")]
        public IActionResult ReadOrderDetailExcel(Guid OrderID, IFormFile File)
        {
            _importService.ReadOrderDetailExcel(new Guid("5471FA67-70FC-48B5-908F-15430112BE36"), File);
            return Ok();
        }
    }
}
