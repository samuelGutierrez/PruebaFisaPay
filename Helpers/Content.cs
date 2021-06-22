namespace FisaPayNetCore.Helpers
{
    public class Content
    {
    }

    public struct Empleado
    {
        public Empleado(int _id, int _cedula, string _nombreCompleto, string _sexo, string _fechaNacimiento, string _edadActual, string _salario, bool _vacunaCovid)
        {
            Id = _id;
            Cedula = _cedula;
            NombreCompleto = _nombreCompleto;
            Sexo = _sexo;
            FechaNacimiento = _fechaNacimiento;
            EdadActual = _edadActual;
            Salario = _salario;
            VacunaCovid = _vacunaCovid;
        }

        public int Id { get; set; }
        public int Cedula { get; set; }
        public string NombreCompleto { get; set; }
        public string Sexo { get; set; }
        public string FechaNacimiento { get; set; }
        public string EdadActual { get; set; }
        public string Salario { get; set; }
        public bool VacunaCovid { get; set; }
    }
}
