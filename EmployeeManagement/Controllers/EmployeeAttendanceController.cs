using EmployeeManagement.DTOs;
using EmployeeManagement.EF;
using EmployeeManagement.EF.Models;
using EmployeeManagement.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace EmployeeManagement.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeAttendanceController : ControllerBase
    {
        private readonly APIContext _dbContext;
        private readonly IEmployeeAttendance _employeeAttendanceRepository;

        public EmployeeAttendanceController(IEmployeeAttendance employeeRepository, APIContext dbContext)
        {
            _employeeAttendanceRepository = employeeRepository;
            _dbContext = dbContext;
        }
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateEmployeeAttendance([FromBody] EmployeeAttendance newAttendance)
        {
            try
            {
                await _employeeAttendanceRepository.CreateAsync(newAttendance);
                return Ok("Employee attendance record created successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpGet("monthly-attendance-report")]
        public async Task<IActionResult> GetMonthlyAttendanceReport()
        {
            var currentDate = DateTime.Now;
            var currentMonth = currentDate.Month;
            var currentYear = currentDate.Year;
            var attendanceReport = await _dbContext.Employees
                .Include(e => e.Attendances)
                .Select(e => new
                {
                    EmployeeName = e.EmployeeName,
                    MonthName = currentDate.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture),
                    PayableSalary = e.EmployeeSalary,
                    TotalPresent = e.Attendances.Count(a => a.AttendanceDate.Month == currentMonth && a.AttendanceDate.Year == currentYear && !a.IsAbsent && !a.IsOffDay), // Modify based on your EmployeeAttendance class
                    TotalAbsent = e.Attendances.Count(a => a.AttendanceDate.Month == currentMonth && a.AttendanceDate.Year == currentYear && a.IsAbsent), // Modify based on your EmployeeAttendance class
                    TotalOffday = e.Attendances.Count(a => a.AttendanceDate.Month == currentMonth && a.AttendanceDate.Year == currentYear && a.IsOffDay)
                    
                })
                .ToListAsync();

            return Ok(attendanceReport);
        }

        [Authorize]
        [HttpGet("employee-hierarchy/{employeeId}")]
        public async Task<IActionResult> GetEmployeeHierarchy(int employeeId)
        {
            
            var employeeNames = new List<string>();

            var employee = await _dbContext.Employees.FindAsync(employeeId);
            while (employee != null)
            {
                employeeNames.Add(employee.EmployeeName);
                employee = employee.Supervisor; 
            }

            employeeNames.Reverse();
            var hierarchyDto = new EmployeeHierarchyDto
            {
                EmployeeNames = string.Join(" -> ", employeeNames)
            };

            return Ok(hierarchyDto);
        }


    }
}
