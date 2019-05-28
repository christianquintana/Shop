namespace Shop.Web.Helpers
{
    using System.Threading.Tasks;
    using Data.Entities;
    using Microsoft.AspNetCore.Identity;
    using Shop.Web.Models;

    // interface de clase Helper personalizada para administrar usuarios
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

    }
}
