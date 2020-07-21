using System;
using System.Linq;
using System.Threading.Tasks;
using Camelot.ViewModels.Implementations;
using Camelot.ViewModels.Implementations.Menu;
using Camelot.Views.Dialogs;
using Xunit;

namespace Camelot.Tests
{
    public class MainWindowTests : IDisposable
    {
        private const int LoadDelayMs = 3000;

        [Fact]
        public async Task TestMainWindowLoading()
        {
            await LoadAppAsync();
            var window = AvaloniaApp.GetMainWindow();

            Assert.NotNull(window);
        }

        [Fact]
        public async Task TestOpenSettings()
        {
            await LoadAppAsync();

            var window = AvaloniaApp.GetMainWindow();
            var openMenuTaskCompletionSource = new TaskCompletionSource<bool>();
            AvaloniaApp.PostAction(() =>
            {
                var viewModel = (MainWindowViewModel) window.DataContext;
                Assert.NotNull(viewModel);
                var menuViewModel = (MenuViewModel) viewModel.MenuViewModel;
                menuViewModel.OpenSettingsCommand.Execute(null);
                openMenuTaskCompletionSource.SetResult(true);
            });

            await openMenuTaskCompletionSource.Task;

            SettingsDialog dialog = null;
            var getDialogTaskCompletionSource = new TaskCompletionSource<bool>();
            AvaloniaApp.PostAction(() =>
            {
                dialog = AvaloniaApp.GetApp().Windows.OfType<SettingsDialog>().SingleOrDefault();
                getDialogTaskCompletionSource.SetResult(true);
            });

            await getDialogTaskCompletionSource.Task;

            Assert.NotNull(dialog);
        }

        public void Dispose() => AvaloniaApp.Stop();

        private static async Task LoadAppAsync()
        {
            AvaloniaApp.RegisterDependencies();
            AvaloniaApp.Start();

            await Task.Delay(LoadDelayMs);
        }
    }
}