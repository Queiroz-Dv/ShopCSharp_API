using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopCSharp_API.Data;
using ShopCSharp_API.Models;

namespace ShopCSharp_API.Controllers
{
  [Route("products")]
  public class ProductController : ControllerBase
  {
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
    {
      var products = await context
      .Products
      .Include(x => x.Category) // Faz select com join
      .AsNoTracking()
      .ToListAsync();
      return products;
    }

    [HttpGet]
    [Route("{id:int}")] // Ele entende que isso é um parâmetro inteiro
    public async Task<ActionResult<Product>> GetById(int id,
   [FromServices] DataContext context)
    {
      var product = await context
      .Products
      .Include(x => x.Category)
      .AsNoTracking()
      .FirstOrDefaultAsync(x => x.Id == id);
      return product;
    }

    [HttpGet]
    [Route("{categories/id:int}")] // Ele entende que isso é um parâmetro inteiro
    public async Task<ActionResult<List<Product>>> GetByCategory([FromServices] DataContext context, int id)
    {
      var products = await context
      .Products
      .Include(x => x.Category)
      .AsNoTracking()
      .Where(x => x.CategoryId == id)
      .ToListAsync(); // Sempre no final
      return products;
    }

    [HttpPost]
    [Route("")]
    public async Task<ActionResult<List<Product>>> Post(
    [FromBody] Product model,
    [FromServices] DataContext context)
    {
      if (!ModelState.IsValid) // Model State verifica se o que está no model é válido
      {
        return BadRequest(ModelState);
      }

      try
      {
        context.Products.Add(model);
        await context.SaveChangesAsync();
        return Ok(model);
      }
      catch
      {
        return BadRequest(new { message = "Categoria não encontrada" });
      }
    }
  }
}