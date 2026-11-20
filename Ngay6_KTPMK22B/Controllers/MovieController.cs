
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ngay6_KTPMK22B.Models;

public class MovieController : Controller
{
    private readonly Ngay6_KTPMK22BContext _context;

    public MovieController(Ngay6_KTPMK22BContext context)
    {
        _context = context;
    }

    // GET: MOVIES
    public async Task<IActionResult> Index(int? page)
    {
        int pageNumber = page ?? 1;
        int pageSize = 10;

        var movies = await _context.Movie
            .Include(m => m.Genre)
            .OrderBy(m => m.Title)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        int totalCount = await _context.Movie.CountAsync();
        int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        ViewBag.CurrentPage = pageNumber;
        ViewBag.TotalPages = totalPages;

        return View(movies);
    }

    // GET: MOVIES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movie
            .Include(m => m.Genre)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // GET: MOVIES/Create
    public IActionResult Create()
    {
        var genres = _context.Genre.ToList();
        Console.WriteLine($"[MovieController.Create GET] Loaded {genres.Count} genres from database");
        foreach (var g in genres)
        {
            Console.WriteLine($"  - ID: {g.Id}, Name: {g.Name}");
        }

        ViewData["GenreId"] = new SelectList(genres, "Id", "Name");
        return View();
    }

    // POST: MOVIES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Duration,GenreId,Genre")] Movie movie)
    {
        // Validate GenreId is not 0 (default/no selection)
        if (movie.GenreId == 0)
        {
            ModelState.AddModelError("GenreId", "Please select a Genre");
        }

        if (ModelState.IsValid)
        {
            // Validate that the Genre exists in the database
            var genreExists = await _context.Genre.AnyAsync(g => g.Id == movie.GenreId);
            if (!genreExists)
            {
                ModelState.AddModelError("GenreId", "The selected Genre does not exist.");
                ViewData["GenreId"] = new SelectList(_context.Genre, "Id", "Name", movie.GenreId);
                return View(movie);
            }

            try
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                // Log the exception and show a user-friendly error message
                ModelState.AddModelError("", "An error occurred while saving. Please try again.");
                ViewData["GenreId"] = new SelectList(_context.Genre, "Id", "Name", movie.GenreId);
                return View(movie);
            }
        }
        ViewData["GenreId"] = new SelectList(_context.Genre, "Id", "Name", movie.GenreId);
        return View(movie);
    }

    // GET: MOVIES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movie.FindAsync(id);
        if (movie == null)
        {
            return NotFound();
        }

        var genres = _context.Genre.ToList();
        Console.WriteLine($"[MovieController.Edit GET] Loaded {genres.Count} genres from database");
        ViewData["GenreId"] = new SelectList(genres, "Id", "Name", movie.GenreId);
        return View(movie);
    }

    // POST: MOVIES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Title,ReleaseDate,Duration,GenreId,Genre")] Movie movie)
    {
        if (id != movie.Id)
        {
            return NotFound();
        }

        // Validate GenreId is not 0 (default/no selection)
        if (movie.GenreId == 0)
        {
            ModelState.AddModelError("GenreId", "Please select a Genre");
        }

        if (ModelState.IsValid)
        {
            // Validate that the Genre exists in the database
            var genreExists = await _context.Genre.AnyAsync(g => g.Id == movie.GenreId);
            if (!genreExists)
            {
                ModelState.AddModelError("GenreId", "The selected Genre does not exist.");
                ViewData["GenreId"] = new SelectList(_context.Genre, "Id", "Name", movie.GenreId);
                return View(movie);
            }

            try
            {
                _context.Update(movie);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(movie.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (DbUpdateException ex)
            {
                // Log the exception and show a user-friendly error message
                ModelState.AddModelError("", "An error occurred while updating. Please try again.");
                ViewData["GenreId"] = new SelectList(_context.Genre, "Id", "Name", movie.GenreId);
                return View(movie);
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["GenreId"] = new SelectList(_context.Genre, "Id", "Name", movie.GenreId);
        return View(movie);
    }

    // GET: MOVIES/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movie
            .Include(m => m.Genre)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // POST: MOVIES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var movie = await _context.Movie.FindAsync(id);
        if (movie != null)
        {
            _context.Movie.Remove(movie);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MovieExists(int? id)
    {
        return _context.Movie.Any(e => e.Id == id);
    }
}
