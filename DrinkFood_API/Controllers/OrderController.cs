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
        /// 自己開團的清單
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpGet("GetMyOrderList/{AccountID}")]
        public IActionResult GetMyOrderList(Guid AccountID, [FromQuery] RequestGetMyOrderListModel Request)
        {
            var Response = _orderService.GetMyOrderList(AccountID, Request);
            if (_orderService.SimpleResult.Success)
            {
                return Json(new ResponseData<object?>(Response, Response.Count));
            }
            else
            {
                return Json(new ResponseModel(_orderService.SimpleResult));
            }
        }

        /// <summary>
        /// 單筆清單詳細
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpGet("GetOrder/{OrderID}")]
        public IActionResult GetOrder(Guid OrderID)
        {
            var Response = _orderService.GetOrder(OrderID);
            if (_orderService.SimpleResult.Success)
            {
                return Json(new ResponseData<object?>(Response, 1));
            }
            else
            {
                return Json(new ResponseModel(_orderService.SimpleResult));
            }
        }

        /// <summary>
        /// 新增訂單
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpPost("PostOrder")]
        public IActionResult PostOrder([FromBody] RequestPostOrderModel Request)
        {
            _orderService.PostOrder(Request);
            if (_orderService.SimpleResult.Success)
            {
                return Json(new ResponseData<object?>(null, 1));
            }
            else
            {
                return Json(new ResponseModel(_orderService.SimpleResult));
            }
        }

        /// <summary>
        /// 修改訂單時間
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpPut("PutOrderTime/{OrderID}")]
        public IActionResult PutOrderTime(Guid OrderID, [FromBody] RequestPutOrderTimeModel Request)
        {
            _orderService.PutOrderTime(OrderID, Request);
            if (_orderService.SimpleResult.Success)
            {
                return Json(new ResponseData<object?>(null, 1));
            }
            else
            {
                return Json(new ResponseModel(_orderService.SimpleResult));
            }
        }

        /// <summary>
        /// 關閉訂單
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpPut("CloseOrder/{OrderID}")]
        public IActionResult CloseOrder(Guid OrderID)
        {
            _orderService.CloseOrder(OrderID);
            if (_orderService.SimpleResult.Success)
            {
                return Json(new ResponseData<object?>(null, 1));
            }
            else
            {
                return Json(new ResponseModel(_orderService.SimpleResult));
            }
        }

        /// <summary>
        /// 新增訂單品項
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpPost("PostOrderDetail")]
        public IActionResult PostOrderDetail([FromBody] RequestPostOrderDetailModel Request)
        {
            _orderService.PostOrderDetail(Request);
            if (_orderService.SimpleResult.Success)
            {
                return Json(new ResponseData<object?>(null, 1));
            }
            else
            {
                return Json(new ResponseModel(_orderService.SimpleResult));
            }
        }

        /// <summary>
        /// 刪除訂單品項
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpDelete("DeleteOrderDetail/{OrderDetailID}")]
        public IActionResult DeleteOrderDetail(Guid OrderDetailID)
        {
            _orderService.DeleteOrderDetail(OrderDetailID);
            if (_orderService.SimpleResult.Success)
            {
                return Json(new ResponseData<object?>(null, 1));
            }
            else
            {
                return Json(new ResponseModel(_orderService.SimpleResult));
            }
        }

        /// <summary>
        /// 更改付款方式
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpPut("PutPayment/{OrderDetailID}")]
        public IActionResult PutPayment(Guid OrderDetailID, [FromBody] RequestPutPaymentModel Request)
        {
            _orderService.PutPayment(OrderDetailID, Request);
            if (_orderService.SimpleResult.Success)
            {
                return Json(new ResponseData<object?>(null, 1));
            }
            else
            {
                return Json(new ResponseModel(_orderService.SimpleResult));
            }
        }

        /// <summary>
        /// 更改付款時間
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpPut("PutPaymentDateTime/{OrderDetailID}")]
        public IActionResult PutPaymentDateTime(Guid OrderDetailID, [FromBody] RequestPutPaymentDateTimeModel Request)
        {
            _orderService.PutPaymentDateTime(OrderDetailID, Request);
            if (_orderService.SimpleResult.Success)
            {
                return Json(new ResponseData<object?>(null, 1));
            }
            else
            {
                return Json(new ResponseModel(_orderService.SimpleResult));
            }
        }
    }
}
