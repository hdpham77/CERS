using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace UPF
{
    public static class ObjectContextFactory
    {
        public static T Create<T>(bool lazyLoadingEnabled = false, bool proxyCreationEnabled = false) where T : ObjectContext, new()
        {
            T context = new T();
            context.ContextOptions.LazyLoadingEnabled = lazyLoadingEnabled;
            context.ContextOptions.ProxyCreationEnabled = proxyCreationEnabled;
            return context;
        }
    }
}
