using Amazon.S3;
using Amazon.S3.Model;
using CloudPhoto.Settings;

namespace CloudPhoto.Handlers
{
    public class ListHandler : ConsoleAppBase
    {
        private readonly CloudSettings _cloudSettings;
        private readonly IAmazonS3 _amazonS3;

        public ListHandler(CloudSettings cloudSettings, IAmazonS3 amazonS3)
        {
            _cloudSettings = cloudSettings;
            _amazonS3 = amazonS3;
        }

        [Command("list")]
        public async Task List([Option("album")] string? album = null)
        {
            var list = await _amazonS3.ListObjectsV2Async(new ListObjectsV2Request()
            {
                BucketName = _cloudSettings.Bucket,
                Prefix = album,
            });

            Func<string, string?> action = album is null ? Path.GetDirectoryName : Path.GetFileName;
            ListObjects(list, action);
        }

        private static void ListObjects(ListObjectsV2Response list, Func<string, string?> func)
        {
            var objects = list.S3Objects
                .Select(x => func(x.Key))
                .Distinct();

            foreach (var obj in objects)
            {
                Console.WriteLine(obj);
            }
        }
    }
}
