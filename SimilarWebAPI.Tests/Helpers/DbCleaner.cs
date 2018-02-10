using SimilarWebAPI.Manager;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Tests.Helpers
{
    public class DbCleaner
    {
        public static void Clean()
        {
            String commandTextMessages = "DELETE FROM MESSAGES;";
            using (var connection = SqlServerConnectionHelper.getConnection())
            {
                using (SqlCommand command = new SqlCommand(commandTextMessages, connection))
                {
                    try
                    {
                        connection.Open();
                        var reader = command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                    }
                }

            }

            String commandTextFollowers = "DELETE FROM FOLLOWERS;";
            using (var connection = SqlServerConnectionHelper.getConnection())
            {
                using (SqlCommand command = new SqlCommand(commandTextFollowers, connection))
                {
                    try
                    {
                        connection.Open();
                        var reader = command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                    }
                }

            }

            String commandTextUsers = "DELETE FROM USERS;";
            using (var connection = SqlServerConnectionHelper.getConnection())
            {
                using (SqlCommand command = new SqlCommand(commandTextUsers, connection))
                {
                    try
                    {
                        connection.Open();
                        var reader = command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                    }
                }

            }
        }
    }
}
