using System;
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

        public static Window GetMainWindow() => GetApp().MainWindow;

        public static IClassicDesktopStyleApplicationLifetime GetApp() =>
            (IClassicDesktopStyleApplicationLifetime) Application.Current.ApplicationLifetime;

        public static void Stop()
        {
            var window = GetMainWindow();

            PostAction(window.Close);
        }

        public static void PostAction(Action action) => Dispatcher.UIThread.Post(action);

        private static AppBuilder BuildAvaloniaApp()
            => AppBuilder
                .Configure<App>()
                .UseHeadless()
                .UsePlatformDetect()
                .LogToDebug()
                .UseReactiveUI();
    }
}