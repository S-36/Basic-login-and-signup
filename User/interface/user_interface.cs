using Login_and_Signup.Error;
using Login_and_Signup.User.model;

namespace Login_and_Signup.User.Interface
{
    // Se dividen las interfaces en Repository y Service
    // Repositore para eliminar, guardar o actualizar datos o hacer CRUD, y Service para la logica del negocio 
    /*
    ## La regla resumida

    Una interfaz nueva cuando:

    1. Hay una entidad nueva en la DB
       → IXxxRepository + IXxxService

    2. Hay una responsabilidad que no encaja en las que existen
       → IEmailService, ITokenService, etc.

    3. Una interfaz existente supera los 5-6 métodos
       → Señal de que está haciendo demasiado
    */
    // CRUD DE USUARIOS
    public interface IUserRepository
    {
        Task CreateUser(UserModel user);
        Task DeleteUser(string password);
        Task UpdateUser(UserModel user);
        Task<UserModel?> GetUserByEmail (string email);
    }
    // Servicios y Logica para Usuarios
    public interface IUserService
    {
        Task<Result<string>> LoginAsync(string email, string password);
        Task<Result> RegisterAsync(string name, string email, string password);
        Task<Result> ValidateEmailAsync(string email);

    }


}