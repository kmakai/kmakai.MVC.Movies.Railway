using kmakai.MVC.Movies.DataAccess;
using kmakai.MVC.Movies.Models;
using Microsoft.AspNetCore.Mvc;

namespace kmakai.MVC.Movies.Controllers;

public class BookController : Controller
{

    private readonly AppDbContext _context;


    public BookController(AppDbContext context)
    {
        _context = context;
    }
    
    public IActionResult Index(string searchString)
    {
        var books = from b in _context.Books
                    select b;
        if(!String.IsNullOrEmpty(searchString))
        {
            books = books.Where(s => s.Title.Contains(searchString) || s.Author.Contains(searchString));
        }

        return View(books);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Author,Published,Genre,Price")] Book book)
    {
        if (ModelState.IsValid)
        {
            book.Published = book.Published.UtcDateTime;
            _context.Add(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        Console.WriteLine("Model state is not valid " + ModelState.ToString());

        foreach (var modelStateEntry in ModelState)
        {
            var key = modelStateEntry.Key;
            var errors = modelStateEntry.Value.Errors;

            // Log the key and its errors
            foreach (var error in errors)
            {
                Console.WriteLine($"Key: {key}, Error: {error.ErrorMessage}");
            }
        }

        return View(book);
    }

    public async Task<IActionResult> Details(int id)
    {
        var book = await _context.Books.FindAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var book = await _context.Books.FindAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,Published,Genre,Price")] Book book)
    {
        if(id != book.Id)
        {
            return NotFound();
        }

        if(ModelState.IsValid)
        {
            book.Published = book.Published.UtcDateTime;
            _context.Update(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(book);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var book = await _context.Books.FindAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id, [Bind("Id,Title,Author,Published,Genre,Price")] Book book)
    {
        if(id != book.Id)
        {
            return NotFound();
        }

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
