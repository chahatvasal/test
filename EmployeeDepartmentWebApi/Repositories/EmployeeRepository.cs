using empDeptWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace EmployeeDepartmentWebApi.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeContext _context;

        public EmployeeRepository(EmployeeContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            try
            {
                return await _context.Employee.Include(e => e.Department).ToListAsync();
            }
            catch(DbUpdateException dbEx)
            {
                throw new Exception("Database update error: " + dbEx.Message);
            }
            catch(Exception ex)
            {
                throw new Exception("Error retrieving employees: " + ex.Message);
            }
            
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            try
            {
                return await _context.Employee.Include(e => e.Department)
                                    .FirstOrDefaultAsync(e => e.EmployeeId == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching employee by ID", ex);
            }
        }

        public async Task AddEmployeeAsync(Employee employee)
        {

            try
            {
                await _context.Employee.AddAsync(employee);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating employee", ex);
            }

        }

        public async Task DeleteEmployeeAsync(int id)
        {
            try
            {
                var employee = await _context.Employee.FindAsync(id);
                if (employee == null) throw new KeyNotFoundException("Employee not found for deletion");

                _context.Employee.Remove(employee);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting employee", ex);
            }
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            try
            {
                _context.Entry(employee).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new KeyNotFoundException("Employee not found for update");
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating employee", ex);
            }
        }
    }
}
