using FisaPayNetCore.Helpers;
using FisaPayNetCore.Model;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace FisaPayNetCore.Services
{
    public interface IUsuariosService
    {
        Task<Usuarios> AuthenticateAsync(string usuario, string password, int intentos);
        Task<Usuarios> CreateAsync(Usuarios usuario, string contrasena);
        Task<Usuarios> GetByIdAsync(int id);
    }

    public class UsuariosService : IUsuariosService
    {
        private readonly DataContext _context;

        public UsuariosService(DataContext context)
        {
            _context = context;
        }

        public async Task<Usuarios> AuthenticateAsync(string usuario, string password, int intentos)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Usuario == usuario).ConfigureAwait(false);

            if (user == null)
                throw new AppException("Usuario/Contraseña invalido.");

            if (user.Activo == false)
                throw new AppException("Usuario bloqueado por favor comunicarse con el administrador.");

            if (intentos >= 3 && user.Activo == true)
            {
                user.Activo = false;
                _context.Usuarios.Update(user);
                await _context.SaveChangesAsync().ConfigureAwait(false);

                throw new AppException("Usuario bloqueado por favor comunicarse con el administrador.");
            }

            if (string.IsNullOrEmpty(usuario))
                throw new AppException("Enviar información del usuario");

            // validar si la contraseña es correcta
            if (!VerifyPassword(user.PasswordHash, password))
                return null;

            // autenticacion con exito
            return user;
        }

        public async Task<Usuarios> CreateAsync(Usuarios usuario, string contrasena)
        {
            if (_context.Usuarios.Any(u => u.CedulaEmpleado == usuario.CedulaEmpleado))
                throw new AppException("Número de cedula: \"" + usuario.CedulaEmpleado + "\" ya existe un usuario con esa número de cedula en nuestro sistema.");

            if (_context.Usuarios.Any(u => u.Usuario == usuario.Usuario))
                throw new AppException("Usuario \"" + usuario.Usuario + "\" ya existe.");

            if (contrasena != null && contrasena.Length == 0)
                throw new AppException("La contraseña es requerida");

            usuario.PasswordHash = HashPassword(contrasena, null, false);

            usuario.Activo = true;

            await _context.Usuarios.AddAsync(usuario).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return usuario;
        }

        public async Task<Usuarios> GetByIdAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id).ConfigureAwait(false);
        }

        #region Metodos privados

        private string HashPassword(string contrasena, byte[] salt = null, bool needsOnlyHash = false)
        {
            if (salt == null || salt.Length != 16)
            {
                // generate a 128-bit salt using a secure PRNG
                salt = new byte[128 / 8];

                using var rng = RandomNumberGenerator.Create();
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
               password: contrasena,
               salt: salt,
               prf: KeyDerivationPrf.HMACSHA256,
               iterationCount: 10000,
               numBytesRequested: 256 / 8));

            if (needsOnlyHash)
                return hashed;

            // la contraseña se concatenará con sal usando ':'
            return $"{hashed}:{Convert.ToBase64String(salt)}";
        }

        private bool VerifyPassword(string hashedPasswordWithSalt, string passwordToCheck)
        {
            // recupera la sal y la contraseña de 'hashedPasswordWithSalt'
            var passwordAndHash = hashedPasswordWithSalt.Split(':');
            if (passwordAndHash == null || passwordAndHash.Length != 2)
                return false;

            var salt = Convert.FromBase64String(passwordAndHash[1]);

            if (salt == null)
                return false;

            // hash la contraseña dada
            var hashOfpasswordToCheck = HashPassword(passwordToCheck, salt, true);

            // comparar ambos hashes
            if (String.Compare(passwordAndHash[0], hashOfpasswordToCheck) == 0)
                return true;

            return false;
        }
        #endregion
    }
}
