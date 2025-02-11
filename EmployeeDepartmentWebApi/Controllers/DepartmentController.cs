

namespace empDeptWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController (IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentClass>>> GetDepartments()
        {
            Log.Information("Fetching all departments");
            try
            {
                var departments = await _departmentRepository.GetAllDepartmentsAsync();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error fetching departments");
                return StatusCode(500, "Internal server error");
            }
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentClass>> GetDepartment(int id)
        {
            Log.Information("Fetching department with ID: {Id}", id);
            try
            {
                var department = await _departmentRepository.GetDepartmentByIdAsync(id);
                if (department == null) 
                {
                    Log.Warning("Department not found for ID: {Id}", id);
                    return NotFound("Department not found");
                }
               
                return department;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error fetching department with ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<DepartmentClass>> CreateDepartment(DepartmentClass department)
        {
            Log.Information("Creating new department");

            try
            {
                await _departmentRepository.AddDepartmentAsync(department);
                return CreatedAtAction(nameof(GetDepartment), new { id = department.DepartmentId }, department);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "error creating department");
                return StatusCode(500, "Internal server error");
            }
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, DepartmentClass department)
        {
            Log.Information("Updating department with ID: {Id}", id);

            try
            {
                var existingDepartment = await _departmentRepository.GetDepartmentByIdAsync(id);
                if (existingDepartment == null) 
                {
                    Log.Warning("Department not found for update with ID: {Id}", id);
                    return NotFound("Department not found");
                }
                await _departmentRepository.UpdateDepartmentAsync(department);
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating department with ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            Log.Information("Deleting department with ID: {Id}", id);
            try
            {
                var existingDepartment = await _departmentRepository.GetDepartmentByIdAsync(id);
                if (existingDepartment == null) 
                {
                    Log.Warning("Department not found for deletion with ID: {Id}", id);
                    return NotFound("Department not found");
                }
                await _departmentRepository.DeleteDepartmentAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error deleting department with ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
