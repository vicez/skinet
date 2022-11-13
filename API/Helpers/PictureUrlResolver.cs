using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class PictureUrlResolver : IValueResolver<Product, ProductsToReturnDTO, string>
    {
        private readonly IConfiguration _config;
        public PictureUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(Product source, ProductsToReturnDTO destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.PictureUrl))
            {
                return _config["ApiUrl"] + source.PictureUrl;
            }

            return null;
        }
    }
}