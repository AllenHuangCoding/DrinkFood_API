using DrinkFood_API.Model;
using DrinkFood_API.Models;
using DrinkFood_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DrinkFood_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseController
    {
        [Inject] private readonly OrderService _orderService;

        public OrderController(IServiceProvider provider) : base()
        {
            provider.Inject(this);
        }

        /// <summary>
        /// 首頁歷史紀錄
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpGet("GetHomeOrderDetailHistory/{AccountID}/{OfficeID}")]
        public IActionResult GetHomeOrderDetailHistory(Guid AccountID, Guid OfficeID)
        {
            var Response = _orderService.GetHomeOrderDetailHistory(AccountID, OfficeID);
            return Json(new ResponseData<object?>(Response, Response.Count));
        }

        /// <summary>
        /// 歷史紀錄
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpGet("GetOrderDetailHistory/{AccountID}")]
        public IActionResult GetOrderDetailHistory(Guid AccountID)
        {
            var Response = _orderService.GetOrderDetailHistory(AccountID);
            return Json(new ResponseData<object?>(Response, Response.Count));
        }

        /// <summary>
        /// 訂單清單
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpGet("GetOrderList")]
        public IActionResult GetOrderList()
        {
            var Response = _orderService.GetOrderList();
            return Json(new ResponseData<object?>(Response, Response.Count));
        }

        /// <summary>
        /// 自己開團的清單
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpGet("GetMyOrderList/{AccountID}")]
        public IActionResult GetMyOrderList(Guid AccountID, [FromQuery] RequestGetMyOrderListModel Request)
        {
            var Response = _orderService.GetMyOrderList(AccountID, Request);
            return Json(new ResponseData<object?>(Response, Response.Count));
        }

        /// <summary>
        /// 單筆清單詳細
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpGet("GetOrder/{OrderID}")]
        public IActionResult GetOrder(Guid OrderID)
        {
            var Response = _orderService.GetOrder(OrderID);
            return Json(new ResponseData<object?>(Response, 1));
        }

        /// <summary>
        /// 新增訂單
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpPost("PostOrder")]
        public IActionResult PostOrder([FromBody] RequestPostOrderModel Request)
        {
            _orderService.PostOrder(Request);
            return Json(new ResponseData<object?>(null, 1));
        }

        /// <summary>
        /// 修改訂單時間
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpPut("PutOrderTime/{OrderID}")]
        public IActionResult PutOrderTime(Guid OrderID, [FromBody] RequestPutOrderTimeModel Request)
        {
            _orderService.PutOrderTime(OrderID, Request);
            return Json(new ResponseData<object?>(null, 1));
        }

        /// <summary>
        /// 關閉訂單
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpPut("CloseOrder/{OrderID}")]
        public IActionResult CloseOrder(Guid OrderID)
        {
            _orderService.CloseOrder(OrderID);
            return Json(new ResponseData<object?>(null, 1));
        }

        /// <summary>
        /// 新增訂單品項
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpPost("PostOrderDetail")]
        public IActionResult PostOrderDetail([FromBody] RequestPostOrderDetailModel Request)
        {
            _orderService.PostOrderDetail(Request);
            return Json(new ResponseData<object?>(null, 1));
        }

        /// <summary>
        /// 刪除訂單品項
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpDelete("DeleteOrderDetail/{OrderDetailID}")]
        public IActionResult DeleteOrderDetail(Guid OrderDetailID)
        {
            _orderService.DeleteOrderDetail(OrderDetailID);
            return Json(new ResponseData<object?>(null, 1));
        }

        /// <summary>
        /// 更改付款方式
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpPut("PutPayment/{OrderDetailID}")]
        public IActionResult PutPayment(Guid OrderDetailID, [FromBody] RequestPutPaymentModel Request)
        {
            _orderService.PutPayment(OrderDetailID, Request);
            return Json(new ResponseData<object?>(null, 1));
        }

        /// <summary>
        /// 更改付款時間
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpPut("PutPaymentDateTime/{OrderDetailID}")]
        public IActionResult PutPaymentDateTime(Guid OrderDetailID, [FromBody] RequestPutPaymentDateTimeModel Request)
        {
            _orderService.PutPaymentDateTime(OrderDetailID, Request);
            return Json(new ResponseData<object?>(null, 1));
        }
    }
}
