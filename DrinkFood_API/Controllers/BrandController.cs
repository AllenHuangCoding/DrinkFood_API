using DrinkFood_API.Model;
using DrinkFood_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DrinkFood_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : BaseController
    {
        [Inject] private readonly BrandService _brandService;

        public BrandController(IServiceProvider provider) : base()
        {
            provider.Inject(this);
        }

        /// <summary>
        /// 品牌下拉選單
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpGet("BrandOptions")]
        public IActionResult BrandOptions()
        {
            var Response = _brandService.BrandOptions();
            return Json(new ResponseData<object?>(Response, Response.Count));
        }

    }
}
