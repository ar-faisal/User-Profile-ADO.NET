using ADO.NET.Application.Common.Interfaces;
using ADO.NET.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbContext _context;
        public UserRepository(IDbContext context)
        {
            _context = context;
        }

        public bool RegisterAdmin()
        {
            _context.OpenConnection();
            bool rolesExist = false;
            using (SqlCommand rolesExistCommand = new SqlCommand("SELECT COUNT(*) FROM Roles", (SqlConnection)_context.Connection))
            {
                int count = (int)rolesExistCommand.ExecuteScalar();
                rolesExist = count > 0;
            }
            bool adminExists = false;
            using (SqlCommand adminExistsCommand = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Email = 'admin@example.com'", (SqlConnection)_context.Connection))
            {
                int count = (int)adminExistsCommand.ExecuteScalar();
                adminExists = count > 0;
            }

            if (!rolesExist || !adminExists)
            {
                if (!rolesExist)
                {
                    using (SqlCommand command = new SqlCommand("INSERT INTO Roles (RoleId, RoleName) VALUES (1, 'admin'), (2, 'user')", (SqlConnection)_context.Connection))
                    {
                        command.ExecuteNonQuery();
                    }

                }

                if (!adminExists)
                {
                    using (SqlCommand command = new SqlCommand("INSERT INTO Users (FirstName, LastName, Password, Email, PhoneNumber, RoleId) VALUES ('Admin', 'Admin', 'Admin@123', 'admin@example.com', '1234567890', 1)", (SqlConnection)_context.Connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                _context.CloseConnection();
                return true;
            }
            _context.CloseConnection();
            return false;
        }

        public bool RegisterUser(User usr)
        {
            _context.OpenConnection();
                     
            // Add the user to the Users table
            using (SqlCommand command = new SqlCommand("AddUser", (SqlConnection)_context.Connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@firstname", usr.FirstName);
                command.Parameters.AddWithValue("@lastname", usr.LastName);
                command.Parameters.AddWithValue("@password", usr.Password);
                command.Parameters.AddWithValue("@email", usr.Email);
                command.Parameters.AddWithValue("@Phonenumber", usr.PhoneNumber);
                command.Parameters.AddWithValue("@RoleId", 2);  // Use the determined role ID

                int rowsAffected = command.ExecuteNonQuery();

                _context.CloseConnection();

                return rowsAffected > 0;
            }
        }

        public User AuthenticateUser(string Email, string Password)
        {
            _context.OpenConnection();

            using (SqlCommand command = new SqlCommand("AuthenticateUser", (SqlConnection)_context.Connection))
            {
                

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Email", Email);
                command.Parameters.AddWithValue("@Password", Password);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {

                        User user = new User
                        {
                            UserId = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            RoleId = reader.GetInt32(2) 
                        };

                        _context.CloseConnection();

                        // User found, return user details
                        return user;
                        
                    }
                    else
                    {
                        _context.CloseConnection();
                        // User not found
                        return null;
                    }
                }
            }
        }

        public string GetRoleName(int RoleId)
        {
            _context.OpenConnection();

            using (SqlCommand GetRoleNameCommand = new SqlCommand("SELECT RoleName FROM Roles WHERE RoleId = @RoleId", (SqlConnection)_context.Connection))
            {
                // Add parameter to prevent SQL injection
                GetRoleNameCommand.Parameters.AddWithValue("@RoleId", RoleId);

                using (SqlDataReader reader = GetRoleNameCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // If there is a result, return the role name
                        return reader["RoleName"].ToString();
                    }
                }
            }

            // If no role is found, return null
            return null;
        }

        public User GetUser(int id)
        {
            _context.OpenConnection();

            using (SqlCommand GetUserCommand = new SqlCommand("SELECT UserId, FirstName,LastName, Email, PhoneNumber, Gender, State, City, ImageUrl, ResumeUrl FROM Users WHERE UserId = @UserId", (SqlConnection)_context.Connection))
            {
                // Add parameter to prevent SQL injection
                GetUserCommand.Parameters.AddWithValue("@UserId", id);

                using (SqlDataReader reader = GetUserCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // If there is a result, create a User object and populate its properties
                        User user = new User
                        {
                            UserId = Convert.ToInt32(reader["UserId"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),   
                            Email = reader["Email"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString(),
                            Gender = reader["Gender"].ToString(),
                            State = reader["State"].ToString(),
                            City = reader["City"].ToString(),
                            ImageUrl = reader["ImageUrl"].ToString(),
                            ResumeUrl = reader["ResumeUrl"].ToString(),
                           
                        };

                        return user;
                    }
                }
            }

            // If no user is found, return null
            return null;
        }

        public bool UpdateUser(User entity)
        {
            _context.OpenConnection();

            // Update the user in the Users table
            using (SqlCommand command = new SqlCommand("UpdateUser", (SqlConnection)_context.Connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@userId", entity.UserId); // Assuming you have a UserId property
                command.Parameters.AddWithValue("@firstname", entity.FirstName);
                command.Parameters.AddWithValue("@lastname", entity.LastName);                
                command.Parameters.AddWithValue("@Phonenumber", entity.PhoneNumber);                    
                command.Parameters.AddWithValue("@gender", entity.Gender);
                command.Parameters.AddWithValue("@state", entity.State);
                command.Parameters.AddWithValue("@city", entity.City);
                command.Parameters.AddWithValue("@imageUrl", entity.ImageUrl);
                command.Parameters.AddWithValue("@resumeUrl", entity.ResumeUrl);

                int rowsAffected = command.ExecuteNonQuery();

                _context.CloseConnection();

                return rowsAffected > 0;
            }
        }

        

        public bool DeleteUser(int userId)
        {
            try
            {
                _context.OpenConnection();

                using (SqlCommand command = new SqlCommand("DeleteUser", (SqlConnection)_context.Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@userId", userId);

                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as appropriate for your application
                throw;
            }
            finally
            {
                _context.CloseConnection();
            }
        }

    }
}
