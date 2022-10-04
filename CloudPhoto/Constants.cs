namespace CloudPhoto
{
    public static class Constants
    {
        public static readonly string ConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".config", "cloudphoto", "cloudphotorc");

        public const string InitCommand = "init";
    }
}
