using FisaPayNetCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FisaPayNetCore.Helpers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUsuariosService _userService;

        public JwtMiddleware(RequestDelegate next,
                             IUsuariosService userService)
        {
            _userService = userService;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await AttachUserToContextAsync(context, token).ConfigureAwait(false);

            await _next(context);
        }

        private async Task<bool> AttachUserToContextAsync(HttpContext context, string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("asdwda1d8a4sd8w4das8d*w8d*asd@#");
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // establezca clockskew en cero para que los tokens caduquen exactamente a la hora de vencimiento del token (en lugar de 5 minutos después)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "unique_name").Value);

            context.Items["User"] = await Task.Run(() => _userService.GetByIdAsync(userId).ConfigureAwait(false)).ConfigureAwait(false);

            return true;
        }
    }
}
