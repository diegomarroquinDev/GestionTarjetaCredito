using AutoMapper;
using GestionTarjetaCredito.Application.DTOs;
using GestionTarjetaCredito.Domain.Entities;

namespace GestionTarjetaCredito.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FinancialConfiguration, FinancialConfigurationDto>();

            CreateMap<Transaction, TransactionDto>()
                .ForMember(
                    destination => destination.TransactionType,
                    option => option.MapFrom(
                        source => source.TransactionType.ToString()));
        }
    }
}