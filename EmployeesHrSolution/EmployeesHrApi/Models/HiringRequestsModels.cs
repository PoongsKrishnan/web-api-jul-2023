using EmployeesHrApi.Data;

namespace EmployeesHrApi.Models;

public record HiringRequestResponseModel
{
    public string Id { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
   
    public string LastName { get; set; } = string.Empty;
    
    public string HomeEmail { get; set; } = string.Empty;
    public string HomePhone { get; set; } = string.Empty;
   
    public string RequestedDepartment { get; set; } = string.Empty;
  
    public decimal RequiredSalary { get; set; }

    public HiringRequestStatus Status { get; set; }

}



