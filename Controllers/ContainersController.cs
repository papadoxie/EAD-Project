using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PUCCI.Data;
using PUCCI.Models;

namespace PUCCI.Controllers
{
    public class ContainersController : Controller
    {
        private readonly PUCCIContext _context;

        public ContainersController(PUCCIContext context)
        {
            _context = context;
        }

        // GET: Containers
        public async Task<IActionResult> Index()
        {
              return _context.Container != null ? 
                          View(await _context.Container.ToListAsync()) :
                          Problem("Entity set 'PUCCIContext.Container'  is null.");
        }

        // GET: Containers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Container == null)
            {
                return NotFound();
            }

            var container = await _context.Container
                .FirstOrDefaultAsync(m => m.ID == id);
            if (container == null)
            {
                return NotFound();
            }

            return View(container);
        }

        // GET: Containers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Containers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Image,Status")] Container container)
        {
            if (ModelState.IsValid)
            {
                _context.Add(container);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(container);
        }

        // GET: Containers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Container == null)
            {
                return NotFound();
            }

            var container = await _context.Container.FindAsync(id);
            if (container == null)
            {
                return NotFound();
            }
            return View(container);
        }

        // POST: Containers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,Name,Image,Status")] Container container)
        {
            if (id != container.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(container);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContainerExists(container.ID))
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
            return View(container);
        }

        // GET: Containers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Container == null)
            {
                return NotFound();
            }

            var container = await _context.Container
                .FirstOrDefaultAsync(m => m.ID == id);
            if (container == null)
            {
                return NotFound();
            }

            return View(container);
        }

        // POST: Containers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Container == null)
            {
                return Problem("Entity set 'PUCCIContext.Container'  is null.");
            }
            var container = await _context.Container.FindAsync(id);
            if (container != null)
            {
                _context.Container.Remove(container);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContainerExists(string id)
        {
          return (_context.Container?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
