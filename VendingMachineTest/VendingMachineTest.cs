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
		}
    }
}
