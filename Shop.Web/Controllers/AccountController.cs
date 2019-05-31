namespace Shop.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Entities;
    using Helpers;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    public class AccountController : Controller
    {
        private readonly IUserHelper userHelper;

        // Constructor que inyecta la IUserHelper que es la interface de la clase UserHelper personalizada para administrar usuarios
        public AccountController(IUserHelper userHelper)
        {
            this.userHelper = userHelper;
        }

        // Acción GET para iniciar sesión

        // IActionResult: Define un contrato que representa el resultado de un método de acción.

        public IActionResult Login()
        {
            // Obtiene un valor boleano que indica si el usuario ha sido autenticado
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return this.View();
        }

        // Acción POST para iniciar sesión

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                // Método para iniciar sesión
                var result = await this.userHelper.LoginAsync(model);

                if (result.Succeeded)
                {
                    // Si el logueo viene con direccion de retorno (al iniciar sesión se redirige a la pagina solicitada del request)
                    if (this.Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return this.Redirect(this.Request.Query["ReturnUrl"].First());
                    }

                    return this.RedirectToAction("Index", "Home");
                }
            }

            this.ModelState.AddModelError(string.Empty, "Failed to login.");

            return this.View(model);
        }

        public async Task<IActionResult> Logout()
        {
            // Método para cerrar sesión
            await this.userHelper.LogoutAsync();

            return this.RedirectToAction("Index", "Home");
        }

        // Acción GET para registro de usuario

        public IActionResult Register()
        {
            return this.View();
        }

        // Acción POST para registro de usuario

        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                // Método para validar si existe y traer el usuario ingresado
                var user = await this.userHelper.GetUserByEmailAsync(model.Username);

                if (user == null)
                {
                    // Llenamos la entidad User
                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Username,
                        UserName = model.Username
                    };

                    // Método para crear usuario
                    var result = await this.userHelper.AddUserAsync(user, model.Password);

                    if (result != IdentityResult.Success)
                    {
                        this.ModelState.AddModelError(string.Empty, "The user couldn't be created.");

                        return this.View(model);
                    }

                    var loginViewModel = new LoginViewModel
                    {
                        Username = model.Username,
                        Password = model.Password,
                        RememberMe = false
                    };

                    // Método para iniciar sesión
                    var result2 = await this.userHelper.LoginAsync(loginViewModel);

                    if (result2.Succeeded)
                    {
                        return this.RedirectToAction("Index", "Home");
                    }

                    this.ModelState.AddModelError(string.Empty, "The user couldn't be login.");

                    return this.View(model);
                }

                this.ModelState.AddModelError(string.Empty, "The username is already registered.");
            }

            return this.View(model);
        }

    }
}
