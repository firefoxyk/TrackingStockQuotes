using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingStockQuotes
{
    public class Quote
    {
        public string Ticker { get; set; }
        public DateTime TradeDate { get; set; }
        public decimal? Open { get; set; } // Сделаем тип decimal nullable
        public decimal? High { get; set; }
        public decimal? Low { get; set; }
        public decimal? Close { get; set; }
        public int Volume { get; set; }
        public decimal Change { get; set; }
        public decimal ChangePercent { get; set; }
    }
}
