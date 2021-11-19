using Domain;
using Domain.Interfaces.Helper;
using Domain.Interfaces.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IRepositoryGeneral _ire;
        private readonly IConfiguration _configuration;
        private readonly IFileProcesor _fil;
        public UsersController(IRepositoryGeneral ire, IConfiguration configuration, IFileProcesor fil)
        {
            _ire = ire;
            _configuration = configuration;
            _fil = fil;
        }
        //[HttpGet("lista")]
        //public async Task<ActionResult<User>> Lista()
        //{
        //    try
        //    {
        //        //validar si el user existe en la base de datos
        //        var users = await _ire.GetAll<User>();
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        var error = ex.Message;
        //        return null;
        //    }

        //}

        [HttpPost("login")]
        public async Task<ActionResult<UserToken>> Login(UserViewModels userViewModels)
        {
            try
            {
                User user = await _ire.GetFirst<User>(z => z.Name == userViewModels.Nombre && z.Password == userViewModels.Clave);
                if (user != null)
                {
                    //return Ok("Usuario logueado");
                    return BuildToken(user);
                }
                else
                {
                    return BadRequest("Usuario con credenciales inválidas");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("No se ha podido autenticar al usuario");
            }
        }

        private UserToken BuildToken(User userInfo)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name,userInfo.Name)
               // new Claim(ClaimTypes.NameIdentifier,userInfo.Token.ToString()), contiene la guid del usuario por eso es sencible y lo comente

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddMinutes(10); //tiempo del token

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}
