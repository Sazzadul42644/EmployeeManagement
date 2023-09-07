using EmployeeManagement.EF.Models;

namespace EmployeeManagement.Interface
{
    public interface IEmployeeAttendance
    {
        Task CreateAsync(EmployeeAttendance attendance);

    }
}
