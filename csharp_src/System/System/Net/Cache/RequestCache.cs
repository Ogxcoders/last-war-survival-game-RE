using System.Collections.Specialized;
using System.IO;

namespace System.Net.Cache;

internal abstract class RequestCache
{
	internal static readonly char[] LineSplits = new char[2] { '\r', '\n' };

	private bool _IsPrivateCache;

	private bool _CanWrite;

	internal bool IsPrivateCache => _IsPrivateCache;

	internal bool CanWrite => _CanWrite;

	protected RequestCache(bool isPrivateCache, bool canWrite)
	{
		_IsPrivateCache = isPrivateCache;
		_CanWrite = canWrite;
	}

	internal abstract Stream Retrieve(string key, out RequestCacheEntry cacheEntry);

	internal abstract Stream Store(string key, long contentLength, DateTime expiresUtc, DateTime lastModifiedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata);

	internal abstract void Remove(string key);

	internal abstract void Update(string key, DateTime expiresUtc, DateTime lastModifiedUtc, DateTime lastSynchronizedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata);

	internal abstract bool TryRetrieve(string key, out RequestCacheEntry cacheEntry, out Stream readStream);

	internal abstract bool TryStore(string key, long contentLength, DateTime expiresUtc, DateTime lastModifiedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata, out Stream writeStream);

	internal abstract bool TryRemove(string key);

	internal abstract bool TryUpdate(string key, DateTime expiresUtc, DateTime lastModifiedUtc, DateTime lastSynchronizedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata);

	internal abstract void UnlockEntry(Stream retrieveStream);
}
