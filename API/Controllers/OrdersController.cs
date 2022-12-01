using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using API.DTOs;
using API.Errors;
using AutoMapper;
using Core.Entities.OrderAggregates;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _mapper = mapper;
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDTO orderDTO)
        {
            // var orderDTOs = JsonSerializer.Deserialize<OrderDTO>(orderDTO);
            var email = User.FindFirstValue(ClaimTypes.Email);
            var address = _mapper.Map<AddressDTO, Address>(orderDTO.ShipToAddress);
            var order = await _orderService.CreateOrderAsync(email, orderDTO.DeliveryMethodId, orderDTO.BasketId, address);

            if(order == null) return BadRequest(new ApiResponse(400, "Problem Creating the order"));

            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDTO>>> GetOrdersForUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderService.GetOrdersForUsersAsync(email);

            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDTO>>(orders));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDTO>> GetOrderByIdForUser(int id)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderByIdAsync(id, email);

            if(order == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Order, OrderToReturnDTO>(order));;
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliverMethods()
        {
            var deliveryMethods = await _orderService.GetDeliveryMethodsAsync();

            return Ok(deliveryMethods);
        }
    }
}