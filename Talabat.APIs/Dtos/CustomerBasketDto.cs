using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities.Products;

namespace Talabat.APIs.Dtos
{
    public class CustomerBasketDto
    {
        [Required]
        public int Id { get; set; }

        public List<BasketItemDto> Items { get; set; }
    }
}
