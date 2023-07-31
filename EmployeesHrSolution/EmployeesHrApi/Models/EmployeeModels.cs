using EmployeesHrApi.Data;

namespace EmployeesHrApi.Models;

public record EmployeesResponseModel
{
    public List<EmployeesSummaryResponseModel> Employees { get; set; } = new();
}

public record EmployeesSummaryResponseModel
{
    public string Id { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
}
