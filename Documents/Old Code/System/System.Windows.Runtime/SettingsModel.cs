using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harness.WinRT;
using Q42.WinRT.Data;
using Q42.WinRT.Storage;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Xaml;
using System.Reflection;

namespace Harness.WinRT
{
    public interface IStorageService : IDependency
    {
        Task<T> LoadAsync<T>(string file, StorageType storageType = StorageType.Temporary, string subfolder = null);
        Task<bool> SaveAsync<T>(T obj, string file, StorageType storageType = StorageType.Temporary, string subfolder = null);
    }

    public class StorageService : IStorageService
    {
        private StorageHelper<T> Helper<T>(StorageType storageType, string subfolder = null)
        {
            return new StorageHelper<T>(storageType, subfolder);
        } 
        
        public StorageService()
        {
            
        }

        public async Task<T> LoadAsync<T>(string file, StorageType storageType = StorageType.Temporary, string subfolder = null)
        {
            try
            {
                return await Helper<T>(storageType, subfolder).LoadAsync(file);    
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public async Task<bool> SaveAsync<T>(T obj, string file, StorageType storageType = StorageType.Temporary , string subfolder = null)
        {
            try
            {
                await Helper<T>(storageType, subfolder).SaveAsync(obj, file);
                return true;    
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public async Task<bool> DeleteAsync<T>(string fileName, StorageType storageType = StorageType.Temporary, string subfolder = null)
        {
            try
            {
                await Helper<T>(storageType, subfolder).DeleteAsync(fileName);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    public interface ICacheService : Harness.IDependency
    {
        
    }

    public class CacheObject : INotifyPropertyChange
    {
        private dynamic Object { get; set; }
        private Type ObjectType { get; set; }

        public DateTime Expires { get; set; }
        public Uri Location { get; set; }
        public Uri Origin { get; set; }
        public bool IsCached { get; set; }
        public Guid Id { get; set; }
        
        public CacheObject()
        {
            Object = null;
            ObjectType = typeof (Object);
        }
  
        public T Value<T>(T newValue = null) where T : class 
        {
            if (newValue == null)
            {
                return Object as T;
            }
            else
            {
                Object = newValue;
                NotifyOfPropertyChange(() => Object);
                return newValue;
            }
        }
    }

    public class Cache : HashSet<CacheObject>
    {
        

        public string Id { get; set; }

        public Cache()
        {
            
        }
       
        private async Task<T> LoadAsync<T>(CacheObject obj) where T : class 
        {
            if (obj.IsCached) return obj.Value<T>();
            return await Harness.Application.Resolve<IStorageService>().LoadAsync<T>(obj.Id.ToString(), StorageType.Temporary, Id + ".cache");
        }

        public async Task<T> GetByIdAsync<T>(Guid id) where T : class
        {
            return await LoadAsync<T>(this.FirstOrDefault(x => x.Id == id));
        }

        public async Task<T> GetByOriginAsync<T>(Uri origin) where T : class
        {
            var target = this.FirstOrDefault(x => x.Origin == origin);
            if (target != null) return await LoadAsync<T>(target);
            return null;

        }


    }

    
    public class CacheService : ICacheService
    {
        private IStorageService _storage;
        private Cache _applicationCache;
      

        public CacheService(IStorageService storage)
        {
            _storage = storage;
            _applicationCache = new Cache();
         
            Application.Current.Suspending += ApplicationSuspending;
            Application.Current.Resuming += ApplicationResuming;
        }

        private async void ApplicationResuming(object sender, object o)
        {

            _applicationCache = await _storage.LoadAsync<Cache>("Application.cache");
        }

        private async void ApplicationSuspending(object sender, SuspendingEventArgs suspendingEventArgs)
        {
            var deferral = suspendingEventArgs.SuspendingOperation.GetDeferral();
            await _storage.SaveAsync(_applicationCache, "Application.cache");
            deferral.Complete();
        }
    }
}
