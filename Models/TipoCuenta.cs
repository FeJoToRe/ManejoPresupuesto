﻿using ManejoPresupuesto.Controllers;
using ManejoPresupuesto.Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Models
{
    public class TipoCuenta //IValidatableObject
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "No se puede dejar el campo vacio")]
        [Display(Name = "Nombre del tipo de cuenta")]
        [FirstLetterCaps]
        [Remote(action: "VerificarExisteTipoCuenta", controller: "TiposCuentas")]
        public string Nombre { get; set; }
        public int UsuarioId { get; set; }
        public int Orden { get; set; }
        public decimal Balance { get; set; }

        /*public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Nombre != null && Nombre.Length > 0)
            {
                var firstLetter = Nombre[0].ToString();

                if (firstLetter != firstLetter.ToUpper())

                {
                    yield return new ValidationResult("La primera letra debe ser mayuscula",
                        new[] { nameof(Nombre)});
                }
            }
        }*/
    }
}
