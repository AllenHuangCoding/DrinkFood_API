using CodeShare.Libs.BaseProject;
using DrinkFood_API.Models;
using DrinkFood_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DrinkFood_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : BaseController
    {
        [Inject] private readonly LoginService _loginService;

        public LoginController(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
            SetHttpContextItem(IApiLogService.Module, "Login");
        }

        /// <summary>
        /// 登入
        /// </summary>
        [ProducesResponseType(typeof(ResponseLoginModel), StatusCodes.Status200OK)]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] RequestLoginModel RequestData)
        {
            ResponseLoginModel Response = _loginService.Login(RequestData);
            return Json(new ResponseData<ResponseLoginModel>(Response, 1));
        }

    }
}
