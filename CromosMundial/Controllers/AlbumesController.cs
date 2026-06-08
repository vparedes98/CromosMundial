
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CromosMundial.Models;
using CromosMundial.Data;

public class AlbumesController : Controller
{
    private readonly AppDbContext _context;

    public AlbumesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: ALBUMES
    public async Task<IActionResult> Index(string searchString, int page = 1)
    {
        int pageSize = 5;

        var albumes = _context.Albumes
            .Include(a => a.Cromos)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            albumes = albumes.Where(a => a.Nombre.Contains(searchString));
        }

        int totalAlbumes = await albumes.CountAsync();

        var albumes2 = await albumes
            .OrderBy(a => a.Nombre)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        ViewData["CurrentFilter"] = searchString;
        ViewData["CurrentPage"] = page;
        ViewData["TotalPages"] = (int)Math.Ceiling((double)totalAlbumes / pageSize);

        return View(albumes2);
    }

    // GET: ALBUMES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var album = await _context.Albumes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (album == null)
        {
            return NotFound();
        }

        return View(album);
    }

    // GET: ALBUMES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ALBUMES/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nombre,Anio,CantidadCromos,EdicionEspecial")] Album album)
    {
        if (ModelState.IsValid)
        {
            _context.Add(album);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(album);
    }

    // GET: ALBUMES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var album = await _context.Albumes.FindAsync(id);
        if (album == null)
        {
            return NotFound();
        }
        return View(album);
    }

    // POST: ALBUMES/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Nombre,Anio,CantidadCromos,EdicionEspecial")] Album album)
    {
        if (id != album.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(album);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlbumExists(album.Id))
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
        return View(album);
    }

    // GET: ALBUMES/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var album = await _context.Albumes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (album == null)
        {
            return NotFound();
        }

        return View(album);
    }

    // POST: ALBUMES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var album = await _context.Albumes.FindAsync(id);
        if (album != null)
        {
            _context.Albumes.Remove(album);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AlbumExists(int? id)
    {
        return _context.Albumes.Any(e => e.Id == id);
    }
}
