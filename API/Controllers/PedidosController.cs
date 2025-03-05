using Shared.Dtos.Request;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/pedidos")]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        /// <summary>
        /// Recebe um novo pedido.
        /// </summary>
        /// <param name="pedido">DTO contendo os dados do pedido.</param>
        /// <returns>
        /// Retorna 201 (Created) se o pedido for criado com sucesso.
        /// Retorna 400 (Bad Request) se houver erros.
        /// </returns>
        [HttpPost("receberPedido")]
        public async Task<IActionResult> ReceberPedido([FromBody] PedidoRequestDto pedido)
        {
            var response = await _pedidoService.CriarPedidoAsync(pedido);

            if (response.Id == 0)
            {
                // Retorna BadRequest com a mensagem apropriada
                return BadRequest(new { mensagem = response.Mensagem });
            }

            // Retorna Created com os dados do pedido criado
            return CreatedAtAction(nameof(ConsultarPedido), new { id = response.Id }, response);
        }

        /// <summary>
        /// Consulta um pedido pelo ID.
        /// </summary>
        /// <param name="id">ID do pedido a ser consultado.</param>
        /// <returns>
        /// Retorna 200 (OK) com o pedido se houver resultado.
        /// Retorna 404 (Not Found) caso nenhum pedido seja encontrado.
        /// </returns>
        [HttpGet("consultarPedido/{id}")]
        public async Task<IActionResult> ConsultarPedido(int id)
        {
            var pedido = await _pedidoService.ConsultarPedidoAsync(id);

            if (pedido == null)
            {
                return NotFound();
            }

            return Ok(pedido);
        }

        /// <summary>
        /// Lista pedidos filtrando por status.
        /// </summary>
        /// <param name="status">Status dos pedidos a serem listados.</param>
        /// <returns>
        /// Retorna 200 (OK) com a lista de pedidos se houver resultados.
        /// Retorna 404 (Not Found) caso nenhum pedido corresponda ao status fornecido.
        /// </returns>
        [HttpGet("listarPedidosPorStatus")]
        public async Task<IActionResult> ListarPedidos([FromQuery] string status)
        {
            var pedidos = await _pedidoService.ListarPedidosPorStatusAsync(status);

            if (!pedidos.Any())
            {
                return NotFound();
            }

            return Ok(pedidos);
        }
    }
}