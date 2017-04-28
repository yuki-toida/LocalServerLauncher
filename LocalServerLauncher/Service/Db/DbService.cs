using System;
using System.Data.SqlLocalDb;
using System.IO;
using System.Threading.Tasks;
using LocalServerLauncher.Service.Dialog;
using LocalServerLauncher.Utility;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.SqlServer.Dac;

namespace LocalServerLauncher.Service.Db
{
    public class DbService
    {
        public const string ExeName = "SqlLocalDB";

        public bool NeedInstall()
        {
            return !EnvironmentPathUtility.Exists(ExeName);
        }

        private readonly string _databaseName;
        private readonly DacServices _dacService;

        public DbService(string databaseName, string connectionString)
        {
            _databaseName = databaseName;
            _dacService = new DacServices(connectionString);
        }

        public bool Exists => SqlLocalDbApi.GetInstanceInfo(_databaseName).Exists;

        public void Create()
        {
            SqlLocalDbApi.CreateInstance(_databaseName);
        }

        public void Delete()
        {
            if (!Exists)
                return;

            SqlLocalDbApi.StopInstance(_databaseName);
            SqlLocalDbApi.DeleteInstance(_databaseName, true);
        }

        public async Task Import(ProgressDialogController progress)
        {
            await Task.Run(() =>
            {
                // open file
                var result = new OpenFileDialogService().Show(fileName =>
                {
                    // initialize LocalDB
                    Delete();
                    Create();

                    // import bacpac
                    _dacService.ProgressChanged += (sender, args) => progress.SetMessage($"{args.Status} : {args.Message}");
                    _dacService.ImportBacpac(BacPackage.Load(fileName), _databaseName);
                });
                progress.SetMessage(result ? "Success" : "Fail");
            });

            await progress.CloseAsync();
        }

        public async Task Export(ProgressDialogController progress)
        {
            if (!Exists)
                return;

            await Task.Run(() =>
            {
                // export bacpac
                var bacpacName = $"{DateTime.Now:yyyyMMdd_HHmm}.bacpac";
                _dacService.ProgressChanged += (sender, args) => progress.SetMessage($"{args.Status} : {args.Message}");
                _dacService.ExportBacpac(bacpacName, _databaseName);

                // save file
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), bacpacName);
                var result = new SaveFileDialogService().Show(
                    bacpacName
                    , fileName =>
                    {
                        if (!File.Exists(fileName))
                            File.Move(filePath, fileName);
                    }
                    , () => File.Delete(filePath)
                );
                progress.SetMessage(result ? "Success" : "Fail");
            });

            await progress.CloseAsync();
        }
    }
}
