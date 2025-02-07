

namespace EmployeeDepartmentWebApi.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly EmployeeContext _context;

        public DepartmentRepository(EmployeeContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<DepartmentClass>> GetAllDepartmentsAsync()
        {
            try
            {
                return await _context.Department.Include(d => d.Employees).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching departments", ex);
            }
        }

        public async Task<DepartmentClass?> GetDepartmentByIdAsync(int id)
        {
            try
            {
                return await _context.Department.Include(d => d.Employees)
                    .FirstOrDefaultAsync(d => d.DepartmentId == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching department by ID", ex);
            }
        }

        public async Task AddDepartmentAsync(DepartmentClass department)
        {
            try
            {
                await _context.Department.AddAsync(department);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating department", ex);
            }
        }

        public async Task UpdateDepartmentAsync(DepartmentClass department)
        {
            try
            {
                _context.Entry(department).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new KeyNotFoundException("Department not found for update");
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating department", ex);
            }
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            try
            {
                var department = await _context.Department.FindAsync(id);
                if (department == null) throw new KeyNotFoundException("Department not found for deletion");

                _context.Department.Remove(department);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting department", ex);
            }
        }
    }
}
