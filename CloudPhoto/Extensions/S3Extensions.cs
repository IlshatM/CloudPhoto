using Amazon.S3;
using Amazon.S3.Model;

namespace CloudPhoto.Extensions
{
    public static class S3Extensions
    {
        public static async Task<bool> Exists(this IAmazonS3 amazonS3, string bucket, string objectKey)
        {
            var list = await amazonS3.ListObjectsV2Async(new ListObjectsV2Request()
            {
                BucketName = bucket,
                Prefix = objectKey,
                MaxKeys = 1,
            });

            return list.S3Objects.Any();
        }
    }
}
