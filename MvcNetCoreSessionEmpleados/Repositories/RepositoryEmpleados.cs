using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreSessionEmpleados.Data;
using MvcNetCoreSessionEmpleados.Models;

namespace MvcNetCoreSessionEmpleados.Repositories
{
    public class RepositoryEmpleados
    {
        private HospitalContext context;
        public RepositoryEmpleados (HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<Empleado>> GetEmpleadosAsync()
        {
            var consulta = from datos in this.context.Empleados select datos;
            return await consulta.ToListAsync();
        }

        public async Task<Empleado> FindEmpleadoAsync(int idEmpleado)
        {
            var consulta = from datos in this.context.Empleados where datos.IdEmpleado == idEmpleado select datos;
            return await consulta.FirstOrDefaultAsync();
        }

        public async Task<List<Empleado>> GetEmpleadosSessionAsync(List<int> ids)
        {
            var consulta = from datos in this.context.Empleados where ids.Contains(datos.IdEmpleado) select datos; //esta consulta es una consulta con un IN
            if(consulta.Count()== 0)
            {
                return null;
            }else
            {
                return await consulta.ToListAsync();
            }
        }

        public async Task<List<Empleado>> GetEmpleadosExcluyendoIdsAsync(List<int> ids)
        {
            var consulta = from datos in this.context.Empleados
                           where ids.Contains(datos.IdEmpleado) == false
                           select datos;
            if (consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                return await consulta.ToListAsync();
            }
        }

        public async Task Delete(int idEmpleado)
        {
            // Buscamos al empleado
            Empleado emp = await this.FindEmpleadoAsync(idEmpleado);

            if (emp != null)
            {
                this.context.Empleados.Remove(emp);
                await this.context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Empleado no encontrado");
            }
        }


    }
}
