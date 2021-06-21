using System;

namespace FisaPayNetCore.Model
{
    public class Empleados
    {
        public int Id { get; set; }
        public int Cedula { get; set; }
        public string Nombres { get; set; }
        public string Sexo { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public double Salario { get; set; }
        public bool VacunaCovid { get; set; }
        public bool Activo { get; set; }
    }
}
