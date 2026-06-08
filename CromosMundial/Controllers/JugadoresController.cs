
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CromosMundial.Models;
using CromosMundial.Data;

public class JugadoresController : Controller
{
    private readonly AppDbContext _context;

    public JugadoresController(AppDbContext context)
    {
        _context = context;
    }

    // GET: JUGADORES
    public async Task<IActionResult> Index(string searchString, int page = 1)
    {
        int pageSize = 5;

        var jugadores = _context.Jugadores
            .Include(j => j.Equipo)
            .AsQueryable();

        // filtro: nombre del jugador o nombre del equipo
        if (!string.IsNullOrEmpty(searchString))
        {
            jugadores = jugadores.Where(j => j.Nombre.Contains(searchString) || j.Equipo!.Nombre.Contains(searchString));
        }

        int totalJugadores = await jugadores.CountAsync();

        var jugadores2 = await jugadores
            .OrderBy(j => j.Nombre)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        ViewData["CurrentFilter"] = searchString;
        ViewData["CurrentPage"] = page;
        ViewData["TotalPages"] = (int)Math.Ceiling((double)totalJugadores / pageSize);

        return View(jugadores2);
    }

    // todos los jugadores de un equipo (el ejemplo del enunciado)
    public async Task<IActionResult> PorEquipo(int Id)
    {
        var jugadores = _context.Jugadores
            .Include(j => j.Equipo)
            .Where(j => j.EquipoId == Id);

        ViewData["EquipoId"] = Id;
        ViewData["EquipoNombre"] = jugadores.FirstOrDefault()?.Equipo?.Nombre;

        return View(await jugadores.OrderBy(j => j.Nombre).ToListAsync());
    }

    // GET: JUGADORES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var jugador = await _context.Jugadores
            .Include(j => j.Equipo)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (jugador == null)
        {
            return NotFound();
        }

        return View(jugador);
    }

    // GET: JUGADORES/Create
    public IActionResult Create()
    {
        ViewData["EquipoId"] = new SelectList(_context.Equipos, "Id", "Nombre");
        return View();
    }

    // POST: JUGADORES/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nombre,Posicion,NumeroCamiseta,FechaNacimiento,EquipoId")] Jugador jugador)
    {
        if (ModelState.IsValid)
        {
            _context.Add(jugador);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["EquipoId"] = new SelectList(_context.Equipos, "Id", "Nombre");
        return View(jugador);
    }

    // GET: JUGADORES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var jugador = await _context.Jugadores.FindAsync(id);
        if (jugador == null)
        {
            return NotFound();
        }
        ViewData["EquipoId"] = new SelectList(_context.Equipos, "Id", "Nombre");
        return View(jugador);
    }

    // POST: JUGADORES/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Nombre,Posicion,NumeroCamiseta,FechaNacimiento,EquipoId")] Jugador jugador)
    {
        if (id != jugador.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(jugador);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JugadorExists(jugador.Id))
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
        ViewData["EquipoId"] = new SelectList(_context.Equipos, "Id", "Nombre");
        return View(jugador);
    }

    // GET: JUGADORES/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var jugador = await _context.Jugadores
            .Include(j => j.Equipo)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (jugador == null)
        {
            return NotFound();
        }

        return View(jugador);
    }

    // POST: JUGADORES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var jugador = await _context.Jugadores.FindAsync(id);
        if (jugador != null)
        {
            _context.Jugadores.Remove(jugador);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool JugadorExists(int? id)
    {
        return _context.Jugadores.Any(e => e.Id == id);
    }
}
