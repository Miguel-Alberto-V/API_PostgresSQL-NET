using MyMicroservice.Data;
using MyMicroservice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMicroservice.Services;
using Microsoft.AspNetCore.SignalR;
using MyMicroservice.Hubs;


[ApiController]
[Route("api/[controller]")]
public class ArticulosController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IHubContext<VisitasHub> _hubContext;

    public ArticulosController(ApplicationDbContext context, IHubContext<VisitasHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
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

    [HttpPost("{id}/visitar")]
    public async Task<IActionResult> VisitarArticulo(int id)
    {
        // Lógica para manejar la visita al artículo
        var producer = new RabbitMQProducer();
        producer.SendVisit("articulo", id);
        producer.Close();

        // Enviar notificación a los clientes conectados al hub
        await _hubContext.Clients.All.SendAsync("ReceiveVisita", $"Nueva visita al artículo con ID {id}");

        return Ok();
    }

    // Nueva ruta para obtener las visitas
    // GET: api/articulos/visitas
    [HttpGet("visitas")]
    public async Task<IActionResult> GetVisitas()
    {
        var visitas = await _context.Articulos
            .GroupJoin(
                _context.Visitas.Where(v => v.tipo_elemento == "articulo"),
                a => a.id,
                v => v.id_elemento,
                (articulo, visitas) => new ArticuloVisitaDto
                {
                    Id = articulo.id,
                    Titulo = articulo.titulo,
                    TotalVisitas = visitas.Count()
                })
            .OrderByDescending(av => av.TotalVisitas)
            .ToListAsync();

        return Ok(visitas);
    }

}
