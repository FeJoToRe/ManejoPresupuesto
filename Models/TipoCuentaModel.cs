using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Models
{
    public class TipoCuentaModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "No se puede dejar el campo vacio")]
        public string Nombre { get; set; }
        public int UsuarioId { get; set; }
        public int Orden { get; set; }
    }
}
