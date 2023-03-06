using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PUCCI.Areas.Identity.Data;
using PUCCI.Data;
using PUCCI.Models;

namespace PUCCI.Controllers
{
	public class ImageController : Controller
	{
		private readonly PUCCIIdentityContext _context;

		// Used to get filepath for saving Dockerfiles
		private readonly IWebHostEnvironment _environment;

		// Used to get instance of User accessing the page
		private readonly UserManager<User> _userManager;

		public ImageController(PUCCIIdentityContext context,
								IWebHostEnvironment environment,
								UserManager<User> userManager)
		{
			_environment = environment;
			_context = context;
			_userManager = userManager;
		}

		// GET: Image
		[Authorize]
		public async Task<IActionResult> Index()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var images = from img in _context.Images
						 where img.UserId == userId
						 select img;
			return _context.Images != null ?
						  View(await images.ToListAsync()) :
						  Problem("Entity set 'PUCCIIdentityContext.Images'  is null.");
		}

		// GET: Image/Details/5
		[Authorize]
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.Images == null)
			{
				return NotFound();
			}

			var image = await _context.Images
				.FirstOrDefaultAsync(m => m.ID == id);
			if (image == null)
			{
				return NotFound();
			}

			return View(image);
		}

		// GET: Image/Create
		[Authorize]
		public IActionResult Create()
		{
			return View();
		}

		// POST: Image/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> Create([Bind("Name")] Image image, IFormFile Dockerfile)
		{
			if (!ModelState.IsValid || Dockerfile is null)
			{
				ViewBag.Message = "Error";
				return Problem("Dockerfile or Image is invalid");
			}

			// Save Dockerfile to webroot
			string wwwPath = _environment.WebRootPath;
			string path = Path.Combine(wwwPath, "Dockerfiles");

			image.Create(Dockerfile, path);

			// Get User to save image in
			var user = await _userManager.GetUserAsync(User);

			if (user is null)
			{
				// Replace this with a reasonable error
				return Problem("User not found");
			}

			// Will only run if user is creating images the first time
			
			
			user.Images ??= new List<Image>();
			
			user.Images.Add(image);

			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}

		// GET: Image/Edit/5
		[Authorize]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.Images == null)
			{
				return NotFound();
			}

			var image = await _context.Images.FindAsync(id);
			if (image == null)
			{
				return NotFound();
			}
			return View(image);
		}

		// POST: Image/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> Edit(int? id, [Bind("ID,ImageID,Name,Tag,Created,DockerfilePath")] Image image)
		{
			if (id != image.ID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(image);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ImageExists(image.ID))
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
			return View(image);
		}

		// GET: Image/Delete/5
		[Authorize]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.Images == null)
			{
				return NotFound();
			}

			var image = await _context.Images
				.FirstOrDefaultAsync(m => m.ID == id);
			if (image == null)
			{
				return NotFound();
			}

			return View(image);
		}

		// POST: Image/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> DeleteConfirmed(int? id)
		{
			if (_context.Images == null)
			{
				return Problem("Entity set 'PUCCIIdentityContext.Images'  is null.");
			}
			var image = await _context.Images.FindAsync(id);
			if (image != null)
			{
				_context.Images.Remove(image);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ImageExists(int? id)
		{
			return (_context.Images?.Any(e => e.ID == id)).GetValueOrDefault();
		}
	}
}
