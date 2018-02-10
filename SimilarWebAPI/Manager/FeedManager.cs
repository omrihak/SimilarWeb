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
        public static List<Message> GetAllMessages()
        {
            string commandText = "SELECT * FROM MESSAGES ORDER BY DATETIME;";
            List<Message> messages = new List<Message>();
            using (SqlConnection connection = SqlServerConnectionHelper.getConnection())
            {
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            messages.Add(new Message(reader.GetString(1), reader.GetString(2), reader.GetDateTime(3)));
                        }
                    }
                        
                }

            }

            return messages;
        }

        /// <summary>
        /// Gets All the messaged for a specific users feed - meaning all of the Messages of users who he follows - this is cached
        /// </summary>
        /// <param name="userName">The username of the user who requested his feed</param>
        /// <returns>A List of all of the messages in his feed</returns>
        public static List<Message> GetAllMessagesForUser(string userName)
        {
            List<Follower> followedUserNames = FollowersCacheManager.GetFollowedUserNames(userName);
            List<Message> messages = MessagesCacheManager.GetMessagesFor(followedUserNames.ConvertAll(follower => follower.FollowedUserName));

            return messages;
        }
    }
}