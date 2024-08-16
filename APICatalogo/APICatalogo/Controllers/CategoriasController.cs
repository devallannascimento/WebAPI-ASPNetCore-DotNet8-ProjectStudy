using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;

        public CategoriasController(IUnitOfWork uof)
        {
            _uof = uof;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            var categorias = _uof.CategoriaRepository.GetAll();
            return Ok(categorias);
        }

        [HttpGet("{id:int}", Name = "obterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {

            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

            if (categoria == null)
            {
                return NotFound($"Categoria com id = {id} não encontrada");
            }

            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {

            if (categoria == null)
            {
                return BadRequest("Dados inválidos");
            }

            var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaCriada.CategoriaId }, categoriaCriada);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest("Dados inválidos");
            }

            _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();
            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

            if (categoria == null)
            {
                return NotFound($"Categoria com id ={id} não encontrada");
            }

            var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();
            return Ok(categoriaExcluida);
        }
    }
}