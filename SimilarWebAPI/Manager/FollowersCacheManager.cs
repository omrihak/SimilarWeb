using SimilarWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimilarWebAPI.Manager
{
    public class FollowersCacheManager
    {
        private static Dictionary<string, MyCache<Follower>> caches = new Dictionary<string, MyCache<Follower>>();
        private static Queue<string> LruQueue = new Queue<string>();
        private const int MAX_FOLLOWERS = 10000;

        /// <summary>
        /// Gets the list of users that the requested user follows
        /// </summary>
        /// <param name="userName">The user whom we are looking for the users which he follows</param>
        /// <returns>A list of all of the user's follows</returns>
        public static List<Follower> GetFollowedUserNames(string userName)
        {
            if (!caches.ContainsKey(userName))
            {
                caches[userName] = new MyCache<Follower>();
            }
            List<Follower> newFollowers = FollowersManager.GetNewFollowersForUser(userName, caches[userName].GetLatestDateTime());
            AddFollowers(newFollowers);
            List<Follower> followersResult = caches[userName].GetData().ToList();

            CleanCache();
            return followersResult;
        }

        private static void AddFollowers(List<Follower> newFollowers)
        {
            foreach (Follower flr in newFollowers)
            {
                if (!caches.ContainsKey(flr.FollowingUserName))
                {
                    caches[flr.FollowingUserName] = new MyCache<Follower>();
                }
                caches[flr.FollowingUserName].AddObject(flr);
                LruQueue.Enqueue(flr.FollowingUserName);
            }
        }

        private static void CleanCache()
        {
            while (LruQueue.Count > MAX_FOLLOWERS)
            {
                caches[LruQueue.Dequeue()].RemoveMessage();
            }
        }
    }
}
