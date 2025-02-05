using empDeptWebApi.Models;
using Microsoft.EntityFrameworkCore;

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
            return await _context.Department.ToListAsync();
        }

        public async Task<DepartmentClass?> GetDepartmentByIdAsync(int id)
        {
            return await _context.Department.FindAsync(id);
        }

        public async Task AddDepartmentAsync(DepartmentClass department)
        {
            _context.Department.Add(department);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDepartmentAsync(DepartmentClass department)
        {
            _context.Department.Update(department);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            var department = await _context.Department.FindAsync(id);
            if (department != null)
            {
                _context.Department.Remove(department);
                await _context.SaveChangesAsync();
            }
        }
    }
}
