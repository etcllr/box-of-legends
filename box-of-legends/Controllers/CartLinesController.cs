using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using box_of_legends.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using box_of_legends.Models;

namespace box_of_legends.Controllers
{
    public class CartLinesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartLinesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CartLines
        public async Task<IActionResult> Index()
        {
            return View(await _context.CartLine.ToListAsync());
        }

        // GET: CartLines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartLine = await _context.CartLine
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cartLine == null)
            {
                return NotFound();
            }

            return View(cartLine);
        }

        // GET: CartLines/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CartLines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Quantity")] CartLine cartLine)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cartLine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cartLine);
        }

        // GET: CartLines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartLine = await _context.CartLine.FindAsync(id);
            if (cartLine == null)
            {
                return NotFound();
            }
            return View(cartLine);
        }

        // POST: CartLines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Quantity")] CartLine cartLine)
        {
            if (id != cartLine.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cartLine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartLineExists(cartLine.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cartLine);
        }

        // GET: CartLines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartLine = await _context.CartLine
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cartLine == null)
            {
                return NotFound();
            }

            return View(cartLine);
        }

        // POST: CartLines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cartLine = await _context.CartLine.FindAsync(id);
            if (cartLine != null)
            {
                _context.CartLine.Remove(cartLine);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartLineExists(int id)
        {
            return _context.CartLine.Any(e => e.Id == id);
        }
    }
}
