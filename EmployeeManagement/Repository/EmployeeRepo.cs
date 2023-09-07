using EmployeeManagement.EF;
using EmployeeManagement.EF.Models;
using EmployeeManagement.Interface;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Repository
{
    public class EmployeeRepo : IEmployee
    {
        private readonly APIContext _dbContext;

        public EmployeeRepo(APIContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Employee> GetByIdAsync(int employeeId)
        {
            return await _dbContext.Employees.FindAsync(keyValues: employeeId);
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _dbContext.Employees.ToListAsync();
        }


        public async Task UpdateAsync(Employee employee)
        {
            if (await _dbContext.Employees.AnyAsync(e => e.EmployeeId != employee.EmployeeId && e.EmployeeCode == employee.EmployeeCode))
            {
                throw new InvalidOperationException("Duplicate EmployeeCode is not allowed.");
            }

            _dbContext.Employees.Update(employee);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateAsync(Employee newEmployee)
        {
            try
            {
                if (await _dbContext.Employees.AnyAsync(e => e.EmployeeCode == newEmployee.EmployeeCode))
                {
                    throw new InvalidOperationException("Duplicate EmployeeCode is not allowed.");
                }

                _dbContext.Employees.Add(newEmployee);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error creating the employee.", ex);
            }
        }

    }
}
