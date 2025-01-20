using Microsoft.AspNetCore.Mvc;
using FirstMvcWebApp.ViewModel;
using FirstMvcWebApp.Interfaces;
using FirstMvcWebApp.Models;
using FirstMvcWebApp.Mappers;
using FirstMvcWebApp.DTOs;

namespace FirstMvcWebApp.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    // Renders the login view
    public IActionResult Login()
    {
        return View("Login");
    }

    // Renders the sign-up view
    public IActionResult SignUp()
    {
        return View("SignUp");
    }

    // Renders the users view with an optional list of users
    public IActionResult Users()
    {
        return View("Users");
    }

    // Fetches all users and displays them in the "Users" view
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        if (!ModelState.IsValid)
        {
            return View("Users", new List<UserModel>()); // Return an empty view if the model is invalid
        }

        var users = await _accountService.GetAllUsers();
        return View("Users", users);
    }

    // Fetches a single user's details by ID and renders the "EditUser" view
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

    // Handles the sign-up logic
    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpViewModel signUpViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(signUpViewModel); // Return the view with validation errors
        }

        var user = await _accountService.CreateUser(signUpViewModel);
        TempData["SuccessMessage"] = "User created successfully.";
        return RedirectToAction(nameof(GetAllUsers));
    }

    // Updates an existing user
    [HttpPost]
    public async Task<IActionResult> UpdateUser([FromQuery] string userId, EditViewModel editViewModel)
    {
        if (!int.TryParse(userId, out int id) || id <= 0)
        {
            return BadRequest("Invalid user ID.");
        }
        if (!editViewModel.UpdatePassword)
        {
            // ModelState.Remove(nameof(editViewModel.Password));
            // ModelState.Remove(nameof(editViewModel.ConfirmPassword));
        }

        if (!ModelState.IsValid)
        {
            return View("EditUser", editViewModel); // Return to the edit view if validation fails
        }

        var updatedUser = await _accountService.UpdateUser(id, editViewModel);
        if (updatedUser == null)
        {
            return NotFound($"User with ID {id} not found.");
        }

        TempData["SuccessMessage"] = "User updated successfully.";
        return RedirectToAction(nameof(GetAllUsers));
    }

    // Deletes a user by ID
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
