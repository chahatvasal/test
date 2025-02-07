

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentClass>>> GetDepartments()
        {
            try
            {
                var departments = await _departmentRepository.GetAllDepartmentsAsync();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Error fetching departments: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentClass>> GetDepartment(int id)
        {
            try
            {
                var department = await _departmentRepository.GetDepartmentByIdAsync(id);
                if (department == null) return NotFound("Department not found");
                return department;
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Error fetching department: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<DepartmentClass>> CreateDepartment(DepartmentClass department)
        {

            if (department == null) return BadRequest("Department data is required.");

            try
            {
                await _departmentRepository.AddDepartmentAsync(department);
                return CreatedAtAction(nameof(GetDepartment), new { id = department.DepartmentId }, department);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Error creating department: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, DepartmentClass department)
        {
            if (id != department.DepartmentId) return BadRequest("Department ID mismatch.");

            try
            {
                var existingDepartment = await _departmentRepository.GetDepartmentByIdAsync(id);
                if (existingDepartment == null) return NotFound("Department not found");

                await _departmentRepository.UpdateDepartmentAsync(department);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound("Failed to update department. It may have been modified by another user.");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Error updating department: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var existingDepartment = await _departmentRepository.GetDepartmentByIdAsync(id);
                if (existingDepartment == null) return NotFound("Department not found");

                await _departmentRepository.DeleteDepartmentAsync(id);
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Error deleting department: {ex.Message}");
            }
        }
    }
}
