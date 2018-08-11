using System;
using System.Runtime.Caching;
using CacheSimpleExample.Interfaces;
using CacheSimpleExample.Repository;

namespace CacheSimpleExample.Controllers
{
    public class Cache : ICache
    {
        private readonly IDAL _dal;
        private const int ExpirationTimeInSec = 2;
        private const string CacheKey = "CacheFromDB";
        private static readonly object CacheLock = new object();

        public Cache()
        {
            _dal = new DAL();
        }

        public string GetCachedData()
        {
            var cachedDate = MemoryCache.Default.Get(CacheKey) as string;

            if (cachedDate != null)
            {
                Console.WriteLine("Returned from cache:");
                return cachedDate;
            }

            lock (CacheLock)
            {
                cachedDate = MemoryCache.Default.Get(CacheKey) as string;

                if (cachedDate != null)
                {
                    return cachedDate;
                }

                var dateFromDb = _dal.GetDateTimeFromSql();
                var policy = new CacheItemPolicy()
                {
                    AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(ExpirationTimeInSec))
                };

                MemoryCache.Default.Set(CacheKey, dateFromDb, policy);
                return dateFromDb;
            }
        }
    }
}
