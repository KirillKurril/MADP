using ALWD.Domain.Models;
using ALWD.Domain.ViewModels;
using ALWD.UI.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ALWD.UI.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowLogin()
        {
            return View("Login", new AuthorizeUserViewModel());
        }

        public IActionResult ShowRegistration()
        {
            return View("Registration", new RegisterUserViewModel());
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel registerData, [FromServices] IAuthService authService)
        {
            if (ModelState.IsValid)
            {
                if (registerData == null)
                {
                    return BadRequest();
                }

                var response = await authService.RegisterUserAsync(
                    registerData.Email,
                    registerData.Password,
                    registerData.Name,
                    registerData.Avatar);

                if (response.Successfull)
                {
                    return Redirect(Url.Action("Index", "Home"));
                }
                else return BadRequest(response.ErrorMessage);
            }
            return View(registerData);
        }

        [HttpPost]
        public async Task<IActionResult> Login(AuthorizeUserViewModel loginData, [FromServices] IAuthService authService)
        {
            ResponseData<bool> response;
            try
            {
                response = await authService.AuthenticateUser(loginData.Name, loginData.Password);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            if (response.Successfull)
            {
                return Redirect(Url.Action("Index", "Home"));
            }
            else return BadRequest(response.ErrorMessage);
        }

        public IActionResult Logout()
        {
            return Ok();
        }
    }
}
