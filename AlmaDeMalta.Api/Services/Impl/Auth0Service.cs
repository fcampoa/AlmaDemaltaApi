
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;

namespace AlmaDeMalta.api.Services.Impl;
public class Auth0Service(IHttpClientFactory clientFactory, IConfiguration configuration) : IAuth0Service
{


    public async Task<string> GetManagementTokenAsync()
    {
        var client = clientFactory.CreateClient("Auth0Client");

        var requestBody = new Dictionary<string, string>
        {
            { "client_id", configuration["Auth0:ClientId"] },
            { "client_secret", configuration["Auth0:ClientSecret"] },
            { "audience", $"https://{configuration["Auth0:Domain"]}/api/v2/" },
            { "grant_type", "client_credentials" }
        };

        var response = await client.PostAsync("oauth/token", new FormUrlEncodedContent(requestBody));
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var token = JsonSerializer.Deserialize<JsonElement>(json).GetProperty("access_token").GetString();
        return token!;
    }
    public async Task<List<Auth0UserDto>> SyncUsers()
    {
        var client = clientFactory.CreateClient("Auth0Client");
        var token = await GetManagementTokenAsync();
        var request = new HttpRequestMessage(HttpMethod.Get, "api/v2/users");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var users = JsonSerializer.Deserialize<List<Auth0UserDto>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return users!;
    }
}

public class Auth0UserDto
{
    public string user_id { get; set; }
    public string email { get; set; }
    public string name { get; set; }
    public Dictionary<string, object> user_metadata { get; set; }
}
