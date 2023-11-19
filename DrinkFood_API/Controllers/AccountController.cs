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
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpGet("Login")]
        public IActionResult Login([FromQuery] RequestLoginModel RequestData)
        {
            var Response = _accountService.Login(RequestData);
            return Json(new ResponseData<object?>(Response, _accountService.SimpleResult.Count));
        }

        /// <summary>
        /// 登入
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
