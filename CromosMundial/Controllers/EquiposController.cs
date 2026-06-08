
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CromosMundial.Models;
using CromosMundial.Data;

public class EquiposController : Controller
{
    private readonly AppDbContext _context;

    public EquiposController(AppDbContext context)
    {
        _context = context;
    }

    // GET: EQUIPOS
    public async Task<IActionResult> Index(string searchString, int page = 1)
    {
        int pageSize = 5; // 5 por pagina

        var equipos = _context.Equipos
            .Include(e => e.Pais)  //para traer el pais y no solo el id
            .AsQueryable();

        // buscar por nombre del equipo o por el pais
        if (!string.IsNullOrEmpty(searchString))
        {
            equipos = equipos.Where(e => e.Nombre.Contains(searchString) || e.Pais!.Nombre.Contains(searchString));
        }

        int total = await equipos.CountAsync();

        var lista = await equipos
            .OrderBy(e => e.Nombre)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        ViewData["CurrentFilter"] = searchString;
        ViewData["CurrentPage"] = page;
        ViewData["TotalPages"] = (int)Math.Ceiling((double)total / pageSize);

        return View(lista);
    }

    // ver los equipos de un pais (boton "Ver Equipos")
    public async Task<IActionResult> PorPais(int Id)
    {
        var equipos = _context.Equipos
            .Include(e => e.Pais)
            .Where(e => e.PaisId == Id);

        ViewData["PaisId"] = Id;
        ViewData["PaisNombre"] = equipos.FirstOrDefault()?.Pais?.Nombre; // para el titulo

        return View(await equipos.OrderBy(e => e.Nombre).ToListAsync());
    }

    // GET: EQUIPOS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var equipo = await _context.Equipos
            .Include(e => e.Pais)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (equipo == null)
        {
            return NotFound();
        }

        return View(equipo);
    }

    // GET: EQUIPOS/Create
    public IActionResult Create()
    {
        ViewData["PaisId"] = new SelectList(_context.Paises, "Id", "Nombre");
        return View();
    }

    // POST: EQUIPOS/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nombre,DirectorTecnico,AnioFundacion,Logo,GrupoMundialista,PaisId,LogoArchivo")] Equipo equipo)
    {
        if (ModelState.IsValid)
        {
            // si subio logo lo guardo con un nombre unico (guid)
            if (equipo.LogoArchivo != null)
            {
                var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(equipo.LogoArchivo.FileName);
                var ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagenes", nombreArchivo);
                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    await equipo.LogoArchivo.CopyToAsync(stream);
                }
                equipo.Logo = "/imagenes/" + nombreArchivo;
            }

            _context.Add(equipo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["PaisId"] = new SelectList(_context.Paises, "Id", "Nombre");
        return View(equipo);
    }

    // GET: EQUIPOS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var equipo = await _context.Equipos.FindAsync(id);
        if (equipo == null)
        {
            return NotFound();
        }
        ViewData["PaisId"] = new SelectList(_context.Paises, "Id", "Nombre");
        return View(equipo);
    }

    // POST: EQUIPOS/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Nombre,DirectorTecnico,AnioFundacion,Logo,GrupoMundialista,PaisId,LogoArchivo")] Equipo equipo)
    {
        if (id != equipo.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                // TODO: validar que el archivo sea imagen (png/jpg)
                if (equipo.LogoArchivo != null)
                {
                    var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(equipo.LogoArchivo.FileName);
                    var ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagenes", nombreArchivo);
                    using (var stream = new FileStream(ruta, FileMode.Create))
                    {
                        await equipo.LogoArchivo.CopyToAsync(stream);
                    }
                    equipo.Logo = "/imagenes/" + nombreArchivo;
                }

                _context.Update(equipo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EquipoExists(equipo.Id))
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
        ViewData["PaisId"] = new SelectList(_context.Paises, "Id", "Nombre");
        return View(equipo);
    }

    // GET: EQUIPOS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var equipo = await _context.Equipos
            .Include(e => e.Pais)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (equipo == null)
        {
            return NotFound();
        }

        return View(equipo);
    }

    // POST: EQUIPOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var equipo = await _context.Equipos.FindAsync(id);
        if (equipo != null)
        {
            _context.Equipos.Remove(equipo);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool EquipoExists(int? id)
    {
        return _context.Equipos.Any(e => e.Id == id);
    }
}
