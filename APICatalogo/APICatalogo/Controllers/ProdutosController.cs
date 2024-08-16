using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutosController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        [HttpGet("produtos/{id}")]
        public ActionResult<IEnumerable<Produto>> GetProdutosCategoria(int id)
        {
            var produtos = _produtoRepository.GetProdutosPorCategoria(id);

            if (produtos == null)
            {
                return NotFound();
            }

            return Ok(produtos);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _produtoRepository.GetAll();
            return Ok(produtos);
        }

        [HttpGet("{id:int:min(1)}", Name = "obterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _produtoRepository.Get(p => p.ProdutoId == id);

            if (produto == null)
            {
                return NotFound($"Produto com id = {id} não encontrado");
            }

            return Ok(produto);
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto == null)
            {
                return BadRequest("Dados inválidos");
            }

            var novoProduto = _produtoRepository.Create(produto);

            return new CreatedAtRouteResult("obterProduto", new { id = novoProduto.ProdutoId }, novoProduto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest("Dados inválidos");
            }

            _produtoRepository.Update(produto);
            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _produtoRepository.Get(p => p.ProdutoId == id);

            if (produto == null)
            {
                return NotFound($"Produto com id = {id} não encontrado");
            }

            var produtoExcluido = _produtoRepository.Delete(produto);
            return Ok(produtoExcluido);
        }
    }
}