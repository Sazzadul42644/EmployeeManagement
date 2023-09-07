using EmployeeManagement.EF.Models;

namespace EmployeeManagement.Interface
{
    public interface IEmployee
    {
        Task<Employee> GetByIdAsync(int employeeId);
        Task<List<Employee>> GetAllAsync();
        Task UpdateAsync(Employee employee);
        Task CreateAsync(Employee newEmployee);
    }
}
