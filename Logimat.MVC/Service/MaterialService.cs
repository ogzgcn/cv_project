using Logimat.API.Models;

public class MaterialService
{
    private readonly HttpClient _httpClient;
    private readonly JwtService _jwtService;

    public MaterialService(HttpClient httpClient, JwtService jwtService)
    {
        _httpClient = httpClient;
        _jwtService = jwtService;
    }

    // Malzeme Listesini Getir
    public async Task<List<Material>> GetMaterialsAsync(string token)
    {
        _jwtService.AddAuthorizationHeader(token);

        try
        {
            var response = await _httpClient.GetAsync("/materials");
            if (response.IsSuccessStatusCode)
            {
                // Use ReadFromJsonAsync instead of ReadAsAsync
                return await response.Content.ReadFromJsonAsync<List<Material>>() ?? new List<Material>();
            }
            else
            {
                // Handle API failure (log or notify the user)
                return new List<Material>();  // Return empty list on failure
            }
        }
        catch (Exception ex)
        {
            // Log or handle exception
            return new List<Material>();  // Return empty list on error
        }
    }

    // Tek Bir Malzemeyi Getir (ID ile)
    public async Task<Material> GetMaterialByIdAsync(string token, Guid id)
    {
        _jwtService.AddAuthorizationHeader(token);

        try
        {
            var response = await _httpClient.GetAsync($"/materials/{id}");
            if (response.IsSuccessStatusCode)
            {
                // Use ReadFromJsonAsync instead of ReadAsAsync
                return await response.Content.ReadFromJsonAsync<Material>();
            }
            else
            {
                // Handle API failure (log or notify the user)
                return null;  // Return null on failure
            }
        }
        catch (Exception ex)
        {
            // Log or handle exception
            return null;  // Return null on error
        }
    }

    // Malzeme Ekle
    public async Task<Material> AddMaterialAsync(string token, Material material)
    {
        _jwtService.AddAuthorizationHeader(token);

        try
        {
            var response = await _httpClient.PostAsJsonAsync("/materials", material);
            if (response.IsSuccessStatusCode)
            {
                // Use ReadFromJsonAsync instead of ReadAsAsync
                return await response.Content.ReadFromJsonAsync<Material>();
            }
            return null;
        }
        catch (Exception ex)
        {
            // Log or handle exception
            return null;  // Return null on error
        }
    }

    // Malzeme Güncelle
    public async Task<Material> UpdateMaterialAsync(string token, Guid id, Material material)
    {
        _jwtService.AddAuthorizationHeader(token);

        try
        {
            var response = await _httpClient.PutAsJsonAsync($"/materials/{id}", material);
            if (response.IsSuccessStatusCode)
            {
                // Use ReadFromJsonAsync instead of ReadAsAsync
                return await response.Content.ReadFromJsonAsync<Material>();
            }
            return null;
        }
        catch (Exception ex)
        {
            // Log or handle exception
            return null;  // Return null on error
        }
    }

    // Malzeme Sil
    public async Task<bool> DeleteMaterialAsync(string token, Guid id)
    {
        _jwtService.AddAuthorizationHeader(token);

        try
        {
            var response = await _httpClient.DeleteAsync($"/materials/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            // Log or handle exception
            return false;  // Return false on error
        }
    }
}
