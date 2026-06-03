using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CromosMundial.Models
{
    public class Equipo
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }

        [DisplayName("Director Técnico")]
        public required string DirectorTecnico { get; set; }

        [DisplayName("Año de Fundación")]
        public int AnioFundacion { get; set; }

        public string? Logo { get; set; }

        [DisplayName("Grupo Mundialista")]
        public required string GrupoMundialista { get; set; }

        // esto NO se guarda en la BD, es solo para recibir el archivo del form
        [NotMapped]
        [DisplayName("Logo")]
        public IFormFile? LogoArchivo { get; set; }

        // un equipo pertenece a un pais
        public int PaisId { get; set; }
        public Pais? Pais { get; set; }

        public ICollection<Jugador>? Jugadores { get; set; }
    }
}
