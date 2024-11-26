using SeaBattle.Models;
using SeaBattle.Data;
using SeaBattle.Entities;
namespace SeaBattle.Services;
public class UserService {
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository) {
        _userRepository = userRepository;
    }

    public async Task<UserModel?> LoginUserAsync(string username, string password) {
        var userEntity = await _userRepository.GetUserByUsernameAsync(username);

        if (userEntity == null || !VerifyPassword(password, userEntity.PasswordHash)) {
            return null;
        }
        var userModel = new UserModel {
            Id = userEntity.Id,
            Username = userEntity.Username,
            PasswordHash = userEntity.PasswordHash
        };

        return userModel;
    }

    public async Task<UserModel?> RegisterUserAsync(string username, string password) {
        var existing = await _userRepository.GetUserByUsernameAsync(username);
        if (existing != null) {
            return null;
        }

        var hashedPassword = HashPassword(password);
        var userEntity = new UserEntity { Username = username, PasswordHash = hashedPassword };
        try {
            int userId = await _userRepository.AddUserAsync(userEntity);
            var userModel = new UserModel {
                Id = userId,
                Username = userEntity.Username,
                PasswordHash = userEntity.PasswordHash
            };
            return userModel;
        }catch (Exception ex) {
            Console.Error.WriteLine($"Error upon creating user: {ex.Message}");
            return null;
        }
       
    }

    private bool VerifyPassword(string password, string storedHashed) {
        // TODO: store hashed
        return password == storedHashed;
    }

    private string HashPassword(string password) {
        // TODO: store hashed
        return password;
    }

    public async Task JoinGame(int id) {
        await _userRepository.JoinGame(id, SessionService.Activeuser.Id);
    }
}