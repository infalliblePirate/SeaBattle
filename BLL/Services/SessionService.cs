using SeaBattle.Data;
using SeaBattle.Models;

namespace SeaBattle.Services;
public class SessionService {
    public UserModel? ActiveUser { get; private set; }
    public void SetActiveUser(UserModel user) {
        ActiveUser = user;
    }

    public void ClearSession() {
        ActiveUser = null;
    }
}