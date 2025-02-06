using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EmployeeDepartmentWebApi.Models;
namespace empDeptWebApi.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        [Column("FirstName", TypeName = "varchar(200)")]
        public string FirstName { get; set; }
        [Column("LastName", TypeName = "varchar(200)")]
        public string LastName { get; set; }
        [Column("Age", TypeName = "int")]
        public int Age { get; set; }
        [Column("Salary", TypeName = "decimal(18, 2)")]
        public decimal Salary { get; set; }

        //Foreign key to Department table
        public int DepartmentId { get; set; } //creates link to the department

        [Required]
        [EmailAddress]
        public string Email { get; set; } //new field

        public User? User { get; set; }
        public DepartmentClass Department { get; set; } //Represents the Department an Employee belongs to

    }
}
