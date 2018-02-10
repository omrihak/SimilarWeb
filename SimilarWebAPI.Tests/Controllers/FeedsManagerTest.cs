using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimilarWebAPI.Manager;
using WebApi.Tests.Helpers;

namespace WebApi.Tests.Controllers
{
    [TestClass]
    public class FeedsManagerTest
    {

        [TestInitialize]
        public void Initialize()
        {
            DbCleaner.Clean();
        }



        [TestMethod]
        public void TestGetMessages()
        {
            HashSet<string> names = new HashSet<string>() { "a", "b", "c", "d", "e", "f" };
            foreach (string name in names)
            {
                Assert.IsTrue(UsersManager.AddNewUser(name).Success);
            }

            Assert.IsTrue(FollowersManager.AddNewFollower("a", "b").Success);
            Assert.IsTrue(FollowersManager.AddNewFollower("a", "c").Success);
            Assert.IsTrue(FollowersManager.AddNewFollower("a", "d").Success);


            for (int i = 0; i < 3; i++)
            {
                Assert.IsTrue(MessagesManager.AddNewMessage("b", "b wrote " + i + i).Success);
            }

            for (int i = 0; i < 4; i++)
            {
                Assert.IsTrue(MessagesManager.AddNewMessage("c", "c wrote " + i + i).Success);
            }

            for (int i = 0; i < 5; i++)
            {
                Assert.IsTrue(MessagesManager.AddNewMessage("d", "d wrote " + i + i).Success);
            }

            for (int i = 0; i < 6; i++)
            {
                Assert.IsTrue(MessagesManager.AddNewMessage("e", "e wrote " + i + i).Success);
            }

            var messagesForUserA = FeedManager.GetAllMessagesForUser("a");
            Assert.AreEqual(messagesForUserA.Data.Count, 12);
            var allMessages = FeedManager.GetAllMessages();
            Assert.AreEqual(allMessages.Data.Count, 18);
        }


        [TestMethod]
        public void TestMoreMessagesThanCache()
        {
            HashSet<string> names = new HashSet<string>() { "a", "b", "c", "d", "e", "f" };
            foreach (string name in names)
            {
                Assert.IsTrue(UsersManager.AddNewUser(name).Success);
            }

            Assert.IsTrue(FollowersManager.AddNewFollower("a", "b").Success);
            Assert.IsTrue(FollowersManager.AddNewFollower("a", "c").Success);
            Assert.IsTrue(FollowersManager.AddNewFollower("a", "d").Success);


            for(int i = 0; i < 400; i++)
            {
                MessagesManager.AddNewMessage("b", "b wrote " + i + i);
            }

            for (int i = 0; i < 400; i++)
            {
                MessagesManager.AddNewMessage("c", "c wrote " + i + i);
            }

            for (int i = 0; i < 400; i++)
            {
                MessagesManager.AddNewMessage("d", "d wrote " + i + i);
            }

            var messages = FeedManager.GetAllMessagesForUser("a");
            int a = 1;

            // Testing how the caching is working
            var messages2 = FeedManager.GetAllMessagesForUser("a");
        }
    }
}
