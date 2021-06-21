namespace FisaPayNetCore.Helpers
{
    public class Content
    {
    }

    public struct Empleado
    {
        public Empleado(int _cedula, string _nombreCompleto, string _sexo, string _fechaNacimiento, double _salario, bool _vacunaCovid)
        {
            Cedula = _cedula;
            NombreCompleto = _nombreCompleto;
            Sexo = _sexo;
            FechaNacimiento = _fechaNacimiento;
            Salario = _salario;
            VacunaCovid = _vacunaCovid;
        }

        public int Cedula { get; set; }
        public string NombreCompleto { get; set; }
        public string Sexo { get; set; }
        public string FechaNacimiento { get; set; }
        public double Salario { get; set; }
        public bool VacunaCovid { get; set; }
    }
}
