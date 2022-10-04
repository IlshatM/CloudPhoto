using Amazon.S3;
using CloudPhoto.Extensions;
using CloudPhoto.Settings;

namespace CloudPhoto.Handlers
{
    public class ListHandler : ConsoleAppBase
    {
        private readonly IAmazonS3 _amazonS3;
        private readonly CloudSettings _cloudSettings;

        public ListHandler(CloudSettings cloudSettings, IAmazonS3 amazonS3)
        {
            _cloudSettings = cloudSettings;
            _amazonS3 = amazonS3;
        }

        [Command("list")]
        public async Task List([Option("album")] string? album = null)
        {
            var objects = album is null
                ? await _amazonS3.GetAllFolders(_cloudSettings.Bucket)
                : await _amazonS3.GetAllObjectNames(_cloudSettings.Bucket, album);

            foreach (var obj in objects) Console.WriteLine(obj);
        }
    }
}
