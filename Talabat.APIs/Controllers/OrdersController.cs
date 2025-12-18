using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Service.Contract;
using Address = Talabat.Core.Entities.Order_Aggregate.Address;

namespace Talabat.APIs.Controllers
{
   
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService,
                                IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Order), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderDto orderDto)
        {
            var address = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);
            var order = await _orderService.CreateOrderAsync(orderDto.BuyerEmail,orderDto.BasketId,
                                                 orderDto.DeliveryMethodId, address);

            if (order is null) return BadRequest(new ApiResponse(400));

            return Ok(order);
        }

        //public async Task<IActionResult> GetOrdersForUsers(string Email)
        //{
        //    var orders = await _orderService.GetOrdersForUsersAsync(Email);

        //    return Ok(orders);
        //}
    }
}
