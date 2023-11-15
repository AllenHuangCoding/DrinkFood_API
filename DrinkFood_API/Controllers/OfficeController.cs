using DrinkFood_API.Model;
using DrinkFood_API.Models;
using DrinkFood_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DrinkFood_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeController : BaseController
    {
        [Inject] private readonly OfficeService _officeService;

        public OfficeController(IServiceProvider provider) : base()
        {
            provider.Inject(this);
        }

        /// <summary>
        /// 辦公室地點清單
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpGet("GetOfficeList")]
        public IActionResult GetOfficeList([FromQuery] RequestOfficeListModel RequestData)
        {
            var Response = _officeService.GetOfficeList(RequestData);
            return Json(new ResponseData<object?>(Response, Response.Count));
        }

        /// <summary>
        /// 辦公室成員清單
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpGet("GetOfficeMemberList/{OfficeID}")]
        public IActionResult GetOfficeMemberList(Guid OfficeID)
        {
            var Response = _officeService.GetOfficeMemberList(OfficeID);
            return Json(new ResponseData<object?>(Response, Response.Count));
        }

        /// <summary>
        /// 加入辦公室成員
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpPost("PostOfficeMember/{OfficeID}")]
        public IActionResult PostOfficeMember(Guid OfficeID, [FromForm] RequestPostOfficeMemberModel RequestData)
        {
            _officeService.PostOfficeMember(OfficeID, RequestData);
            return Json(new ResponseData<object?>(null, 0));
        }


        /// <summary>
        /// 刪除辦公室成員
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpDelete("DeleteOfficeMember")]
        public IActionResult DeleteOfficeMember([FromForm] RequestDeleteOfficeMemberModel RequestData)
        {
            _officeService.DeleteOfficeMember(RequestData);
            return Json(new ResponseData<object?>(null, 0));
        }
    }
}
