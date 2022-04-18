using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopCSharp_API.Data;
using ShopCSharp_API.Models;

namespace ShopCSharp_API.Controllers
{
  [Route("v1/products")]
  public class ProductController : Controller
  {
    [HttpGet]
    [Route("")]
    [AllowAnonymous]
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
    [AllowAnonymous]
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
    [Route("{categories/{id:int}")] // Ele entende que isso é um parâmetro inteiro
    [AllowAnonymous]
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
    [Authorize(Roles = "employee")]
    public async Task<ActionResult<Product>> Post(
    [FromBody] Product model,
    [FromServices] DataContext context)
    {
      if (ModelState.IsValid)
      {
        context.Products.Add(model);
        await context.SaveChangesAsync();
        return model;
      }
      else
      {
        return BadRequest(ModelState);
      }
    }
  }
}