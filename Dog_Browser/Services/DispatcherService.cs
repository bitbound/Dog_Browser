using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dog_Browser.Services
{
    public interface IDispatcherService
    {
        void Invoke(Action callback);
        T Invoke<T>(Func<T> callback);
    }

    public class DispatcherService : IDispatcherService
    {
        public void Invoke(Action callback)
        {
            App.Current.Dispatcher.Invoke(callback);
        }

        public T Invoke<T>(Func<T> callback)
        {
            return App.Current.Dispatcher.Invoke(callback);
        }
    }
}
