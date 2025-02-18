using System.ComponentModel.DataAnnotations;

namespace Logimat.MVC.ViewModel;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Ad ve Soyad gereklidir.")]
    [StringLength(100, ErrorMessage = "Ad ve soyad 100 karakterden uzun olmamalıdır.")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Email gereklidir.")]
    [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Şifre gereklidir.")]
    [StringLength(100, ErrorMessage = "Şifre en az 6 karakter olmalıdır.", MinimumLength = 6)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Şifreyi onaylamanız gerekmektedir.")]
    [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
    public string ConfirmPassword { get; set; }
}