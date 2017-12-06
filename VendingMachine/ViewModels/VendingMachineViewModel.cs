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
		private const string DEFAULT_DISPLAY_STRING = "INSERT COIN";
        private const string THANK_YOU_STRING = "THANK YOU";
        private ProductType _dispensedItem;
        private string _displayString;

        private List<ProductType> _products { get; set; }        
        private List<string> _coinReturn { get; set; }
        private List<string> _insertedCoins;
        
		public VendingMachineViewModel(){
			_displayString = DEFAULT_DISPLAY_STRING;
            _coinReturn = new List<string>();
            _dispensedItem = null;
            _insertedCoins = new List<string>();
            _products = new List<ProductType>() { new ProductType(1, "Soda", 1.00), new ProductType(2, "Chips", 0.50), new ProductType(3, "Candy", 0.65) };
		}

        private ProductType _selectedProduct;
        public ProductType SelectedProduct
        {
            get { return _selectedProduct; }
            set { _selectedProduct = value; }
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
                _insertedCoins.Add(coinName);
                CreditValue += coinValue;
                TryToDispenseProduct();
            }
            else
            {
                _coinReturn.Add(coinName);
            }
		}

        public void ButtonPressed(int buttonNumber)
        {
            SelectedProduct = _products.FirstOrDefault(x => x.ButtonNumber == buttonNumber);

            TryToDispenseProduct();
        }

        public ProductType CheckForDispensedProducts()
        {
            return _dispensedItem;
        }

        public void TryToDispenseProduct()
        {
            if(SelectedProduct != null)
            {
                if(SelectedProduct.Price <= CreditValue)
                {
                    _dispensedItem = SelectedProduct;
                    _displayString = THANK_YOU_STRING;
                    CreditValue -= SelectedProduct.Price;

                    MakeChange();                    
                }
                else
                {
                    _displayString = String.Format("{0:C2}", SelectedProduct.Price);                    
                }
            }
        }

        public void MakeChange()
        {
            while(CreditValue > 0)
            {
                if(CreditValue >= .25)
                {
                    _coinReturn.Add("Quarter");
                    CreditValue -= .25;
                }
                else if(CreditValue >= .1)
                {
                    _coinReturn.Add("Dime");
                    CreditValue -= .1;
                }
                else if (CreditValue >= .05)
                {
                    _coinReturn.Add("Nickle");
                    CreditValue -= .05;
                }
            }
        }

        public void ClearChange()
        {
            _coinReturn.Clear();
        }

        public void ReturnChangeButtonPressed()
        {
            _coinReturn.AddRange(_insertedCoins);
            _insertedCoins.Clear();
        }

        public List<string> GetCoinsInReturnSlot()
        {
            return _coinReturn;
        }

        public double GetValueOfCoinReturn()
        {
            double value = 0;
            foreach(var coin in _coinReturn)
            {
                value += CoinHelper.GetCoinValue(coin);
            }

            return value;
        }

        public string CheckDisplay()
        {
            var returnString = _displayString;
            if (_displayString == THANK_YOU_STRING)
            {
                _displayString = DEFAULT_DISPLAY_STRING;
            }
            else if(CreditValue > 0 )
            {
                _displayString = String.Format("{0:C2}", CreditValue);
            }
            else
            {
                _displayString = DEFAULT_DISPLAY_STRING;
            }

            return returnString;
        }
	}
}
