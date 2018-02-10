using SimilarWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SimilarWebAPI.Manager
{
    public class UsersManager
    {
        /// <summary>
        /// Adds a new user if a user with the same name does not exist
        /// </summary>
        public static ManagerResult<string> AddNewUser(string name)
        {
            if (name == null || name.Length > 50)
            {
                return new ManagerResult<String>(false, "Username is not valid");
            }
            String commandText = "INSERT INTO USERS values ('' + @username + '');";
            String successResult = "User " + name + " was added succesfully";
            String failResultExists = "User " + name + " already exists";
            String failResultOther = "User " + name + " had not been added";
            using (var connection = SqlServerConnectionHelper.getConnection())
            {
                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@username", name);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0 ? new ManagerResult<String>(true, successResult) : new ManagerResult<String>(false, failResultOther);
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627)
                    {
                        return new ManagerResult<String>(false, failResultExists);
                    }
                }

            }
            return new ManagerResult<String>(false, failResultOther);
        }

        /// <summary>
        /// Gets all of the users in the database - mainly used for testing
        /// </summary>
        public static List<string> GetAllUsers()
        {
            List<string> users = new List<string>();
            String commandText = "SELECT * FROM USERS;";
            using (var connection = SqlServerConnectionHelper.getConnection())
            {
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    try
                    {
                        connection.Open();
                        var reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                users.Add(reader.GetString(0));
                            }
                        }
                        reader.Close();
                        return users;
                    }
                    catch (SqlException ex)
                    {
                        LoggerManager.Log(ex.Message);
                    }
                }

            }

            return users;
        }
    }
}