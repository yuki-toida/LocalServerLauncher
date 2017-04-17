using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalServerLauncher.UI.Utility
{
    public static class EnvironmentPathUtility
    {
        public static bool Exists(string file)
        {
            var allPath = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine);
            if (string.IsNullOrWhiteSpace(allPath))
                return false;

            if (Path.GetExtension(file) == string.Empty)
                file = file + ".exe";

            return allPath.Split(';').Any(path => File.Exists(Path.Combine(path, file)));
        }
    }
}
