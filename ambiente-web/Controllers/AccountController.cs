using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ambiente_web.Data;
using ambiente_web.Models;

namespace ambiente_web.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        private static readonly Dictionary<string, (int Tentativas, DateTime BloqueadoAte)> _tentativasLogin = new();

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string senha, string? returnUrl = null)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            if (_tentativasLogin.TryGetValue(ip, out var info))
            {
                if (info.Tentativas >= 5 && DateTime.Now < info.BloqueadoAte)
                {
                    var segundosRestantes = (int)(info.BloqueadoAte - DateTime.Now).TotalSeconds;
                    ModelState.AddModelError("", $"Conta bloqueada. Tente novamente em {segundosRestantes} segundos.");
                    ViewBag.ReturnUrl = returnUrl;
                    return View();
                }
                if (info.Tentativas >= 5 && DateTime.Now >= info.BloqueadoAte)
                {
                    _tentativasLogin.Remove(ip);
                }
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email && u.Ativo);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(senha, usuario.Senha))
            {
                if (!_tentativasLogin.ContainsKey(ip))
                    _tentativasLogin[ip] = (0, DateTime.MinValue);

                var tentativas = _tentativasLogin[ip].Tentativas + 1;
                if (tentativas >= 5)
                {
                    _tentativasLogin[ip] = (tentativas, DateTime.Now.AddMinutes(5));
                    ModelState.AddModelError("", "Número máximo de tentativas excedido. Conta bloqueada por 5 minutos.");
                }
                else
                {
                    _tentativasLogin[ip] = (tentativas, DateTime.MinValue);
                    ModelState.AddModelError("", $"E-mail ou senha inválidos. Tentativa {tentativas}/5.");
                }

                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            _tentativasLogin.Remove(ip);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim("Perfil", usuario.Perfil)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                });

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}