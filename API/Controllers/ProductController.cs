using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseApiController
    {

        private readonly IGenericRepository<ProductBrand> _brandRepo;
        private readonly IGenericRepository<ProductType> _typeRepo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;

        public ProductController(IGenericRepository<ProductBrand> brandRepo, IGenericRepository<ProductType> typeRepo, IGenericRepository<Product> productRepo, IMapper mapper)
        {
            _brandRepo = brandRepo;
            _typeRepo = typeRepo;
            _productRepo = productRepo;
            _mapper = mapper;
        }

        [HttpGet("products")]
        public async Task<ActionResult<List<ProductDto>>> GetProducts()
        {
            //var products = await _productRepo.GetAllAsync();
            var spec = new ProductWithSpecifications();
            var products = await _productRepo.ListAsyncWithSpecifications(spec);
            /*return products.Select(product => new ProductDto() 
            {
                Id = product.Id,
                Description = product.Description,
                Name = product.Name,
                PictureUrl = product.PictureUrl,
                Price = product.Price,
                ProductType = product.ProductType.Name,
                ProductBrand = product.ProductBrand.Name
            }).ToList();*/
            return Ok(_mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductDto>>(products));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            //var product = await _productRepo.GetByIdAsync(id);
            var spec = new ProductWithSpecifications(id);
            var product = await _productRepo.GetEntityWithSpecifications(spec);
            /*return new ProductDto()
            {
                Id = product.Id,
                Description = product.Description,
                Name = product.Name,
                PictureUrl = product.PictureUrl,
                Price = product.Price,
                ProductType = product.ProductType.Name,
                ProductBrand = product.ProductBrand.Name
            };*/
            return _mapper.Map<Product, ProductDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductsBrands()
        {
            return Ok(await _brandRepo.GetAllAsync());
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductsTypes()
        {
            return Ok(await _typeRepo.GetAllAsync());
        }
        


    }


}
