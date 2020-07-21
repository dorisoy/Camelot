using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.LogicalTree;
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
            AvaloniaApp.RegisterDependencies();
            AvaloniaApp.Start();

            await Task.Delay(LoadDelayMs);
        }

        [Fact]
        public async Task TestOpenSettings()
        {
            AvaloniaApp.RegisterDependencies();
            AvaloniaApp.Start();

            await Task.Delay(LoadDelayMs);

            var window = AvaloniaApp.GetMainWindow();
            AvaloniaApp.PostAction(() =>
            {
                var viewModel = (MainWindowViewModel) window.DataContext;
                Assert.NotNull(viewModel);
                var menuViewModel = (MenuViewModel) viewModel.MenuViewModel;
                menuViewModel.OpenSettingsCommand.Execute(null);
            });

            await Task.Delay(LoadDelayMs);

            SettingsDialog dialog = null;
            AvaloniaApp.PostAction(() => dialog = window.GetLogicalDescendants().OfType<SettingsDialog>().SingleOrDefault());

            await Task.Delay(LoadDelayMs);

            Assert.NotNull(dialog);
        }

        public void Dispose() => AvaloniaApp.Stop();
    }
}