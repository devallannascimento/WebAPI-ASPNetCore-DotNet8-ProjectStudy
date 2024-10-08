﻿using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base (context) {}

    public async Task<IPagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParams)
    {
        var categorias = await GetAllAsync();

        var categoriasObtidas = categorias.OrderBy(c => c.CategoriaId).AsQueryable();

        var categoriasOrdenados = await categoriasObtidas.ToPagedListAsync(
            categoriasParams.PageNumber, 
            categoriasParams.PageSize);

        return categoriasOrdenados;
    }

    public async Task<IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(
        CategoriasFiltroNome categoriasParams)
    {
        var categorias = await GetAllAsync();

        if (!string.IsNullOrEmpty(categoriasParams.Nome))
        {
            categorias = categorias.Where(c => c.Nome.Contains(categoriasParams.Nome));
        }

        var categoriasFiltradas = await categorias.ToPagedListAsync(
                                             categoriasParams.PageNumber,
                                             categoriasParams.PageSize);

        return categoriasFiltradas;
    }
}  
