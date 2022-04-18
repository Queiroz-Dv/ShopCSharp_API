using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopCSharp_API.Data;
using ShopCSharp_API.Models;
using ShopCSharp_API.Services;

namespace ShopCSharp_API.Controllers
{
  [Route("users")]
  public class UserController : ControllerBase
  {
    [HttpGet]
    [Route("")]
    [Authorize(Roles = "manager")]
    public async Task<ActionResult<List<User>>> Get([FromServices] DataContext context)
    {
      var users = await context
            .Users
            .AsNoTracking()
            .ToListAsync();
      return users;
    }

    [HttpPost]
    [Route("")]
    [AllowAnonymous]
    public async Task<ActionResult<User>> Post(
        [FromServices] DataContext context,
        [FromBody] User model
    )
    {
      if (!ModelState.IsValid) // Model State verifica se o que está no model é válido
      {
        return BadRequest(ModelState);
      }

      try
      {
        context.Users.Add(model);
        await context.SaveChangesAsync();
        return model;
      }
      catch
      {
        return BadRequest(new { message = "Não foi possível criar usuário" });
      }
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = "manager")]
    public async Task<ActionResult<User>> Put
      (int id,
      [FromBody] User model,
      [FromServices] DataContext context)
    {
      // Verifica se o Id informado é o mesmo do modelo
      if (id != model.Id)
      {
        return NotFound(new { message = "Usuário não encontrada" });
      }

      // Verifica os dados
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      try
      {
        context.Entry(model).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return model;
      }
      catch (DbUpdateConcurrencyException)
      {
        return BadRequest(new { message = "Este registro já foi atualizado" });
      }
      catch (Exception)
      {
        return BadRequest(new { message = "Não foi possível atualizar o usuário" });
      }

    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<dynamic>> Authenticate
       ([FromBody] User model, [FromServices] DataContext context)
    {
      var user = await context.Users
      .AsNoTracking()
      .Where(x => x.Username == model.Username && x.Password == model.Password)
      .FirstOrDefaultAsync();

      if (user == null)
      {
        return NotFound(new { message = "Usuário ou senha inválidos" });
      }
      var token = TokenService.GenerateToken(user);
      return new
      {
        user = user,
        token = token
      };
    }
  }
}