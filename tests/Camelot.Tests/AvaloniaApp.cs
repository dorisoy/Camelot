using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Headless;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using Splat;

namespace Camelot.Tests
{
    public static class AvaloniaApp
    {
        public static void Start(params string[] args) =>
            Task.Run(() => BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args, ShutdownMode.OnMainWindowClose));

        public static void RegisterDependencies() => Bootstrapper.Register(Locator.CurrentMutable, Locator.Current);

        public static void Stop()
        {
            var window = ((IClassicDesktopStyleApplicationLifetime) Application.Current.ApplicationLifetime)
                .MainWindow;

            Dispatcher.UIThread.Post(window.Close);
        }

        private static AppBuilder BuildAvaloniaApp()
            => AppBuilder
                .Configure<App>()
                .UsePlatformDetect()
                .LogToDebug()
                .UseReactiveUI()
                .UseHeadless();
    }
}