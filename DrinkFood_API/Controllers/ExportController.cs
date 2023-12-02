using CodeShare.Libs.BaseProject;
using DrinkFood_API.Models;
using DrinkFood_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrinkFood_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController : CheckTokenController
    {
        [Inject] private readonly ExportService _exportService;

        public ExportController(IServiceProvider provider) : base(provider)
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
        [HttpGet("ExportMonthReport/{Month}")]
        public IActionResult ExportMonthReport(DateTime Month)
        {
            return _exportService.ExportMonthReport(new RequestMonthReportModel { 
                Month = Month
            });
        }
    }
}
