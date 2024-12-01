using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using SeaBattle.Entities;
namespace SeaBattle.Data;
public interface IUserRepository {
    UserEntity GetUserByUsername(string username);
    int AddUser(UserEntity user);
    void JoinGame(int gameId, int userId);
}