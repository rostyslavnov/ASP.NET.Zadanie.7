using Microsoft.AspNetCore.Mvc;
using ASP.NET.Data;
using ASP.NET.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                hero.superhero_name ??= "Nieznana nazwa";
                hero.full_name ??= "Nieznane pełne imię";
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
        [ValidateAntiForgeryToken]
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

        public IActionResult Edytuj(int id)
        {
            var superhero = _context.Superheroes.FirstOrDefault(s => s.Id == id);
            if (superhero == null)
                return NotFound();

            return View(superhero);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edytuj(Superhero superhero)
        {
            if (ModelState.IsValid)
            {
                var existingHero = _context.Superheroes.FirstOrDefault(s => s.Id == superhero.Id);
                if (existingHero == null)
                    return NotFound();

                existingHero.superhero_name = superhero.superhero_name;
                existingHero.full_name = superhero.full_name;
                existingHero.weight_kg = superhero.weight_kg;
                existingHero.height_cm = superhero.height_cm;

                _context.SaveChanges();
                return RedirectToAction(nameof(Lista));
            }

            return View(superhero);
        }
        
        public IActionResult DodajSuperheroWithPowers()
        {
            var model = new SuperheroViewModel
            {
                Superhero = new Superhero(),
                Superpowers = _context.Superpowers
                    .Select(sp => new SelectListItem { Value = sp.Id.ToString(), Text = sp.power_name })
                    .ToList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DodajSuperheroWithPowers(SuperheroViewModel model)
        {
            if (ModelState.IsValid)
            {
                var superhero = model.Superhero;
                superhero.Superpowers = _context.Superpowers
                    .Where(sp => model.SelectedSuperpowerIds.Contains(sp.Id))
                    .ToList();

                _context.Superheroes.Add(superhero);
                _context.SaveChanges();

                return RedirectToAction(nameof(Lista));
            }
            
            model.Superpowers = _context.Superpowers
                .Select(sp => new SelectListItem { Value = sp.Id.ToString(), Text = sp.power_name })
                .ToList();

            return View(model);
        }
    }
}
