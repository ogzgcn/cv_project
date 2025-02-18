using System.ComponentModel.DataAnnotations;

namespace Logimat.MVC.ViewModel;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email gereklidir.")]
    [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Şifre gereklidir.")]
    public string Password { get; set; }

}