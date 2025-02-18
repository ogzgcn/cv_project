namespace Logimat.MVC.Models;

public class LoginResponse
{
    public bool Success { get; set; }
    public string Token { get; set; } // JWT token if login is successful
    public string ErrorMessage { get; set; } // Optional error message if login fails
}