﻿using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext context) : base(context) {}

    public IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParams)
    {
        return GetAll()
            .OrderBy(p => p.Nome)
            .Skip((produtosParams.PageNumber -1) * produtosParams.PageSize)
            .Take(produtosParams.PageSize).ToList();
    }

    IEnumerable<Produto> IProdutoRepository.GetProdutosPorCategoria(int id)
    {
        return GetAll().Where(c => c.CategoriaId == id);
    }
}
