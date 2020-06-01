using System;
using System.Runtime.Caching;

namespace Pulse.ServiceMonitor.Utils
{
    public static class CacheControls
    {
        private static readonly ObjectCache _cache = MemoryCache.Default;
        
        public static void UpsertCacheModel<TResult>(TResult model, string cacheName) where TResult : new ()
        {
            if (_cache[cacheName] == null)
                _cache[cacheName] = new TResult();

            _cache[cacheName] = model;
        }

        public static TResult GetCacheModel<TResult>(string cacheName) where TResult : new ()
        {
            if (_cache[cacheName] == null)
                _cache[cacheName] = new TResult();

            return (TResult)_cache[cacheName];
        }
    }
}