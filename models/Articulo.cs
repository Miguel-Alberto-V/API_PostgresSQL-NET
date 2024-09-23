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

}