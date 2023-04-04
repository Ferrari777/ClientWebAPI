using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientWebAPI.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string? FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string? LastName { get; set; }

        [Required]
        public string? Email { get; set; }

        public DateTime Registered { get; set; }
    }
}
