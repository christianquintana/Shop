namespace Shop.Web.Helpers
{
    using System.Threading.Tasks;
    using Data.Entities;
    using Microsoft.AspNetCore.Identity;
    using Shop.Web.Models;

    // Clase Helper personalizada para administrar usuarios    
    public class UserHelper : IUserHelper // Hereda de interface IUserHelper
    {
        //API: es una interfaz de programación de aplicaciones (Application Programming Interface)

        // Proporciona las API para administrar usuarios en un almacén de persistencia.
        private readonly UserManager<User> userManager;
        // Proporciona las API para el inicio de sesión del usuario.
        private readonly SignInManager<User> signInManager;

        // Constructor
        public UserHelper(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        // Método para validar si existe y traer el usuario ingresado
        public async Task<User> GetUserByEmailAsync(string email)
        {
            // Busca un usuario por su e-mail.
            return await this.userManager.FindByEmailAsync(email);
        }

        // Método para crear usuario
        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            // Crea un usuario en el almacén de respaldo con la contraseña dada, como una operación asíncrona.
            return await this.userManager.CreateAsync(user, password);
        }

        // Método para iniciar sesión
        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            // Inicia sesión con el usuario utilizando el nombre de usuario y la contraseña
            return await this.signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.RememberMe,
                false);
        }

        // Método para cerrar sesión
        public async Task LogoutAsync()
        {
            // Cierra la sesión del usuario actual de la aplicación.
            await this.signInManager.SignOutAsync();
        }
    }
}
