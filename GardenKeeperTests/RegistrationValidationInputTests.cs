using System;
using GardenKeeper.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GardenKeeperTests
{
    [TestClass]
    public class RegistrationValidationInputTests
    {
        [TestMethod]
        public void ValidEmailCheck_1()
        {
            string email = "saeifjojhlsdfgkjh@gmail.com";
            RegistrationViewModel viewModel = new RegistrationViewModel();
            var isValid = viewModel.IsEmailValid(email);
            Assert.AreEqual(true, isValid);
        }
        [TestMethod]
        public void ValidEmailCheck_2()
        {
            string email = "MWMasd@yandex.ru";
            RegistrationViewModel viewModel = new RegistrationViewModel();
            var isValid = viewModel.IsEmailValid(email);
            Assert.AreEqual(true, isValid);
        }
        [TestMethod]
        public void InvalidEmailCheck_1()
        {
            string email = "easdas";
            RegistrationViewModel viewModel = new RegistrationViewModel();
            var isValid = viewModel.IsEmailValid(email);
            Assert.AreEqual(false, isValid);
        }
        [TestMethod]
        public void InvalidEmailCheck_2()
        {
            string email = "a23e12";
            RegistrationViewModel viewModel = new RegistrationViewModel();
            var isValid = viewModel.IsEmailValid(email);
            Assert.AreEqual(false, isValid);
        }
        [TestMethod]
        public void InvalidEmailCheck_3()
        {
            string email = "1";
            RegistrationViewModel viewModel = new RegistrationViewModel();
            var isValid = viewModel.IsEmailValid(email);
            Assert.AreEqual(false, isValid);
        }
    }
}
