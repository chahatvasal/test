using empDeptWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using empDeptWebApi.EmployeeDTO;
using EmployeeDepartmentWebApi.Repositories;

namespace empDeptWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeApiController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeApiController(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetEmployees")]
        public async Task<ActionResult<IEnumerable<EmployeeTransferDTO>>> GetEmployees()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();
            var employeeDTOs = _mapper.Map<IEnumerable<EmployeeTransferDTO>>(employees);
            return Ok(employeeDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeTransferDTO>> GetEmployee(int id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee == null) return NotFound();
            var employeeDTO = _mapper.Map<EmployeeTransferDTO>(employee);
            return Ok(employeeDTO);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeTransferDTO>> CreateEmployee(EmployeeCreateDTO employeeCreateDTO)
        {
            var employee = _mapper.Map<Employee>(employeeCreateDTO);
            await _employeeRepository.AddEmployeeAsync(employee);
            var resultDTO = _mapper.Map<EmployeeTransferDTO>(employee);
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, resultDTO);
            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, EmployeeCreateDTO employeeDTO)
        {
            var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (existingEmployee == null) return NotFound();

            _mapper.Map(employeeDTO, existingEmployee);

            await _employeeRepository.UpdateEmployeeAsync(existingEmployee);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee == null) return NotFound();
            await _employeeRepository.DeleteEmployeeAsync(id);
            return NoContent();
        }
    }
}
