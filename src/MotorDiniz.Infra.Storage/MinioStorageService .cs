// src/MotorDiniz.Infra.Storage/MinioStorageService.cs
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;
using MotorDiniz.Application.Interfaces;

namespace MotorDiniz.Infra.Storage
{
    public sealed class MinioStorageService : IObjectStorage
    {
        private readonly IMinioClient _minio;
        private readonly string _bucket;

        public MinioStorageService(IMinioClient minio, IConfiguration cfg)
        {
            _minio = minio;
            _bucket = cfg["Minio:BucketCnh"] ?? "cnh-images";
        }

        public async Task<string> UploadCnhImageAsync(string riderIdentifier, byte[] imageBytes, string contentType, CancellationToken ct)
        {
            
            var exists = await _minio.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucket), ct);
            if (!exists)
                await _minio.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucket), ct);

            var ext = contentType.Equals("image/png", StringComparison.OrdinalIgnoreCase) ? ".png" : ".bmp";
            var safeId = new string((riderIdentifier ?? "").Where(ch => char.IsLetterOrDigit(ch) || ch == '-' || ch == '_').ToArray());
            if (string.IsNullOrWhiteSpace(safeId)) safeId = "unknown";
            var objectKey = $"delivery-riders/{safeId}/cnh{ext}";

            using var ms = new MemoryStream(imageBytes);
            var put = new PutObjectArgs()
                .WithBucket(_bucket)
                .WithObject(objectKey)
                .WithStreamData(ms)
                .WithObjectSize(ms.Length)
                .WithContentType(contentType);

            await _minio.PutObjectAsync(put, ct);
            return objectKey;
        }
    }
}
