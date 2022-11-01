using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Models
{
    public class TipoCuentaModel
    {
        [Required(ErrorMessage = "No se puede dejar el campo vacio")]
        [StringLength(maximumLength: 100, MinimumLength = 10, ErrorMessage = "El mensaje debe contener entre {2} y {1} caracteres")]
        public int ID { get; set; }
        public string Nombre { get; set; }
        public int UsuarioId { get; set; }
        public int Orden { get; set; }
    }
}
