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

        public ProductType(int buttonNumber, string name, double price)
        {
            _buttonNumber = buttonNumber;
            _name = name;
            _price = price;
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

        private double _price;
        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }

    }
}
