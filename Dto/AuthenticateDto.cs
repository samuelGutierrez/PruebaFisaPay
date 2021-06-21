using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FisaPayNetCore.Dto
{
    public class AuthenticateDto
    {
        [Required]
        public string Usuario { get; set; }

        [Required]
        public string Contrasena { get; set; }

        public int Intentos { get; set; }
    }
}
