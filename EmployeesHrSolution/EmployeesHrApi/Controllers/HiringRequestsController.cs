using AutoMapper;
using AutoMapper.QueryableExtensions;
using EmployeesHrApi.Data;
using EmployeesHrApi.HttpAdapters;
using EmployeesHrApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeesHrApi.Controllers;

public class HiringRequestsController : ControllerBase
{
    private readonly EmployeeDataContext _context;
    private readonly IMapper _mapper;
    private readonly MapperConfiguration _mapperConfig;
    private readonly TelecomHttpAdapter _telecomHttp;

    public HiringRequestsController(EmployeeDataContext context, IMapper mapper, MapperConfiguration mapperConfig, TelecomHttpAdapter telecomHttp)
    {
        _context = context;
        _mapper = mapper;
        _mapperConfig = mapperConfig;
        _telecomHttp = telecomHttp;
    }

    [HttpPost("/approved-hiring-requests")]
    public async Task<ActionResult> ApproveHiringRequestAsync([FromBody] HiringRequestResponseModel request)
    {

        var id = int.Parse(request.Id);

        var savedHiringRequest = await _context.HiringRequests.Where(h => h.Id == id)
            .SingleOrDefaultAsync();



        if (savedHiringRequest == null)
        {
            return BadRequest();
        }
        else
        {
            if (savedHiringRequest.Status != HiringRequestStatus.WaitingForJobAssignment)
            {
                return BadRequest("Can only deny pending assignments");
            }
            savedHiringRequest.Status = HiringRequestStatus.Hired;

            var newHireRequest = new NewHireRequestModel
            {
                Id = id,
                Department = request.RequestedDepartment,
                FirstName = request.FirstName,
                LastName = request.LastName,
            };

            string emailAddress = "";
            string phoneExtension = "";
            try
            {
                var teleComInfo = await _telecomHttp.GetTelecomInfoForNewHire(newHireRequest);
                if (teleComInfo == null)
                {

                    throw new ArgumentNullException("The Api Done Crashed");
                }
                emailAddress = teleComInfo.EmailAddress;
                phoneExtension = teleComInfo.PhoneExtension;
            }
            catch (Exception)
            {
                // do your plan B.
                emailAddress = "Check With Your Manager";
                phoneExtension = "Check With Manager";
                // log an error out, do something so the manager or whoever can follow up on this.
            }


            var employee = new Employee
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Department = request.RequestedDepartment,
                Salary = request.RequiredSalary,
                Email = emailAddress,
                PhoneExtensions = phoneExtension
            };
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return NoContent(); // or the mapped hiring request.
        }
    }

    [HttpPost("/denied-hiring-requests")]
    public async Task<ActionResult> DenyHiringRequestAsync([FromBody] HiringRequestResponseModel request)
    {
        var id = int.Parse(request.Id);

        var savedHiringRequest = await _context.HiringRequests.Where(h => h.Id == id)
            .SingleOrDefaultAsync();

        if (savedHiringRequest == null)
        {
            return BadRequest();
        }
        else
        {
            if (savedHiringRequest.Status != HiringRequestStatus.WaitingForJobAssignment)
            {
                return BadRequest("Can only deny pending assignments");
            }
            savedHiringRequest.Status = HiringRequestStatus.Denied;
            await _context.SaveChangesAsync();
            return NoContent(); // or the mapped hiring request.
        }
    }

    //[Authorize(Roles ="hiring-manager")]
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 10)]
    [HttpPost("/hiring-requests")]
    public async Task<ActionResult> AddHiringRequestAsync([FromBody] HiringRequestCreateRequest request)
    {
        // 1. Validate it a little? - if it isn't valid, send them a 400 (Bad Request)
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // 400
        }
        // 2. Save it to the database.

        var newHiringRequest = _mapper.Map<HiringRequests>(request);
        _context.HiringRequests.Add(newHiringRequest);

        await _context.SaveChangesAsync();

        var response = _mapper.Map<HiringRequestResponseModel>(newHiringRequest);
        return CreatedAtRoute("hiring-request#gethiringrequestbyidasync", new { id = response.Id }, response);
    }

    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 10)]
    [HttpGet("/hiring-requests/{id:int}", Name = "hiring-request#gethiringrequestbyidasync")]
    public async Task<ActionResult> GetHiringRequestByIdAsync(int id)
    {
        var hiringRequest = await _context.HiringRequests
            .Where(e => e.Id == id)
            .ProjectTo<HiringRequestResponseModel>(_mapperConfig)
            .SingleOrDefaultAsync();

        if (hiringRequest is not null)
        {
            return Ok(hiringRequest);
        }
        else
        {
            return NotFound();
        }
    }
}