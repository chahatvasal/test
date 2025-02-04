using System.ComponentModel.DataAnnotations;
using empDeptWebApi.Models;

namespace empDeptWebApi.EmployeeDTO
{
    public class EmployeeTransferDTO
    {
        
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public decimal Salary { get; set; }
        public string DepartmentName { get; set; }
      
    }
}
