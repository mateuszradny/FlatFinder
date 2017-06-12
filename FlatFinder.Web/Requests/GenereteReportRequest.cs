using System;
using System.ComponentModel.DataAnnotations;

namespace FlatFinder.Web.Requests
{
    public class GenerateReportRequest
    {
        [Required]
        [Display(Name = "From date")]
        [DataType(DataType.Date)]
        public DateTime From { get; set; }

        [Required]
        [Display(Name = "To date")]
        [DataType(DataType.Date)]
        public DateTime To { get; set; }
    }
}