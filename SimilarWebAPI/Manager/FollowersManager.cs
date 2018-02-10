using SimilarWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SimilarWebAPI.Manager
{
    public class FollowersManager
    {
        /// <summary>
        /// Adds a now follow between two users
        /// </summary>
        public static ResultModel<string> AddNewFollower(string followingUserName, string followedUserName)
        {
            // validating that the data is correct
            if (followingUserName == null || followingUserName.Length > 50 || followedUserName == null || followedUserName.Length > 50)
            {
                return new ResultModel<string>(false, "Usernames are not valid");
            }
            if(followingUserName == followedUserName)
            {
                return new ResultModel<string>(false, "A user cannot follow himself");
            }

            string commandText = "INSERT INTO FOLLOWERS ([Followed_User_Name],[Following_User_Name],[DateTime]) values('' + @followedUserName + '', '' + @followingUserName + '', '' + @dateTime + '');";
            string successResult = followingUserName + " is now following " + followedUserName;
            string failResultExists = followingUserName + " is already following " + followedUserName;
            string failResultUserNotExist = followedUserName + " or " + followingUserName + " do not exist";
            string failResultOther = "Failed to follow " + followedUserName;
            using (SqlConnection connection = SqlServerConnectionHelper.getConnection())
            {
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@followedUserName", followedUserName);
                    command.Parameters.AddWithValue("@followingUserName", followingUserName);
                    command.Parameters.AddWithValue("@dateTime", DateTime.Now);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0 ? new ResultModel<string>(true, successResult) : new ResultModel<string>(false, failResultOther);
                    }
                    catch (SqlException ex)
                    {
                        // Primary Key violation - meaning that this exact following is already in the database
                        if (ex.Number == 2627)
                        {
                            return new ResultModel<string>(false, failResultExists);
                        }
                        else if (ex.Number == 547) // Foreign key violation - meaning that one of the users does not exist
                        {
                            return new ResultModel<string>(false, failResultUserNotExist);
                        }
                    }
                }

            }
            return new ResultModel<string>(false, failResultOther);
        }

        /// <summary>
        /// Retrieves a users follows that occured after a specific date time
        /// </summary>
        public static List<Follower> GetNewFollowersForUser(string user, DateTime dateTime)
        {
            string commandText = "Select * from Followers where following_User_name = '' + @username + '' AND Datetime > '' + @datetime + ''";
            List<Follower> followers = new List<Follower>();
            using (SqlConnection connection = SqlServerConnectionHelper.getConnection())
            {
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@username", user);
                    command.Parameters.AddWithValue("@datetime", dateTime);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                followers.Add(new Follower(reader.GetString(1), reader.GetString(0), reader.GetDateTime(2)));
                            }
                        }
                    }
                }
            }

            return followers;
        }

        /// <summary>
        /// Delletes a follow between two users
        /// </summary>
        public static ResultModel<string> DeleteFollower(string followingUserName, string followedUserName)
        {
            // validating that the data is correct
            if (followingUserName == null || followingUserName.Length > 50 || followedUserName == null || followedUserName.Length > 50)
            {
                return new ResultModel<string>(false, "Usernames are not valid");
            }
            if (followingUserName == followedUserName)
            {
                return new ResultModel<string>(false, "A user cannot unfollow himself");
            }

            string commandText = "DELETE FROM FOLLOWERS WHERE [Followed_User_Name] = '' + @followedUserName + '' AND [Following_User_Name] = '' + @followingUserName + '';";
            string successResult = followingUserName + " has unfollowed " + followedUserName;
            string failResultNotFollowing = followingUserName + " is not following " + followedUserName;
            string failResultOther = "Failed to follow " + followedUserName;
            using (SqlConnection connection = SqlServerConnectionHelper.getConnection())
            {
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@followedUserName", followedUserName);
                    command.Parameters.AddWithValue("@followingUserName", followingUserName);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0 ? new ResultModel<string>(true, successResult) : new ResultModel<string>(false, failResultNotFollowing);
                    }
                    catch (SqlException ex)
                    {
                        LoggerManager.Log(ex.Message);
                    }
                }
            }
            return new ResultModel<string>(false, failResultOther);
        }

        /// <summary>
        /// Gets all of the follows from the database - mainly for testing
        /// </summary>
        public static List<Follower> GetAllFollowers()
        {
            string commandText = "SELECT * FROM FOLLOWERS;";
            List<Follower> followers = new List<Follower>();
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
                                followers.Add(new Follower(reader.GetString(1), reader.GetString(0), reader.GetDateTime(2)));
                            }
                        }
                    }
                        
                }

            }

            return followers;
        }
    }
}