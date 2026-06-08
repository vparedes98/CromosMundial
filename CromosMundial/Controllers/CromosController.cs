
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CromosMundial.Models;
using CromosMundial.Data;

public class CromosController : Controller
{
    private readonly AppDbContext _context;

    public CromosController(AppDbContext context)
    {
        _context = context;
    }

    // GET: CROMOS
    public async Task<IActionResult> Index(string searchString, int page = 1)
    {
        int pageSize = 5;

        var cromos = _context.Cromos
            .Include(c => c.Jugador)
            .Include(c => c.Album)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            // se puede buscar por edicion o por el jugador
            cromos = cromos.Where(c => c.Edicion.Contains(searchString) || c.Jugador!.Nombre.Contains(searchString));
        }

        int totalCromos = await cromos.CountAsync();

        var cromos2 = await cromos
            .OrderBy(c => c.NumeroCromo)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        ViewData["CurrentFilter"] = searchString;
        ViewData["CurrentPage"] = page;
        ViewData["TotalPages"] = (int)Math.Ceiling((double)totalCromos / pageSize);

        return View(cromos2);
    }

    // cromos que pertenecen a un album
    public async Task<IActionResult> PorAlbum(int Id)
    {
        var cromos = _context.Cromos
            .Include(c => c.Jugador)
            .Include(c => c.Album)
            .Where(c => c.AlbumId == Id);

        ViewData["AlbumId"] = Id;
        ViewData["AlbumNombre"] = cromos.FirstOrDefault()?.Album?.Nombre;

        return View(await cromos.OrderBy(c => c.NumeroCromo).ToListAsync());
    }

    // GET: CROMOS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cromo = await _context.Cromos
            .Include(c => c.Jugador)
            .Include(c => c.Album)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (cromo == null)
        {
            return NotFound();
        }

        return View(cromo);
    }

    // GET: CROMOS/Create
    public IActionResult Create()
    {
        ViewData["JugadorId"] = new SelectList(_context.Jugadores, "Id", "Nombre");
        ViewData["AlbumId"] = new SelectList(_context.Albumes, "Id", "Nombre");
        return View();
    }

    // POST: CROMOS/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,NumeroCromo,Edicion,ValorMercado,Foto,JugadorId,AlbumId,FotoArchivo")] Cromo cromo)
    {
        if (ModelState.IsValid)
        {
            if (cromo.FotoArchivo != null)
            {
                var nombreImg = Guid.NewGuid() + Path.GetExtension(cromo.FotoArchivo.FileName);
                var rutaImg = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagenes", nombreImg);
                using (var st = new FileStream(rutaImg, FileMode.Create))
                {
                    await cromo.FotoArchivo.CopyToAsync(st);
                }
                cromo.Foto = "/imagenes/" + nombreImg;
            }

            _context.Add(cromo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["JugadorId"] = new SelectList(_context.Jugadores, "Id", "Nombre");
        ViewData["AlbumId"] = new SelectList(_context.Albumes, "Id", "Nombre");
        return View(cromo);
    }

    // GET: CROMOS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cromo = await _context.Cromos.FindAsync(id);
        if (cromo == null)
        {
            return NotFound();
        }
        ViewData["JugadorId"] = new SelectList(_context.Jugadores, "Id", "Nombre");
        ViewData["AlbumId"] = new SelectList(_context.Albumes, "Id", "Nombre");
        return View(cromo);
    }

    // POST: CROMOS/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,NumeroCromo,Edicion,ValorMercado,Foto,JugadorId,AlbumId,FotoArchivo")] Cromo cromo)
    {
        if (id != cromo.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                // solo cambio la foto si subieron una nueva, si no se queda la de antes
                if (cromo.FotoArchivo != null)
                {
                    var nombreImg = Guid.NewGuid() + Path.GetExtension(cromo.FotoArchivo.FileName);
                    var rutaImg = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagenes", nombreImg);
                    using (var st = new FileStream(rutaImg, FileMode.Create))
                    {
                        await cromo.FotoArchivo.CopyToAsync(st);
                    }
                    cromo.Foto = "/imagenes/" + nombreImg;
                }

                _context.Update(cromo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CromoExists(cromo.Id))
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
        ViewData["JugadorId"] = new SelectList(_context.Jugadores, "Id", "Nombre");
        ViewData["AlbumId"] = new SelectList(_context.Albumes, "Id", "Nombre");
        return View(cromo);
    }

    // GET: CROMOS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cromo = await _context.Cromos
            .Include(c => c.Jugador)
            .Include(c => c.Album)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (cromo == null)
        {
            return NotFound();
        }

        return View(cromo);
    }

    // POST: CROMOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var cromo = await _context.Cromos.FindAsync(id);
        if (cromo != null)
        {
            _context.Cromos.Remove(cromo);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CromoExists(int? id)
    {
        return _context.Cromos.Any(e => e.Id == id);
    }
}
