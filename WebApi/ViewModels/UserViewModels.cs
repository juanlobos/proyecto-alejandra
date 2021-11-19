using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels
{
    public class UserViewModels
    {
        [Required(ErrorMessage = "Debe ingresar el nombre de usuario")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Debe ingresar la clave de usuario")]
        public string Clave { get; set; }
    }
}
