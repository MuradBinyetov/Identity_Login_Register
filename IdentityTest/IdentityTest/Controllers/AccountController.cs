using AutoMapper;
using IdentityTest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Ns = Microsoft.AspNetCore.Identity;

namespace IdentityTest.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserDbContext _context;
        private readonly UserManager<AppUser> _manager;
        private readonly IMapper mapper;
        private readonly IPasswordValidator<AppUser> password;
        private readonly SignInManager<AppUser> signIn;
        public AccountController(UserDbContext context, UserManager<AppUser> manager, IMapper _mapper, IPasswordValidator<AppUser> _password, SignInManager<AppUser> _signIn)
        {
            _context = context;
            _manager = manager;
            mapper = _mapper;
            password = _password;
            signIn = _signIn;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register register)
        {
             if(!ModelState.IsValid)
            {
                return View(nameof(Register));
            }


            AppUser appUser = await _manager.FindByEmailAsync(register.Email);
            var a = ModelState;
            if (appUser != null)
            {
                ModelState.AddModelError("", "bu email istifade olunub");
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var user = mapper.Map<AppUser>(register);
                    var regist = await _manager.CreateAsync(user, register.Password);
                    if (regist.Succeeded)
                    {
                        return View(nameof(Login));
                    }
                    else
                    {
                        foreach (var item in regist.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }
                }
               
               
            }
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {

            if (!ModelState.IsValid)
            {
                return View(nameof(Login));
            }

            AppUser currentUser = await _manager.FindByEmailAsync(login.Email);

            var checkPassword = password.ValidateAsync(_manager,currentUser,login.Password);


            if(currentUser != null && checkPassword.Result.Succeeded)
            {
              if(await signIn.UserManager.IsInRoleAsync(currentUser, "Admin"))
                {
                    return Content("Salam Admin");
                }
                else
                {
                    Ns.SignInResult signInResult = await signIn.PasswordSignInAsync(currentUser, login.Password, true, false);
                    if (signInResult.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return View(nameof(Register));
                    }
                }
            }
            return View();
        }

    }
}