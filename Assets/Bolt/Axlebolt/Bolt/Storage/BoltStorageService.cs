using System.Collections.Generic;
using System.Threading.Tasks;
using Axlebolt.Bolt.Api;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Storage
{
	public class BoltStorageService : BoltService<BoltStorageService>
	{
		private readonly StorageRemoteService _storageRemoteService;

		private readonly Dictionary<string, byte[]> _storageFiles;

		public BoltStorageService()
		{
			_storageFiles = new Dictionary<string, byte[]>();
			_storageRemoteService = new StorageRemoteService(BoltApi.Instance.ClientService);
		}

		public Task WriteFile(string filename, byte[] file)
		{
			return BoltApi.Instance.Async(delegate
			{
				_storageFiles[filename] = file;
			});
		}

		public Task<byte[]> ReadFileFromServer(string filename)
		{
			return BoltApi.Instance.Async(delegate
			{
				byte[] array = _storageRemoteService.ReadFile(filename);
				if (!_storageFiles.ContainsKey(filename))
				{
					_storageFiles.Add(filename, array);
				}
				return array;
			});
		}

		public byte[] ReadFile(string filename)
		{
			return _storageFiles.ContainsKey(filename) ? _storageFiles[filename] : null;
		}

		public Task DeleteFile(string filename)
		{
			return BoltApi.Instance.Async(delegate
			{
				_storageRemoteService.DeleteFile(filename);
				_storageFiles.Remove(filename);
			});
		}

		internal override void Load()
		{
			List<BoltStorageFile> list = new List<BoltStorageFile>();
			foreach (BoltStorageFile item in list)
			{
				if (Logger.LogDebug)
				{
					Logger.Debug(string.Format("Loaded {0} ({1})", item.Filename, item.File.Length));
				}
				_storageFiles.Add(item.Filename, item.File);
			}
		}

		internal override void Unload()
		{
			_storageFiles.Clear();
		}
	}
}
