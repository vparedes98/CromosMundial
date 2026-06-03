using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CromosMundial.Models
{
    public class Cromo
    {
        public int Id { get; set; }

        [DisplayName("Número de Cromo")]
        public int NumeroCromo { get; set; }

        public required string Edicion { get; set; }

        [DisplayName("Valor de Mercado")]
        public decimal ValorMercado { get; set; }

        public string? Foto { get; set; }

        [NotMapped]
        [DisplayName("Foto")]
        public IFormFile? FotoArchivo { get; set; }  // archivo temporal del form

        // un cromo es de un jugador y de un album
        public int JugadorId { get; set; }
        public Jugador? Jugador { get; set; }

        public int AlbumId { get; set; }
        public Album? Album { get; set; }
    }
}
