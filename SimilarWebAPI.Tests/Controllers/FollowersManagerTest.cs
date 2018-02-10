using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimilarWebAPI.Manager;
using WebApi.Tests.Helpers;

namespace WebApi.Tests.Controllers
{
    [TestClass]
    public class FollowersManagerTest
    {

        [TestInitialize]
        public void Initialize()
        {
            DbCleaner.Clean();
        }

        [TestMethod]
        public void TestAddFollow()
        {
            HashSet<string> names = new HashSet<string>() { "a", "b", "c", "d" };
            foreach (string name in names)
            {
                Assert.IsTrue(UsersManager.AddNewUser(name).Success);
            }


            Assert.IsTrue(FollowersManager.AddNewFollower("a", "b").Success);
            Assert.IsTrue(FollowersManager.AddNewFollower("c", "b").Success);
            Assert.IsFalse(FollowersManager.AddNewFollower("c", "b").Success);
            Assert.IsFalse(FollowersManager.AddNewFollower("b", "b").Success);
            Assert.IsFalse(FollowersManager.AddNewFollower("f", "b").Success);
            Assert.IsFalse(FollowersManager.AddNewFollower("b", "f").Success);

            var followers = FollowersManager.GetAllFollowers();
            Assert.AreEqual(2, followers.Count);
            Assert.AreEqual("b", followers[0].Item1);
            Assert.AreEqual("a", followers[0].Item2);
            Assert.AreEqual("b", followers[1].Item1);
            Assert.AreEqual("c", followers[1].Item2);
        }


        [TestMethod]
        public void TestRemoveFollow()
        {
            HashSet<string> names = new HashSet<string>() { "a", "b", "c", "d" };
            foreach (string name in names)
            {
                Assert.IsTrue(UsersManager.AddNewUser(name).Success);
            }


            Assert.IsTrue(FollowersManager.AddNewFollower("a", "b").Success);
            Assert.IsTrue(FollowersManager.AddNewFollower("c", "b").Success);
            Assert.IsTrue(FollowersManager.AddNewFollower("a", "c").Success);
            Assert.IsTrue(FollowersManager.AddNewFollower("c", "a").Success);



            Assert.IsTrue(FollowersManager.DeleteFollower("a", "c").Success);
            Assert.IsFalse(FollowersManager.DeleteFollower("a", "c").Success);
            Assert.IsFalse(FollowersManager.DeleteFollower("a", "g").Success);
            Assert.IsTrue(FollowersManager.DeleteFollower("c", "a").Success);

            var followers = FollowersManager.GetAllFollowers();
            Assert.AreEqual(2, followers.Count);
            Assert.AreEqual("b", followers[0].Item1);
            Assert.AreEqual("a", followers[0].Item2);
            Assert.AreEqual("b", followers[1].Item1);
            Assert.AreEqual("c", followers[1].Item2);
        }
    }
}
