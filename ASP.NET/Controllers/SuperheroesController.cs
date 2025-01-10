using Microsoft.AspNetCore.Mvc;
using ASP.NET.Data;
using ASP.NET.Models;
using System.Linq;

namespace ASP.NET.Controllers
{
    public class SuperheroesController : Controller
    {
        private readonly ApplicationContext _context;

        public SuperheroesController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult Lista()
        {
            var superheroes = _context.Superheroes.ToList();

            foreach (var hero in superheroes)
            {
                hero.superhero_name ??= "Unknown Name";
                hero.full_name ??= "Unknown Full Name";
                hero.weight_kg ??= 0;
                hero.height_cm ??= 0;
            }

            return View(superheroes);
        }


        public IActionResult Dodaj()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Dodaj(Superhero superhero)
        {
            if (ModelState.IsValid)
            {
                _context.Superheroes.Add(superhero);
                _context.SaveChanges();
                return RedirectToAction(nameof(Lista));
            }
            return View(superhero);
        }

        public IActionResult Szczegoly(int id)
        {
            var superhero = _context.Superheroes.FirstOrDefault(s => s.Id == id);
            if (superhero == null)
                return NotFound();

            return View(superhero);
        }

        public IActionResult Usun(int id)
        {
            var superhero = _context.Superheroes.FirstOrDefault(s => s.Id == id);
            if (superhero == null)
                return NotFound();

            _context.Superheroes.Remove(superhero);
            _context.SaveChanges();
            return RedirectToAction(nameof(Lista));
        }
    }
}