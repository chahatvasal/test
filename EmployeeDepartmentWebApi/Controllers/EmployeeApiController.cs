


namespace empDeptWebApi.Controllers
{
    [Authorize]
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

        [AllowAnonymous]
        [HttpGet]
        [Route("GetEmployees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            Log.Information("Fetching all employees");
            try
            {
                var employees = await _employeeRepository.GetAllEmployeesAsync();
                var employeeDTOs = _mapper.Map<IEnumerable<EmployeeTransferDTO>>(employees);
                return Ok(employeeDTOs);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error fetching employees");
                return StatusCode(500, "Internal server error");
            }
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            Log.Information("Fetching employee with ID: {Id}", id);
            try
            {
                var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
                if (employee == null) 
                {
                    Log.Warning("Employee not found for ID: {Id}", id);
                    return NotFound("Employee not found");
                }
                var employeeDTO = _mapper.Map<EmployeeTransferDTO>(employee);
                return Ok(employeeDTO);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error fetching employee with ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddEmployee(EmployeeCreateDTO employeeCreateDTO)
        {
            Log.Information("Adding a new employee");
           
            try
            {
                var employee = _mapper.Map<Employee>(employeeCreateDTO);
                await _employeeRepository.AddEmployeeAsync(employee);
                return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, employee);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error adding employee");
                return StatusCode(500, "Internal server error");
            }
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, EmployeeCreateDTO employeeDTO)
        {
            Log.Information("Updating employee with ID: {Id}", id);
            try
            {
                var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);
                if (existingEmployee == null) 
                {
                    Log.Warning("Employee not found for update with ID: {Id}", id);
                    return NotFound("Employee not found");
                }
                
                _mapper.Map(employeeDTO, existingEmployee);
                await _employeeRepository.UpdateEmployeeAsync(existingEmployee);

                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating employee with ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            Log.Information("Deleting employee with ID: {Id}", id);
            try
            {
                await _employeeRepository.DeleteEmployeeAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error deleting employee with ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
