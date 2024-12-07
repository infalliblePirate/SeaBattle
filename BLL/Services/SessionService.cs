using SeaBattle.Data;
using SeaBattle.Models;
namespace SeaBattle.Services;

public class SessionService {
    public UserModel? ActiveUser { get; private set; }
    public GameModel? ActiveGame { get; private set; }

    public void SetActiveUser(UserModel user) {
        ActiveUser = user;
    }

    public void SetActiveGame(GameModel game) {
        ActiveGame = game;
    }

    public void ClearSession() {
        ActiveUser = null;
        ActiveGame = null;
    }
}