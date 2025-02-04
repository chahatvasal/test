using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace empDeptWebApi.Models
{
    public class DepartmentClass
    {
        [Key]
        public int DepartmentId { get; set; }

        [Column("DepartmentName", TypeName = "varchar(200)")]
        public string DepartmentName { get; set; }

        //A department can have many employees
        public ICollection<Employee> Employees { get; set; }

    }
}
