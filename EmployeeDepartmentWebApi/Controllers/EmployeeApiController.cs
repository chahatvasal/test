using empDeptWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using empDeptWebApi.EmployeeDTO;

namespace empDeptWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeApiController : ControllerBase
    {
        private readonly EmployeeContext employeeContext;
        private readonly IMapper _mapper;

        public EmployeeApiController(EmployeeContext employeeContext, IMapper mapper)
        {
            this.employeeContext = employeeContext;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetEmployees")]
        public async Task<ActionResult<IEnumerable<EmployeeTransferDTO>>> GetEmployees()
        {
            var employees = await employeeContext.Employee.Include(e => e.Department).ToListAsync();
            var employeeDTOs = _mapper.Map<IEnumerable<EmployeeTransferDTO>>(employees);
            return Ok(employeeDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeTransferDTO>> GetEmployee(int id)
        {
            var employee = await employeeContext.Employee.Include(e => e.Department).FirstOrDefaultAsync(e => e.EmployeeId == id);
            if (employee == null) return NotFound();
            var employeeDTO = _mapper.Map<EmployeeTransferDTO>(employee);
            return Ok(employeeDTO);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeTransferDTO>> CreateEmployee(EmployeeCreateDTO employeeCreateDTO)
        {
            var employee = new Employee
            {
                FirstName = employeeCreateDTO.FirstName,
                LastName = employeeCreateDTO.LastName,
                Age = employeeCreateDTO.Age,
                Salary = employeeCreateDTO.Salary,
                DepartmentId = employeeCreateDTO.DepartmentId
            };
            employeeContext.Employee.Add(employee);
            await employeeContext.SaveChangesAsync();

            var resultDTO = _mapper.Map<EmployeeTransferDTO>(employee);
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, resultDTO);
            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, EmployeeCreateDTO employeeDTO)
        {
            var existingEmployee = await employeeContext.Employee.FindAsync(id);
            if (existingEmployee == null) return NotFound();

            _mapper.Map(employeeDTO, existingEmployee);

            try
            {
                await employeeContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!employeeContext.Employee.Any(e => e.EmployeeId == id))
                    return NotFound();
                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await employeeContext.Employee.FindAsync(id);
            if (employee == null) return NotFound();

            employeeContext.Employee.Remove(employee);
            await employeeContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
