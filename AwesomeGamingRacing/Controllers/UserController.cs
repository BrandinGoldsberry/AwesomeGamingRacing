using AwesomeGamingRacing.Data;
using AwesomeGamingRacing.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AwesomeGamingRacing.Controllers
{
    public class UserController : Controller
    {
        private IConfiguration _configuration;
        private IUserRepository _userRepository;
        private IImageManager _imageManager;

        public UserController(IConfiguration config, IUserRepository userRepository, IImageManager imageManager)
        {
            _configuration = config;
            _userRepository = userRepository;
            _imageManager = imageManager;
        }

        public IActionResult GetUser()
        {
            return View();
        }
        #region SignUp
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromForm] User user)
        {
            ModelState.Remove("Salt");
            ModelState.Remove("Bio");
            ModelState.Remove("Role");
            ModelState.Remove("Image");
            ModelState.Remove("RowId");
            ModelState.Remove("ImageBlob");
            user.Image = new Uri(_imageManager.BaseImagePath + "/" + _imageManager.DefaultImage);
            user.Role = "Member";
            if (ModelState.IsValid)
            {
                byte[] salt = null;
                string passHash = Hasher.HashString(user.NewPassword, out salt);
                user.NewPassword = passHash;
                user.Salt = salt;
                bool success = await _userRepository.AddUser(user);
                if(success)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    return View(user);
                }
            }
            else
            {
                return View(user);
            }
        }
        #endregion

        #region Login
        [AllowAnonymous]
        public IActionResult Login([FromQuery] string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] LoginRequest Login)
        {
            User user = _userRepository.GetUser(Login.UserName);
            if(user != null)
            {
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20),
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
                HttpContext.Response.Cookies.Append("isLoggedIn", "true");

                return Redirect(Login.ReturnUrl ?? "/");
            }
            else
            {
                ModelState.AddModelError("notfound", "User not found");
                return View(Login);
            }

        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
        #endregion
        [Authorize(Roles = "Administrator")]
        public IResult TestAuth()
        {
            return Results.Ok("Success!");
        }

        #region PrivateMethods

        #endregion
    }
}
