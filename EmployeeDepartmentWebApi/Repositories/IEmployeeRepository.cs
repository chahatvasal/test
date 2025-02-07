
namespace EmployeeDepartmentWebApi.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task AddEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(int id);
        Task UpdateEmployeeAsync(Employee employee);
    }
}
