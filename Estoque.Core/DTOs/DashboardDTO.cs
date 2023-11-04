using System;
namespace Estoque.Core.DTOs
{
	public class DashboardDTO
	{
        public DashboardDTO(decimal quantidadeProdutosVendidos, decimal quantidadeVendas, decimal faturamento, decimal quantidadeCompras, decimal quantidadeProdutosComprados, decimal valorGasto)
        {
            QuantidadeProdutosVendidos = quantidadeProdutosVendidos;
            QuantidadeVendas = quantidadeVendas;
            FaturamentoBruto = faturamento;
            FaturamentoLiquido = faturamento - valorGasto;
            TicketMedio = faturamento == 0 ? 0 : faturamento / quantidadeVendas;

            QuantidadeProdutosComprados = quantidadeProdutosComprados;
            QuantidadeCompras = quantidadeCompras;
            ValorGasto = valorGasto;
        }

        public decimal QuantidadeVendas { get; set; }
        public decimal QuantidadeProdutosVendidos { get; set; }
        public decimal FaturamentoBruto { get; set; }
        public decimal FaturamentoLiquido { get; set; }
        public decimal TicketMedio { get; set; }

        public decimal QuantidadeProdutosComprados { get; set; }
        public decimal QuantidadeCompras { get; set; }
        public decimal ValorGasto { get; set; }
    }
}

