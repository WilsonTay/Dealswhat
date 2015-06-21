using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;

namespace DealsWhat.Infrastructure.DataAccess
{
    public class EFRepositoryFactory : IRepositoryFactory
    {
        private readonly DealsWhatUnitOfWork dbContext;

        public EFRepositoryFactory(DealsWhatUnitOfWork dbContext)
        {
            this.dbContext = dbContext;

            AutoMapper.Mapper.CreateMap<Models.Deal, DealsWhat.Domain.Model.Deal>()
                .ForMember(dest => dest.ShortTitle, opt => opt.MapFrom(src => src.ShortTitle))
                .ForMember(dest => dest.ShortDescription, opt => opt.MapFrom(src => src.ShortDescription))
                .ForMember(dest => dest.LongTitle, opt => opt.MapFrom(src => src.LongTitle))
                .ForMember(dest => dest.LongDescription, opt => opt.MapFrom(src => src.LongDescription))
                .ForMember(dest => dest.Highlight, opt => opt.MapFrom(src => src.Highlight))
                .ForMember(dest => dest.FinePrint, opt => opt.MapFrom(src => src.FinePrint))
                .ForMember(dest => dest.IsFeatured, opt => opt.MapFrom(src => src.IsFeatured))
                .ForMember(dest => dest.RegularPrice, opt => opt.MapFrom(src => src.RegularPrice))
                .ForMember(dest => dest.SpecialPrice, opt => opt.MapFrom(src => src.SpecialPrice))
                .ForMember(dest => dest.SKU, opt => opt.MapFrom(src => src.SKU))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime))
                .ForMember(dest => dest.DateAdded, opt => opt.MapFrom(src => src.DateAdded))
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.Id.ToString()));

            AutoMapper.Mapper.CreateMap<Models.DealCategory, DealsWhat.Domain.Model.DealCategory>();
        }

        public IRepository<Deal> CreateDealRepository()
        {
            return new EFDealRepository(this.dbContext);
        }

        public IRepository<DealCategory> CreateDealCategoryRepository()
        {
            return new EFDealCategoryRepository(this.dbContext);
        }
    }
}
