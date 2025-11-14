using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

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
        return await _httpClient.GetFromJsonAsync<T>(RootPath);
    }
}