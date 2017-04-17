using System.Reflection;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using LocalServerLauncher.UI.Utility;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace LocalServerLauncher.UI.Service.Dotnet
{
    public class DotnetService
    {
        public const string ExeName = "dotnet";

        private string GetVersion()
        {
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo()
                {
                    FileName = ExeName,
                    Arguments = "--version",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                };

                // webserver run
                process.Start();
                var currentVersion = process.StandardOutput.ReadToEnd().Replace("\r", "").Replace("\n", "");
                process.WaitForExit();
                return currentVersion;
            }
        }

        public bool NeedInstall(string version)
        {
            if (!EnvironmentPathUtility.Exists(ExeName))
                return true;

            return GetVersion() != version;
        }

        public async Task Run(ProgressDialogController progress)
        {
            var dialog = new CommonOpenFileDialog()
            {
                Title = "プロジェクトフォルダを選択してください",
                IsFolderPicker = true,
            };

            var result = dialog.ShowDialog();
            if (result != CommonFileDialogResult.Ok)
            {
                await progress.CloseAsync();
                return;
            }

            var projectPath = dialog.FileName;

            progress.Canceled += (sender, args) => Kill();

            await Task.Run(() =>
            {
                // restore nuget & run
                Command(progress, projectPath, "restore");
                Command(progress, projectPath, "run");
            });

            await progress.CloseAsync();
        }

        private void Command(ProgressDialogController progress, string projectPath, string arguments)
        {
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo()
                {
                    FileName = ExeName,
                    WorkingDirectory = projectPath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                };

                // webserver run
                process.OutputDataReceived += (sender, args) => progress.SetMessage(args.Data);
                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();
            }
        }

        public void Kill()
        {
            foreach (var process in Process.GetProcessesByName(ExeName))
            {
                // webserver kill
                process.Kill();
            }
        }
    }
}
