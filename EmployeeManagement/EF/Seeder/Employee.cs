using EmployeeManagement.EF.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EmployeeManagement.EF.Seeders
{
    public static class EmployeeSeeder
    {
        public static void Seed(APIContext dbContext)
        {
            if (!dbContext.Employees.Any())
            {
                var employees = new Employee[]
                {
                    new Employee
                    {
                        EmployeeName = "Mehedi Hasan",
                        EmployeeCode = "EMP320",
                        EmployeeSalary = 50000,
                        SupervisorId = 502036 
                    },
                    new Employee
                    {
                        EmployeeName = "Ashikur Rahman",
                        EmployeeCode = "EMP321",
                        EmployeeSalary = 45000,
                        SupervisorId = 502036
                    },
                    
                };
                dbContext.Employees.AddRange(employees);
                try
                {
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Error creating the employee attendance record.", ex);
                }

            }
        }
    }
}
