using KSE.Core.Mediatr;
using KSE.Order.Application.Commands;
using KSE.Order.Application.Querys.Interfaces;
using KSE.WebApi.Core.Controllers;
using KSE.WebApi.Core.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace KSE.Order.V1.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("v{version:ApiVersion}/Order")]
    public class OrderController : MainController
    {
        private readonly IMediatrHandler _mediatr;
        private readonly IAspNetUser _aspNetUser;
        private readonly IOrderQuery _orderQuery;

        public OrderController(IMediatrHandler mediatr, IAspNetUser aspNetUser, IOrderQuery orderQuery)
        {
            _mediatr = mediatr;
            _aspNetUser = aspNetUser;
            _orderQuery = orderQuery;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreatedOrderCommand order)
        {
            order.ClientId = _aspNetUser.UserId;
            return CustomResponse(await _mediatr.SendCommand(order));
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(Guid orderId)
        {
            var order = await _orderQuery.GetOrderId(orderId);

            return order == null ? NotFound() : CustomResponse(order);
        }

        [HttpGet("last-order")]
        public async Task<IActionResult> LastOrder()
        {
            var order = await _orderQuery.GetLastOrder(_aspNetUser.UserId);

            return order == null ? NotFound() : CustomResponse(order);
        }

        [HttpGet("list-order")]
        public async Task<IActionResult> GetAllOrder()
        {
            var order = await _orderQuery.GetFindAll(_aspNetUser.UserId);

            return order == null ? NotFound() : CustomResponse(order);
        }
    }
}
