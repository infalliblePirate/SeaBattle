using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using SeaBattle.Entities;
namespace SeaBattle.Data;
public interface IUserRepository {
    int AddUser(UserEntity user);
    void UpdateUserScore(int playerId, int scoreChange);
    string GetUserNameById(int id);
}