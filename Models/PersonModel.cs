using System.ComponentModel.DataAnnotations;

namespace Fetch_API.Models
{
    public class PersonModel
    {
        [Required]
        public int PersonID { get; set; }
        [Required]
        public string Pname { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Contact { get; set; }
    }
}
