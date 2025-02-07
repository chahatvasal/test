

namespace EmployeeDepartmentWebApi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public Employee Employee { get; set; } // Navigation property
    }
}
