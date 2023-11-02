using DrinkFood_API.Exceptions;
using DrinkFood_API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DrinkFood_API.Filters
{
    /// <summary>
    /// 例外處理
    /// </summary>
    public class HandleExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (typeof(ApiException) == context.Exception.GetType())
            {
                context.Result = ApiExceptionResponse((ApiException)context.Exception);
            }
            else
            {
                context.Result = ExceptionResponse(context);
            }
        }

        /// <summary>
        /// 回傳 API 例外回應
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected JsonResult ApiExceptionResponse(ApiException ex)
        {
            return new JsonResult(new ResponseModel(false, ex.ErrorCode, ex.Message));
        }

        /// <summary>
        /// 回傳例外回應
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected JsonResult ExceptionResponse(ExceptionContext context)
        {
            //line錯誤異常警告
            return new JsonResult(new ResponseModel(false, 500, context.Exception.Message));
        }
    }
}
