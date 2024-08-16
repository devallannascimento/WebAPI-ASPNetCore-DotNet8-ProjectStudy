using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Models;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public CategoriasController(IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
            var categorias = _uof.CategoriaRepository.GetAll();

            if ( categorias == null)
            {
                return NotFound("Não existem categorias");
            }

            var categoriasDto = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);

            return Ok(categoriasDto);
        }

        [HttpGet("{id:int}", Name = "obterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id)
        {

            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

            if (categoria == null)
            {
                return NotFound($"Categoria com id = {id} não encontrada");
            }

            var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

            return Ok(categoriaDto);
        }

        [HttpPost]
        public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDto)
        {

            if (categoriaDto == null)
            {
                return BadRequest("Dados inválidos");
            }

            var categoria = categoriaDto.ToCategoria();

            var novaCategoria = _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();

            var novaCategoriaDto = _mapper.Map<CategoriaDTO>(novaCategoria);

            return new CreatedAtRouteResult("ObterCategoria", new { id = novaCategoriaDto.CategoriaId }, novaCategoriaDto);
        }

        [HttpPut("{id:int}")]
        public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.CategoriaId)
            {
                return BadRequest("Dados inválidos");
            }

            var categoria = categoriaDto.ToCategoria();

            var categoriaAtualizada = _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();

            var categoriaAtualizadaDto = _mapper.Map<CategoriaDTO>(categoriaAtualizada);

            return Ok(categoriaAtualizadaDto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

            if (categoria == null)
            {
                return NotFound($"Categoria com id = {id} não encontrada");
            }

            var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();

            var categoriaExcluidaDto = _mapper.Map<CategoriaDTO>(categoriaExcluida);

            return Ok(categoriaExcluidaDto);
        }
    }
}