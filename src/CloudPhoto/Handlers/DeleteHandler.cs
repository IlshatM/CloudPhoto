using Amazon.S3;
using Amazon.S3.Model;
using CloudPhoto.Extensions;
using CloudPhoto.Settings;

namespace CloudPhoto.Handlers
{
    public class DeleteHandler : ConsoleAppBase
    {
        private readonly IAmazonS3 _amazonS3;
        private readonly CloudSettings _cloudSettings;

        public DeleteHandler(CloudSettings cloudSettings, IAmazonS3 amazonS3)
        {
            _cloudSettings = cloudSettings;
            _amazonS3 = amazonS3;
        }

        [Command("delete")]
        public async Task Delete([Option("album")] string album, [Option("photo")] string? photo = null)
        {
            if (photo is null)
                await DeleteFolder(album);
            else
                await DeletePhoto(album, photo);
        }

        private async Task DeletePhoto(string album, string photo)
        {
            var objectKey = $"{album}/{photo}";
            var doesObjectExists = await _amazonS3.Exists(_cloudSettings.Bucket, objectKey);
            if (!doesObjectExists) throw new ApplicationException("No object!");

            await _amazonS3.DeleteObjectAsync(new DeleteObjectRequest
            {
                BucketName = _cloudSettings.Bucket,
                Key = objectKey
            });
        }

        private async Task DeleteFolder(string album)
        {
            var list = await _amazonS3.ListObjectsV2Async(new ListObjectsV2Request
            {
                BucketName = _cloudSettings.Bucket,
                Prefix = album
            });

            if (!list.S3Objects.Any()) throw new ApplicationException("No album!");

            var objects = list.S3Objects.Select(x => new KeyVersion { Key = x.Key }).ToList();

            await _amazonS3.DeleteObjectsAsync(new DeleteObjectsRequest
            {
                BucketName = _cloudSettings.Bucket,
                Objects = objects
            });
        }
    }
}
