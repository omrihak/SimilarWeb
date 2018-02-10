using SimilarWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimilarWebAPI.Manager
{
    public class MessagesManager
    {
        public static ManagerResult<string> AddNewMessage(string userName, string message)
        {
            if (userName == null || userName.Length > 50)
            {
                return new ManagerResult<String>(false, "Username is not valid");
            }
            String commandText = "INSERT INTO MESSAGES ([Message_Id],[User_Name],[Text],[DateTime]) values('' + @messageId + '', '' + @username + '', '' + @message + '', '' + @dateTime + '');";
            String successResult = userName + "'s message was added successfully";
            String failResultForeign = "User " + userName + " does not exists";
            String failResultOther = "User " + userName + " had not been added";
            using (var connection = SqlServerConnectionHelper.getConnection())
            {
                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@messageId", Guid.NewGuid().ToString());
                command.Parameters.AddWithValue("@username", userName);
                command.Parameters.AddWithValue("@message", message);
                command.Parameters.AddWithValue("@dateTime", DateTime.Now);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0 ? new ManagerResult<String>(true, successResult) : new ManagerResult<String>(false, failResultOther);
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 547) // Foreign key violation - meaning that the user does not exist
                    {
                        return new ManagerResult<String>(false, failResultForeign);
                    }
                }

            }
            return new ManagerResult<String>(false, failResultOther);
        }

        /// <summary>
        /// Gets messages for users that are newer than a specific time for each user
        /// </summary>
        public static List<Message> GetMessagesForUsers(List<Tuple<string, DateTime>> usersLastMessageDateTime)
        {
            List<Message> messages = new List<Message>();

            // If there are no users, no need to query
            if (usersLastMessageDateTime.Count == 0)
                return messages;

            //Building the query
            String commandText = "select * from messages where (User_Name = '' + @username0 + '' AND Datetime > '' + @datetime0 + '')";
            for (int i = 1; i < usersLastMessageDateTime.Count; i++)
            {
                commandText += " OR (User_Name = '' + @username" + i + " + '' AND Datetime > '' + @datetime" + i + " + '')";
            }
            commandText += " Order by Datetime;";

            using (var connection = SqlServerConnectionHelper.getConnection())
            {
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    for (int i = 0; i < usersLastMessageDateTime.Count; i++)
                    {
                        command.Parameters.AddWithValue("@username" + i, usersLastMessageDateTime[i].Item1);
                        command.Parameters.AddWithValue("@datetime" + i, usersLastMessageDateTime[i].Item2);
                    }
                    try
                    {
                        connection.Open();
                        var reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                messages.Add(new Message(reader.GetString(1), reader.GetString(2), reader.GetDateTime(3)));
                            }
                        }
                        reader.Close();
                    }
                    catch (SqlException ex)
                    {
                        LoggerManager.Log(ex.Message);
                    }
                }

            }

            return messages;
        }
    }
}