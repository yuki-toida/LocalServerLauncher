using System.Configuration;
using System.IO;
using System.Reflection;

namespace LocalServerLauncher.UI
{
    public static class AppConfig
    {
        public static readonly string DefaultConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        public static readonly string DefaultDatabase = ConfigurationManager.AppSettings["DefaultDatabase"];

        public static readonly string DotnetCoreSdkVersion = ConfigurationManager.AppSettings["DotnetCoreSdkVersion"];
        public static readonly string DotnetCoreSdkInstaller = ConfigurationManager.AppSettings["DotnetCoreSdkInstaller"];
        public static readonly string SqlLocalDbInstaller = ConfigurationManager.AppSettings["SqlLocalDbInstaller"];

        public static readonly string LocationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static readonly string ComponentsPath = Path.Combine(LocationPath, "Components");
    }
}
