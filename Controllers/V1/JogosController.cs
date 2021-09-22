using ApiCatalogoJogos.Exceptions;
using ApiCatalogoJogos.InputModel;
using ApiCatalogoJogos.Services;
using ApiCatalogoJogos.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Controllers.V1
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class JogosController : ControllerBase
    {
        private readonly IJogoService _jogoService;
        public JogosController(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }
        

        /// <summary>
        /// Busque todos os jogos de forma paginada.
        /// </summary>
        /// <remarks>
        /// É necessário informar a paginação
        /// </remarks>
        /// <param name="pagina">Indique qual página está sendo consultada. Mínimo 1</param>
        /// <param name="quantidade">Indique a quantidade de registros por página. Min.: 1 | Max.: 50</param>
        /// <response code="200">Retorna a lista de jogos</response>
        /// <response code="204">Não há jogos inseridos</response>
        
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JogoViewModel>>> Obter([FromQuery, Range(1, int.MaxValue)] int pagina = 1, [FromQuery, Range(1, 50)] int quantidade = 5)
        {
            //throw new Exception(); TESTANDO O MIDDLEWARE

            var jogos = await _jogoService.Obter(pagina, quantidade);

            if (jogos.Count() == 0)
                return NoContent();

            return Ok(jogos);

        }
        /// <summary>
        /// Busque um jogo pelo seu ID.
        /// </summary>
        /// <param name="idJogo">Indique o ID do jogo requerido</param>
        /// <response code ="200">Retorna o jogo filtrado</response>
       /// <response code ="204">Não há jogo com este ID.</response>
        /// <returns></returns>
        [HttpGet("{idJogo:guid}")]
        public async Task<ActionResult<JogoViewModel>> Obter([FromRoute]Guid idJogo)
        {
            var jogo = await _jogoService.Obter(idJogo);

            if (jogo == null)
                return NoContent();

            return Ok(jogo);

        }
        /// <summary>
        /// Insira um novo jogo.
        /// </summary>
        /// <param name="jogoInputModel">Preencha os dados solicitados</param>
        /// <response code="200">Inserido com sucesso.</response>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<JogoViewModel>> InserirJogo([FromBody] JogoInputModel jogoInputModel)
        {
            try
            {
                var jogo = await _jogoService.Inserir(jogoInputModel);

                return Ok(jogo);
            }
           catch (JogoJaCadastradoException ex)
         
            {
                return UnprocessableEntity("Já existe um jogo com este nome para esta produtora.");
            };
           
        }

      
        /// <summary>
        /// Atualize as informações de um jogo.
        /// </summary>
        /// <param name="idJogo">Insira o ID do jogo.</param>
        /// <response code="200"> Atualizado com sucesso.</response>
        
        /// <returns></returns>
        [HttpPut("{idJogo:guid}")] 
        public async Task<ActionResult> AtualizarJogo([FromRoute]Guid idJogo, JogoInputModel jogoInputModel)
        {
            try
            {
                await _jogoService.Atualizar(idJogo, jogoInputModel);
                return Ok();

            }
            catch(JogoNaoCadastradoException ex)
            
            {
                return NotFound("Não exite este jogo.");
            };
           
        }
        /// <summary>
        /// Atualize o preço de um jogo.
        /// </summary>
        /// <param name="idJogo">Insira o ID do jogo.</param>
        /// <param name="preco">Informe o novo valor.</param>
        /// <returns></returns>
        [HttpPatch("{idJogo:guid}/preco/{preco:double}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute]Guid idJogo, [FromRoute]double preco)
        {
            try
            {
                await _jogoService.Atualizar(idJogo, preco);
                return Ok();
            }
            catch(JogoNaoCadastradoException ex)
            
            {
                return NotFound("Não existe este jogo.");
            }
            
        }
        /// <summary>
        /// Delete um jogo
        /// </summary>
        /// <param name="idJogo">Insira o ID do jogo</param>
        /// <response code="200">Excluído com sucesso.</response>
        /// <returns></returns>
        [HttpDelete("{idJogo:guid}")]
        public async Task<ActionResult> ApagarJogo([FromRoute]Guid idJogo)
        {
            try
            {
                await _jogoService.Remover(idJogo);
                return Ok();
            }
            catch(JogoNaoCadastradoException ex)
            
            {
                return NotFound("Não existe este jogo.");
            }
            
        }

    }
}
