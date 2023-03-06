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
		
		// Used to get instance of User accessing the page
        private readonly UserManager<User> _userManager;

        public ContainerController(PUCCIIdentityContext context, UserManager<User> userManager)
		{
			_context = context;
			_userManager = userManager;
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
		public async Task<IActionResult> Details(int id)
		{
			if (_context.Containers == null)
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

		[Authorize]
		public IActionResult Run (int id)
		{
			if (_context.Containers == null)
			{
				return NotFound();
			}

			//var container = (from ctrs in _context.Containers
			//				where ctrs.ID == id
			//				select ctrs).First();

			//container.Run();

			return View();
		}

		// GET: Container/Create
		[Authorize]
		public async Task<IActionResult> Create(string ImageID)
		{
			var container = new Container();
			container.Create(ImageID);

            // Get User to save image in
            var user = await _userManager.GetUserAsync(User);

            if (user is null)
            {
                // Replace this with a reasonable error
                return Problem("User not found");
            }

            // Will only run if user is creating images the first time
            user.Containers ??= new List<Container>();
            user.Containers.Add(container);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

		// GET: Container/Edit/5
		[Authorize]
		public async Task<IActionResult> Edit(int id)
		{
			if (_context.Containers == null)
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

		// GET: Container/Delete/5
		[Authorize]
		public async Task<IActionResult> Delete(int id)
		{
			if (_context.Containers == null)
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
		public async Task<IActionResult> DeleteConfirmed(int id)
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

		private bool ContainerExists(int id)
		{
			return (_context.Containers?.Any(e => e.ID == id)).GetValueOrDefault();
		}
	}
}
