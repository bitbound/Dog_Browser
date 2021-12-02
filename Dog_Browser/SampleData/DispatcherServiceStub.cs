using Dog_Browser.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dog_Browser.SampleData
{
    public class DispatcherServiceStub : IDispatcherService
    {
        public void Invoke(Action callback)
        {
            callback();
        }

        public T Invoke<T>(Func<T> callback)
        {
            return callback();
        }
    }
}
