using System.ComponentModel.DataAnnotations;

namespace EmployeesHrApi.Models;

public record HiringRequestCreateRequest : IValidatableObject
{
    [Required, MinLength(2), MaxLength(200)]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;
    [Required, EmailAddress]
    public string HomeEmail { get; set; } = string.Empty;
    public string HomePhone { get; set; } = string.Empty;
    [Required]
    public string RequestedDepartment { get; set; } = string.Empty;
    [Range(5000,double.MaxValue)]
    public decimal RequiredSalary { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FirstName.ToLower() == "darth" && LastName.ToLower() == "vader")
        {
            yield return new ValidationResult("We have a no Sith Policy");
        }

        if (RequiredSalary < 150000 && RequestedDepartment.ToLower() == "dev")
        {
            yield return new ValidationResult("Programmers are worth WAY more than that!");
        }
    }
}



