using AutoMapper;
using BlobService.Core.Entities;
using BlobService.Core.Models;

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
