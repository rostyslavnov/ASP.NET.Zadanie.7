using System.ComponentModel.DataAnnotations;

namespace ASP.NET.Models
{
    public class Superpower
    {
        public int Id { get; set; }
        
        public string power_name { get; set; }
        
        public ICollection<Superhero> Superheroes { get; set; }
    }
}