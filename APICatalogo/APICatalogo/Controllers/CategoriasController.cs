using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
        {
            var categorias = await _uof.CategoriaRepository.GetAllAsync();

            if ( categorias == null)
            {
                return NotFound("Não existem categorias");
            }

            var categoriasDto = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);

            return Ok(categoriasDto);
        }

        [HttpGet("{id:int}", Name = "obterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {

            var categoria = await _uof.CategoriaRepository.GetAsync(c => c.CategoriaId == id);

            if (categoria == null)
            {
                return NotFound($"Categoria com id = {id} não encontrada");
            }

            var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

            return Ok(categoriaDto);
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get(
            [FromQuery] CategoriasParameters categoriasParameters
            )
        {
            var categorias = await _uof.CategoriaRepository.GetCategoriasAsync(categoriasParameters);

            if (categorias == null)
            {
                return NotFound();
            }

            var metadada = new
            {
                categorias.TotalCount,
                categorias.PageSize,
                categorias.CurrentPage,
                categorias.TotalPages,
                categorias.HasNext,
                categorias.HasPrevius
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadada));

            var categoriasDto = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);

            return Ok(categoriasDto);
        }

        [HttpGet("filter/preco/pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasFiltradas(
            [FromQuery] CategoriasFiltroNome categoriasFiltro
            )
        {
            var categoriasFiltradas = await _uof.CategoriaRepository.GetCategoriasFiltroNomeAsync(categoriasFiltro);

            return ObterCategorias(categoriasFiltradas);
        }

        private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(PagedList<Categoria> categorias)
        {
            var metadada = new
            {
                categorias.TotalCount,
                categorias.PageSize,
                categorias.CurrentPage,
                categorias.TotalPages,
                categorias.HasNext,
                categorias.HasPrevius
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadada));

            var categoriasDto = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);

            return Ok(categoriasDto);
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> Post(CategoriaDTO categoriaDto)
        {

            if (categoriaDto == null)
            {
                return BadRequest("Dados inválidos");
            }

            var categoria = _mapper.Map<Categoria>(categoriaDto);

            var novaCategoria = _uof.CategoriaRepository.Create(categoria);
            await _uof.CommitAsync();

            var novaCategoriaDto = _mapper.Map<CategoriaDTO>(novaCategoria);

            return new CreatedAtRouteResult("ObterCategoria", new { id = novaCategoriaDto.CategoriaId }, novaCategoriaDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Put(int id, CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.CategoriaId)
            {
                return BadRequest("Dados inválidos");
            }

            var categoria = _mapper.Map<Categoria>(categoriaDto);

            var categoriaAtualizada = _uof.CategoriaRepository.Update(categoria);
            await _uof.CommitAsync();

            var categoriaAtualizadaDto = _mapper.Map<CategoriaDTO>(categoriaAtualizada);

            return Ok(categoriaAtualizadaDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            var categoria = await _uof.CategoriaRepository.GetAsync(c => c.CategoriaId == id);

            if (categoria == null)
            {
                return NotFound($"Categoria com id = {id} não encontrada");
            }

            var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
            await _uof.CommitAsync();

            var categoriaExcluidaDto = _mapper.Map<CategoriaDTO>(categoriaExcluida);

            return Ok(categoriaExcluidaDto);
        }
    }
}