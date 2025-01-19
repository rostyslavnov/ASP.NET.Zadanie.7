using System.Collections.Generic;
using ASP.NET.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

public class SuperheroViewModel
{
    public Superhero Superhero { get; set; } = new Superhero();
    public List<SelectListItem> Superpowers { get; set; } = new List<SelectListItem>();
    public List<int> SelectedSuperpowerIds { get; set; } = new List<int>();
}