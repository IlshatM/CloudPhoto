using Amazon.S3;
using Amazon.S3.Model;
using CloudPhoto.Extensions;
using CloudPhoto.Helpers;
using CloudPhoto.Models;
using CloudPhoto.Settings;

namespace CloudPhoto.Handlers
{
    public class MkSiteHandler : ConsoleAppBase
    {
        private readonly IAmazonS3 _amazonS3;
        private readonly CloudSettings _cloudSettings;

        public MkSiteHandler(IAmazonS3 amazonS3, CloudSettings cloudSettings)
        {
            _amazonS3 = amazonS3;
            _cloudSettings = cloudSettings;
        }

        [Command("mksite")]
        public async Task MkSite()
        {
            await _amazonS3.MakePublicReadAsync(_cloudSettings.Bucket);
            await _amazonS3.CreateWebSite(_cloudSettings.Bucket);

            var allFolders = await _amazonS3.GetAllFolders(_cloudSettings.Bucket);

            var albumHtmlList = new List<ImageModel>();

            for (var i = 0; i < allFolders.Count; i++)
            {
                var album = allFolders[i];

                var objects = await _amazonS3.ListObjectsV2Async(new ListObjectsV2Request
                {
                    BucketName = _cloudSettings.Bucket,
                    Prefix = album
                });

                var model = objects.S3Objects
                    .Select(obj => new ImageModel
                    {
                        Name = obj.Key.Replace(album + "/", ""),
                        Source = $"https://storage.yandexcloud.net/{_cloudSettings.Bucket}/{obj.Key}"
                    })
                    .ToList();

                var albumHtml = await RenderViewHelper.GetHtmlFromRazor($"{AppDomain.CurrentDomain.BaseDirectory}/Pages/Album.cshtml", model);
                await using (var stream = RenderViewHelper.GenerateStreamFromString(albumHtml))
                {
                    await _amazonS3.PutObjectAsync(new PutObjectRequest
                    {
                        BucketName = _cloudSettings.Bucket,
                        InputStream = stream,
                        Key = $"album{i + 1}.html"
                    });
                }

                albumHtmlList.Add(new ImageModel
                {
                    Source = $"album{i + 1}.html",
                    Name = album
                });
            }

            var indexHtml = await RenderViewHelper.GetHtmlFromRazor($"{AppDomain.CurrentDomain.BaseDirectory}/Pages/Index.cshtml", albumHtmlList);
            await using (var stream = RenderViewHelper.GenerateStreamFromString(indexHtml))
            {
                await _amazonS3.PutObjectAsync(new PutObjectRequest
                {
                    BucketName = _cloudSettings.Bucket,
                    InputStream = stream,
                    Key = "index.html"
                });
            }

            var errorHtml = await RenderViewHelper.GetHtmlFromRazor($"{AppDomain.CurrentDomain.BaseDirectory}/Pages/Error.cshtml");
            await using (var stream = RenderViewHelper.GenerateStreamFromString(errorHtml))
            {
                await _amazonS3.PutObjectAsync(new PutObjectRequest
                {
                    BucketName = _cloudSettings.Bucket,
                    InputStream = stream,
                    Key = "error.html"
                });
            }

            Console.WriteLine($"https://{_cloudSettings.Bucket}.website.yandexcloud.net/");
        }
    }
}
