using System;

namespace LocalServerLauncher.UI.Command
{
    public class SyncCommand : SyncCommandBase
    {
        private readonly Action _command;

        public SyncCommand(Action command)
        {
            _command = command;
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            _command();
        }
    }
}
