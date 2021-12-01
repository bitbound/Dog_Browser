using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dog_Browser.Services
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly IServiceProvider _services;

        public FileLoggerProvider(IServiceProvider services)
        {
            _services = services;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(_services, categoryName);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
