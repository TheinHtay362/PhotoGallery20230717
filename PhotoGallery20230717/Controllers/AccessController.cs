using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using PhotoGallery20230717.Models;

namespace PhotoGallery20230717.Controllers
{
    public class AccessController : Controller
    {
        public IActionResult Login()
        {
            // Check if the user is already logged in
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Photo");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel modelLogin)
        {
            // Here use a query to a database for users
            if(modelLogin.UserName == "user@gmail.com" && modelLogin.Password == "123")
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, modelLogin.UserName),
                    new Claim("OtherProperties", "Example Role")
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = modelLogin.KeepLoggedIn
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);

                return RedirectToAction("Index", "Photo");
            }
            ViewData["ValidateMessage"] = "User not found";
            return View();
        }
    }
}
