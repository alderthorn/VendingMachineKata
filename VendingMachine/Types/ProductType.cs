using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Types
{
    public class ProductType
    {
        public ProductType()
        {
        }

        public ProductType(int buttonNumber, string name, decimal price, int quantity)
        {
            _buttonNumber = buttonNumber;
            _name = name;
            _price = price;
            _quantity = quantity;
        }

        private int _buttonNumber;
        public int ButtonNumber
        {
            get { return _buttonNumber; }
            set { _buttonNumber = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private decimal _price;
        public decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }

        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }
    }
}
