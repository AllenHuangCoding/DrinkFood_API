using CodeShare.Libs.BaseProject;
using DrinkFood_API.Models;
using DrinkFood_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DrinkFood_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : CheckTokenController
    {
        [Inject] private readonly AccountService _accountService;

        public AccountController(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
            SetHttpContextItem(IApiLogService.Module, "Account");
        }

        /// <summary>
        /// 查詢個人資料
        /// </summary>
        [ProducesResponseType(typeof(ViewAccount), StatusCodes.Status200OK)]
        [HttpGet("GetProfile")]
        public IActionResult GetProfile()
        {
            var Response = _accountService.GetProfile();
            return Json(new ResponseData<ViewAccount>(Response, 1));
        }

        /// <summary>
        /// 修改個人資料
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpPut("UpdateProfile/{AccountID}")]
        public IActionResult UpdateProfile(Guid AccountID, [FromBody] RequestUpdateProfileModel RequestData)
        {
            _accountService.UpdateProfile(AccountID, RequestData);
            return Json(new ResponseData<object?>(null));
        }

        /// <summary>
        /// 午餐與飲料付款方式選單
        /// </summary>
        [ProducesResponseType(typeof(ResponseProfileDialogOptions), StatusCodes.Status200OK)]
        [HttpGet("GetProfileDialogOptions")]
        public IActionResult GetProfileDialogOptions()
        {
            var Response = _accountService.GetProfileDialogOptions();
            return Json(new ResponseData<ResponseProfileDialogOptions>(Response, 1));
        }

        /// <summary>
        /// 綁定Line
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpPut("BindLine/{AccountID}")]
        public IActionResult BindLine(Guid AccountID, [FromBody] RequestBindLineModel RequestData)
        {
            _accountService.BindLine(AccountID, RequestData);
            return Json(new ResponseData<object?>(null));
        }

        /// <summary>
        /// 解除綁定Line
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpPut("UnbindLine/{AccountID}")]
        public IActionResult UnbindLine(Guid AccountID)
        {
            _accountService.UnbindLine(AccountID);
            return Json(new ResponseData<object?>(null));
        }


        /// <summary>
        /// 使用者清單
        /// </summary>
        [ProducesResponseType(typeof(List<ViewAccount>), StatusCodes.Status200OK)]
        [HttpGet("GetAccountList")]
        public IActionResult GetAccountList()
        {
            var Response = _accountService.GetAccountList();
            return Json(new ResponseData<List<ViewAccount>>(Response, Response.Count));
        }

        /// <summary>
        /// 新增使用者
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpPost("CreateAccount")]
        public IActionResult CreateAccount([FromBody] RequestCreateAccountModel RequestData)
        {
            _accountService.CreateAccount(RequestData);
            return Json(new ResponseData<object?>(null));
        }
    }
}
