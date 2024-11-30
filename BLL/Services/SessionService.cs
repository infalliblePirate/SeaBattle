using SeaBattle.Data;
using SeaBattle.Models;
namespace SeaBattle.Services;

public class SessionService {
    private static UserModel _activeUser;
    private static GameModel _activeGame;

    public static UserModel ActiveUser {
        get => _activeUser;
        set => _activeUser = value;
    }

    public static GameModel ActiveGame {
        get => _activeGame;
        set => _activeGame = value;
    }
}