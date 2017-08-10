using BlobService.Core.Entities;
using System.Collections.Generic;

namespace BlobService.Core.Models
{
    public static class ModelMapper
    {
        public static ContainerViewModel ToViewModel(this IContainer entity)
        {
            return new ContainerViewModel()
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public static BlobViewModel ToViewModel(this IBlob entity)
        {
            var metaData = ToViewModelList(entity.MetaData);

            return new BlobViewModel()
            {
                Id = entity.Id,
                ContainerId = entity.ContainerId,
                MimeType = entity.MimeType,
                SizeInBytes = entity.SizeInBytes,
                OrigFileName = entity.OrigFileName,
                DownloadRelativeUrl = $"/blobs/{entity.Id}/raw",
                MetaData = metaData
            };
        }

        public static BlobMetaDataViewModel ToViewModel(this IBlobMetaData entity)
        {
            return new BlobMetaDataViewModel()
            {
                Key = entity.Key,
                Value = entity.Value
            };
        }

        public static IEnumerable<ContainerViewModel> ToViewModelList(this IEnumerable<IContainer> entities)
        {
            foreach (var entity in entities)
            {
                yield return ToViewModel(entity);
            }
        }

        public static IEnumerable<BlobMetaDataViewModel> ToViewModelList(this IEnumerable<IBlobMetaData> entities)
        {
            foreach (var entity in entities)
            {
                yield return ToViewModel(entity);
            }
        }

        public static IEnumerable<BlobViewModel> ToViewModelList(this IEnumerable<IBlob> entities)
        {
            foreach (var entity in entities)
            {
                yield return ToViewModel(entity);
            }
        }
    }
}
