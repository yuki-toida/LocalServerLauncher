using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LocalServerLauncher.UI.Command;
using LocalServerLauncher.UI.Service.Db;
using LocalServerLauncher.UI.Service.Dialog;
using LocalServerLauncher.UI.Service.Dotnet;
using LocalServerLauncher.UI.Utility;

namespace LocalServerLauncher.UI.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            _dotnetService = new DotnetService();
            _dbService = new DbService(AppConfig.DefaultDatabase, AppConfig.DefaultConnectionString);
            _needDotnet = _dotnetService.NeedInstall(AppConfig.DotnetCoreSdkVersion);
            _needDb = _dbService.NeedInstall();
        }

        private readonly DotnetService _dotnetService;
        private readonly DbService _dbService;
        private bool _needDotnet;
        private bool _needDb;

        // DI from xaml
        public IDialogCoordinator DialogCoordinator { get; set; }

        public ICommand InstallDotnetCommand => new SyncCommand(InstallDotnet);
        public ICommand InstallDbCommand => new SyncCommand(InstallDb);
        public ICommand ImportCommand => new AsyncCommand(Import);
        public ICommand ExportCommand => new AsyncCommand(Export);
        public ICommand RunCommand => new AsyncCommand(Run);
        public ICommand KillCommand => new SyncCommand(_dotnetService.Kill);

        public bool CanLaunch => !NeedDotnet && !NeedDb;

        public bool NeedDotnet
        {
            get => _needDotnet;
            set
            {
                _needDotnet = value;
                NotifyPropertyChanged(nameof(NeedDotnet));
                NotifyPropertyChanged(nameof(CanLaunch));
            }
        }

        public bool NeedDb
        {
            get => _needDb;
            set
            {
                _needDb = value;
                NotifyPropertyChanged(nameof(NeedDb));
                NotifyPropertyChanged(nameof(CanLaunch));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        public void InstallDotnet()
        {
            Installer.Install(AppConfig.DotnetCoreSdkInstaller, AppConfig.ComponentsPath);
            if (EnvironmentPathUtility.Exists(DotnetService.ExeName))
                NeedDotnet = false;
        }

        public void InstallDb()
        {
            Installer.Install(AppConfig.SqlLocalDbInstaller, AppConfig.ComponentsPath);
            if (EnvironmentPathUtility.Exists(DbService.ExeName))
                NeedDb = false;
        }

        public async Task Import()
        {
            var progress = await DialogCoordinator.ShowProgressAsync(this, "Database Importing", "Please wait...");
            await _dbService.Import(progress);
        }

        public async Task Export()
        {
            var progress = await DialogCoordinator.ShowProgressAsync(this, "Database Exporting", "Please wait...");
            await _dbService.Export(progress);
        }

        public async Task Run()
        {
            var progress = await DialogCoordinator.ShowProgressAsync(this, "WebServer Running", "Please wait...", true);
            await _dotnetService.Run(progress);
        }
    }
}
