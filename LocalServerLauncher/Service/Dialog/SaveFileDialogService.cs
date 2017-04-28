using System;
using Microsoft.Win32;

namespace LocalServerLauncher.Service.Dialog
{
    public class SaveFileDialogService
    {
        public bool Show(string fileName, Action<string> successAction, Action failAction = null)
        {
            var dialog = new SaveFileDialog()
            {
                Title = "bacpacファイルを保存する場所を選択してください",
                Filter = "bacpacファイル(*.bacpac)|*.bacpac",
                FileName = fileName,
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
