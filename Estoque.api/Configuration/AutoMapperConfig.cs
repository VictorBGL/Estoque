using AutoMapper;
using Estoque.Api.Core.Models;
using Estoque.Core.Entities;
using TimeZoneConverter;

namespace Estoque.Api.Configuration
{
    public class GlobalMapperConfig : Profile
    {
        public GlobalMapperConfig()
        {
            // timezone
            CreateMap<DateTime, DateTime>().ConvertUsing<UtcConverter>();

            CreateMap<DateTime, string>().ConvertUsing(dt => dt.ToString("dd/MM/yyyy"));
        }
    }

    public class ControleMapperConfig : Profile
    {
        public ControleMapperConfig()
        {
            CreateMap<ProdutoModel, Produto>();
            CreateMap<Produto, ProdutoResponseModel>();

            //CreateMap<PedidoVendaModel, PedidoVenda>();
            CreateMap<PedidoVenda, PedidoVendaResponseModel>();
        }
    }

    public class UtcConverter : AutoMapper.ITypeConverter<DateTime, DateTime>
    {
        public DateTime Convert(DateTime source, DateTime destination, ResolutionContext context)
        {
            try
            {
                if (source.Kind == DateTimeKind.Unspecified)
                {
                    return TimeZoneInfo.ConvertTimeToUtc(source, GetTimeZone()).ToUniversalTime();
                }
                else if (source.Kind == DateTimeKind.Utc)
                {
                    return source.Date.AddHours(3);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }

            return source;
        }

        private static TimeZoneInfo TimeZoneInfo = null;

        public static TimeZoneInfo GetTimeZone()
        {
            if (TimeZoneInfo == null)
                TimeZoneInfo = TZConvert.GetTimeZoneInfo(Environment.GetEnvironmentVariable("WEBSITE_TIME_ZONE") ?? "E. South America Standard Time");

            return TimeZoneInfo;
        }
    }
}
