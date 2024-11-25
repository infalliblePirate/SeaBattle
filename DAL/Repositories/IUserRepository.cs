using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using SeaBattle.Models;
namespace SeaBattle.Data;
public interface IUserRepository {
    Task<User> GetUserByUsernameAsync(string username);
    Task AddUserAsync(User user);
}