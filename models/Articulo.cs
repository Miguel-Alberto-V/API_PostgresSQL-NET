using System.ComponentModel.DataAnnotations.Schema;

namespace MyMicroservice.Models
{
    [Table("articulos")]
    public class Articulo
    {
        public int id { get; set; }
        public string? titulo { get; set; } // Acepta valores nulos
        public string? descripcion { get; set; } // Acepta valores nulos
        public string? url_imagen { get; set; } // Acepta valores nulos
        public string? url_articulo { get; set; } // Acepta valores nulos
        public DateTime fecha_creacion { get; set; } = DateTime.Now;
    }

    [Table("visitas")]
    public class Visita
    {
        public int id { get; set; }
        public string? tipo_elemento { get; set; }
        public int? id_elemento { get; set; }
        public DateTime fecha_visita { get; set; }
    }

    public class ArticuloVisitaDto
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public int TotalVisitas { get; set; }
    }

}