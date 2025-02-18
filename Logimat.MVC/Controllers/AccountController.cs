using Microsoft.AspNetCore.Mvc;
using Logimat.MVC.ViewModel;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;

public class AccountController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly INotyfService _notyf;
    public AccountController(IHttpClientFactory httpClientFactory, INotyfService notyf)
    {
        _httpClientFactory = httpClientFactory;
        _notyf = notyf;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("http://localhost:5170/register", content);

            if (response.IsSuccessStatusCode)
            {
                _notyf.Success("Kullanıcı başarıyla oluşturuldu.");
                return RedirectToAction("Login");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                _notyf.Error( "Kullanıcı oluşturulurken bir hata oluştu: "+error);
            }
        }
        else
        {
            _notyf.Warning("Lütfen gerekli alanları doldurun.");
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("http://localhost:5170/login", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<dynamic>(result);

                if (tokenResponse?.token != null)
                {
                    HttpContext.Session.Set("JwtToken", Encoding.UTF8.GetBytes(tokenResponse.token.ToString()));
                    TempData["SuccessMessage"] = "Giriş başarılı.";
                    return RedirectToAction("Index", "Material");
                }
                else
                {
                    _notyf.Error("Geçersiz kullanıcı adı veya şifre");

                }
            }
            else
            {
                _notyf.Error("Geçersiz kullanıcı adı veya şifre");

            }
        }

        return View(model);
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Remove("JwtToken");
        TempData["SuccessMessage"] = "Çıkış başarılı.";
        return RedirectToAction("Login");
    }
}
