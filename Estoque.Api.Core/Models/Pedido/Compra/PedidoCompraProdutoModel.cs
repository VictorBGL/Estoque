﻿namespace Estoque.Api.Core.Models
{
    public class PedidoCompraProdutoModel
    {
        public Guid? Id { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ValorPorQuantidade { get; set; }
        public decimal Valor { get; set; }
        public Guid ProdutoId { get; set; }
    }
}
