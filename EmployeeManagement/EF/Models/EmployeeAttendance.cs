using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.EF.Models
{
    public class EmployeeAttendance
    {
        [Key]
        public int EmployeeAttendanceId { get; set; }

        [Required(ErrorMessage = "EmployeeId is required.")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "AttendanceDate is required.")]
        [DataType(DataType.Date)]
        public DateTime AttendanceDate { get; set; }

        [Required(ErrorMessage = "IsPresent is required.")]
        public bool IsPresent { get; set; }

        [Required(ErrorMessage = "IsAbsent is required.")]
        public bool IsAbsent { get; set; }

        [Required(ErrorMessage = "IsOffDay is required.")]
        public bool IsOffDay { get; set; }

        public Employee Employee { get; set; }
    }

}
