using Microsoft.AspNetCore.Mvc;
using FirstMvcWebApp.ViewModel;
using FirstMvcWebApp.Interfaces;
using FirstMvcWebApp.Mappers;

namespace FirstMvcWebApp.Controllers;

[CheckSession]
public class AccountController : Controller
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View("Login");
    }

    [HttpPost]
    public async Task<IActionResult> Login(LogInViewModel logInViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(logInViewModel);
        }

        var res = await _accountService.Login(logInViewModel);
        if (res != null)
        {
            // Store IsLoggedIn true in session 
            HttpContext.Session.SetString("IsLoggedIn", "true");       // Mark as Logged In
            TempData["SuccessMessage"] = "User logged in successfully.";
            return RedirectToAction(nameof(GetAllUsers));
        }
        else
        {
            TempData["ErrorMessage"] = "Failed to log in.";
            return RedirectToAction(nameof(Login));
        }
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _accountService.Logout();
        TempData["SuccessMessage"] = "User logged out successfully.";
        HttpContext.Session.SetString("IsLoggedIn", "false");       // Mark as Logged Out
        return RedirectToAction(nameof(Login));
    }

    public IActionResult Users()
    {
        return View("Users");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _accountService.GetAllUsers();
        return View("Users", users);
    }

    [HttpGet]
    public async Task<IActionResult> EditUser(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid user ID.");
        }

        var user = await _accountService.GetUserById(id);
        if (user == null)
        {
            return NotFound($"User with ID {id} not found.");
        }

        var editViewModel = user.ToEditUser();
        return View("EditUser", editViewModel);
    }

    public IActionResult SignUp()
    {
        return View("SignUp");
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpViewModel signUpViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(signUpViewModel);
        }

        var user = await _accountService.CreateUser(signUpViewModel);
        TempData["SuccessMessage"] = "User created successfully.";
        return RedirectToAction(nameof(GetAllUsers));
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUser([FromQuery] string userId, EditViewModel editViewModel)
    {
        if (!int.TryParse(userId, out int id) || id <= 0)
        {
            return BadRequest("Invalid user ID.");
        }
        if (!editViewModel.UpdatePassword)
        {
            ModelState.Remove(nameof(editViewModel.Password));
            ModelState.Remove(nameof(editViewModel.ConfirmPassword));
        }

        if (!ModelState.IsValid)
        {
            return View("EditUser", editViewModel);
        }

        var updatedUser = await _accountService.UpdateUser(id, editViewModel);
        if (updatedUser == null)
        {
            return NotFound($"User with ID {id} not found.");
        }

        TempData["SuccessMessage"] = "User updated successfully.";
        return RedirectToAction(nameof(GetAllUsers));
    }

    [HttpGet]
    public async Task<IActionResult> DeleteUser([FromQuery] string userId)
    {
        if (!int.TryParse(userId, out int id) || id <= 0)
        {
            return BadRequest("Invalid user ID.");
        }

        var result = await _accountService.DeleteUser(id);
        if (!result)
        {
            TempData["ErrorMessage"] = $"Failed to delete user with ID {id}.";
        }
        else
        {
            TempData["SuccessMessage"] = "User deleted successfully.";
        }

        return RedirectToAction(nameof(GetAllUsers));
    }
}
