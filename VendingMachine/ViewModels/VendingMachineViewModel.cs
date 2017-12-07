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
        private const string EXACT_CHANGE_ONLY_STRING = "EXACT CHANGE ONLY";
        private const string THANK_YOU_STRING = "THANK YOU";
        private ProductType _dispensedItem;
        private string _displayString;
        private int _quartersAvailibleForChange;
        private int _dimesAvailibleForChange;
        private int _nicklesAvailibleForChange;
        private bool _exactChangeOnlyMode;

        private List<ProductType> _products { get; set; }        
        private List<string> _coinReturn { get; set; }
        private List<string> _insertedCoins;                

        //Default Test Mode
		public VendingMachineViewModel(){
            InitObjects();
            _quartersAvailibleForChange = 50;
            _dimesAvailibleForChange = 50;
            _nicklesAvailibleForChange = 50;
            _products = new List<ProductType>() { new ProductType(1, "Soda", 1.00M, 50), new ProductType(2, "Chips", 0.50M, 50), new ProductType(3, "Candy", 0.65M, 50) };
		}
        //Starting Product
        public VendingMachineViewModel(List<ProductType> products, int startingQuarters, int startingDimes, int startingNickles)
        {
            InitObjects();
            _products = products;
            _quartersAvailibleForChange = startingQuarters;
            _dimesAvailibleForChange = startingDimes;
            _nicklesAvailibleForChange = startingNickles;
            _exactChangeOnlyMode = startingQuarters <= 2 || startingDimes <= 2 || startingNickles <= 2;

            if (_exactChangeOnlyMode)
            {
                _displayString = EXACT_CHANGE_ONLY_STRING;
            }
        }

        private void InitObjects()
        {
            _displayString = DEFAULT_DISPLAY_STRING;
            _dispensedItem = null;
            _exactChangeOnlyMode = false;
            _coinReturn = new List<string>();
            _insertedCoins = new List<string>();
            _products = new List<ProductType>();
        }        

        private ProductType _selectedProduct;
        public ProductType SelectedProduct
        {
            get { return _selectedProduct; }
            set { _selectedProduct = value; }
        }

		private decimal _creditValue;
		public decimal CreditValue
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
                if(SelectedProduct.Quantity == 0)
                {
                    SelectedProduct = null;
                    _displayString = "SOLD OUT";
                }
                else if(SelectedProduct.Price <= CreditValue)
                {
                    _dispensedItem = SelectedProduct;
                    SelectedProduct.Quantity -= 1;
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
                if(CreditValue >= .25M)
                {
                    _coinReturn.Add("Quarter");
                    _quartersAvailibleForChange -= 1;
                    CreditValue -= .25M;
                }
                else if(CreditValue >= .1M)
                {
                    _coinReturn.Add("Dime");
                    _dimesAvailibleForChange -= 1;
                    CreditValue -= .1M;
                }
                else if (CreditValue >= .05M)
                {
                    _coinReturn.Add("Nickle");
                    _nicklesAvailibleForChange -= 1;
                    CreditValue -= .05M;
                }
            }
            _exactChangeOnlyMode = _quartersAvailibleForChange <= 2 || _dimesAvailibleForChange <= 2 || _nicklesAvailibleForChange <= 2;
            if(_exactChangeOnlyMode)
            {
                _displayString = EXACT_CHANGE_ONLY_STRING;
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

        public decimal GetValueOfCoinReturn()
        {
            decimal value = 0;
            foreach(var coin in _coinReturn)
            {
                value += CoinHelper.GetCoinValue(coin);
            }

            return value;
        }

        public string CheckDisplay()
        {
            var returnString = _displayString;
            if(CreditValue > 0 )
            {
                _displayString = String.Format("{0:C2}", CreditValue);
            }
            else
            {

                _displayString = _exactChangeOnlyMode ? EXACT_CHANGE_ONLY_STRING : DEFAULT_DISPLAY_STRING;
            }

            return returnString;
        }
	}
}
