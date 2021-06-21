using FisaPayNetCore.Model;
using Microsoft.EntityFrameworkCore;

namespace FisaPayNetCore.Helpers
{
    /// <summary>
    /// Contenido del contexto para poder realizar Operaciones crud
    /// </summary>
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Empleados> Empleados { get; set; }
    }
}
