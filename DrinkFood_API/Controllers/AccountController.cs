using DrinkFood_API.Model;
using DrinkFood_API.Models;
using DrinkFood_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DrinkFood_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        [Inject] private readonly AccountService _accountService;

        public AccountController(IServiceProvider provider) : base()
        {
            provider.Inject(this);
        }

        /// <summary>
        /// 登入
        /// </summary>
        [ProducesResponseType(typeof(ResponseLoginModel), StatusCodes.Status200OK)]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] RequestLoginModel RequestData)
        {
            ResponseLoginModel Response = _accountService.Login(RequestData);
            return Json(new ResponseData<ResponseLoginModel>(Response, _accountService.SimpleResult.Count));
        }

        /// <summary>
        /// 取得使用者清單
        /// </summary>
        [ProducesResponseType(typeof(List<ViewAccount>), StatusCodes.Status200OK)]
        [HttpGet("GetAccountList")]
        public IActionResult GetAccountList()
        {
            var Response = _accountService.GetAccountList();
            return Json(new ResponseData<List<ViewAccount>>(Response, Response.Count));
        }

        /// <summary>
        /// 修改基本資料
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpPut("UpdateProfile/{AccountID}")]
        public IActionResult UpdateProfile(Guid AccountID, [FromBody] RequestUpdateProfileModel RequestData)
        {
            _accountService.UpdateProfile(AccountID, RequestData);
            return Json(new ResponseData<object?>(null));
        }

        /// <summary>
        /// 修改基本資料選單 (午餐付款方式、飲料付款方式)
        /// </summary>
        [ProducesResponseType(typeof(ResponseProfileDialogOptions), StatusCodes.Status200OK)]
        [HttpGet("GetProfileDialogOptions")]
        public IActionResult GetProfileDialogOptions()
        {
            var Response = _accountService.GetProfileDialogOptions();
            return Json(new ResponseData<ResponseProfileDialogOptions>(Response, 1));
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
