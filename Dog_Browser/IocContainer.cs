using Dog_Browser.Services;
using Dog_Browser.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dog_Browser
{
    public static class IocContainer
    {
        private static readonly SemaphoreSlim _buildLock = new(1, 1);

        private static IServiceProvider? _serviceProvider;

        public static T GetRequiredService<T>()
            where T: notnull
        {
            return GetServiceProvider().GetRequiredService<T>();
        }

        private static IServiceProvider GetServiceProvider()
        {
            try
            {
                // Prevent race conditions if this method is somehow called from different threads.
                // A simple lock statement would work too.
                _buildLock.Wait();

                if (_serviceProvider is not null)
                {
                    return _serviceProvider;
                }

                var collection = new ServiceCollection();

                // The IHttpClientFactory makes better use of resources when disposing of HttpClients.
                // A typical desktop app will likely never make frequent enough requests to run into
                // socket exhaustion or other performance issues, but I still use it anyway.
                collection.AddHttpClient();

                collection.AddLogging(builder =>
                {
                    builder.AddDebug();
                    builder.AddEventLog();
                });

                // I usually create services to abstract System.IO.File/Directory
                // and DateTime/DateTimeOffset.  During tests, you don't want to hit
                // the real file system, and it's often very difficult to test time-based
                // services like schedulers without mocking system time.
                collection.AddSingleton<IFileSystem, FileSystem>();
                collection.AddSingleton<ISystemTime, SystemTime>();

                // Lets us mock MessageBox.Show when unit-testing view models.
                collection.AddSingleton<IDialogService, DialogService>();

                collection.AddScoped<IHttpClientWrapper, HttpClientWrapper>();
                collection.AddScoped<IDogBreedsApi, DogBreedsApi>();

                // Use Singletons for Window and Page view models, so their state is
                // persisted when navigating away and back.
                collection.AddSingleton<MainWindowViewModel>();
                collection.AddSingleton<BreedBrowserViewModel>();
                collection.AddSingleton<LogsPageViewModel>();

                // We need a new instance every time for this view model.
                collection.AddTransient<BreedDetailsViewModel>();

                _serviceProvider = collection.BuildServiceProvider();

                // Add a logging provider to log to the file system.
                _serviceProvider
                    .GetRequiredService<ILoggerFactory>()
                    .AddProvider(new FileLoggerProvider(_serviceProvider));

                return _serviceProvider;
            }
            finally
            {
                // Always release in finally block to guarantee it gets called.
                _buildLock.Release();
            }
        }
    }
}
