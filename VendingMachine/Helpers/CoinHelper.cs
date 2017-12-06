using System;
using System.Collections.Generic;
using System.Text;
using VendingMachine.Enums;

namespace VendingMachine.Helpers
{
	public static class CoinHelper
	{
		public static double GetCoinValue(string coinName)
		{
			if (IsValidCoin(coinName))
			{
				return (int)Enum.Parse(typeof(AcceptedCoinEnum), coinName) / 100.0;
			}
			else
			{
				return 0;
			}
		}

		private static bool IsValidCoin(string coinName)
		{
			return Enum.IsDefined(typeof(AcceptedCoinEnum), coinName);			
		}
    }
}
