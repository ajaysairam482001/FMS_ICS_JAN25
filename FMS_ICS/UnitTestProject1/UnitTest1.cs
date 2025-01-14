using FMS_ICS.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AuthenticateUser_ValidCredentials_ReturnsTrue()
        {
            // Arrange
            var loginController = new LoginController();
            string username = "testuser";
            string password = "password123";

            // Act
            bool result = loginController.AuthenticateUser(username, password);

            // Assert
            Assert.IsTrue(result, "Expected valid credentials to return true.");
        }

        [TestMethod]
        public void AuthenticateUser_InvalidCredentials_ReturnsFalse()
        {
            // Arrange
            var loginController = new LoginController();
            string username = "wronguser";
            string password = "wrongpassword";

            // Act
            bool result = loginController.AuthenticateUser(username, password);

            // Assert
            Assert.IsFalse(result, "Expected invalid credentials to return false.");
        }
    }
}
