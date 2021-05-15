using System.ComponentModel.DataAnnotations;
using static BestRoute.Domain.Extensions.CustomAttributes;

namespace BestRoute.Domain.Models
{
    public class BestRouteRequest
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "The field {0} is required.")]
        [StringLength(3, ErrorMessage = "The field {0} needs contains {1} characters.")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string From { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The field {0} is required.")]
        [StringLength(3, ErrorMessage = "The field {0} needs contains {1} characters.")]
        [NotEqual(nameof(From))]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string To { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The field {0} is required.")]
        [RegularExpression("^.*\\.(csv)$", ErrorMessage = "Enter a valid CSV file.")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string DataSource { get; set; }
    }
}
