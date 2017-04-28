using System.Threading.Tasks;
using System.Windows.Input;

namespace LocalServerLauncher.Command
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}
