using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using GardenKeeper.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GardenKeeperTests
{
    [TestClass]
    public class HashGeneratorTests
    {
        static RegistrationViewModel registrationModel = new RegistrationViewModel();

        [TestMethod]
        public void GenerateHashPassedTest()
        {
            string password = "qwerty";

            MD5 hasher = MD5.Create();
            byte[] data = hasher.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            var hash = registrationModel.GenerateHash(password);
            Assert.AreEqual(hash, sBuilder.ToString());
        }

        [TestMethod]
        public void GenerateHashFailedTest()
        {
            string password = "qwerty";

            MD5 hasher = MD5.Create();
            byte[] data = hasher.ComputeHash(Encoding.UTF8.GetBytes("qwerty12345"));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            var hash = registrationModel.GenerateHash(password);
            Assert.AreNotEqual(hash, sBuilder.ToString());
        }

        [TestMethod]
        public void GenerateHashEmptyPasswordTest()
        {
            string emptyPassword = "";

            var hash = registrationModel.GenerateHash(emptyPassword);
            Assert.IsNotNull(hash);
            Assert.IsFalse(string.IsNullOrEmpty(hash));
        }

        [TestMethod]
        public void GenerateHashSpecialCharactersTest()
        {
            string specialPassword = "!@#$%^&*()_+-=[]{}|;:,.<>?";

            var hash = registrationModel.GenerateHash(specialPassword);

            Assert.IsNotNull(hash);
            Assert.IsFalse(string.IsNullOrEmpty(hash));
        }

        [TestMethod]
        public void GenerateHashLongPasswordTest()
        {
            string longPassword = new string('a', 1000);

            var hash = registrationModel.GenerateHash(longPassword);

            Assert.IsNotNull(hash);
            Assert.IsFalse(string.IsNullOrEmpty(hash));
        }
        [TestMethod]
        public void GenerateHashWhitespaceTest()
        {
            // Arrange
            var whitespacePasswords = new[]
            {
                " ",
                "  ",
                "\t",
                "\n",
                "\r\n",
                " \t\n\r"
            };

            // Act & Assert
            foreach (var password in whitespacePasswords)
            {
                var hash = registrationModel.GenerateHash(password);
                Assert.IsNotNull(hash);
                Assert.IsFalse(string.IsNullOrEmpty(hash));
                Assert.AreEqual(32, hash.Length);
            }
        }

        [TestMethod]
        public void GenerateHashCaseSensitivityTest()
        {
            var passwords = new[]
            {
                "Password",
                "password",
                "PASSWORD",
                "pAsSwOrD"
            };
            var hashes = passwords.Select(p => registrationModel.GenerateHash(p)).ToList();
            Assert.IsTrue(hashes.Distinct().Count() == hashes.Count); // все хэши должны быть уникальными, при разных регистрах
        }
    }
}
