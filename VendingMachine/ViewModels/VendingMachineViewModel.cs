using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendingMachine.Helpers;

namespace VendingMachine.ViewModels
{
    public class VendingMachineViewModel
    {
		private const string DEFAULTDISPLAYSTRING = "INSERT COIN";

        public List<string> CoinReturn { get; set; }

		public VendingMachineViewModel(){
			DisplayString = DEFAULTDISPLAYSTRING;
            CoinReturn = new List<string>();
		}

		private string displayString;
		public string DisplayString
		{
			get { return displayString; }
			set { displayString = value; }
		}

		private double creditValue;
		public double CreditValue
		{
			get { return creditValue; }
			set { creditValue = value; }
		}

		public void InsertCoin(string coinName)
		{
			var coinValue = CoinHelper.GetCoinValue(coinName);
            if (coinValue > 0)
            {
                CreditValue += coinValue;
            }
            else
            {
                CoinReturn.Add(coinName);
            }
		}
	}
}
