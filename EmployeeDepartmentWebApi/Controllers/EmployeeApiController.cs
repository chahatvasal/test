


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

        [HttpGet]
        [Route("GetEmployees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var employees = await _employeeRepository.GetAllEmployeesAsync();
                var employeeDTOs = _mapper.Map<IEnumerable<EmployeeTransferDTO>>(employees);
                return Ok(employeeDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            try
            {
                var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
                if (employee == null) return NotFound("Employee not found");

                var employeeDTO = _mapper.Map<EmployeeTransferDTO>(employee);
                return Ok(employeeDTO);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(EmployeeCreateDTO employeeCreateDTO)
        {
            if (employeeCreateDTO == null)
                return BadRequest("Employee creation data is required.");
            try
            {
                var employee = _mapper.Map<Employee>(employeeCreateDTO);
                await _employeeRepository.AddEmployeeAsync(employee);
                return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, employee);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    $"Database update error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, EmployeeCreateDTO employeeDTO)
        {
            try
            {
                var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);
                if (existingEmployee == null) return NotFound("Employee not found");

                _mapper.Map(employeeDTO, existingEmployee);
                await _employeeRepository.UpdateEmployeeAsync(existingEmployee);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                await _employeeRepository.DeleteEmployeeAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
