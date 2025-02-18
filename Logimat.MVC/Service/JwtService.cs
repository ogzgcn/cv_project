using System.Text.Json;

public class JwtService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public JwtService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    // Login olup token almak
    public async Task<string> LoginAsync(string email, string password)
    {
        var loginModel = new
        {
            Email = email,
            Password = password
        };

        var response = await _httpClient.PostAsJsonAsync("/login", loginModel);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<JsonElement>();
            return result.GetProperty("Token").GetString();
        }
        return null;
    }

    // API isteklerinde token kullanma
    public void AddAuthorizationHeader(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }
}