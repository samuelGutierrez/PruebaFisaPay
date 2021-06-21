namespace FisaPayNetCore.Model
{
    public class Usuarios
    {
        public int Id { get; set; }
        public int CedulaEmpleado { get; set; }
        public string Usuario { get; set; }
        public string PasswordHash { get; set; }
        public bool Activo { get; set; }
    }
}
