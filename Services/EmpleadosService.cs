using FisaPayNetCore.Helpers;
using FisaPayNetCore.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace FisaPayNetCore.Services
{
    public interface IEmpleadosService
    {
        Task<Empleados> CreateAsync(Empleados empleado);
        Task<bool> Delete(int id);
        Task<Empleados> Update(Empleados empleado, int id);
        Task<IEnumerable<Empleado>> GetEmpleadosAsync();
    }

    public class EmpleadosService : IEmpleadosService
    {
        private readonly DataContext _context;
        private IList<Empleado> listEmpleado;

        public EmpleadosService(DataContext context)
        {
            _context = context;
        }

        public async Task<Empleados> CreateAsync(Empleados empleado)
        {
            await _context.Empleados.AddAsync(empleado).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return empleado;
        }

        public async Task<IEnumerable<Empleado>> GetEmpleadosAsync()
        {
            listEmpleado = new List<Empleado>();

            var empleados = await (from e in _context.Empleados
                                   orderby e.Nombres descending
                                   select new { e }).ToListAsync().ConfigureAwait(false);

            foreach (var itemEmpleado in empleados)
            {
                Empleado data = new Empleado(itemEmpleado.e.Cedula, itemEmpleado.e.Nombres, itemEmpleado.e.Sexo, itemEmpleado.e.FechaNacimiento.ToString("yyyy/MM/dd"), itemEmpleado.e.Salario, itemEmpleado.e.VacunaCovid);
                listEmpleado.Add(data);
            }

            return listEmpleado;
        }

        public async Task<bool> Delete(int id)
        {
            var currentEmpleado = await _context.Empleados.FirstOrDefaultAsync(x => x.Id == id);

            _context.Empleados.Remove(currentEmpleado);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Empleados> Update(Empleados empleado, int id)
        {
            var currentEmpleado = await _context.Empleados.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);

            currentEmpleado.Salario = empleado.Salario;
            currentEmpleado.Nombres = empleado.Nombres;
            currentEmpleado.Sexo = empleado.Sexo;

            _context.Empleados.Update(currentEmpleado);
            await _context.SaveChangesAsync();

            return empleado;
        }
    }
}
