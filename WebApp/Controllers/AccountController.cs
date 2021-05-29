using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WebApp.Models;
using System.Collections.Generic;
using WebApp.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            Dictionary<int, int> years = new Dictionary<int, int>();
            for (int i = 1900; i < 2006; i++)
                years[i]=i;
            ViewBag.Years = new SelectList(years, "Key", "Value");
            return View();
        }
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, code);
                if (result.Succeeded)
                {
                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);
                    return View("AcceptEmail", "Почта успешно подтверждена, теперь можете войти в свой аккаунт");
                }
                else return View("AcceptEmail", "Не удалось подтвердить аккаунт");
            }
            else
            {
                ModelState.AddModelError("", "Пользователь не найден");
            }
            return View("Home", "Index");
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email, Year = Convert.ToInt32(model.Year) };
                // додаємо користувача
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // установка кукі
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    await _userManager.AddToRoleAsync(user, "user");
                    var callback = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    EmailService emailService = new EmailService();
                  await emailService.SendEmail(model.Email, "Подтвердите свой аккаунт", $"Подтврердите регистрацию, перейдя по ссылке <a href='{callback}'>link</a>");
                   
                    return View("AcceptEmail","Для завершения регистрации проверьте свою почту и перейдите по ссылке");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            Dictionary<int, int> years = new Dictionary<int, int>();
            for (int i = 1900; i < 2006; i++)
                years[i] = i;
            ViewBag.Years = new SelectList(years, "Key", "Value");
            return View(model);
        }
        [HttpGet]
        public IActionResult Login(string returnURL = null)
        {
            return View(new LoginViewModel { ReturnURL = returnURL });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if(result.Succeeded)
                {
                    if(!(await _userManager.FindByEmailAsync(model.Email)).EmailConfirmed)
                    {
                        await Logout();
                        return View("AcceptEmail","Подтвердите сперва свою почту");
                    }
                    if (!string.IsNullOrEmpty(model.ReturnURL) && Url.IsLocalUrl(model.ReturnURL))
                    {
                        return Redirect(model.ReturnURL);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Concerts");
                    }
                }
            }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин или пароль");
                }
                return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> ChangePassword() => View(new ChangePasswordUserViewModel { Id = (await _userManager.FindByNameAsync(User.Identity.Name)).Id });
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordUserViewModel model)
        {
            if(ModelState.IsValid)
            {
                User user =await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                   IdentityResult result= await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        await _userManager.UpdateAsync(user);
                        return await Logout();
                    }
                    else foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
                }
                else ModelState.AddModelError(string.Empty, "Пользователь не был найден");
            }
            return View(model);
        }
    }
}