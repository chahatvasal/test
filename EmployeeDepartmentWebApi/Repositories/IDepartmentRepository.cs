using empDeptWebApi.Models;

namespace EmployeeDepartmentWebApi.Repositories
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<DepartmentClass>> GetAllDepartmentsAsync();
        Task<DepartmentClass?> GetDepartmentByIdAsync(int id);
        Task AddDepartmentAsync(DepartmentClass department);
        Task DeleteDepartmentAsync(int id);
        Task UpdateDepartmentAsync(DepartmentClass department);
    }
}
