using ebyteLearner.Helpers;
using System.Runtime.Caching;

namespace ebyteLearner.Services
{
    public interface ICacheService
    {
        T GetData<T>(string Key);
        bool SetData<T>(string key, T value, DateTimeOffset expirationTime);
        object RemoveData(string key);
    }
    public class CacheService : ICacheService
    {
        private ObjectCache _memoryCache = MemoryCache.Default;
        private SemaphoreSlim semaphore = new SemaphoreSlim(1);

        public T GetData<T>(string Key)
        {
            semaphore.Wait();
            try
            {
                T item = (T)_memoryCache.Get(Key);
                return item;
            }
            catch (Exception ex)
            {
                throw new AppException($"Error found: '{ex.Message}'");
            }
            finally
            {
                semaphore.Release();
            }

        }

        public object RemoveData(string key)
        {
            var result = true;
            try
            {
                if (!string.IsNullOrEmpty(key))
                    _memoryCache.Remove(key);
                else
                    result = false;

                return result;

            }
            catch (Exception ex)
            {
                throw new AppException($"Error found: '{ex.Message}'");
            }
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var result = true;
            semaphore.Wait();
            try
            {
                if (!string.IsNullOrEmpty(key) && value != null)
                    _memoryCache.Set(key, value, expirationTime);                
                else                
                    result = false;
                
                return result;

            }
            catch (Exception ex)
            {
                throw new AppException($"Error found: '{ex.Message}'");
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
