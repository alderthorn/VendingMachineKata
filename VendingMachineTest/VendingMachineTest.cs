using System;
using NUnit;
using NUnit.Framework;
using VendingMachine.ViewModels;
using System.Linq;
using VendingMachine.Types;
using System.Collections.Generic;

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

        [Test]
        public void ItemSelectedByCustomerIsSoldOutDisplaySoldOutTest()
        {
            var soda = new ProductType(1, "Soda", 1.00M, 3);
            var chips = new ProductType(2, "Chips", .5M, 2);
            var candy = new ProductType(3, "Candy", .65M, 5);

            VendingMachine = new VendingMachineViewModel(new List<ProductType>() { soda, chips, candy }, 50, 50, 50);
            var display = VendingMachine.CheckDisplay();

            VendingMachine.ButtonPressed(2);
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.InsertCoin("Quarter");

            VendingMachine.ButtonPressed(2);
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.InsertCoin("Quarter");

            // chips should now be sold out
            VendingMachine.ButtonPressed(2);
            display = VendingMachine.CheckDisplay();
            Assert.AreEqual(display, "SOLD OUT");
        }

        [Test]
        public void DisplayResetsToInsertCoinOnDisplayCheckAfterSoldOutIfNoMoneyIsInMachineTest()
        {
            var soda = new ProductType(1, "Soda", 1.00M, 3);
            var chips = new ProductType(2, "Chips", .5M, 2);
            var candy = new ProductType(3, "Candy", .65M, 5);

            VendingMachine = new VendingMachineViewModel(new List<ProductType>() { soda, chips, candy }, 50, 50, 50);
            var display = VendingMachine.CheckDisplay();

            VendingMachine.ButtonPressed(2);
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.InsertCoin("Quarter");

            VendingMachine.ButtonPressed(2);
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.InsertCoin("Quarter");

            // chips should now be sold out
            VendingMachine.ButtonPressed(2);
            display = VendingMachine.CheckDisplay();
            Assert.AreEqual(display, "SOLD OUT");
            display = VendingMachine.CheckDisplay();
            Assert.AreEqual(display, INSERT_COIN);
        }

        [Test]
        public void DisplayResetsToCeditAmountOnDisplayCheckAfterSoldOutIfMoneyIsInMachineTest()
        {
            var soda = new ProductType(1, "Soda", 1.00M, 3);
            var chips = new ProductType(2, "Chips", .5M, 2);
            var candy = new ProductType(3, "Candy", .65M, 5);

            VendingMachine = new VendingMachineViewModel(new List<ProductType>() { soda, chips, candy }, 50, 50, 50);
            var display = VendingMachine.CheckDisplay();

            VendingMachine.ButtonPressed(2);
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.InsertCoin("Quarter");

            VendingMachine.ButtonPressed(2);
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.InsertCoin("Quarter");

            // chips should now be sold out
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.ButtonPressed(2);
            display = VendingMachine.CheckDisplay();
            Assert.AreEqual(display, "SOLD OUT");
            display = VendingMachine.CheckDisplay();
            Assert.AreEqual(display, "$0.50");
        }

        [Test]
        public void DisplayExactChangeOnlyWHenMachineIsLowOnChangeToReturnOnStartTest()
        {
            var soda = new ProductType(1, "Soda", 1.00M, 3);
            var chips = new ProductType(2, "Chips", .5M, 2);
            var candy = new ProductType(3, "Candy", .65M, 5);

            VendingMachine = new VendingMachineViewModel(new List<ProductType>() { soda, chips, candy }, 1, 3, 5);

            var display = VendingMachine.CheckDisplay();

            Assert.AreEqual(display, "EXACT CHANGE ONLY");
        }

        [Test]
        public void DisplayExactChangeOnlyWHenMachineIsLowOnChangeToReturnAfterItemPurchaseTest()
        {
            var soda = new ProductType(1, "Soda", 1.00M, 3);
            var chips = new ProductType(2, "Chips", .5M, 2);
            var candy = new ProductType(3, "Candy", .65M, 5);

            VendingMachine = new VendingMachineViewModel(new List<ProductType>() { soda, chips, candy }, 3, 3, 3);
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.InsertCoin("Quarter");
            VendingMachine.ButtonPressed(3);

            var display = VendingMachine.CheckDisplay();

            Assert.AreEqual(display, "EXACT CHANGE ONLY");
        }
    }
}
