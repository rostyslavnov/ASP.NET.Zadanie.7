using System.ComponentModel.DataAnnotations;

namespace ASP.NET.Models
{
    public class Superpower
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string power_name { get; set; }
        
        public ICollection<Superhero> Superheroes { get; set; }
    }
}