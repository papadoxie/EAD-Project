using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using PUCCI.Areas.Identity.Data;
using PUCCI.Data;
using PUCCI.Models;

namespace PUCCI.Controllers
{
	public class ContainerController : Controller
	{
		private readonly PUCCIIdentityContext _context;

		public ContainerController(PUCCIIdentityContext context)
		{
			_context = context;
		}

		// GET: Container
		[Authorize]
		public async Task<IActionResult> Index()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var containers = from ctr in _context.Containers
							 where ctr.UserId == userId
							 select ctr;

			return _context.Containers != null ?
				View(await containers.ToListAsync()) :
				Problem("Entity set 'PUCCIIdentityContext.Containers'  is null.");
		}

		// GET: Container/Details/5
		[Authorize]
		public async Task<IActionResult> Details(string id)
		{
			if (id == null || _context.Containers == null)
			{
				return NotFound();
			}

			var container = await _context.Containers
				.FirstOrDefaultAsync(m => m.ID == id);
			if (container == null)
			{
				return NotFound();
			}

			return View(container);
		}

		// GET: Container/Create
		[Authorize]
		public IActionResult Create()
		{
			return View();
		}

		// POST: Container/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[Authorize]
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

		// GET: Container/Edit/5
		[Authorize]
		public async Task<IActionResult> Edit(string id)
		{
			if (id == null || _context.Containers == null)
			{
				return NotFound();
			}

			var container = await _context.Containers.FindAsync(id);
			if (container == null)
			{
				return NotFound();
			}
			return View(container);
		}

		// POST: Container/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[Authorize]
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

		// GET: Container/Delete/5
		[Authorize]
		public async Task<IActionResult> Delete(string id)
		{
			if (id == null || _context.Containers == null)
			{
				return NotFound();
			}

			var container = await _context.Containers
				.FirstOrDefaultAsync(m => m.ID == id);
			if (container == null)
			{
				return NotFound();
			}

			return View(container);
		}

		// POST: Container/Delete/5
		[HttpPost, ActionName("Delete")]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			if (_context.Containers == null)
			{
				return Problem("Entity set 'PUCCIIdentityContext.Containers'  is null.");
			}
			var container = await _context.Containers.FindAsync(id);
			if (container != null)
			{
				_context.Containers.Remove(container);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ContainerExists(string id)
		{
			return (_context.Containers?.Any(e => e.ID == id)).GetValueOrDefault();
		}
	}
}
