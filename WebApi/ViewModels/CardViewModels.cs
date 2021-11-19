using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels
{
    public class CardViewModels
    {
        [Required(ErrorMessage = "Debe ingresar el nombre de laa tarjeta habiente")]
        public string Name { get; set; }
     
        [Required(ErrorMessage ="Debe ingresar el pan de la tarjeta")]
        public string Pan { get; set; }
        
        public decimal Amount { get; set; }
    }
}
