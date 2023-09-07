using EmployeeManagement.EF;
using EmployeeManagement.EF.Models;
using EmployeeManagement.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployee _employeeRepository;
        private readonly APIContext _dbContext;

        public EmployeeController(IEmployee employeeRepository, APIContext dbContext)
        {
            _employeeRepository = employeeRepository;
            _dbContext = dbContext;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateEmployee([FromBody] Employee newEmployee)
        {
            try
            {
                await _employeeRepository.CreateAsync(newEmployee);
                return Ok("Employee created successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
        [Authorize]
        [HttpPut("update/{employeeId}")]
        public async Task<IActionResult> UpdateEmployee(int employeeId, [FromBody] Employee updatedEmployee)
        {
            var existingEmployee = await _employeeRepository.GetByIdAsync(employeeId);
            if (existingEmployee == null)
            {
                return NotFound();
            }

            existingEmployee.EmployeeName = updatedEmployee.EmployeeName;
            existingEmployee.EmployeeCode = updatedEmployee.EmployeeCode;

            try
            {
                await _employeeRepository.UpdateAsync(existingEmployee);
                return Ok("Employee updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("thirdhighestsalary")]
        public async Task<IActionResult> GetEmployeeWithThirdHighestSalary()
        {
            var employee = await _dbContext.Employees
                .OrderByDescending(e => e.EmployeeSalary)
                .Skip(2)
                .Take(1) 
                .SingleOrDefaultAsync();

            if (employee != null)
            {
                return Ok(employee); 
            }

            return NotFound(); 
        }
        [HttpGet("employees-without-absent")]
        public async Task<IActionResult> GetEmployeesWithoutAbsentRecords()
        {
            var employeesWithoutAbsent = await _dbContext.Employees
                .Where(e => e.Attendances.All(a => a.IsAbsent == false)) 
                .OrderByDescending(e => e.EmployeeSalary)
                .ToListAsync();

            return Ok(employeesWithoutAbsent);
        }
       
    }
}
