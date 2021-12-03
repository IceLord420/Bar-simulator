using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bar_simulator
{
    class Drinks
    {
        public string name { get;set; }
        public double price { get; set; }
        public int count { get; set; }
        public double sale { get; set; }

        public Drinks(string name, double price, int count, double sale)
        {
            this.name = name;
            this.price = price;
            this.count = count;
            this.sale = sale;
        }
    }
}
