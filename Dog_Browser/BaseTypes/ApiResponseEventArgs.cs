using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dog_Browser.BaseTypes
{
    public class ApiResponseEventArgs<T> : EventArgs
    {
        public ApiResponseEventArgs(Result<T> result, bool resolvedFromCache)
        {
            Result = result;
            ResolvedFromCache = resolvedFromCache;
        }

        public Result<T> Result { get; init; }
        public bool ResolvedFromCache { get; init; }
    }
}
