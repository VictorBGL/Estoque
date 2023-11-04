namespace Estoque.Api.Core.Models
{
    public class DashboardFilterModel
    {
        public DashboardFilterModel()
        {
        }

        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public Guid? VendedorId { get; set; }
    }
}
