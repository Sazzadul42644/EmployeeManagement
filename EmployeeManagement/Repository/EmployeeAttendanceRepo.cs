using EmployeeManagement.EF;
using EmployeeManagement.EF.Models;
using EmployeeManagement.Interface;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Repository
{
    public class EmployeeAttendanceRepo : IEmployeeAttendance
    {
        private readonly APIContext _dbContext;

        public EmployeeAttendanceRepo(APIContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(EmployeeAttendance attendance)
        {
            try
            {
                if (await _dbContext.EmployeeAttendances
                    .AnyAsync(a => a.EmployeeId == attendance.EmployeeId && a.AttendanceDate == attendance.AttendanceDate))
                {
                    throw new InvalidOperationException("An attendance record for this date already exists for the employee.");
                }

                _dbContext.EmployeeAttendances.Add(attendance);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error creating the employee attendance record.", ex);
            }
        }

    }
}
