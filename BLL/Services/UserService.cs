using SeaBattle.Models;
using SeaBattle.Data;
using SeaBattle.Entities;
namespace SeaBattle.Services;
public class UserService {
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository) {
        _userRepository = userRepository;
    }

    public UserModel? LoginUser(string username, string password) {
        var userEntity = _userRepository.GetUserByUsername(username);

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

    public UserModel? RegisterUser(string username, string password) {
        var existing = _userRepository.GetUserByUsername(username);
        if (existing != null) {
            return null;
        }

        var hashedPassword = HashPassword(password);
        var userEntity = new UserEntity { Username = username, PasswordHash = hashedPassword };
        try {
            int userId = _userRepository.AddUser(userEntity);
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
}