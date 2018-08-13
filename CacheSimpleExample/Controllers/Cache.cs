using System;
using System.Runtime.Caching;
using CacheSimpleExample.Interfaces;

namespace CacheSimpleExample.Controllers
{
    public class Cache : ICache
    {
        private readonly IDAL _dal;
        private const int ExpirationTimeInSec = 2;
        private const string CacheKey = "CacheFromDB";

        public Cache(IDAL dal)
        {
            _dal = dal;
        }

        public string GetCachedData()
        {
            if (!MemoryCache.Default.Contains(CacheKey))
            {
                var dateFromDb = _dal.GetDateTimeFromSql();
                var policy = new CacheItemPolicy()
                {
                    AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(ExpirationTimeInSec))
                };

                MemoryCache.Default.Set(CacheKey, dateFromDb, policy);
                return dateFromDb;
            }

            Console.WriteLine("Returned from cache:");
            return MemoryCache.Default.Get(CacheKey) as string;
        }
    }
}
