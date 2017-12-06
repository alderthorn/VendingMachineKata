using System;
using NUnit;
using NUnit.Framework;
using VendingMachine.ViewModels;
using System.Linq;

namespace ViewModel
{
    [TestFixture]
    public class VendingMachineViewModelTests
    {
        VendingMachineViewModel VendingMachine;
        private const string INSERT_COIN = "INSERT COIN";

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
            var displayString = VendingMachine.CheckDisplay();
            Assert.AreEqual(INSERT_COIN, displayString);
        }

        [Test]
        public void CoinReturnHasRejectedCoinsTest()
        {
            VendingMachine.InsertCoin("Penny");
            var coinsReturned = VendingMachine.GetCoinsInReturnSlot();
            Assert.IsTrue(coinsReturned.Contains("Penny"));
        }

        [Test]
        public void ProductSelectedTest()
        {
            VendingMachine.ButtonPressed(1);
            Assert.AreEqual(VendingMachine.SelectedProduct.ButtonNumber, 1);
        }

        [Test]
        public void SelectedProductDispensedWithEnoughCreditTest()
        {
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.ButtonPressed(2);
            var item = VendingMachine.CheckForDispensedProducts();

            Assert.AreEqual(item?.ButtonNumber, 2);
        }

        [Test]
        public void SelectedProductNotDispensedWhenCreditIsInsufficientTest()
        {
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.ButtonPressed(1);
            var item = VendingMachine.CheckForDispensedProducts();

            Assert.AreEqual(item, null);
        }

        [Test]
        public void AfterProductHasDispensedThankYouIsDisplayedTest()
        {
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.ButtonPressed(2);

            var display = VendingMachine.CheckDisplay();
            Assert.AreEqual(display, "THANK YOU");
        }

        [Test]
        public void AfterProductHasDispensedThankYouIsDisplayedThenInsertCoinIsDisplayedTest()
        {
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.ButtonPressed(2);

            var display = VendingMachine.CheckDisplay();
            display = VendingMachine.CheckDisplay();
            Assert.AreEqual(display, INSERT_COIN);
        }

        [Test]
        public void AfterProductHasDispensedCreditIsSetToZeroTest()
        {
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.ButtonPressed(2);
            
            Assert.AreEqual(VendingMachine.CreditValue, 0.0);
        }

        [Test]
        public void SelectedProductNotDispensedWhenCreditIsInsufficientDisplaysPriceOfProductTest()
        {
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.ButtonPressed(1);
            var display = VendingMachine.CheckDisplay();

            Assert.AreEqual(display, "$1.00");
        }

        [Test]
        public void SelectedProductNotDispensedWhenCreditIsInsufficientDisplaysPriceOfProductThenDisplayCurrentCreditIfGreaterThanZeroTest()
        {
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.ButtonPressed(1);

            var display = VendingMachine.CheckDisplay();
            Assert.AreEqual(display, "$1.00");

            display = VendingMachine.CheckDisplay();
            Assert.AreEqual(display, "$0.50");
        }

        [Test]
        public void SelectedProductNotDispensedWhenCreditIsInsufficientDisplaysPriceOfProductThenDisplayInsertCoinIfCreditIsZeroTest()
        {
            VendingMachine.ButtonPressed(1);

            var display = VendingMachine.CheckDisplay();
            Assert.AreEqual(display, "$1.00");

            display = VendingMachine.CheckDisplay();
            Assert.AreEqual(display, INSERT_COIN);
        }

        [Test]
        public void SelectedProductClickedBeforeChangeInsertedThenCorrectChangeIsInsertedToDispenseProductTest()
        {
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.ButtonPressed(1);
            var item = VendingMachine.CheckForDispensedProducts();

            Assert.AreEqual(item, null);

            VendingMachine.InsertCoin("Quarter");
            VendingMachine.InsertCoin("Quarter");

            item = VendingMachine.CheckForDispensedProducts();
            Assert.AreEqual(item.ButtonNumber, 1);
        }

        [Test]
        public void SelectProductWithExcessCreditGetAppropriateChange()
        {
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.InsertCoin("Nickle");
            VendingMachine.InsertCoin("Dime");
            VendingMachine.InsertCoin("Dime");
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.ButtonPressed(2);  //Cost of .5  total is $1.00

            var change = VendingMachine.GetValueOfCoinReturn();
            Assert.AreEqual(change, 0.5);
        }

        [Test]
        public void ReturnCoinsButtonReturnsExactCoinsTheCustomerEntered()
        {
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.InsertCoin("Nickle");
            VendingMachine.InsertCoin("Dime");
            VendingMachine.InsertCoin("Dime");
            VendingMachine.InsertCoin("Quarter");

            VendingMachine.ReturnChangeButtonPressed();
            var coinsReturned = VendingMachine.GetCoinsInReturnSlot();

            var NumberOfQuarters = coinsReturned.Where(x => x == "Quarter").Count();
            var NumberOfDimes = coinsReturned.Where(x => x == "Dime").Count();
            var NumberOfNickles = coinsReturned.Where(x => x == "Nickle").Count();
            Assert.AreEqual(NumberOfQuarters, 3);
            Assert.AreEqual(NumberOfDimes, 2);
            Assert.AreEqual(NumberOfNickles, 1);
        }
    }
}
