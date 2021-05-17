using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProAgil.Repository;
using ProAgil.Domain;
using AutoMapper;
using ProAgil.WebApi.Dtos;
using System.IO;
using System.Net.Http.Headers;

namespace ProAgil.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly IProAgilRepository _repository;
        private readonly IMapper _mapper;

        public EventoController(IProAgilRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await _repository.GetAllEventoAsync(true);
                var results = _mapper.Map<IEnumerable<EventoDto>>(eventos);
                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de dados de falhou {ex.Message}");
            }
        }

        [HttpGet("{EventoId}")]
        public async Task<IActionResult> Get(int EventoId)
        {

            try
            {
                var evento = await _repository.GetEventoAsyncById(EventoId, true);
                var result = _mapper.Map<EventoDto>(evento);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de dados de falhou {ex.Message}");
            }
        }

        [HttpGet("getByTema/{Tema}")]
        public async Task<IActionResult> Get(string Tema)
        {

            try
            {
                var results = await _repository.GetAllEventoAsyncByTema(Tema, true);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no Banco de dados!");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);
                _repository.Add(evento);
                if (await _repository.SaveChangesAsync())
                {
                    return Created($"/api/evento/{model.EventoId}", evento);
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no Banco de dados!");
            }
            return BadRequest();
        }

       [HttpPost("upload")]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                    var fullPath = Path.Combine(pathToSave, filename.Replace("\"", " ").Trim());

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

                return Ok();
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar realizar uploam {ex.Message}");
            }
        }

        [HttpPut("{EventoId}")]
        public async Task<IActionResult> Put(int EventoId, EventoDto model)
        {
            try
            {
                var evento = await _repository.GetEventoAsyncById(EventoId, false);

                if (evento == null)
                    return NotFound();

                _mapper.Map(model, evento);
                _repository.Update(evento);

                if (await _repository.SaveChangesAsync())
                    return Created($"/api/evento/{model.EventoId}", model);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha no Banco de dados! {ex}");
            }
            return BadRequest();
        }

        [HttpDelete("{EventoId}")]
        public async Task<IActionResult> Delete(int EventoId)
        {
            try
            {
                var evento = await _repository.GetEventoAsyncById(EventoId, false);
                if (evento == null) return NotFound();

                _repository.Delete(evento);

                if (await _repository.SaveChangesAsync()) return Ok();
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            return BadRequest();
        }
    }
}
