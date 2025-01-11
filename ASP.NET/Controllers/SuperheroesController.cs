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

        public IActionResult Lista(int page = 1, int pageSize = 20)
        {
            var totalItems = _context.Superheroes.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var superheroes = _context.Superheroes
                .OrderBy(s => s.superhero_name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            foreach (var hero in superheroes)
            {
                hero.superhero_name ??= "Unknown Name";
                hero.full_name ??= "Unknown Full Name";
                hero.weight_kg ??= 0;
                hero.height_cm ??= 0;
            }

            var viewModel = new PaginationViewModel<Superhero>
            {
                Items = superheroes,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(viewModel);
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
