using SimilarWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SimilarWebAPI.Manager
{
    public class FeedManager
    {
        /// <summary>
        /// Getting all the messages from the database - this is not cached
        /// </summary>
        /// <returns> A list of all of the messages in the database</returns>
        public static ManagerResult<List<Message>> GetAllMessages()
        {
            String commandText = "SELECT * FROM MESSAGES ORDER BY DATETIME;";
            List<Message> messages = new List<Message>();
            using (var connection = SqlServerConnectionHelper.getConnection())
            {
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    try
                    {
                        connection.Open();
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            messages.Add(new Message(reader.GetString(1), reader.GetString(2), reader.GetDateTime(3)));
                        }
     
                        reader.Close();
                    }
                    catch (SqlException ex)
                    {
                        LoggerManager.Log(ex.Message);
                    }
                }

            }

            return new ManagerResult<List<Message>>(true, messages);
        }

        /// <summary>
        /// Gets All the messaged for a specific users feed - meaning all of the Messages of users who he follows - this is cached
        /// </summary>
        /// <param name="userName">The username of the user who requested his feed</param>
        /// <returns>A List of all of the messages in his feed</returns>
        public static ManagerResult<List<Message>> GetAllMessagesForUser(string userName)
        {
            var followedUserNames = FollowersCacheManager.getFollowedUserNames(userName);
            List<Message> messages = MessagesCacheManager.getMessagesFor(followedUserNames.ConvertAll(follower => follower.FollowedUserName));

            return new ManagerResult<List<Message>>(true, messages);
        }
    }
}