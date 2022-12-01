using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregates;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IBasketRepository basketRepo, IUnitOfWork unitOfWork)
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {
            var basket = await _basketRepo.GetBasketAsync(basketId);
            var items = new List<OrderItem>();
            foreach(var item in basket.Items)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered, item.Quantity, productItem.Price);
                items.Add(orderItem);
            }
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            var subtotal = items.Sum(items => items.Price * items.Quantity);
            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, items, subtotal);

            _unitOfWork.Repository<Order>().Add(order);
            var result = await _unitOfWork.Complete();

            if(result <= 0) return null;

            await _basketRepo.DeleteBasketAsync(basketId);
            
            return order;        
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrdersWithIyemsAndOrderingSpec(id, buyerEmail);

            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUsersAsync(string buyerEmail)
        {
            var spec = new OrdersWithIyemsAndOrderingSpec(buyerEmail);

            var orders = await _unitOfWork.Repository<Order>().ListAsync(spec);
            return orders;
        }
    }
}