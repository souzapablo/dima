using System.Net.Http.Json;
using System.Text;
using Dima.Core.Handlers;
using Dima.Core.Requests.Account;
using Dima.Core.Responses;

namespace Dima.Web.Handlers.Account;

public class AccountHandler(IHttpClientFactory httpClientFactory) : IAccountHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);

    public async Task<Response<string>> LoginAsync(LoginRequest request)
    {
        var result = await _client.PostAsJsonAsync("v1/identity/login?useCookies=true", request);
        return result.IsSuccessStatusCode 
            ? new Response<string>("Login realizado com sucesso", message: "Login realizado com sucesso")
            : new Response<string>(null, 400, "Não foi possível realizar login");
    }

    public async Task LogoutAsync()
    {
        var emptyContent = new StringContent("{}", Encoding.UTF8, "application/json");
        await _client.PostAsJsonAsync("v1/identity/logout", emptyContent);
    }

    public async Task<Response<string>> RegisterAsync(RegisterRequest request)
    {
        var result = await _client.PostAsJsonAsync("v1/identity/register", request);
        return result.IsSuccessStatusCode 
            ? new Response<string>("Cadastro realizado com sucesso", message: "Cadastro realizado com sucesso")
            : new Response<string>(null, 400, "Não foi possível realizar seu cadastro");
    }

}