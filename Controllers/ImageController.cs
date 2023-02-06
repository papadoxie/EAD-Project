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
    public class ImageController : Controller
    {
        private readonly PUCCIIdentityContext _context;
        private readonly IWebHostEnvironment Environment;
        public ImageController(PUCCIIdentityContext context, IWebHostEnvironment environment)
        {
            Environment = environment;
            _context = context;
        }

        // GET: Image
        public async Task<IActionResult> Index()
        {
              return _context.Images != null ? 
                          View(await _context.Images.ToListAsync()) :
                          Problem("Entity set 'PUCCIIdentityContext.Images'  is null.");
        }

        // GET: Image/Details/5
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Image/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Image image,IFormFile Dockerfile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(image);
                await _context.SaveChangesAsync();
                if (Dockerfile == null)
                {
                    ModelState.AddModelError("FileURL", "Please upload file");
                }
                string wwwPath = this.Environment.WebRootPath;
                string path = Path.Combine(wwwPath, "DockerFiles");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var fileName = Path.GetFileName(Dockerfile.FileName);
                var pathWithFileName = Path.Combine(path, fileName);
                using (FileStream stream = new
                    FileStream(pathWithFileName,
                    FileMode.Create))
                {
                    Dockerfile.CopyTo(stream);
                    ViewBag.Message = "file uploaded successfully";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(image);
        }

        // GET: Image/Edit/5
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
