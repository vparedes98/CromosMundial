using System.ComponentModel;
using CromosMundial.Models.Validations;

namespace CromosMundial.Models
{
    public class Album
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }

        [NoFuture] //Validación personalizada: el año no puede ser futuro
        public int Anio { get; set; }

        [DisplayName("Cantidad de Cromos")]
        public int CantidadCromos { get; set; }

        [DisplayName("Edición Especial")]
        public bool EdicionEspecial { get; set; }

        //Un álbum tiene muchos cromos
        public ICollection<Cromo>? Cromos { get; set; }
    }
}
