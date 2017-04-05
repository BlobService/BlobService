using BlobService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlobService.Core.Models
{
    public static class ModelMapper
    {
        public static ContainerModel ToModel(this IContainerMeta entity)
        {
            return new ContainerModel()
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public static BlobModel ToModel(this IBlobMeta entity)
        {
            return new BlobModel()
            {
                Id = entity.Id,
                ContainerId = entity.ContainerId,
                MimeType = entity.MimeType,
                SizeInBytes = entity.SizeInBytes,
                OrigFileName = entity.OrigFileName,
                DownloadRelativeUrl = $"/blobs/{entity.Id}/download"
            };
        }

        public static IEnumerable<ContainerModel> ToModelList(this IEnumerable<IContainerMeta> entities)
        {
            foreach (var entity in entities)
            {
                yield return ToModel(entity);
            }
        }

        public static IEnumerable<BlobModel> ToModelList(this IEnumerable<IBlobMeta> entities)
        {
            foreach (var entity in entities)
            {
                yield return ToModel(entity);
            }
        }
    }
}
