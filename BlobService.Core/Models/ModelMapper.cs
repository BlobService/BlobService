using BlobService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlobService.Core.Models
{
    public static class ModelMapper
    {
        public static ContainerMeta ToEntity(this ContainerModel model)
        {
            return new ContainerMeta()
            {
                Id = model.Id,
                Name = model.Name
            };
        }

        public static ContainerModel ToModel(this ContainerMeta entity)
        {
            return new ContainerModel()
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public static BlobMeta ToEntity(this BlobModel model)
        {
            return new BlobMeta()
            {
                Id = model.Id,
                ContainerId = model.ContainerId,
                MimeType = model.MimeType,
                OrigFileName = model.OrigFileName,
                SizeInBytes = model.SizeInBytes
            };
        }

        public static BlobModel ToModel(this BlobMeta entity)
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

        public static IEnumerable<ContainerModel> ToModelList(this IEnumerable<ContainerMeta> entities)
        {
            foreach (var entity in entities)
            {
                yield return ToModel(entity);
            }
        }

        public static IEnumerable<BlobModel> ToModelList(this IEnumerable<BlobMeta> entities)
        {
            foreach (var entity in entities)
            {
                yield return ToModel(entity);
            }
        }
    }
}
