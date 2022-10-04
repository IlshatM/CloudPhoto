using Amazon.Runtime.Internal.Util;

namespace CloudPhoto.Settings
{
    public class CloudSettings
    {
        public string BaseUrl { get; set; } = null!;

        public string AwsAccessKeyId { get; set; } = null!;

        public string AwsSecretAccessKey { get; set; } = null!;

        public string Bucket { get; set; } = null!;

        public static CloudSettings FromIni(string path)
        {
            var ini = new IniFile(path);
            var hasSection = ini.TryGetSection("DEFAULT", out var config);
            var isAllValuePresent = config.All(x => !string.IsNullOrWhiteSpace(x.Value));
            if (!hasSection || !isAllValuePresent)
            {
                throw new ApplicationException("Ini file is invalid");
            }

            var settings = new CloudSettings
            {
                BaseUrl = config["endpoint_url"],
                Bucket = config["bucket"],
                AwsAccessKeyId = config["aws_access_key_id"],
                AwsSecretAccessKey = config["aws_secret_access_key"]
            };

            return settings;
        }
    }
}
