using SeaBattle.Data;
using SeaBattle.Models;
namespace SeaBattle.Services;

public class SessionService {
    private static UserModel _activeUser;

    public static UserModel Activeuser {
        get => _activeUser;
        set => _activeUser = value;
    }
}