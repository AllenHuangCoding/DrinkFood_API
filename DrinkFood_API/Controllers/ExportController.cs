using DrinkFood_API.Model;
using DrinkFood_API.Models;
using DrinkFood_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrinkFood_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController : BaseController
    {
        [Inject] private readonly ExportService _exportService;

        public ExportController(IServiceProvider provider) : base()
        {
            provider.Inject(this);
        }

        /// <summary>
        /// 匯出歷史紀錄
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpGet("ExportOrderDetailHistory/{AccountID}")]
        public IActionResult ExportOrderDetailHistory(Guid AccountID)
        {
            return _exportService.ExportOrderDetailHistory(AccountID);
        }


        /// <summary>
        /// 匯出扣款統計
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpGet("ExportMonthReport")]
        public IActionResult ExportMonthReport([FromBody] RequestMonthReportModel RequestData)
        {
            var Response = _exportService.ExportMonthReport(RequestData);
            return Json(new ResponseData<object?>(Response, Response.Count));
        }
    }
}
