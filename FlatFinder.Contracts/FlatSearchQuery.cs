namespace FlatFinder.Contracts
{
    public class FlatSearchQuery
    {
        public int? MinNumberOfRooms { get; set; }
        public int? MaxNumberOfRooms { get; set; }
        public double? MinArea { get; set; }
        public double? MaxArea { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public bool? HasBalcony { get; set; }
    }
}