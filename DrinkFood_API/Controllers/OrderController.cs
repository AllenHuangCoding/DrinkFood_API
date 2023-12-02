using CodeShare.Libs.BaseProject;
using DrinkFood_API.Models;
using DrinkFood_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DrinkFood_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : CheckTokenController
    {
        [Inject] private readonly OrderService _orderService;

        public OrderController(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }

        /// <summary>
        /// 歷史紀錄
        /// </summary>
        [ProducesResponseType(typeof(List<ViewDetailHistory>), StatusCodes.Status200OK)]
        [HttpGet("GetOrderDetailHistory/{AccountID}")]
        public IActionResult GetOrderDetailHistory(Guid AccountID)
        {
            List<ViewDetailHistory> Response = _orderService.GetOrderDetailHistory(AccountID);
            return Json(new ResponseData<List<ViewDetailHistory>>(Response, Response.Count));
        }

        /// <summary>
        /// 訂單清單
        /// </summary>
        [ProducesResponseType(typeof(List<OrderListModel>), StatusCodes.Status200OK)]
        [HttpGet("GetOrderList")]
        public IActionResult GetOrderList()
        {
            List<OrderListModel> Response = _orderService.GetOrderList();
            return Json(new ResponseData<List<OrderListModel>>(Response, Response.Count));
        }

        /// <summary>
        /// 單筆清單詳細
        /// </summary>
        [ProducesResponseType(typeof(ViewOrderAndDetail), StatusCodes.Status200OK)]
        [HttpGet("GetOrder/{OrderID}")]
        public IActionResult GetOrder(Guid OrderID)
        {
            ViewOrderAndDetail Response = _orderService.GetOrder(OrderID);
            return Json(new ResponseData<ViewOrderAndDetail>(Response, 1));
        }

        /// <summary>
        /// 新增訂單視窗的下拉選單
        /// </summary>
        [ProducesResponseType(typeof(ResponseOrderDialogOptions), StatusCodes.Status200OK)]
        [HttpGet("GetCreateOrderDialogOptions")]
        public IActionResult GetCreateOrderDialogOptions([FromQuery] Guid? TypeID)
        {
            ResponseOrderDialogOptions response = _orderService.GetCreateOrderDialogOptions(TypeID);
            return Json(new ResponseData<ResponseOrderDialogOptions>(response, 1));
        }

        /// <summary>
        /// 新增訂單
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpPost("PostOrder")]
        public IActionResult PostOrder([FromBody] RequestPostOrderModel RequestData)
        {
            _orderService.PostOrder(RequestData);
            return Json(new ResponseData<object?>(null, 1));
        }

        /// <summary>
        /// 加入訂單
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpPost("JoinOrder/{OrderID}")]
        public IActionResult JoinOrder(Guid OrderID)
        {
            _orderService.JoinOrder(OrderID);
            return Json(new ResponseData<object?>(null, 1));
        }

        /// <summary>
        /// 修改用餐時間
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpPut("PutArrivalTime/{OrderID}")]
        public IActionResult PutArrivalTime(Guid OrderID, [FromBody] RequestPutArrivalTimeModel RequestData)
        {
            _orderService.PutArrivalTime(OrderID, RequestData);
            return Json(new ResponseData<object?>(null, 1));
        }

        /// <summary>
        /// 修改結單時間
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpPut("PutCloseTime/{OrderID}")]
        public IActionResult PutCloseTime(Guid OrderID, [FromBody] RequestPutCloseTimeModel RequestData)
        {
            _orderService.PutCloseTime(OrderID, RequestData);
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
        public IActionResult PostOrderDetail([FromBody] RequestPostOrderDetailModel RequestData)
        {
            _orderService.PostOrderDetail(RequestData);
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
        public IActionResult PutPayment(Guid OrderDetailID, [FromBody] RequestPutPaymentModel RequestData)
        {
            _orderService.PutPayment(OrderDetailID, RequestData);
            return Json(new ResponseData<object?>(null, 1));
        }

        /// <summary>
        /// 更改付款時間
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpPut("PutPaymentDateTime/{OrderDetailID}")]
        public IActionResult PutPaymentDateTime(Guid OrderDetailID, [FromBody] RequestPutPaymentDateTimeModel RequestData)
        {
            _orderService.PutPaymentDateTime(OrderDetailID, RequestData);
            return Json(new ResponseData<object?>(null, 1));
        }
    }
}
