using SimilarWebAPI.Models;
using System;
using System.Collections.Generic;

namespace SimilarWebAPI.Manager
{
    public class MessagesCacheManager
    {
        private static Dictionary<string, MyCache<Message>> caches = new Dictionary<string, MyCache<Message>>();
        private static Queue<string> lruCache = new Queue<string>();
        private const int MAX_MESSAGES = 10000;

        /// <summary>
        /// Gets All of the messages for the list of users - updates the cache with new data from the database which is missing in the cache
        /// </summary>
        /// <param name="userNames">The list uf users to retrieve their messages</param>
        /// <returns>A list of all of the requested users messages</returns>
        public static List<Message> GetMessagesFor(List<string> userNames)
        {
            Dictionary<string, DateTime> usersLastMessageDateTime = new Dictionary<string, DateTime>();
            userNames.ForEach(userName => {
                if (!caches.ContainsKey(userName))
                {
                    caches[userName] = new MyCache<Message>();
                }
                usersLastMessageDateTime[userName] = caches[userName].GetLatestDateTime();
            });
            List<Message> newMessages = MessagesManager.GetMessagesForUsers(usersLastMessageDateTime);
            AddMessages(newMessages);
            List<Message> messagesResult = new List<Message>();

            //todo: instead of adding the lists one after another , add the messages while keeping the new list sorted
            userNames.ForEach(userName => messagesResult.AddRange(caches[userName].GetData()));
            CleanCache();
            return messagesResult;
        }

        private static void CleanCache()
        {
            while (lruCache.Count > MAX_MESSAGES)
            {
                caches[lruCache.Dequeue()].RemoveMessage();
            }
        }

        private static void AddMessages(List<Message> newMessages)
        {
            foreach (Message msg in newMessages)
            {
                if (!caches.ContainsKey(msg.UserName))
                {
                    caches[msg.UserName] = new MyCache<Message>();
                }
                caches[msg.UserName].AddObject(msg);
                lruCache.Enqueue(msg.UserName);
            }
        }
    }
}
