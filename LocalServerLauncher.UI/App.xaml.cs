using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Reflection;

namespace LocalServerLauncher.UI
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            if (Directory.Exists(AppConfig.ComponentsPath))
                Directory.Delete(AppConfig.ComponentsPath, true);

            var zipFile = Path.Combine(AppConfig.LocationPath, "Components.zip");
            using (var archive = ZipFile.OpenRead(zipFile))
                archive.ExtractToDirectory(AppConfig.LocationPath);
        }
    }
}
