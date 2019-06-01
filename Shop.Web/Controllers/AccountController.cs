namespace Shop.Web.Controllers
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Data.Entities;
    using Helpers;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using Models;

    public class AccountController : Controller
    {
        private readonly IUserHelper userHelper;
        private readonly IConfiguration configuration;

        // Constructor que inyecta la IUserHelper que es la interface de la clase UserHelper personalizada para administrar usuarios
        // y IConfiguration que es para acceder a los valores de configuracion en el appsettings.json
        public AccountController(IUserHelper userHelper, IConfiguration configuration)
        {
            this.userHelper = userHelper;
            this.configuration = configuration;
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


        // Acción GET para modificar de usuario

        public async Task<IActionResult> ChangeUser()
        {
            // Método para validar si existe y traer el usuario ingresado
            var user = await this.userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var model = new ChangeUserViewModel();

            if (user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
            }

            return this.View(model);
        }

        // Acción POST para modificar de usuario

        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                // Método para validar si existe y traer el usuario ingresado
                var user = await this.userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;

                    // Método para modificar usuario
                    var respose = await this.userHelper.UpdateUserAsync(user);

                    if (respose.Succeeded)
                    {
                        this.ViewBag.UserMessage = "User updated!";
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, respose.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User no found.");
                }
            }

            return this.View(model);
        }

        // Acción GET para modificar password de usuario

        public IActionResult ChangePassword()
        {
            return this.View();
        }

        // Acción POST para modificar password de usuario

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                // Método para validar si existe y traer el usuario ingresado
                var user = await this.userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                if (user != null)
                {
                    // Método para modificar password 
                    var result = await this.userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                    if (result.Succeeded)
                    {
                        return this.RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User no found.");
                }
            }

            return this.View(model);
        }

        // Método que al no tener GET no se puede consumir por web, al solo tener POST se consume por un llamado de Api 
        // [FromBody] Para forzar a la API web a leer un tipo en el cuerpo de la solicitud, en este caso se envia un LoginViewModel

        // Acción POST para crear un Token (autenticación basada en Token (JWT Json Web Token) para proteger nuestras Apis de usuarios no autorizados)
        // Este es el método que usamos para hacer Login en la APLICACIÓN MÓVIL

        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                // Método para validar si existe y traer el usuario ingresado
                var user = await this.userHelper.GetUserByEmailAsync(model.Username);

                if (user != null)
                {
                    // Método para validar si el usuario y password ingresados son validos para iniciar sesión
                    var result = await this.userHelper.ValidatePasswordAsync(
                        user,
                        model.Password);

                    if (result.Succeeded)
                    {
                        // Se genera una coleccion de nombre Claims (notificaciones) con el usuario y con un Guid para que cada llamado sea unico
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        // Se genera la llave con una clave simetrica, se trae el "Tokens:Key" del appsettings.json
                        // SymmetricSecurityKey no se puede decompilar | AsymmetricSecurityKey se puede compilar y decompilar
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Tokens:Key"]));

                        // Se generan las credenciales mediante la llave de seguridad generada y es encriptada mediante un algoritmo Sha256
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        // Se genera el token mediante los "Tokens:Issuer", "Tokens:Audience" del appsettings.json, claims, tiempo de expiración 
                        var token = new JwtSecurityToken(
                            this.configuration["Tokens:Issuer"],
                            this.configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(15),
                            signingCredentials: credentials);

                        var results = new
                        {
                            // Serializa un JwtSecurityToken en un JWT en formato de serialización compacto
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        // Crea un objeto CreateResult que produce una respuesta Microsoft.AspNetCore.Http.StatusCodes.Status201Created
                        return this.Created(string.Empty, results);
                    }
                }
            }

            // Crea un BadRequestResult que produce una respuesta Status400BadRequest (solicitud incorrecta)
            return this.BadRequest();
        }

    }
}
