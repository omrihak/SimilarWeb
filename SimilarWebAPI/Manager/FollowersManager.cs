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
        public static ManagerResult<string> AddNewFollower(string followingUserName, string followedUserName)
        {
            // validating that the data is correct
            if (followingUserName == null || followingUserName.Length > 50 || followedUserName == null || followedUserName.Length > 50)
            {
                return new ManagerResult<String>(false, "Usernames are not valid");
            }
            if(followingUserName == followedUserName)
            {
                return new ManagerResult<String>(false, "A user cannot follow himself");
            }

            String commandText = "INSERT INTO FOLLOWERS ([Followed_User_Name],[Following_User_Name],[DateTime]) values('' + @followedUserName + '', '' + @followingUserName + '', '' + @dateTime + '');";
            String successResult = followingUserName + " is now following " + followedUserName;
            String failResultExists = followingUserName + " is already following " + followedUserName;
            String failResultUserNotExist = followedUserName + " or " + followingUserName + " do not exist";
            String failResultOther = "Failed to follow " + followedUserName;
            using (var connection = SqlServerConnectionHelper.getConnection())
            {
                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@followedUserName", followedUserName);
                command.Parameters.AddWithValue("@followingUserName", followingUserName);
                command.Parameters.AddWithValue("@dateTime", DateTime.Now);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0 ? new ManagerResult<String>(true, successResult) : new ManagerResult<String>(false, failResultOther);
                }
                catch (SqlException ex)
                {
                    // Primary Key violation - meaning that this exact following is already in the database
                    if (ex.Number == 2627)
                    {
                        return new ManagerResult<String>(false, failResultExists);
                    } else if (ex.Number == 547) // Foreign key violation - meaning that one of the users does not exist
                    {
                        return new ManagerResult<String>(false, failResultUserNotExist);
                    }
                }

            }
            return new ManagerResult<String>(false, failResultOther);
        }

        /// <summary>
        /// Retrieves a users follows that occured after a specific date time
        /// </summary>
        public static List<Follower> getNewFollowersForUser(string user, DateTime dateTime)
        {
            String commandText = "Select * from Followers where following_User_name = '' + @username + '' AND Datetime > '' + @datetime + ''";
            List<Follower> followers = new List<Follower>();
            using (var connection = SqlServerConnectionHelper.getConnection())
            {
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@username", user);
                    command.Parameters.AddWithValue("@datetime", dateTime);
                    try
                    {
                        connection.Open();
                        var reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                followers.Add(new Follower(reader.GetString(1), reader.GetString(0), reader.GetDateTime(2)));
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

            return followers;
        }

        /// <summary>
        /// Delletes a follow between two users
        /// </summary>
        public static ManagerResult<string> DeleteFollower(string followingUserName, string followedUserName)
        {
            // validating that the data is correct
            if (followingUserName == null || followingUserName.Length > 50 || followedUserName == null || followedUserName.Length > 50)
            {
                return new ManagerResult<String>(false, "Usernames are not valid");
            }
            if (followingUserName == followedUserName)
            {
                return new ManagerResult<String>(false, "A user cannot unfollow himself");
            }

            String commandText = "DELETE FROM FOLLOWERS WHERE [Followed_User_Name] = '' + @followedUserName + '' AND [Following_User_Name] = '' + @followingUserName + '';";
            String successResult = followingUserName + " has unfollowed " + followedUserName;
            String failResultNotFollowing = followingUserName + " is not following " + followedUserName;
            String failResultOther = "Failed to follow " + followedUserName;
            using (var connection = SqlServerConnectionHelper.getConnection())
            {
                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@followedUserName", followedUserName);
                command.Parameters.AddWithValue("@followingUserName", followingUserName);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0 ? new ManagerResult<String>(true, successResult) : new ManagerResult<String>(false, failResultNotFollowing);
                }
                catch (SqlException ex)
                {
                    LoggerManager.Log(ex.Message);
                }

            }
            return new ManagerResult<String>(false, failResultOther);
        }

        /// <summary>
        /// Gets all of the follows from the database - mainly for testing
        /// </summary>
        public static List<Tuple<String,String>> GetAllFollowers()
        {
            String commandText = "SELECT * FROM FOLLOWERS;";
            using (var connection = SqlServerConnectionHelper.getConnection())
            {
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    try
                    {
                        connection.Open();
                        var reader = command.ExecuteReader();
                        List<Tuple<String, String>> followers = new List<Tuple<String, String>>();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                followers.Add(new Tuple<String, String>(reader.GetString(0), reader.GetString(1)));
                            }
                        }
                        reader.Close();
                        return followers;
                    }
                    catch (SqlException ex)
                    {
                        LoggerManager.Log(ex.Message);
                    }
                }

            }

            return null;
        }
    }
}