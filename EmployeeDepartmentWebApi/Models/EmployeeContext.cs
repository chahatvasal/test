using Microsoft.EntityFrameworkCore;

namespace empDeptWebApi.Models
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<DepartmentClass> Department { get; set; }
        //Configure relationships and models
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //configure one to many relationship between EmployeeMaster and Department
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)//Each employee has one department
                .WithMany(d => d.Employees) //Each department has many employees
                .HasForeignKey(e => e.DepartmentId); //foreign key linking EmployeeMaster to Department 
        }



    }
}
