using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.Text;
using System.Threading.Tasks;

namespace GGApp
{
    sealed class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.

        [STAThread]
        public static async Task Main(string[] args)
        {
            // ✅ Устанавливаем кодировку ПЕРЕД запуском приложения
            Console.OutputEncoding = Encoding.UTF8;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // Инициализируем тестовые данные
            await TestDataInitializer.InitializeTestDataAsync();

            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace()
                .UseReactiveUI();
    }
}