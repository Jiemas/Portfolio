using System.Net.Http.Json;
using System.Text.Json;

public class DatabaseService
{
    private readonly HttpClient _httpClient;
    private const string RootPath = "https://portfolio-a3134-default-rtdb.firebaseio.com/.json";

    public DatabaseService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Fetch all data from the root of the Firebase database
    public async Task<T?> GetAllDataAsync<T>()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<T>(RootPath);
        }
        catch (HttpRequestException ex)
        {
            // Network error (no internet, DNS failure, Firebase down, etc.)
            Console.Error.WriteLine($"[DatabaseService] Network error: {ex.Message}");
        }
        catch (TaskCanceledException ex)
        {
            // Timeout or request aborted
            Console.Error.WriteLine($"[DatabaseService] Request timeout: {ex.Message}");
        }
        catch (NotSupportedException ex)
        {
            // JSON format not supported
            Console.Error.WriteLine($"[DatabaseService] Unsupported JSON: {ex.Message}");
        }
        catch (System.Text.Json.JsonException ex)
        {
            // Invalid JSON from Firebase
            Console.Error.WriteLine($"[DatabaseService] JSON parse error: {ex.Message}");
        }
        catch (Exception ex)
        {
            // Anything unexpected
            Console.Error.WriteLine($"[DatabaseService] Unexpected error: {ex.Message}");
        }

        return default; // return null safely for T?
    }

    public async Task<T?> GetAllLocalDataAsync<T>()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<T>("backup.json");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Local backup load failed: {ex.Message}");
            return default;
        }
    }
}
