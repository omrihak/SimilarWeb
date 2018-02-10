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
        public static ResultModel<string> AddNewUser(string name)
        {
            if (name == null || name.Length > 50)
            {
                return new ResultModel<string>(false, "Username is not valid");
            }

            string commandText = "INSERT INTO USERS values ('' + @username + '');";
            string successResult = "User " + name + " was added succesfully";
            string failResultExists = "User " + name + " already exists";
            string failResultOther = "User " + name + " had not been added";
            using (SqlConnection connection = SqlServerConnectionHelper.getConnection())
            {
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@username", name);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0 ? new ResultModel<string>(true, successResult) : new ResultModel<string>(false, failResultOther);
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2627)
                        {
                            return new ResultModel<string>(false, failResultExists);
                        }
                        return new ResultModel<string>(false, failResultOther);
                    }
                }
            }
        }

        /// <summary>
        /// Gets all of the users in the database - mainly used for testing
        /// </summary>
        public static List<string> GetAllUsers()
        {
            List<string> users = new List<string>();
            string commandText = "SELECT * FROM USERS;";
            using (SqlConnection connection = SqlServerConnectionHelper.getConnection())
            {
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                users.Add(reader.GetString(0));
                            }
                        }
                        return users;
                    }
                }
            }
        }
    }
}