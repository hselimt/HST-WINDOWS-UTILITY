/*
 * 
 * HAVING ISSUES WITH WINDOWS 11 24H2 UPDATE SO THIS PART IS UNDER PROGRESS
 * 
 * 
 * 
 using RemovalTools;

namespace EdgeRemoverApp
{
    public class EdgeRemover
    {
        private readonly RemovalHelpers _removalTools;

        public EdgeRemover(RemovalHelpers removalTools)
        {
            _removalTools = removalTools ?? throw new ArgumentNullException(nameof(removalTools));
        }

        public async Task RemoveEdgeAsync()
        {
            await _removalTools.RunCommandAsync("taskkill", "/f /im msedge.exe");
            await _removalTools.RunCommandAsync("taskkill", "/f /im msedgeupdate.exe");
            await _removalTools.RunCommandAsync("taskkill", "/f /im MicrosoftEdgeUpdate.exe");
            await Task.Delay(1000);

            _removalTools.DeleteFileIfExists(@"%AppData%\Microsoft\Windows\Start Menu\Programs\Microsoft Edge.lnk");
            _removalTools.DeleteFileIfExists(@"%ProgramData%\Microsoft\Windows\Start Menu\Programs\Microsoft Edge.lnk");
            _removalTools.DeleteFileIfExists(@"%Public%\Desktop\Microsoft Edge.lnk");
            _removalTools.DeleteFileIfExists(@"%UserProfile%\Desktop\Microsoft Edge.lnk");

            _removalTools.DeleteDirectoryIfExists(@"C:\Program Files (x86)\Microsoft\Edge");
            _removalTools.DeleteDirectoryIfExists(@"C:\Program Files (x86)\Microsoft\EdgeCore");
            _removalTools.DeleteDirectoryIfExists(@"C:\Program Files (x86)\Microsoft\EdgeUpdate");
            _removalTools.DeleteDirectoryIfExists(@"C:\Program Files (x86)\Microsoft\EdgeWebView");
        }
    }
}*/