using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base (context) {}

    public async Task<PagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParams)
    {
        var categorias = await GetAllAsync();

        var categoriasObtidas = categorias.OrderBy(c => c.CategoriaId).AsQueryable();

        var categoriasOrdenados = PagedList<Categoria>.ToPagedList(categoriasObtidas, categoriasParams.PageNumber, categoriasParams.PageSize);
        return categoriasOrdenados;
    }

    public async Task<PagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriasParams)
    {
        var categorias = await GetAllAsync();

        if (!string.IsNullOrEmpty(categoriasParams.Nome))
        {
            categorias = categorias.Where(c => c.Nome.IndexOf(categoriasParams.Nome, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        var categoriasFiltradas = PagedList<Categoria>.ToPagedList(
            categorias.AsQueryable(), 
            categoriasParams.PageNumber, 
            categoriasParams.PageSize
            );

        return categoriasFiltradas;
    }
}  
