using FirstMvcWebApp.Interfaces;
using FirstMvcWebApp.ViewModel;
using FirstMvcWebApp.Models;
using Newtonsoft.Json;
using System.Text;
using FirstMvcWebApp.Mappers;
using FirstMvcWebApp.DTOs;
using System.Net.Http.Headers;

namespace FirstMvcWebApp.Services;

public class AccountService : IAccountService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _httpClient = httpClientFactory.CreateClient("ApiClient");
    }

    private class TokenResponse
    {
        public string Token { get; set; }
    }

    public async Task<string> Login(LogInViewModel logInViewModel)
    {
        try
        {

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("Users/Login", logInViewModel);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to log in. Status: {response.StatusCode}");
                return null; // Handle login failure
            }

            // Read the response content only once
            var responseJson = await response.Content.ReadAsStringAsync();

            // Store the token in a cookie
            _httpContextAccessor.HttpContext.Response.Cookies.Append("jwt", responseJson, new CookieOptions
            {
                HttpOnly = true,  // Prevents JavaScript access (mitigates XSS attacks)
                Secure = true,    // Ensures cookie is sent only over HTTPS (important in production)
                SameSite = SameSiteMode.Lax, // Allows the cookie to be sent in cross-site requests
                Expires = DateTime.UtcNow.AddMinutes(480) // Set the cookie expiration time
            });

            var jwtToken = await response.Content.ReadAsStringAsync();
            return jwtToken.ToString();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error logging in: {ex.Message}");
            return null;
        }
    }

    public async Task Logout()
    {
        try
        {
            HttpResponseMessage response = await _httpClient.PostAsync("Users/LogOut", null);

            // Remove the JWT token cookie
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("jwt");

            Console.WriteLine("User logged out successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error logging out: {ex.Message}");
        }
    }


    public async Task<List<UserModel>> GetAllUsers()
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync("Users");
            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadFromJsonAsync<List<UserModel>>();
                return users ?? new List<UserModel>();
            }
            else
            {
                Console.WriteLine($"Failed to fetch users. Status: {response.StatusCode}");
                return new List<UserModel>();
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error fetching users: {ex.Message}");
            return new List<UserModel>();
        }
    }


    // /// <summary>
    // /// Fetches all users from the API.
    // /// </summary>
    // /// <returns>List of users.</returns>
    // public async Task<List<UserModel>> GetAllUsers()
    // {
    //     try
    //     {
    //         // Retrieve the JWT token from the cookie
    //         // Retrieve the token from cookies
    //         var token = _httpContextAccessor.HttpContext.Request.Cookies["jwt"];
    //         if (string.IsNullOrEmpty(token))
    //         {
    //             // return "Unauthorized: No token found";
    //             return null;
    //         }

    //         // Set Authorization header
    //         _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    //         HttpResponseMessage response = await _httpClient.GetAsync("Users");
    //         if (response.IsSuccessStatusCode && (Convert.ToInt32(response.StatusCode) == 200))
    //         {
    //             var users = await response.Content.ReadFromJsonAsync<List<UserModel>>();
    //             return users ?? new List<UserModel>();
    //         }
    //         else
    //         {
    //             Console.WriteLine($"Failed to fetch users. Status: {response.StatusCode}");
    //             return new List<UserModel>();
    //         }
    //     }
    //     catch (HttpRequestException ex)
    //     {
    //         Console.WriteLine($"Error fetching users: {ex.Message}");
    //         return new List<UserModel>();
    //     }
    // }

    /// <summary>
    /// Fetches a single user by ID from the API.
    /// </summary>
    /// <param name="id">User ID.</param>
    /// <returns>User details.</returns>
    public async Task<UserModel> GetUserById(int id)
    {
        try
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["jwt"];
            if (string.IsNullOrEmpty(token))
            {
                // return "Unauthorized: No token found";
                return null;
            }

            // Set Authorization header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"Users/{id}");
            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadFromJsonAsync<UserModel>();
                return user ?? new UserModel();
            }
            else
            {
                Console.WriteLine($"Failed to fetch user with ID {id}. Status: {response.StatusCode}");
                return null;
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error fetching user by ID: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Creates a new user by sending data to the API.
    /// </summary>
    /// <param name="signUpViewModel">User sign-up details.</param>
    /// <returns>Created user details.</returns>
    public async Task<UserModel> CreateUser(SignUpViewModel signUpViewModel)
    {
        try
        {
            var userModel = signUpViewModel.FromSignUpViewModelToCreateUserRequestDto();
            string json = JsonConvert.SerializeObject(userModel);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("Users", httpContent);

            if (response.IsSuccessStatusCode)
            {
                var createdUser = await response.Content.ReadFromJsonAsync<UserModel>();
                return createdUser;
            }
            else
            {
                Console.WriteLine($"Failed to create user. Status: {response.StatusCode}");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating user: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Updates an existing user in the API.
    /// </summary>
    /// <param name="id">User ID.</param>
    /// <param name="editViewModel">Updated user details.</param>
    /// <returns>Updated user details.</returns>
    public async Task<UserDto> UpdateUser(int id, EditViewModel editViewModel)
    {
        try
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["jwt"];
            if (string.IsNullOrEmpty(token))
            {
                // return "Unauthorized: No token found";
                return null;
            }

            // Set Authorization header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var editUserDto = editViewModel.ToUserDto();
            string json = JsonConvert.SerializeObject(editUserDto);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"Users/{id}", httpContent);

            if (response.IsSuccessStatusCode)
            {
                var updatedUser = await response.Content.ReadFromJsonAsync<UserDto>();
                return updatedUser;
            }
            else
            {
                Console.WriteLine($"Failed to update user with ID {id}. Status: {response.StatusCode}");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating user: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Deletes a user by ID from the API.
    /// </summary>
    /// <param name="id">User ID.</param>
    /// <returns>True if successful; otherwise false.</returns>
    public async Task<bool> DeleteUser(int id)
    {
        try
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["jwt"];
            if (string.IsNullOrEmpty(token))
            {
                // return "Unauthorized: No token found";
                return false;
            }

            // Set Authorization header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.DeleteAsync($"Users/{id}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                Console.WriteLine($"Failed to delete user with ID {id}. Status: {response.StatusCode}");
                return false;
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error deleting user: {ex.Message}");
            return false;
        }
    }
}
