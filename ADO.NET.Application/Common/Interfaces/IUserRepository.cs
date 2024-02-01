using ADO.NET.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET.Application.Common.Interfaces
{
    public interface IUserRepository
    {
        //IEnumerable<User> GetAll();

        User GetUser(int id);

        bool RegisterAdmin();
        bool RegisterUser(User entity);
        User AuthenticateUser(string Email, string Password);
        string GetRoleName(int RoleId);
        bool UpdateUser(User entity);

        bool DeleteUser(int id);
        

    }
}
