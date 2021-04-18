using System.ComponentModel.DataAnnotations;

namespace ClientsExercise.Models
{
    public class PhoneNumber
    {
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Address should be minimum 2 characters and a maximum of 25 characters")]
        [DataType(DataType.Text)]
        public string Type { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string Number { get; set; }
    }
}
