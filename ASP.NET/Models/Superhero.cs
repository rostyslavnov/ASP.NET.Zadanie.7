namespace ASP.NET.Models
{
    public class Superhero
    {
        public int Id { get; set; }
        public string? superhero_name { get; set; }
        public string? full_name { get; set; }
        public int? weight_kg { get; set; }
        public int? height_cm { get; set; }
        
        public ICollection<Superpower> Superpowers { get; set; }
    }
}