namespace Shop.Web.Helpers
{
    using System.Threading.Tasks;
    using Data.Entities;
    using Microsoft.AspNetCore.Identity;
    using Models;

    // Clase Helper personalizada para administrar usuarios    
    public class UserHelper : IUserHelper // Hereda de interface IUserHelper
    {
        //API: es una interfaz de programación de aplicaciones (Application Programming Interface)

        // Proporciona las API para administrar usuarios en un almacén de persistencia.
        private readonly UserManager<User> userManager;
        // Proporciona las API para el inicio de sesión del usuario.
        private readonly SignInManager<User> signInManager;
        // Proporciona las API para administrar roles de usuario
        private readonly RoleManager<IdentityRole> roleManager;

        // Constructor que inyecta la clase UserManager para administracion de usuarios, SignInManager para administrar la sesión de usuario
        // y RoleManager para administrar roles de usuario
        public UserHelper(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
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
            // Crea un usuario con la contraseña especificada, como una operación asíncrona.
            return await this.userManager.CreateAsync(user, password);
        }

        // Método para iniciar sesión
        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            // Inicia sesión con el usuario utilizando el nombre de usuario y la contraseña, como una operación asíncrona.
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

        // Método para modificar usuario
        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            // Modifica un usuario con la contraseña especificada, como una operación asíncrona.
            return await this.userManager.UpdateAsync(user);
        }

        // Método para modificar password de usuario
        public async Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword)
        {
            // Cambia la contraseña de un usuario después de confirmar que la contraseña actual especificada es correcta, como una operación asíncrona
            return await this.userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        // Método para validar si el usuario y password ingresados son validos para iniciar sesión
        public async Task<SignInResult> ValidatePasswordAsync(User user, string password)
        {
            // Intenta iniciar sesión con la contraseña de usuario
            return await this.signInManager.CheckPasswordSignInAsync(
                user,
                password,
                false);
        }

        // Método para verificar si existe un Rol
        public async Task CheckRoleAsync(string roleName)
        {
            // Obtiene un indicador que indica si existe el nombre de rol especificado
            var roleExists = await this.roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
            {
                // Crea el rol especificado en el almacén de persistencia
                await this.roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }

        // Método para agregar usuario creado a un Rol 
        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            // Agrega el usuario especificado al rol nombrado
            await this.userManager.AddToRoleAsync(user, roleName);
        }

        // Método para validar si usuario pertenece a un Rol
        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            // Devuelve un indicador que indica si el usuario especificado es miembro del Rol indicado
            return await this.userManager.IsInRoleAsync(user, roleName);
        }
    }
}
