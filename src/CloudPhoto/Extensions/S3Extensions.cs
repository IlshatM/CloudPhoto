using Amazon.S3;
using Amazon.S3.Model;

namespace CloudPhoto.Extensions
{
    public static class S3Extensions
    {
        public static async Task<bool> Exists(this IAmazonS3 amazonS3, string bucket, string objectKey)
        {
            var list = await amazonS3.ListObjectsV2Async(new ListObjectsV2Request
            {
                BucketName = bucket,
                Prefix = objectKey,
                MaxKeys = 1
            });

            return list.S3Objects.Any();
        }

        public static Task MakePublicReadAsync(this IAmazonS3 amazonS3, string bucket)
        {
            return amazonS3.PutACLAsync(new PutACLRequest
            {
                BucketName = bucket,
                CannedACL = S3CannedACL.PublicRead
            });
        }

        public static Task<List<string>> GetAllFolders(this IAmazonS3 amazonS3, string bucket)
        {
            return GetAll(amazonS3, bucket, Path.GetDirectoryName);
        }

        public static Task<List<string>> GetAllObjectNames(this IAmazonS3 amazonS3, string bucket, string prefix)
        {
            return GetAll(amazonS3, bucket, Path.GetFileName, prefix);
        }

        public static Task CreateWebSite(this IAmazonS3 amazonS3, string bucket,
            string indexHtml = "index.html",
            string errorHtml = "error.html")
        {
            return amazonS3.PutBucketWebsiteAsync(new PutBucketWebsiteRequest
            {
                BucketName = bucket,
                WebsiteConfiguration = new WebsiteConfiguration
                {
                    ErrorDocument = errorHtml,
                    IndexDocumentSuffix = indexHtml
                }
            });
        }


        private static async Task<List<string>> GetAll(IAmazonS3 amazonS3, string bucket, Func<string, string?> func,
            string? prefix = null)
        {
            var list = await amazonS3.ListObjectsV2Async(new ListObjectsV2Request
            {
                BucketName = bucket,
                Prefix = prefix
            });

            return list.S3Objects
                .Select(x => func(x.Key))
                .Where(x => x is not null && !string.IsNullOrWhiteSpace(x))
                .Distinct().ToList()!;
        }
    }
}
