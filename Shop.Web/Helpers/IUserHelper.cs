namespace Shop.Web.Helpers
{
    using System.Threading.Tasks;
    using Data.Entities;
    using Microsoft.AspNetCore.Identity;
    using Models;

    // interface de la clase UserHelper personalizada para administrar usuarios
    public interface IUserHelper
    {
        // interface del método para validar si existe y traer el usuario ingresado
        Task<User> GetUserByEmailAsync(string email);

        // interface del método para crear usuario
        Task<IdentityResult> AddUserAsync(User user, string password);

        // interface del método para iniciar sesión
        Task<SignInResult> LoginAsync(LoginViewModel model);

        // interface del método para cerrar sesión
        Task LogoutAsync();

        // interface del método para modificar usuario
        Task<IdentityResult> UpdateUserAsync(User user);

        // interface del método para modificar password de usuario 
        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);

        // interface del método para validar si el usuario y password ingresados son validos para iniciar sesión
        Task<SignInResult> ValidatePasswordAsync(User user, string password);

    }
}
