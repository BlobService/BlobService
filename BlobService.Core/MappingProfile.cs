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
            CreateMap<Container, ContainerModel>().ReverseMap();
            CreateMap<Blob, BlobModel>().ReverseMap();
        }
    }
}
