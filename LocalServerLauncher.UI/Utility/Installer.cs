using System;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalServerLauncher.UI.Utility
{
    public static class Installer
    {
        public static void Install(string fileName, string workingDirectory = null)
        {
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    WorkingDirectory = workingDirectory ?? Directory.GetCurrentDirectory(),
                };

                process.Start();
                process.WaitForExit();
            }
        }
    }
}
