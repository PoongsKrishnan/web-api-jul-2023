using AutoMapper.QueryableExtensions;
using EmployeesHrApi.Data;
using EmployeesHrApi.Models;
using Microsoft.AspNetCore.Mvc;
using EmployeesHrApi.Models;

namespace EmployeesHrApi.Controllers;

public class HiringRequestsController : ControllerBase
{
    private readonly EmployeeDataContext _context;

    public HiringRequestsController(EmployeeDataContext context)
    {
        _context = context;
    }

   

    [HttpPost("/hiring-requests")]
    public async Task<ActionResult> AddHiringRequestAsync([FromBody] HiringRequestCreateRequest request)
    {       
       // 1. Validate it a little? - if it isn't valid, send them a 400 (Bad Request)
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // 400
        }
        // 2. Save it to the database.
        var newHiringRequest = new HiringRequests
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            HomeEmail = request.HomeEmail,
            HomePhone = request.HomePhone,
            RequestedDepartment = request.RequestedDepartment,
            RequiredSalary = request.RequiredSalary,
            Status = HiringRequestStatus.WaitingForJobAssignment
        };

        _context.HiringRequests.Add(newHiringRequest);
        await _context.SaveChangesAsync();

        // 3. Return a 201 Created Status Code 
        //   - Add Header "Location" - with the Url of the new resource.
        //   - Return them a copy of the new resource

        return Ok(newHiringRequest);
    }
}
