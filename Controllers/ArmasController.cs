using Microsoft.AspNetCore.Mvc;
using RpgApi.Data;
using RpgApi.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace RpgApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ArmasController:ControllerBase
    {
        private readonly DataContext _dataContext;

        public ArmasController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingle(int id)
        {
            try
            {
                Arma? p = await _dataContext.Armas
                .FirstOrDefaultAsync(pBusca => pBusca.Id == id);
                return Ok(p);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<Arma> lista = await _dataContext.Armas.ToListAsync();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Arma novoArma)
        {
            try
            {
                if (novoArma.Dano == 0)
                {
                    throw new System.Exception("O Dano da Arma não pode ser ZERO");
                }
                else if (novoArma.Dano > 100)                    
                {
                    throw new Exception("Muito forte");
                }
                Personagem? p = await _dataContext.Personagens.FirstOrDefaultAsync(p=> p.Id == novoArma.PersonagemId);

                if(p == null){
                    throw new System.Exception("Não existe personagem com o Id informado.");
                }

                await _dataContext.Armas.AddAsync(novoArma);
                await _dataContext.SaveChangesAsync();
                return Ok(novoArma.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(Arma novoArma)
        {
            try
            {
                if (novoArma.Dano == 0)
                {
                    throw new System.Exception("O Dano da Arma não pode ser ZERO");
                }
                else if (novoArma.Dano > 100)                    
                {
                    throw new Exception("Muito forte");
                }
                Personagem? p = await _dataContext.Personagens.FirstOrDefaultAsync(p=> p.Id == novoArma.PersonagemId);

                if(p == null){
                    throw new System.Exception("Não existe personagem com o Id informado.");
                }

                _dataContext.Armas.Update(novoArma);
                int linhasAfetadas = await _dataContext.SaveChangesAsync();
                return Ok(linhasAfetadas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Arma? pRemover = await _dataContext.Armas
                .FirstOrDefaultAsync(p => p.Id == id);

                _dataContext.Armas.Remove(pRemover);
                int linhasAfetadas = await _dataContext.SaveChangesAsync();

                return Ok(linhasAfetadas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}