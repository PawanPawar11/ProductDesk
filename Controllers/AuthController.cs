using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductDesk.Data;
using ProductDesk.Dto;
using ProductDesk.Models;

namespace ProductDesk.Controllers
{
    public class AuthController(AppDbContext context) : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

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

            var doesUserExist = await context.Users.FirstOrDefaultAsync(User => User.Email == userDto.Email);

            if(doesUserExist == null)
            {
                var user = new User
                {
                    Username = userDto.Username,
                    Email = userDto.Email,
                    Password = userDto.Password,
                };

                context.Users.Add(user);
                await context.SaveChangesAsync();

                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                ViewBag.Message = "User already exits. Please Login!";
                return View("Register");
            }
        }

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

            var doesUserExist = await context.Users.FirstOrDefaultAsync(User => User.Email == userDto.Email);

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

            return RedirectToAction("Index", "Dashboard");
        }
    }
}
