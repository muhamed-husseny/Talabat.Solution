using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Entities.Products;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Service.Contract;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IGenaricRepositoriy<Product> _productrepo;
        private readonly IGenaricRepositoriy<DeliveryMethod> _deliveryrepo;
        private readonly IGenaricRepositoriy<Order> _ordersrepo;

        public OrderService(
            IBasketRepository basketRepository,
            IGenaricRepositoriy<Product> productrepo,
            IGenaricRepositoriy<DeliveryMethod> deliveryrepo,
            IGenaricRepositoriy<Order> ordersrepo)
        {
            _basketRepository = basketRepository;
            _productrepo = productrepo;
            _deliveryrepo = deliveryrepo;
            _ordersrepo = ordersrepo;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            // 1. Get Basket From Basket Repo.
            var basket = await _basketRepository.GetBasketAsync(basketId);

            // 2. Get Selected Items at Basket From Products Repo.

            var orderItems = new List<OrderItem>();

            if(basket?.Items?.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _productrepo.GetAsync(item.Id);

                    var productItemOrdered = new ProductItemOrdered(item.Id, product.Name, product.PictureUrl);
                    
                    var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);

                    orderItems.Add(orderItem);
                }
            }

            // 3. Calac SubTotal.

            var subTotal = orderItems.Sum(orderItems => orderItems.Price * orderItems.Quantity);

            // 4. Get Delivery Method From DeliveryMethod Repo.

            var deliveryMethod = await _deliveryrepo.GetAsync(deliveryMethodId);

            // 5. Create Order.

            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subTotal);

            await _ordersrepo.AddAsync(order);

            return order;
            // 6. Save To Database [TODO]
        }

        public Task<Order> GetOrderByIdAsync(int orderId, string buyerEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Order>> GetOrdersForUsersAsync(string buyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}
