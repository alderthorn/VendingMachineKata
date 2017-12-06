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
        public void DisplayINSERTCOINWithNoCoinsInserted()
        {
            Assert.AreEqual("INSERT COIN", VendingMachine.DisplayString);
        }

        [Test]
        public void CoinReturnHasRejectedCoins()
        {
            VendingMachine.InsertCoin("Penny");
            Assert.IsTrue(VendingMachine.CoinReturn.Contains("Penny"));
        }
    }
}
