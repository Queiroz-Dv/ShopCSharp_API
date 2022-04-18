using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ShopCSharp_API.Models;

namespace ShopCSharp_API.Services
{
  public static class TokenService
  {
    public static string GenerateToken(User user)
    {
      var tokenHandler = new JwtSecurityTokenHandler(); // Manipula nosso token
      var key = Encoding.ASCII.GetBytes(Settings.Secret); // Precisa da chave
      var tokenDescriptor = new SecurityTokenDescriptor // Descrição do que vai ter no token
      {
        Subject = new ClaimsIdentity(new Claim[]{
              new Claim(ClaimTypes.Name, user.Id.ToString()),
              new Claim(ClaimTypes.Role, user.Role.ToString())
          }),
        Expires = DateTime.UtcNow.AddHours(2), // Tempo de expiração por duas horas
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      
      var token = tokenHandler.CreateToken(tokenDescriptor); // Manda criar token
      return tokenHandler.WriteToken(token); // Retorna token
    }
  }
}