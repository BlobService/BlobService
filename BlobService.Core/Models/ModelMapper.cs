using BlobService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlobService.Core.Models
{
    public static class ModelMapper
    {
        public static ContainerViewModel ToViewModel(this IContainerMeta entity)
        {
            return new ContainerViewModel()
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public static BlobViewModel ToViewModel(this IBlobMeta entity)
        {
            return new BlobViewModel()
            {
                Id = entity.Id,
                ContainerId = entity.ContainerId,
                MimeType = entity.MimeType,
                SizeInBytes = entity.SizeInBytes,
                OrigFileName = entity.OrigFileName,
                DownloadRelativeUrl = $"/blobs/{entity.Id}/raw"
            };
        }

        public static IEnumerable<ContainerViewModel> ToViewModelList(this IEnumerable<IContainerMeta> entities)
        {
            foreach (var entity in entities)
            {
                yield return ToViewModel(entity);
            }
        }

        public static IEnumerable<BlobViewModel> ToViewModelList(this IEnumerable<IBlobMeta> entities)
        {
            foreach (var entity in entities)
            {
                yield return ToViewModel(entity);
            }
        }
    }
}
