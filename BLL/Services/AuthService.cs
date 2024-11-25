using SeaBattle.Models;
using SeaBattle.Data;
namespace SeaBattle.Services;
public class AuthService {
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository) {
        _userRepository = userRepository;
    }

    public async Task<bool> ValidataeUserAsync(string username, string password) {
        var user = await _userRepository.GetUserByUsernameAsync(username);

        if (user == null || !VerifyPassword(password, user.PasswordHash)) {
            return false;
        }
        return true;
    }

    public async Task<bool> RegisterUserAsync(string username, string password) {
        var existing = await _userRepository.GetUserByUsernameAsync(username);
        if (existing != null) {
            return false;
        }

        var hashedPassword = HashPassword(password);
        var user = new User { Username = username, PasswordHash = hashedPassword };
        await _userRepository.AddUserAsync(user);
        return true;
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