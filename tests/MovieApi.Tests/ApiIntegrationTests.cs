using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using MovieApi.Modules.Customers.Contracts;
using MovieApi.Modules.Identity.Contracts.Auth;
using MovieApi.Modules.Identity.Contracts.Users;
using MovieApi.Modules.Movies.Contracts;

namespace MovieApi.Tests;

public sealed class ApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private const string AdminUsername = "admin";
    private const string AdminPassword = "Admin123!";
    private const string UserUsername = "user";
    private const string UserPassword = "User123!";

    private readonly HttpClient _client;

    public ApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact(DisplayName = "Movies endpoint rejects anonymous requests")]
    public async Task MoviesRequireAuthentication()
    {
        var response = await _client.GetAsync("/api/movies");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact(DisplayName = "Login returns a token that can access protected movie endpoints")]
    public async Task LoginReturnsTokenAndAllowsMovieList()
    {
        var login = await LoginAsync(AdminUsername, AdminPassword);
        UseBearerToken(login.AccessToken);

        var movies = await _client.GetFromJsonAsync<MovieResponse[]>("/api/movies");

        Assert.NotNull(movies);
        Assert.Contains(movies, movie => movie.Title == "The Matrix");
    }

    [Fact(DisplayName = "Authenticated users can complete movie CRUD")]
    public async Task AuthenticatedUserCanCreateReadUpdateAndDeleteMovie()
    {
        var login = await LoginAsync(UserUsername, UserPassword);
        UseBearerToken(login.AccessToken);

        var createResponse = await _client.PostAsJsonAsync("/api/movies", new MovieRequest
        {
            Title = "Interstellar",
            Genre = "Science Fiction",
            ReleaseYear = 2014,
            Director = "Christopher Nolan"
        });
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var created = await createResponse.Content.ReadFromJsonAsync<MovieResponse>();
        Assert.NotNull(created);
        Assert.Equal("Interstellar", created.Title);

        var readResponse = await _client.GetAsync($"/api/movies/{created.Id}");
        Assert.Equal(HttpStatusCode.OK, readResponse.StatusCode);

        var updateResponse = await _client.PutAsJsonAsync($"/api/movies/{created.Id}", new MovieRequest
        {
            Title = "Interstellar",
            Genre = "Adventure",
            ReleaseYear = 2014,
            Director = "Christopher Nolan"
        });
        Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

        var deleteResponse = await _client.DeleteAsync($"/api/movies/{created.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var readAfterDeleteResponse = await _client.GetAsync($"/api/movies/{created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, readAfterDeleteResponse.StatusCode);
    }

    [Fact(DisplayName = "Authenticated users can complete customer CRUD")]
    public async Task AuthenticatedUserCanCreateReadUpdateAndDeleteCustomer()
    {
        var login = await LoginAsync(UserUsername, UserPassword);
        UseBearerToken(login.AccessToken);

        var createResponse = await _client.PostAsJsonAsync("/api/customers", new CustomerRequest
        {
            FullName = "Ada Lovelace",
            Email = "ADA.LOVELACE@EXAMPLE.COM",
            PhoneNumber = "+44 20 0000 0000"
        });
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var created = await createResponse.Content.ReadFromJsonAsync<CustomerResponse>();
        Assert.NotNull(created);
        Assert.Equal("Ada Lovelace", created.FullName);
        Assert.Equal("ada.lovelace@example.com", created.Email);

        var readResponse = await _client.GetAsync($"/api/customers/{created.Id}");
        Assert.Equal(HttpStatusCode.OK, readResponse.StatusCode);

        var updateResponse = await _client.PutAsJsonAsync($"/api/customers/{created.Id}", new CustomerRequest
        {
            FullName = "Ada Byron",
            Email = "ada.byron@example.com",
            PhoneNumber = null
        });
        Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

        var deleteResponse = await _client.DeleteAsync($"/api/customers/{created.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var readAfterDeleteResponse = await _client.GetAsync($"/api/customers/{created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, readAfterDeleteResponse.StatusCode);
    }

    [Fact(DisplayName = "Current user endpoint returns the caller profile")]
    public async Task CurrentUserProfileReturnsCallerData()
    {
        var login = await LoginAsync(UserUsername, UserPassword);
        UseBearerToken(login.AccessToken);

        var profile = await _client.GetFromJsonAsync<UserResponse>("/api/users/me");

        Assert.NotNull(profile);
        Assert.Equal(login.User.Id, profile.Id);
        Assert.Equal("user", profile.Username);
    }

    [Fact(DisplayName = "Regular users cannot list all users")]
    public async Task RegularUserCannotListUsers()
    {
        var login = await LoginAsync(UserUsername, UserPassword);
        UseBearerToken(login.AccessToken);

        var response = await _client.GetAsync("/api/users");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact(DisplayName = "Admins can list all users")]
    public async Task AdminCanListUsers()
    {
        var login = await LoginAsync(AdminUsername, AdminPassword);
        UseBearerToken(login.AccessToken);

        var users = await _client.GetFromJsonAsync<UserResponse[]>("/api/users");

        Assert.NotNull(users);
        Assert.Contains(users, user => user.Username == "admin");
        Assert.Contains(users, user => user.Username == "user");
    }

    [Fact(DisplayName = "Invalid login returns unauthorized")]
    public async Task InvalidLoginReturnsUnauthorized()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new LoginRequest
        {
            Username = AdminUsername,
            Password = "wrong-password"
        });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact(DisplayName = "Swagger document exposes endpoints from every module")]
    public async Task SwaggerDocumentExposesModularEndpoints()
    {
        var response = await _client.GetAsync("/swagger/v1/swagger.json");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);
        var pathNames = document.RootElement
            .GetProperty("paths")
            .EnumerateObject()
            .Select(path => path.Name)
            .ToArray();

        Assert.Contains("/api/auth/login", pathNames);
        Assert.Contains("/api/users/me", pathNames);
        Assert.Contains("/api/movies", pathNames);
        Assert.Contains("/api/customers", pathNames);
    }

    private async Task<LoginResponse> LoginAsync(string username, string password)
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new LoginRequest
        {
            Username = username,
            Password = password
        });
        response.EnsureSuccessStatusCode();

        var login = await response.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(login);

        return login;
    }

    private void UseBearerToken(string accessToken)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    }
}
