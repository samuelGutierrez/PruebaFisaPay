using FisaPayNetCore.Helpers;
using FisaPayNetCore.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Globalization;
using FisaPayNetCore.Dto;

namespace FisaPayNetCore.Services
{
    public interface IEmpleadosService
    {
        Task<Empleados> CreateAsync(Empleados empleado);
        Task<bool> Delete(int id);
        Task<bool> Update(EmpleadoDto empleado);
        Task<IEnumerable<Empleado>> GetEmpleadosAsync();
        Task<Empleados> GetempleadobyId(int id);
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
                                   orderby e.Nombres ascending
                                   select new { e }).ToListAsync().ConfigureAwait(false);

            foreach (var itemEmpleado in empleados)
            {
                int ano, mes, dia;

                string edadActual;

                //Calcular edad actual
                ano = itemEmpleado.e.FechaNacimiento.Year;
                mes = itemEmpleado.e.FechaNacimiento.Month;
                dia = itemEmpleado.e.FechaNacimiento.Day;

                DateTime nacimiento = new DateTime(ano, mes, dia); //Fecha de nacimiento
                int edad = DateTime.Today.AddTicks(-nacimiento.Ticks).Year - 1;

                edadActual = edad + " años";

                Empleado data = new Empleado(itemEmpleado.e.Id, itemEmpleado.e.Cedula, itemEmpleado.e.Nombres, itemEmpleado.e.Sexo, itemEmpleado.e.FechaNacimiento.ToString("yyyy-MM-dd"), edadActual, itemEmpleado.e.Salario.ToString("C", CultureInfo.CurrentCulture), itemEmpleado.e.VacunaCovid);
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

        public async Task<bool> Update(EmpleadoDto empleado)
        {
            var currentEmpleado = await _context.Empleados.FirstOrDefaultAsync(x => x.Id == empleado.Id).ConfigureAwait(false);

            currentEmpleado.Salario = empleado.Salario;
            currentEmpleado.Nombres = empleado.Nombres;
            currentEmpleado.Sexo = empleado.Sexo;
            currentEmpleado.VacunaCovid = empleado.Vacuna;

            _context.Empleados.Update(currentEmpleado);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Empleados> GetempleadobyId(int id)
        {
            var empleado = await _context.Empleados.FirstOrDefaultAsync(e => e.Id == id).ConfigureAwait(false);

            return empleado;
        }
    }
}
