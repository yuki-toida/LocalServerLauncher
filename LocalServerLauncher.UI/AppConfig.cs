using System.Configuration;
using System.IO;
using System.Reflection;

namespace LocalServerLauncher.UI
{
    public static class AppConfig
    {
        public static readonly string DatabaseName = ConfigurationManager.AppSettings["DatabaseName"];
        public static readonly string ConnectionString = ConfigurationManager.ConnectionStrings[DatabaseName].ConnectionString;

        public static readonly string DotnetCoreSdkVersion = ConfigurationManager.AppSettings["DotnetCoreSdkVersion"];
        public static readonly string DotnetCoreSdkInstaller = ConfigurationManager.AppSettings["DotnetCoreSdkInstaller"];
        public static readonly string SqlLocalDbInstaller = ConfigurationManager.AppSettings["SqlLocalDbInstaller"];

        public static readonly string LocationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static readonly string ComponentsPath = Path.Combine(LocationPath, "Components");
    }
}
