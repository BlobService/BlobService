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
            CreateMap<BlobMeta, BlobModel>().AfterMap((blobMeta, blobModel) =>
            {
                blobModel.DownloadRelativeUrl = $"/blobs/{blobMeta.Id}/download";
            });
        }
    }
}
