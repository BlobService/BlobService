using AutoMapper;
using BlobService.Core.Entities;
using BlobService.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlobService.Core
{
    public class MappingProfile : Profile 
    {
        public MappingProfile()
        {
            CreateMap<ContainerMeta, ContainerModel>().ReverseMap();
            CreateMap<BlobMeta, BlobModel>().ReverseMap();
        }
    }
}
