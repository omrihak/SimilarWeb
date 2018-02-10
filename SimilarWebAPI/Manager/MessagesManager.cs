using SimilarWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SimilarWebAPI.Manager
{
    public class MessagesManager
    {
        public static ResultModel<string> AddNewMessage(string userName, string message)
        {
            if (userName == null || userName.Length > 50)
            {
                return new ResultModel<string>(false, "Username is not valid");
            }
            string commandText = "INSERT INTO MESSAGES ([Message_Id],[User_Name],[Text],[DateTime]) values('' + @messageId + '', '' + @username + '', '' + @message + '', '' + @dateTime + '');";
            string successResult = userName + "'s message was added successfully";
            string failResultForeign = "User " + userName + " does not exists";
            string failResultOther = "Message was not published";
            using (SqlConnection connection = SqlServerConnectionHelper.getConnection())
            {
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@messageId", Guid.NewGuid().ToString());
                    command.Parameters.AddWithValue("@username", userName);
                    command.Parameters.AddWithValue("@message", message);
                    command.Parameters.AddWithValue("@dateTime", DateTime.Now);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0 ? new ResultModel<string>(true, successResult) : new ResultModel<string>(false, failResultOther);
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 547) // Foreign key violation - meaning that the user does not exist
                        {
                            return new ResultModel<string>(false, failResultForeign);
                        }
                        return new ResultModel<string>(false, failResultOther);
                    }
                }
            }
        }

        /// <summary>
        /// Gets messages for users that are newer than a specific time for each user
        /// </summary>
        public static List<Message> GetMessagesForUsers(Dictionary<string, DateTime> usersLastMessageDateTime)
        {
            List<Message> messages = new List<Message>();

            // If there are no users, no need to query
            if (usersLastMessageDateTime.Count == 0)
                return messages;

            //Building the query
            string commandText = "select * from messages where (User_Name = '' + @username0 + '' AND Datetime > '' + @datetime0 + '')";
            for (int i = 1; i < usersLastMessageDateTime.Count; i++)
            {
                commandText += " OR (User_Name = '' + @username" + i + " + '' AND Datetime > '' + @datetime" + i + " + '')";
            }
            commandText += " Order by Datetime;";

            using (SqlConnection connection = SqlServerConnectionHelper.getConnection())
            {
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    int i = 0;
                    foreach(string user in usersLastMessageDateTime.Keys)
                    {
                        command.Parameters.AddWithValue("@username" + i, user);
                        command.Parameters.AddWithValue("@datetime" + i, usersLastMessageDateTime[user]);
                        i++;
                    }
                    
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                messages.Add(new Message(reader.GetString(1), reader.GetString(2), reader.GetDateTime(3)));
                            }
                        }
                    }
                }

            }

            return messages;
        }
    }
}