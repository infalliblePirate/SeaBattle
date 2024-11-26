using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using SeaBattle.Entities;
namespace SeaBattle.Data;
public interface IUserRepository {
    Task<UserEntity> GetUserByUsernameAsync(string username);
    Task AddUserAsync(UserEntity user);
}