using System;
using Microsoft.Win32;

namespace LocalServerLauncher.Service.Dialog
{
    public class OpenFileDialogService
    {
        public bool Show(Action<string> successAction, Action failAction = null)
        {
            var dialog = new OpenFileDialog
            {
                Title = "bacpacファイルを選択してください",
                Filter = "bacpacファイル(*.bacpac)|*.bacpac",
            };

            if (dialog.ShowDialog() == true)
            {
                successAction(dialog.FileName);
                return true;
            }

            failAction?.Invoke();
            return false;
        }
    }
}
