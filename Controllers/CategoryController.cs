using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// Endpoint é uma URL
// https://localhost:5001/categories/
namespace ShopCSharp_API.Controllers
{
  [Route("categories")]
  public class CategoryController : ControllerBase
  {
    [HttpGet]
    [Route("")]
    public string Get()
    {
      return "Método Get";
    }

    [HttpGet]
    [Route("{id:int}")] // Ele entende que isso é um parâmetro inteiro
    public string GetById(int id)
    {
      return "Método GetById" + id.ToString();
    }

    [HttpPost]
    [Route("")]
    public string Post()
    {
      return "Método Post!";
    }

    [HttpPut]
    [Route("")]
    public string Put()
    {
      return "Método Put!";
    }

    [HttpDelete]
    [Route("")]
    public string Delete()
    {
      return "Método Delete!";
    }
  }
}