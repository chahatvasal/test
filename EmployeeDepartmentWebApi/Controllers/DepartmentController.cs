using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using empDeptWebApi.Models;

namespace empDeptWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly EmployeeContext employeeContext;

        public DepartmentController (EmployeeContext employeeContext)
        {
            this.employeeContext = employeeContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentClass>>> GetDepartments()
        {
            return await employeeContext.Department.Include(d => d.Employees).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentClass>> GetDepartment(int id)
        {
            var department = await employeeContext.Department.Include(d => d.Employees)
                            .FirstOrDefaultAsync(d => d.DepartmentId == id);
            if (department == null) return NotFound();
            return department;
        }

        [HttpPost]
        public async Task<ActionResult<DepartmentClass>> CreateDepartment(DepartmentClass department)
        {
            employeeContext.Department.Add(department);
            await employeeContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDepartment), new { id = department.DepartmentId }, department);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, DepartmentClass department)
        {
            if (id != department.DepartmentId) return BadRequest();

            employeeContext.Entry(department).State = EntityState.Modified;

            try
            {
                await employeeContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!employeeContext.Department.Any(e => e.DepartmentId == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await employeeContext.Department.FindAsync(id);
            if (department == null) return NotFound();

            employeeContext.Department.Remove(department);
            await employeeContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
