using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopCSharp_API.Data;
using ShopCSharp_API.Models;

// Endpoint é uma URL
// https://localhost:5001/categories/ //
namespace ShopCSharp_API.Controllers
{
  [Route("v1/categories")]
  public class CategoryController : Controller
  {
    [HttpGet]
    [Route("")]
    [AllowAnonymous]
    [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 30)]
    public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
    {
      var categories = await context.Categories.AsNoTracking().ToListAsync();
      return Ok(categories);
    }

    [HttpGet]
    [Route("{id:int}")] // Ele entende que isso é um parâmetro inteiro
    [AllowAnonymous]
    public async Task<ActionResult<Category>> GetById(int id,
    [FromServices] DataContext context)
    {
      var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
      return category;
    }

    [HttpPost]
    [Route("")]
    //[Authorize(Roles = "employee")]
    [AllowAnonymous]
    public async Task<ActionResult<Category>> Post
    ([FromBody] Category model, [FromServices] DataContext context)
    {
      if (!ModelState.IsValid) // Model State verifica se o que está no model é válido
      {
        return BadRequest(ModelState);
      }

      try
      {
        context.Categories.Add(model);
        await context.SaveChangesAsync();
        return model;
      }
      catch (Exception)
      {
        return BadRequest(new { message = "Não foi possível criar a categoria" });
      }
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = "employee")]
    public async Task<ActionResult<Category>> Put
    (int id,
    [FromBody] Category model,
    [FromServices] DataContext context)
    {
      // Verifica se o Id informado é o mesmo do modelo
      if (id != model.Id)
      {
        return NotFound(new { message = "Categoria não encontrada" });
      }

      // Verifica os dados
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      try
      {
        context.Entry<Category>(model).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return model;
      }
      catch (DbUpdateConcurrencyException)
      {
        return BadRequest(new { message = "Não foi possível atualizar a categoria" });
      }
    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = "employee")]
    public async Task<ActionResult<Category>> Delete(
      int id,
      [FromServices] DataContext context)
    {
      var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
      if (category == null)
      {
        return NotFound(new { message = "Categoria não encontrada" });
      }

      try
      {
        context.Categories.Remove(category);
        await context.SaveChangesAsync();
        return category;
      }
      catch (Exception)
      {
        return BadRequest(new { message = "Não foi possível remover a categoria" });
      }
    }
  }
}