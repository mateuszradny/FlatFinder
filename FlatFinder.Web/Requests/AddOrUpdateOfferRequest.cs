using System.ComponentModel.DataAnnotations;

namespace FlatFinder.Web.Requests
{
    public class AddOrUpdateFlatRequest
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Range(15.0, 1_000.0)]
        [Display(Name = "Area")]
        public double Area { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [StringLength(4000, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 100)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Range(-1, 15)]
        [Display(Name = "Floor")]
        public int Floor { get; set; }

        [Required]
        [Display(Name = "Has balcony")]
        public bool HasBalcony { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        [Display(Name = "Title")]
        public string Name { get; set; }

        [Required]
        [Range(1, 15)]
        [Display(Name = "Number of rooms")]
        public int NumberOfRooms { get; set; }

        [Required]
        [RegularExpression("^[0-9]{2}-[0-9]{3}$", ErrorMessage = "Invalid postal code.")]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Post code")]
        public string PostCode { get; set; }

        [Required]
        [Range(100.0, 10_000.0)]
        [DataType(DataType.Currency)]
        [Display(Name = "Price")]
        public double Price { get; set; }
    }
}