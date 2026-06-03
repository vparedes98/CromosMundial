using System.ComponentModel;

namespace CromosMundial.Models
{
    public class Pais
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Continente { get; set; }

        [DisplayName("Código FIFA")]
        public required string CodigoFifa { get; set; }

        [DisplayName("Ranking FIFA")]
        public int RankingFifa { get; set; }

        public ICollection<Equipo>? Equipos { get; set; } // un pais tiene varios equipos
    }
}
