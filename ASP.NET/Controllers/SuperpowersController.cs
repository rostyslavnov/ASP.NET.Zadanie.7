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

        // Lista supermocy z paginacją
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

        // Dodaj nową supermoc
        public IActionResult Dodaj()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Dodaj(Superpower superpower)
        {
            if (ModelState.IsValid)
            {
                _context.Superpowers.Add(superpower);
                _context.SaveChanges();
                return RedirectToAction(nameof(Lista));
            }

            return View(superpower);
        }

        // Szczegóły supermocy
        public IActionResult Szczegoly(int id)
        {
            var superpower = _context.Superpowers.FirstOrDefault(sp => sp.Id == id);
            if (superpower == null)
                return NotFound();

            return View(superpower);
        }
    }
}
