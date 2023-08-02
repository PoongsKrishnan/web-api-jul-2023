namespace EmployeesHrApi.HttpAdapters;

public class TelecomHttpAdapter
{
    private readonly HttpClient _httpClient;

    public TelecomHttpAdapter(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<NewHireResponseModel?> GetTelecomInfoForNewHire(NewHireRequestModel request)
    {
        var response = await _httpClient.PostAsJsonAsync("/new-hires", request);
        response.EnsureSuccessStatusCode();
        var info = await response.Content.ReadFromJsonAsync<NewHireResponseModel>();
        return info;
    }
}

public record NewHireRequestModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Id { get; set; }
    public string Department { get; set; } = string.Empty;
}

public class NewHireResponseModel
{
    public int Id { get; set; }
    public string PhoneExtension { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
}