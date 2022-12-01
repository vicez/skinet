using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Entities.OrderAggregates;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductsToReturnDTO>()
            .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
            .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
            .ForMember(d => d.PictureUrl, o => o.MapFrom<PictureUrlResolver>());

            CreateMap<Core.Entities.Identity.Address, AddressDTO>().ReverseMap();
            CreateMap<CustometBasketDTO, CustomerBasket>();
            CreateMap<BasketItemDTO, BasketItem>();
            CreateMap<AddressDTO, Core.Entities.OrderAggregates.Address>();
            CreateMap<OrderItem, OrderItemDTO>()
            .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ProductItemOrdered.ProductItemId))
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ProductItemOrdered.ProductName))
            .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>());
            CreateMap<Order, OrderToReturnDTO>()
            .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
            .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price));
        }

    }
}