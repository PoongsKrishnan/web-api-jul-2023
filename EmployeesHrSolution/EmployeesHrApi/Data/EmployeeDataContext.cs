using Microsoft.EntityFrameworkCore;

namespace EmployeesHrApi.Data;

public class EmployeeDataContext : DbContext
{
    public EmployeeDataContext(DbContextOptions<EmployeeDataContext> options) : base(options)
    {

    }
    public DbSet<Employee> Employees { get; set; }

    public IQueryable<Employee> GetEmployeesByDepartment(string department)
    {
        if(department != "All")
        {
            return Employees.Where(e=>e.Department== department);
        }
        else
        {
            return Employees;
        }
    }
}