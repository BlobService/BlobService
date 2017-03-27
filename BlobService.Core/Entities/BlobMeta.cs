﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BlobService.Core.Entities
{
    public class BlobMeta
    {
        public string Id { get; set; }
        public string ContainerId { get; set; }
        public string OrigFileName { get; set; }
        public int SizeInBytes { get; set; }
        public string MimeType { get; set; }
        public string StorageSubject { get; set; }
    }
}