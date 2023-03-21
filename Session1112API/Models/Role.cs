using System.ComponentModel.DataAnnotations;

namespace Session1112API.Models
{
    public class Role
    {
        [Required]
        public string Name { get; set; }
    }
}
