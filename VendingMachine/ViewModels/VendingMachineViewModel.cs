using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendingMachine.Helpers;
using VendingMachine.Types;

namespace VendingMachine.ViewModels
{
    public class VendingMachineViewModel
    {
		private const string DEFAULTDISPLAYSTRING = "INSERT COIN";
        private ProductType _despensedItem;

        public List<ProductType> Products { get; set; }        
        public List<string> CoinReturn { get; set; }
        
		public VendingMachineViewModel(){
			DisplayString = DEFAULTDISPLAYSTRING;
            CoinReturn = new List<string>();
            _despensedItem = null;
            Products = new List<ProductType>() { new ProductType(1, "Soda", 1.00), new ProductType(2, "Chips", 0.50), new ProductType(3, "Candy", 0.65) };
		}

        private ProductType _selectedProduct;
        public ProductType SelectedProduct
        {
            get { return _selectedProduct; }
            set { _selectedProduct = value; }
        }

        private string _displayString;
		public string DisplayString
		{
			get { return _displayString; }
			set { _displayString = value; }
		}

		private double _creditValue;
		public double CreditValue
		{
			get { return _creditValue; }
			set { _creditValue = value; }
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

        public void ButtonPressed(int buttonNumber)
        {
            SelectedProduct = Products.FirstOrDefault(x => x.ButtonNumber == buttonNumber);

            TryToDespenseProduct();
        }

        public ProductType CheckForDespensedProducts()
        {
            return _despensedItem;
        }

        public void TryToDespenseProduct()
        {
            if(SelectedProduct != null)
            {
                if(SelectedProduct.Price <= CreditValue)
                {
                    _despensedItem = SelectedProduct;
                }
            }
        }
	}
}
