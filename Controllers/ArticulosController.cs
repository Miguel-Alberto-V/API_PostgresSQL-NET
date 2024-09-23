using MyMicroservice.Data;
using MyMicroservice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ArticulosController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ArticulosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/articulos
    [HttpGet]
    public async Task<IActionResult> GetArticulos()
    {
        var articulos = await _context.Articulos.ToListAsync();
        return Ok(articulos);
    }

    // GET: api/articulos/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetArticulo(int id)
    {
        var articulo = await _context.Articulos.FindAsync(id);
        if (articulo == null)
        {
            return NotFound();
        }
        return Ok(articulo);
    }
}
