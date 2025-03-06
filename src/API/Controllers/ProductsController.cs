using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductsController(IUnitOfWork unitOfWork) : BaseApiController
{
  [HttpGet]
   public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(
     [FromQuery]ProductSpecParams specParams)
   {
        var spec = new ProductSpecification(specParams);

        return await CreatePagedResult(unitOfWork.Repository<Product>(), spec, specParams.PageIndex,
          specParams.PageSize);
   }

   [HttpGet("{id:int}")]  // api/products/2
   public async Task<ActionResult<Product>> GetProduct(int id)
   {
        var product = await unitOfWork.Repository<Product>().GetByIdAsync(id);
        if (product == null) return NotFound();
        return product;
   }

   [HttpPost]
   public async Task<ActionResult<Product>> CreateProduct(Product product)
   {
        unitOfWork.Repository<Product>().Add(product);
        if(await unitOfWork.Complete())
        {
          return CreatedAtAction("GetProduct", new {id = product.Id}, product);
        }
        return BadRequest("Problem creating product");
   }

   [HttpPut("{id:int}")]
   public async Task<ActionResult> UpdateProduct(int id, Product product)
   {
          if(product.Id != id || !ProductExits(id)) 
               return BadRequest("Cannot update this product");

          unitOfWork.Repository<Product>().Update(product);
          if(await unitOfWork.Complete())
          {
               return NoContent();
          }
          return BadRequest("Problem update the product");
   }

   [HttpDelete("{id:int}")]
   public async Task<ActionResult> DeleteProduct(int id)
   {
          var product = await unitOfWork.Repository<Product>().GetByIdAsync(id);
          if(product == null) return NotFound();
          unitOfWork.Repository<Product>().Remove(product);
          if(await unitOfWork.Complete())
          {
               return NoContent();
          }
          return BadRequest("Problem deleting the product");
   }

   [HttpGet("brands")]
   public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
   {
       var spec = new BrandListSpecification();
       return Ok(await unitOfWork.Repository<Product>().ListAsync(spec));
   }  

   [HttpGet("types")]
   public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
   {
        var spec = new TypeListSpecification();
        return Ok(await unitOfWork.Repository<Product>().ListAsync(spec));
   } 
   private bool ProductExits(int id)
   {
     return unitOfWork.Repository<Product>().Exists(id);
   }
}
