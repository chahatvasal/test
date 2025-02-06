namespace EmployeeDepartmentWebApi.Authentication
{
    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public int TokenValidityInHours { get; set; }
    }
}
