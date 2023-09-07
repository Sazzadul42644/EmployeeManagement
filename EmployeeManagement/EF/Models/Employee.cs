using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.EF.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "EmployeeName is required.")]
        [MaxLength(50, ErrorMessage = "EmployeeName cannot exceed 50 characters.")]
        public string EmployeeName { get; set; }

        [Required(ErrorMessage = "EmployeeCode is required.")]
        [MaxLength(10, ErrorMessage = "EmployeeCode cannot exceed 10 characters.")]
        public string EmployeeCode { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "EmployeeSalary must be a non-negative value.")]
        public decimal EmployeeSalary { get; set; }

        public int? SupervisorId { get; set; }

        [ForeignKey("SupervisorId")] 
        public Employee Supervisor { get; set; }

        public ICollection<EmployeeAttendance> Attendances { get; set; } = new List<EmployeeAttendance>();
    }

}
