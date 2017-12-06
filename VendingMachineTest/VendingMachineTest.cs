using System;
using NUnit;
using NUnit.Framework;
using VendingMachine.ViewModels;

namespace ViewModel
{
    [TestFixture]
    public class VendingMachineViewModelTests
    {
        VendingMachineViewModel VendingMachine;

        [SetUp]
        public void Init()
        {
            VendingMachine = new VendingMachineViewModel();
        }

        [Test]
        public void InsertValidCoinTest()
        {
            var initCredit = VendingMachine.CreditValue;
            VendingMachine.InsertCoin("Quarter");
            Assert.AreNotEqual(initCredit, VendingMachine.CreditValue);

            initCredit = VendingMachine.CreditValue;
            VendingMachine.InsertCoin("Dime");
            Assert.AreNotEqual(initCredit, VendingMachine.CreditValue);

            initCredit = VendingMachine.CreditValue;
            VendingMachine.InsertCoin("Nickle");
            Assert.AreNotEqual(initCredit, VendingMachine.CreditValue);
        }

        [Test]
        public void InsertInValidCoinTest()
        {
            var initCredit = VendingMachine.CreditValue;
            VendingMachine.InsertCoin("Penny");
            Assert.AreEqual(initCredit, VendingMachine.CreditValue);
        }

        [Test]
        public void DisplayINSERTCOINWithNoCoinsInsertedTest()
        {
            Assert.AreEqual("INSERT COIN", VendingMachine.DisplayString);
        }

        [Test]
        public void CoinReturnHasRejectedCoinsTest()
        {
            VendingMachine.InsertCoin("Penny");
            Assert.IsTrue(VendingMachine.CoinReturn.Contains("Penny"));
        }

        [Test]
        public void ProductSelectedTest()
        {
            VendingMachine.ButtonPressed(1);
            Assert.AreEqual(VendingMachine.SelectedProduct.ButtonNumber, 1);
        }

        [Test]
        public void SelectedProductDespensedWithEnoughCredit()
        {
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.ButtonPressed(2);
            var item = VendingMachine.CheckForDespensedProducts();

            Assert.AreEqual(item?.ButtonNumber, 2);
        }

        [Test]
        public void SelectedProductNotDespensedWhenCreditIsInsufficient()
        {
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.ButtonPressed(1);
            var item = VendingMachine.CheckForDespensedProducts();

            Assert.AreEqual(item, null);
        }
    }
}
