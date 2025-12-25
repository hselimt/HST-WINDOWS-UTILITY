namespace HST.Controllers.Helpers
{
    public static class Paths
    {
        // Paths of common directiories used by this utility
        public static readonly string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static readonly string ProgramDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        public static readonly string PublicDesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
        public static readonly string UserDesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static readonly string UserProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        public static readonly string LocalAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static readonly string SystemRoot = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
    }
}