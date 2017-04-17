using System.Threading.Tasks;
using System.Windows.Input;

namespace LocalServerLauncher.UI.Command
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}
