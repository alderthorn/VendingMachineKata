using System;
using System.Collections.Generic;
using System.Text;
using VendingMachine.Enums;

namespace VendingMachine.Helpers
{
	public static class CoinHelper
	{
		public static decimal GetCoinValue(string coinName)
		{
			if (IsValidCoin(coinName))
			{
				return (int)Enum.Parse(typeof(AcceptedCoinEnum), coinName) / 100.0M;
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
