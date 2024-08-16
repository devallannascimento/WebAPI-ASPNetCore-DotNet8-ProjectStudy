using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;

        public ProdutosController(IUnitOfWork uof)
        {
            _uof = uof;
        }

        [HttpGet("produtos/{id}")]
        public ActionResult<IEnumerable<Produto>> GetProdutosCategoria(int id)
        {
            var produtos = _uof.ProdutoRepository.GetProdutosPorCategoria(id);

            if (produtos == null)
            {
                return NotFound();
            }

            return Ok(produtos);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _uof.ProdutoRepository.GetAll();
            return Ok(produtos);
        }

        [HttpGet("{id:int:min(1)}", Name = "obterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

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

            var novoProduto = _uof.ProdutoRepository.Create(produto);
            _uof.Commit();

            return new CreatedAtRouteResult("obterProduto", new { id = novoProduto.ProdutoId }, novoProduto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest("Dados inválidos");
            }

            _uof.ProdutoRepository.Update(produto);
            _uof.Commit();
            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

            if (produto == null)
            {
                return NotFound($"Produto com id = {id} não encontrado");
            }

            var produtoExcluido = _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();
            return Ok(produtoExcluido);
        }
    }
}