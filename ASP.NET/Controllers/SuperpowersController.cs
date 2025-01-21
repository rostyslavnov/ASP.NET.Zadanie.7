using Microsoft.AspNetCore.Mvc;
using ASP.NET.Data;
using ASP.NET.Models;
using System.Linq;

namespace ASP.NET.Controllers
{
    public class SuperpowersController : Controller
    {
        private readonly ApplicationContext _context;

        public SuperpowersController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult Lista(int page = 1, int pageSize = 20)
        {
            var totalItems = _context.Superpowers.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var superpowers = _context.Superpowers
                .OrderBy(sp => sp.power_name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            foreach (var power in superpowers)
            {
                power.power_name ??= "Nieznana nazwa";
            }

            var viewModel = new PaginationViewModel<Superpower>
            {
                Items = superpowers,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(viewModel);
        }

        public IActionResult ListaWithHeroCount(int page = 1, int pageSize = 20)
        {
            var superpowersWithCount = _context.Superpowers
                .Select(sp => new SuperpowerWithHeroCount
                {
                    SuperpowerName = sp.power_name,
                    HeroCount = _context.Set<Dictionary<string, object>>("hero_power")
                        .Count(hp => (int)hp["power_id"] == sp.Id)
                })
                .OrderBy(sp => sp.SuperpowerName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalItems = _context.Superpowers.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var viewModel = new PaginationViewModel<SuperpowerWithHeroCount>
            {
                Items = superpowersWithCount,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(viewModel);
        }


        [HttpGet]
        public IActionResult Dodaj()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Dodaj(Superpower superpower)
        {
            _context.Superpowers.Add(superpower);
            
            _context.SaveChanges();
            
            return RedirectToAction(nameof(Lista));
        }



        public IActionResult Szczegoly(int id)
        {
            var superpower = _context.Superpowers.FirstOrDefault(sp => sp.Id == id);
            if (superpower == null)
                return NotFound();

            return View(superpower);
        }
    }
}
