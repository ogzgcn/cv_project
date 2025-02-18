using Logimat.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace Logimat.MVC.Controllers
{
    public class MaterialController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly INotyfService _notyf;

        public MaterialController(HttpClient httpClient, INotyfService notyf)
        {
            _httpClient = httpClient;
            _notyf = notyf;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
            {
                _notyf.Error("JWT token bulunamadı. Lütfen tekrar giriş yapın.");
                return RedirectToAction("Login", "Account");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("http://localhost:5170/materials");
            if (string.IsNullOrEmpty(response.Content.ReadAsStringAsync().Result)) 
            {
                return View(new List<Material>());
            }
            if (response.IsSuccessStatusCode)
            {
                var materials = await response.Content.ReadFromJsonAsync<IEnumerable<Material>>();
                return View(materials?.ToList());
            }
            _notyf.Error("Malzemeler alınırken hata oluştu.");
            return View(new List<Material>());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Material material)
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
            {
                _notyf.Error("JWT token bulunamadı. Lütfen tekrar giriş yapın");
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsJsonAsync("http://localhost:5170/materials", material);

                if (response.IsSuccessStatusCode)
                {
                    _notyf.Success("Malzeme başarıyla eklendi.");
                    return RedirectToAction("Index");
                }
                else
                {
                    _notyf.Error("Malzeme eklenirken hata oluştu.");
                }
            }

            return View(material);
        }
        public async Task<IActionResult> Details(Guid id)
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
            {
                _notyf.Error("JWT token bulunamadı. Lütfen tekrar giriş yapın.");

                return RedirectToAction("Login", "Account");
            }

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:5170/materials/{id}");
            requestMessage.Headers.Add("Authorization", $"Bearer {token}");

            var response = await _httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                var material = await response.Content.ReadFromJsonAsync<Material>();
                return View(material);
            }

            _notyf.Error("Malzeme bilgisi alınırken hata oluştu.");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
            {
                _notyf.Error("JWT token bulunamadı. Lütfen tekrar giriş yapın.");
                return RedirectToAction("Login", "Account");
            }

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:5170/materials/{id}");
            requestMessage.Headers.Add("Authorization", $"Bearer {token}");

            var response = await _httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                var material = await response.Content.ReadFromJsonAsync<Material>();
                return View(material);
            }

            _notyf.Error("Malzeme bilgisi alınırken hata oluştu.");
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Material material)
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
            {
                _notyf.Error("JWT token bulunamadı. Lütfen tekrar giriş yapın.");
                return RedirectToAction("Login", "Account");
            }

            if (id != material.Id)
            {
                return BadRequest("ID uyuşmuyor.");
            }

            var jsonContent = new StringContent(JsonConvert.SerializeObject(material), Encoding.UTF8, "application/json");

            var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"http://localhost:5170/materials/{id}")
            {
                Content = jsonContent
            };
            requestMessage.Headers.Add("Authorization", $"Bearer {token}");

            var response = await _httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            _notyf.Error("Güncelleme sırasında hata oluştu.");
            return View(material);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
            {
                _notyf.Error("JWT token bulunamadı. Lütfen tekrar giriş yapın.");
                return RedirectToAction("Login", "Account");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"http://localhost:5170/materials/{id}");

            if (response.IsSuccessStatusCode)
            {
                _notyf.Success("Malzeme başarıyla silindi");
            }
            else
            {
                _notyf.Error("Malzeme silinirken hata oluştu.");
            }

            return RedirectToAction("Index");
        }
    }
}
