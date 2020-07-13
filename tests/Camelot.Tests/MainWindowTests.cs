using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
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
            AvaloniaApp.PostAction(() => window.RaiseEvent(new KeyEventArgs {RoutedEvent = InputElement.KeyDownEvent, Key = Key.C, KeyModifiers = KeyModifiers.Alt }));
            AvaloniaApp.PostAction(() => window.RaiseEvent(new KeyEventArgs {RoutedEvent = InputElement.KeyUpEvent, Key = Key.C, KeyModifiers = KeyModifiers.Alt }));
            await Task.Delay(500);
            var menu = window.GetLogicalDescendants().OfType<Menu>().First();
            // AvaloniaApp.PostAction(() => menu.RaiseEvent(new KeyEventArgs {RoutedEvent = InputElement.KeyDownEvent, Key = Key.S }));
            // AvaloniaApp.PostAction(() => menu.RaiseEvent(new KeyEventArgs {RoutedEvent = InputElement.KeyUpEvent, Key = Key.S }));
            // AvaloniaApp.PostAction(() => menu.RaiseEvent(new KeyEventArgs {RoutedEvent = InputElement.KeyDownEvent, Key = Key.Enter }));
            // AvaloniaApp.PostAction(() => menu.RaiseEvent(new KeyEventArgs {RoutedEvent = InputElement.KeyUpEvent, Key = Key.Enter }));
            var item = menu.GetLogicalDescendants().OfType<MenuItem>().ToArray()[3];
            {
                AvaloniaApp.PostAction(() => item.RaiseEvent(new RoutedEventArgs{RoutedEvent = InputElement.PointerPressedEvent}));
                AvaloniaApp.PostAction(() => item.RaiseEvent(new RoutedEventArgs{RoutedEvent = InputElement.PointerReleasedEvent}));
            }
            // var menuItem = menu.GetLogicalDescendants().OfType<MenuItem>().ToArray()[3];
            // AvaloniaApp.PostAction(() => menuItem.RaiseEvent(new RoutedEventArgs{RoutedEvent = InputElement.TappedEvent}));
            await Task.Delay(500000);
        }

        public void Dispose() => AvaloniaApp.Stop();
    }
}