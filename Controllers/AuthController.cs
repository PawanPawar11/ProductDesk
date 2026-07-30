using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductDesk.Data;
using ProductDesk.Dto;
using ProductDesk.Models;
using ProductDesk.Services;

namespace ProductDesk.Controllers
{
    public class AuthController(AppDbContext _dbContext, JwtTokenService _jwtTokenService) : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Delete JWT Cookie on Logout
            Response.Cookies.Delete("jwtToken");
            return RedirectToAction("Login", "Auth");
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(UserDto userDto)
        {
            if(userDto == null)
            {
                ViewBag.Message = "All fields (Username, Email and Password) are required in order to Register!";
                return View("Register");
            }

            if(userDto.Username == null || userDto.Email == null || userDto.Password == null)
            {
                ViewBag.Message = "Please check whether all the required fields (Username, Email and Password) are filled!";
                return View("Register");
            }

            var doesUserExist = await _dbContext.Users.FirstOrDefaultAsync(User => User.Email == userDto.Email);

            if(doesUserExist == null)
            {
                var user = new User
                {
                    Username = userDto.Username,
                    Email = userDto.Email,
                    Password = userDto.Password,
                };

                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();

                // Generate JWT Token
                string token = _jwtTokenService.GenerateToken(user);

                // Store JWT in HttpOnly Cookie
                SetJwtCookie(token);

                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                ViewBag.Message = "User already exits. Please Login!";
                return View("Register");
            }
        }

        [HttpPost]
        public async Task<IActionResult> LoginUser(UserDto userDto)
        {
            if(userDto == null)
            {
                ViewBag.Message = "All fields (Email and Password) are required in order to Login!";
                return View("Login");
            }

            if(userDto.Email == null || userDto.Password == null)
            {
                ViewBag.Message = "Please check whether all the required fields (Email and Password) are filled!";
                return View("Login");
            }

            var doesUserExist = await _dbContext.Users.FirstOrDefaultAsync(User => User.Email == userDto.Email);

            if(doesUserExist == null)
            {
                ViewBag.Message = "User doesn't exit. Please Register!";
                return View("Login");
            }
            
            if(doesUserExist.Password != userDto.Password)
            {
                ViewBag.Message = "Password is incorrect. Please type correct password in order to Login!";
                return View("Login");
            }

            // Generate JWT Token
            string token = _jwtTokenService.GenerateToken(doesUserExist);

            // Store JWT in HttpOnly Cookie
            SetJwtCookie(token);

            return RedirectToAction("Index", "Dashboard");
        }

        private void SetJwtCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // Protects against XSS attacks
                Secure = true,   // Set to true for HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(60)
            };
            Response.Cookies.Append("jwtToken", token, cookieOptions);
        }
    }
}
