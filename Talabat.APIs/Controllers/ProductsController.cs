using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities.Products;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Spicifications.Product_Spec;

namespace Talabat.APIs.Controllers
{
    
    public class ProductsController : BaseApiController
    {
        private readonly IGenaricRepositoriy<Product> _productRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenaricRepositoriy<Product> ProductRepo, IMapper mapper)
        {
            _productRepo = ProductRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductToReturnDto>>> GetProducts()
        {
            var spec = new ProductWithBrandAndCategorySpecifications();

            var Products = await _productRepo.GetAllWithSpecAsync(spec);

            return Ok(_mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDto>>(Products));
        }

        [HttpGet("id")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id) 
        {
            var spec = new ProductWithBrandAndCategorySpecifications(id);

            var Product = await _productRepo.GetWithSpecAsync(spec);

            if(Product is null)
                return NotFound();

            return Ok(_mapper.Map<Product,ProductToReturnDto>(Product));

        }
    }
}
