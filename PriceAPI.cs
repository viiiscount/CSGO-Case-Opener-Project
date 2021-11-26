using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class PriceAPI
    {
        public bool success { get; set; }
        public double average_price { get; set; }
        public double median_price { get; set; }
        public int amount_sold { get; set; }
        public double standard_deviation { get; set; }
        public double lowest_price { get; set; }
        public double highest_price { get; set; }
        public int first_sale_date { get; set; }
        public int time { get; set; }
        public string currency { get; set; }
    }
}
