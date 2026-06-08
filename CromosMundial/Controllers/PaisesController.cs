
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CromosMundial.Models;
using CromosMundial.Data;

public class PaisesController : Controller
{
    private readonly AppDbContext _context;

    public PaisesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: PAISES
    public async Task<IActionResult> Index(string searchString, int page = 1)
    {
        int pageSize = 5;

        //var paises = _context.Paises.ToList(); // <- asi lo tenia al inicio sin filtro
        var paises = _context.Paises
            .Include(p => p.Equipos)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            paises = paises.Where(p => p.Nombre.Contains(searchString) || p.Continente.Contains(searchString));
        }

        int totalPaises = await paises.CountAsync();

        var paises2 = await paises
            .OrderBy(p => p.Nombre)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        ViewData["CurrentFilter"] = searchString;
        ViewData["CurrentPage"] = page;
        ViewData["TotalPages"] = (int)Math.Ceiling((double)totalPaises / pageSize);

        return View(paises2);
    }

    // GET: PAISES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var pais = await _context.Paises
            .FirstOrDefaultAsync(m => m.Id == id);
        if (pais == null)
        {
            return NotFound();
        }

        return View(pais);
    }

    // GET: PAISES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: PAISES/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nombre,Continente,CodigoFifa,RankingFifa")] Pais pais)
    {
        if (ModelState.IsValid)
        {
            _context.Add(pais);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(pais);
    }

    // GET: PAISES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var pais = await _context.Paises.FindAsync(id);
        if (pais == null)
        {
            return NotFound();
        }
        return View(pais);
    }

    // POST: PAISES/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Nombre,Continente,CodigoFifa,RankingFifa")] Pais pais)
    {
        if (id != pais.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(pais);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaisExists(pais.Id))
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
        return View(pais);
    }

    // GET: PAISES/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var pais = await _context.Paises
            .FirstOrDefaultAsync(m => m.Id == id);
        if (pais == null)
        {
            return NotFound();
        }

        return View(pais);
    }

    // POST: PAISES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var pais = await _context.Paises.FindAsync(id);
        if (pais != null)
        {
            _context.Paises.Remove(pais);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PaisExists(int? id)
    {
        return _context.Paises.Any(e => e.Id == id);
    }
}
