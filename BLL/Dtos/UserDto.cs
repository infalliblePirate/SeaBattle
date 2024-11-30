using SeaBattle.Models;

namespace SeaBattle.Dtos;

public class UserDto {
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public int Score { get; set; }

    public UserDto(UserModel model) {
        
    }
}