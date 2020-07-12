using System;
using System.Threading.Tasks;
using Xunit;

namespace Camelot.Tests
{
    public class MainWindowTests : IDisposable
    {
        private const int LoadDelayMs = 5000;

        [Fact]
        public async Task TestMainWindowLoading()
        {
            AvaloniaApp.RegisterDependencies();
            AvaloniaApp.Start();

            await Task.Delay(LoadDelayMs);
        }

        public void Dispose() => AvaloniaApp.Stop();
    }
}