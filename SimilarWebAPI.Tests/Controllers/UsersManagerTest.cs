using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimilarWebAPI.Manager;
using WebApi.Tests.Helpers;

namespace SimilarWebApi.Tests.Controllers
{
    [TestClass]
    public class UsersManagerTest
    {

        [TestInitialize]
        public void Initialize()
        {
            DbCleaner.Clean();
        }


        [TestMethod]
        public void AddUserTest()
        {
            HashSet<string> names = new HashSet<string>() { "a", "b", "c", "d" };
            foreach(string name in names)
            {
                Assert.IsTrue(UsersManager.AddNewUser(name).Success);
            }
            foreach (string name in names)
            {
                Assert.IsFalse(UsersManager.AddNewUser(name).Success);
            }

            // Act
            List<string> result = UsersManager.GetAllUsers();

            // Assert
            Assert.IsTrue(result.All(name => names.Contains(name)));
            Assert.AreEqual(names.Count, result.Count);
        }
    }
}
