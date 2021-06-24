using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FisaPayNetCore.Dto
{
    public class EmpleadoDto
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Sexo { get; set; }
        public double Salario { get; set; }
        public bool Vacuna { get; set; }
    }
}
