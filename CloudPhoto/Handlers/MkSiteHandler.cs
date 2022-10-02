using Amazon.S3;
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
        public void MkSite()
        {
        }
    }
}
