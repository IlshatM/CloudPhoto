using Amazon.S3.Model;
using CloudPhoto.Helpers;
using CloudPhoto.Settings;

namespace CloudPhoto.Handlers
{
    public class InitHandler : ConsoleAppBase
    {
        [Command("init")]
        public async Task Handle()
        {
            var cloudSettings = GetCloudSettings();
            SaveCloudSettings(cloudSettings);
            await CreateBucketIfDoesNotExist(cloudSettings);
        }

        private CloudSettings GetCloudSettings()
        {
            var cloudSettings = new CloudSettings
            {
                BaseUrl = @"https://storage.yandexcloud.net"
            };
            
            Console.WriteLine("Добро пожаловать в cloudphoto!");

            Console.Write("Пожалуйста, введите aws_access_key_id: ");
            cloudSettings.AwsAccessKeyId = Console.ReadLine() ?? throw new ArgumentException();

            Console.Write("Теперь введите aws_secret_access_key: ");
            cloudSettings.AwsSecretAccessKey = Console.ReadLine() ?? throw new ArgumentException();

            Console.Write("Введите бакет: ");
            cloudSettings.Bucket = Console.ReadLine() ?? throw new ArgumentException();

            return cloudSettings;
        }

        private void SaveCloudSettings(CloudSettings cloudSettings)
        {
            EnsureDirectoryExists(Constants.ConfigPath);
            using var streamWriter = new StreamWriter(Constants.ConfigPath);
            streamWriter.WriteLine("[DEFAULT]");
            streamWriter.WriteLine($"bucket = {cloudSettings.Bucket}");
            streamWriter.WriteLine($"aws_access_key_id = {cloudSettings.AwsAccessKeyId}");
            streamWriter.WriteLine($"aws_secret_access_key = {cloudSettings.AwsSecretAccessKey}");
            streamWriter.WriteLine("region = ru-central1");
            streamWriter.WriteLine($"endpoint_url = {cloudSettings.BaseUrl}");
        }

        private static void EnsureDirectoryExists(string filePath)
        {
            var fi = new FileInfo(filePath);
            if (!fi.Directory!.Exists) Directory.CreateDirectory(fi.DirectoryName!);
        }

        private async Task CreateBucketIfDoesNotExist(CloudSettings cloudSettings)
        {
            using var client = AwsHelper.CreateClient(cloudSettings);
            var bucket = cloudSettings.Bucket;
            var doesBucketExist = await client.DoesS3BucketExistAsync(bucket);
            if (!doesBucketExist)
            {
                var request = new PutBucketRequest
                {
                    BucketName = bucket
                };

                await client.PutBucketAsync(request);
            }
        }
    }
}
