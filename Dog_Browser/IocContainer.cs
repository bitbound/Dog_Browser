using Dog_Browser.Services;
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
            if (_serviceProvider is null)
            {
                throw new Exception("ServiceProvider has not been configured yet.");
            }

            return _serviceProvider.GetRequiredService<T>();
        }

        public static void BuildServices()
        {
            try
            {
                // Prevent race conditions if this method is somehow called from different threads.
                // A simple lock statement would work too.

                _buildLock.Wait();

                if (_serviceProvider is not null)
                {
                    return;
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

                collection.AddSingleton<IFileSystem, FileSystem>();
                collection.AddSingleton<ISystemTime, SystemTime>();

                _serviceProvider = collection.BuildServiceProvider();

                _serviceProvider
                    .GetRequiredService<ILoggerFactory>()
                    .AddProvider(new FileLoggerProvider(_serviceProvider));

            }
            finally
            {
                // Always release in finally block to guarantee it gets called.
                _buildLock.Release();
            }
        }
    }
}
