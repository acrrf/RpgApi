using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RpgApi.Models;
using RpgApi.Data;

namespace RpdApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DisputasController : ControllerBase
    {
        private readonly DataContext _context;
        public DisputasController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("Arma")]
        public async Task<IActionResult> AtaqueComArmaAsync(Disputa d)
        {
            try
            {

                Personagem? atacante = await _context.Personagens
                    .Include(p => p.Arma)
                    .FirstOrDefaultAsync(p => p.Id == d.AtacanteId);
                Personagem? oponente = await _context.Personagens
                    .FirstOrDefaultAsync(p => p.Id == d.OponenteId);

                int dano = atacante.Arma.Dano + (new Random().Next(atacante.Forca));

                dano = dano - new Random().Next(oponente.Defesa);

                if (dano > 0)
                    oponente.PontosVida = oponente.PontosVida - (int)dano;
                if (oponente.PontosVida <= 0)
                    d.Narracao = $"{oponente.Nome} Foi derrotado!";

                _context.Personagens.Update(oponente);
                await _context.SaveChangesAsync();

                StringBuilder dados = new StringBuilder();
                dados.AppendFormat("Atacante: {0}. ", atacante.Nome);
                dados.AppendFormat("Oponente: {0}. ", oponente.Nome);
                dados.AppendFormat("Pontos de vida do atacante: {0}. ", atacante.PontosVida);
                dados.AppendFormat("Pontos de vida do oponente: {0}. ", oponente.PontosVida);
                dados.AppendFormat("Arma Utilizada: {0}. ", atacante.Arma.Nome);
                dados.AppendFormat("Dano: {0}. ", dano);

                d.Narracao += dados.ToString();
                d.DataDisputa = DateTime.Now;
                _context.Disputas.Add(d);
                _context.SaveChanges();

                return Ok(d);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Habilidade")]
        public async Task<IActionResult> AtaqueComHabilidadeAsync(Disputa d)
        {
            try
            {
                Personagem? atacante = await _context.Personagens
                .Include(p=> p.PersonagemHabilidades)
                .ThenInclude(ph=> ph.Habilidade)
                .FirstOrDefaultAsync(p=> p.Id == d.AtacanteId);

                Personagem? oponente = await _context.Personagens
                .FirstOrDefaultAsync(p=> p.Id == d.OponenteId);

                PersonagemHabilidade? ph = await _context.PersonagemHabilidades
                .Include(p=> p.Habilidade)
                .FirstOrDefaultAsync(phBusca => phBusca.HabilidadeId == d.HabilidadeId && phBusca.PersonagemId == d.AtacanteId);

                if(ph == null)
                {
                    d.Narracao = $"{atacante.Nome} n??o possui esta habilidade.";
                }
                else
                {
                    int dano = ph.Habilidade.Dano + (new Random().Next(atacante.Inteligencia));
                    dano = dano - new Random().Next(oponente.Defesa);

                    if(dano > 0)
                    {
                        oponente.PontosVida = oponente.PontosVida - dano;
                    }
                    if(oponente.PontosVida <=0)
                    {
                        d.Narracao += $"{oponente.Nome} foi derrotado!!";
                    }

                    _context.Personagens.Update(oponente);
                    await _context.SaveChangesAsync();

                    StringBuilder dados = new StringBuilder();
                    dados.AppendFormat("Atacante: {0}. ", atacante.Nome);
                    dados.AppendFormat("Oponente: {0}. ", oponente.Nome);
                    dados.AppendFormat("Pontos de Vida do atacante: {0}. ", atacante.PontosVida);
                    dados.AppendFormat("Pontos de Vida do oponente: {0}. ", oponente.PontosVida);
                    dados.AppendFormat("Habilidade Utilizada: {0}. ", ph.Habilidade.Nome);
                    dados.AppendFormat("Dano: {0}. ", dano);

                    d.Narracao += dados.ToString();
                    d.DataDisputa = DateTime.Now;
                    _context.Disputas.Add(d);
                    _context.SaveChanges();

                }

                return Ok(d);
            }
            catch (System.Exception ex)
            {
                
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("DisputaEmGrupo")]
        public async Task<IActionResult> DisputaEmgrupoAsync(Disputa d)
        {
            try
            {
                d.Resultados = new List<string>();//Instancia a lista de resultados

                //Busca na base dos personagens informados no parametro incluindo Armas e Habilidades.
                List<Personagem> personagens = await _context.Personagens
                .Include(p => p.Arma)
                .Include(p => p.PersonagemHabilidades)
                .ThenInclude(ph => ph.Habilidade)
                .Where(personagens => d.ListaIdPersonagens.Contains(personagens.Id))
                .ToListAsync();
                
                //Contagem de personagens vivos na lista obtida do banco de dados.
                int qtdPersonagensVivos = personagens.FindAll(p => p.PontosVida > 0).Count;

                //Enquanto houver mais de um personagem vivo haver?? disputa.
                while (qtdPersonagensVivos > 1)
                {
                    //Seleciona personagens com pontos de vida positivos e depois faz sorteio.
                    List<Personagem> atacantes = personagens.Where(p => p.PontosVida > 0).ToList();
                    Personagem atacante = atacantes[new Random().Next(atacantes.Count)];
                    d.AtacanteId = atacante.Id;

                    //Seleciona personagens com pontos positivos, exceto o atacante escolhido e depois faz sorteio.
                    List<Personagem> oponentes = personagens.Where(p => p.Id != atacante.Id && p.PontosVida > 0).ToList();
                    Personagem oponente = oponentes[new Random().Next(oponentes.Count)];
                    d.OponenteId = oponente.Id;

                    //declara e redefine a cada passagem do while o valor das vari??veis que ser??o usadas.
                    int dano = 0;
                    string ataqueUsado = string.Empty;
                    string resultado = string.Empty;

                    //Sorteia entre 0 e 1: 0 ?? umataque com arma e 1 ?? um ataque com habilidades.
                    bool ataqueUsaArma = (new Random().Next(1) == 0);

                    if (ataqueUsaArma && atacante.Arma != null)
                    {
                        //Programa????o do ataque com arma caso o atacante possua arma (o != null) do if.

                        //Sorteio da for??a.
                        dano = atacante.Arma.Dano + (new Random().Next(atacante.Forca));
                        dano = dano - new Random().Next(oponente.Defesa);//Sorteio da defesa.
                        ataqueUsado = atacante.Arma.Nome;

                        if (dano > 0) oponente.PontosVida = oponente.PontosVida - (int)dano;

                        //Formata a mensagem.
                        resultado = string.Format("{0} atacou {1} usando {2} com o dano {3}.", atacante.Nome, oponente.Nome, ataqueUsado, dano);
                        d.Narracao += resultado;//Concatena o resultado com as narr????es existentes.
                        d.Resultados.Add(resultado);//Adiciona o resultado atual na lista de resultados.
                    }
                    else if (atacante.PersonagemHabilidades.Count != 0)//Verifica se o personagem tem habolidades
                    {
                        //Programa????o do ataque com Habilidade.

                        //Realiza o sorteio entre as habilidades existentes e na linha seguinte a seleciona.
                        int sorteioHabilidadeId = new Random().Next(atacante.PersonagemHabilidades.Count);
                        Habilidade habilidadeEscolhida = atacante.PersonagemHabilidades[sorteioHabilidadeId].Habilidade;
                        ataqueUsado = habilidadeEscolhida.Nome;

                        //Sorteio da inteligencia somada ao dano.
                        dano = habilidadeEscolhida.Dano + (new Random().Next(atacante.Inteligencia));
                        dano = dano - new Random().Next(oponente.Defesa);//sorteio da defesa.

                        if(dano > 0)oponente.PontosVida = oponente.PontosVida - (int)dano;

                        resultado = string.Format("{0} atacou {1} usado {2} com o dano {3}.", atacante.Nome, oponente.Nome, ataqueUsado, dano);
                        d.Narracao += resultado;
                        d.Resultados.Add(resultado);
                    }
                    //Aten????o: Aqui ficar?? a programa????o da verifica????o do ataque usado e verifica????o se existe mais de um personagem vivo.
                    if (!string.IsNullOrEmpty(ataqueUsado))
                    {
                        //Incrementa os dados dos combates.
                        atacante.Vitorias++;
                        oponente.Derrotas++;
                        atacante.Disputas++;
                        oponente.Disputas++;

                        d.Id = 0;//Zera o Id para poder salvar os dados de disputa sem erro de chave.
                        d.DataDisputa = DateTime.Now;
                        _context.Disputas.Add(d);
                        await _context.SaveChangesAsync();
                    }

                    qtdPersonagensVivos = personagens.FindAll(p => p.PontosVida > 0).Count;

                    if (qtdPersonagensVivos == 1)//Havendo s?? um personagem vivo, existe um CAMPE??O!
                    {
                        string resultadoFinal = $"{atacante.Nome.ToUpper()} ?? CAMPE??O com {atacante.PontosVida} pontos de vida restantes!";

                        d.Narracao += resultadoFinal;//Concatena o resultado finsl com as demais narra????es.
                        d.Resultados.Add(resultadoFinal);//Concatena o resultado final comos demais resultados.

                        break;//break vai para o while.
                    }

                }//Fim do While.
                //C??digo ap??s o fechamento do while. Atualizar?? os pontos de vida,
                //disputas, vit??rias e derrotas de todos personagens ao final das batalhas
                _context.Personagens.UpdateRange(personagens);
                await _context.SaveChangesAsync();

                return Ok(d);//retorna os dados de disputas
            }
            catch(System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("ApagarDisputas")]
        public async Task<IActionResult> DeleteAsync()
        {
            try
            {
                List<Disputa> disputas = await _context.Disputas.ToListAsync();

                _context.Disputas.RemoveRange(disputas);
                await _context.SaveChangesAsync();

                return Ok("Disputas apagadas");
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
                
        }
        [HttpGet("Listar")]
        public async Task<IActionResult> ListarAsync()
        {
            try
            {
                List<Disputa> disputas = await _context.Disputas.ToListAsync();

                return Ok (disputas);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





    }
} 