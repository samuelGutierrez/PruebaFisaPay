using AutoMapper;
using FisaPayNetCore.Dto;
using FisaPayNetCore.Helpers;
using FisaPayNetCore.Model;
using FisaPayNetCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FisaPayNetCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuariosService _userService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UsuariosController(IUsuariosService userService,
                                  IConfiguration configuration,
                                  IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateDto authenticateDto)
        {
            try
            {
                var usuario = await _userService.AuthenticateAsync(authenticateDto.Usuario, authenticateDto.Contrasena, authenticateDto.Intentos).ConfigureAwait(false);

                if (usuario == null)
                    return Unauthorized(new AppException("Usuario/Contraseña invalido."));

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["AppSettings:SecretKey"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, usuario.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new
                {
                    usuario.Id,
                    nameUser = usuario.Usuario,
                    Token = tokenString
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Usuarios usuarioModel)
        {
            try
            {
                // crear un usuario
                var usuario = await _userService.CreateAsync(usuarioModel, usuarioModel.PasswordHash).ConfigureAwait(false);

                return Ok(new
                {
                    status = HttpStatusCode.Accepted,
                    sms = usuario
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = ex.Message });
            }
        }
    }
}
