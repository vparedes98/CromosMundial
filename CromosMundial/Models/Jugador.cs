using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CromosMundial.Models
{
    public class Jugador
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Posicion { get; set; }

        [DisplayName("Número de Camiseta")]
        public int NumeroCamiseta { get; set; }

        [DisplayName("Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        // el jugador pertenece a un equipo (FK)
        public int EquipoId { get; set; }
        public Equipo? Equipo { get; set; }

        public ICollection<Cromo>? Cromos { get; set; }
    }
}
