namespace AssetManagementSystem.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string Email { get; set; }

        public string Department { get; set; }  

        public ICollection<Asset> Assets { get; set; }
    }
}
