using Domain;
using Domain.Interfaces.Helper;
using Domain.Interfaces.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CardsController : ControllerBase
    {
        private readonly IRepositoryGeneral _ire;
        private readonly IFileProcesor _fil;

        public CardsController(IRepositoryGeneral ire, IFileProcesor fil)
        {
            _ire = ire;
            _fil = fil;
        }
        /// <summary>
        /// registrar tarjeta en la base de datos
        /// </summary>
        /// <param name="cardViewModels"></param>
        /// <returns></returns>
        [HttpPost("guardar-tarjeta")]
        public async Task<IActionResult> CrearTarjeta(CardViewModels cardViewModels)
        {
            try
            {
                Card card = new Card(cardViewModels.Name, cardViewModels.Pan);
                card.Amount = cardViewModels.Amount;
                await _ire.Add(card);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("No se ha podido guardar la tarjeta");
            }
        }

        /// <summary>
        /// dejar el monto de la tarjeta en 0
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("quitar-todo-saldo/{id}")]
        public async  Task<IActionResult> QuitarTodoSaldo(string id)
        {
            try
            {
                Card card = await _ire.GetFirst<Card>(z => z.Pan == id);
                if (card != null)
                {
                    card.Amount = 0;
                    await _ire.Update(card, card.Id);
                    return Ok();
                }
                else
                {
                    return BadRequest("No se ha encontrado la tarjeta");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("No se ha podido quitar el saldo de la tarjeta");
            }
        }

        /// <summary>
        /// restar parte del saldo de la tarjeta
        /// </summary>
        /// <param name="id"></param>
        /// <param name="monto"></param>
        /// <returns></returns>
        [HttpPut("quitar-parte-saldo/{id}")]
        public async Task<IActionResult> QuitarParteSaldo(string id, decimal monto)
        {
            try
            {
                Card card = await _ire.GetFirst<Card>(z => z.Pan == id);
                if (card != null)
                {
                    card.Amount = card.Amount - monto;
                    await _ire.Update(card, card.Id);
                    return Ok();
                }
                else
                {
                    return BadRequest("No se ha encontrado la tarjeta");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("No se ha podido quitar el saldo de la tarjeta");
            }
        }

        /// <summary>
        /// buscar la tarjeta por el guid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("buscar-tarjeta/{id}")]
        public async Task<IActionResult> BuscarTarjeta(string id)
        {
            try
            {
                Card card = await _ire.GetFirst<Card>(z =>z.Id.ToString()==id);
                
                if (card != null)
                {
                    CardViewModels cardViewModels = new CardViewModels { Name = card.Name, Pan = card.Pan, Amount = card.Amount };
                    return Ok(cardViewModels);
                }
                else
                {
                    return BadRequest("No se ha encontrado la tarjeta");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Problemas en sistema al buscar la tarjeta");
            }
        }

        /// <summary>
        /// buscar todas las tarjetas por el nombre de la persona
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        [HttpGet("lista-tarjetas")]
        public async Task<IActionResult> ListaTarjetas([FromQuery] string nombre)
        {
            try
            {
                List<Card> cards = await _ire.GetList<Card>(z => z.Name == nombre);
                if (cards != null)
                {
                    List<CardViewModels> cardViewModels = cards.Select(z => new CardViewModels { Name = z.Name, Pan = z.Pan, Amount = z.Amount }).ToList();
                    return Ok(cardViewModels);
                }
                else
                {
                    return BadRequest("No se ha encontrado la tarjeta");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Problemas en sistema al buscar la tarjeta");
            }
        }


        /// <summary>
        /// actualizar el nombre a quien pertenece la tarjeta
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nombre"></param>
        /// <returns></returns>
        [HttpPut("actualizar-nombre-persona/{id}")]
        public async Task<IActionResult> ActualizarNombreTarjeta(string id, string nombre)
        {
            try
            {
                Card card = await _ire.GetFirst<Card>(z => z.Pan == id);

                if (card != null)
                {
                    card.Name = nombre;
                    await _ire.Update(card, card.Id);
                    return Ok();
                }
                else
                {
                    return BadRequest("No se ha encontrado la tarjeta");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("No se ha podido actualizar el pin de la tarjeta");
            }
        }

        [HttpPut("actualizar-pin-persona/{id}")]
        public async Task<IActionResult> ActualizarPinTarjeta(string id, int pin)
        {
            try
            {
                Card card = await _ire.GetFirst<Card>(z => z.Pan == id);

                if (card != null)
                {
                    //codificamos el pin para guardarlo en la base de datos
                    string pinCodificado = _fil.EncryptString(pin.ToString());
                    card.Pin = pinCodificado;
                    await _ire.Update(card, card.Id);
                    return Ok();
                }
                else
                {
                    return BadRequest("No se ha encontrado la tarjeta");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("No se ha podido actualizar el pin de la tarjeta");
            }
        }

        /// <summary>
        /// actualizar el estado de vigencia de la tarjeta
        /// </summary>
        /// <param name="id"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPut("establecer-vigencia-tarjeta/{id}")]
        public async Task<IActionResult> ActualizarVigenciaTarjeta(string id, bool estado)
        {
            try
            {
                Card card = await _ire.GetFirst<Card>(z => z.Pan == id);

                if (card != null)
                {
                    card.Vigente = estado;
                    await _ire.Update(card, card.Id);
                    return Ok();
                }
                else
                {
                    return BadRequest("No se ha encontrado la tarjeta");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("No se ha podido el estado de vigencia de la tarjeta");
            }
        }
    }
}
