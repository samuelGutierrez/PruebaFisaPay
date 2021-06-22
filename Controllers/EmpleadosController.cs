using AutoMapper;
using FisaPayNetCore.Dto;
using FisaPayNetCore.Model;
using FisaPayNetCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace FisaPayNetCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadosController : ControllerBase
    {
        private readonly IEmpleadosService _empleadoService;
        private readonly IMapper _mapper;

        public EmpleadosController(IEmpleadosService empleadosService,
                                   IMapper mapper)
        {
            _empleadoService = empleadosService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetEmpleados()
        {
            try
            {
                //obtener listado de empleados
                var empleadosList = await _empleadoService.GetEmpleadosAsync().ConfigureAwait(false);

                return Ok(empleadosList);
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmpleadobyId(int id)
        {
            try
            {
                //obtener empleado
                var empleado = await _empleadoService.GetempleadobyId(id).ConfigureAwait(false);

                return Ok(empleado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Empleados empleadoDto)
        {
            try
            {
                // crear un empleado
                var empleado = await _empleadoService.CreateAsync(empleadoDto).ConfigureAwait(false);

                return Ok(new
                {
                    status = HttpStatusCode.Created,
                    sms = "Empleado " + empleado.Nombres + " creado con exito"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] EmpleadoDto empleadoDto, int id)
        {
            try
            {
                // actualizar un empleado
                var empleado = await _empleadoService.Update(empleadoDto, id).ConfigureAwait(false);

                return Ok(new
                {
                    status = HttpStatusCode.Created,
                    sms = "Empleado actualizado con exito"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // eliminar un empleado
                var empleado = await _empleadoService.Delete(id).ConfigureAwait(false);

                return Ok(new
                {
                    status = HttpStatusCode.Created,
                    sms = "Empleado eliminado con exito"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = ex.Message });
            }
        }
    }
}
