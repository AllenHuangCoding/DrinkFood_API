using System.Net.Mime;
using System.Text;

namespace CodeShare.Libs.BaseProject
{
    public class BaseMiddleware
    {
        private readonly RequestDelegate _next;

        public BaseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            // 只記錄 json
            if (httpContext.Request.ContentType == MediaTypeNames.Application.Json)
            {
                // 確保 HTTP Request 可以多次讀取
                httpContext.Request.EnableBuffering();

                try
                {
                    // 讀取 HTTP Request Body 內容
                    // 注意！要設定 leaveOpen 屬性為 true 使 StreamReader 關閉時，HTTP Request 的 Stream 不會跟著關閉
                    using (var bodyReader = new StreamReader(stream: httpContext.Request.Body,
                                                              encoding: Encoding.UTF8,
                                                              detectEncodingFromByteOrderMarks: false,
                                                              bufferSize: 1024,
                                                              leaveOpen: true))
                    {
                        string body = bodyReader.ReadToEndAsync().GetAwaiter().GetResult();

                        httpContext.Items.Add(IApiLogService.Body, body);
                    }

                    // 將 HTTP Request 的 Stream 起始位置歸零
                    httpContext.Request.Body.Position = 0;
                }
                catch (Exception)
                {
                    // exception 不紀錄
                }
            }

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class BaseMiddlewareExtensions
    {
        public static IApplicationBuilder UseBaseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BaseMiddleware>();
        }
    }
}
