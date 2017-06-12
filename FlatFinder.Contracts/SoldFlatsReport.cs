using System;

namespace FlatFinder.Contracts
{
    public class SoldFlatsReport
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public int Count { get; set; }
        public double AveragePrice { get; set; }
        public double AverageArea { get; set; }
    }
}