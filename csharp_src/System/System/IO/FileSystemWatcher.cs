using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Enumeration;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace System.IO;

[DefaultEvent("Changed")]
public class FileSystemWatcher : Component, ISupportInitialize
{
	private sealed class RunningInstance
	{
		private struct NotifyEvent
		{
			internal int wd;

			internal uint mask;

			internal uint cookie;

			internal string name;
		}

		private sealed class WatchedDirectory
		{
			[ThreadStatic]
			private static StringBuilder t_builder;

			internal WatchedDirectory Parent;

			internal int WatchDescriptor;

			internal string Name;

			internal List<WatchedDirectory> Children;

			internal List<WatchedDirectory> InitializedChildren
			{
				get
				{
					if (Children == null)
					{
						Children = new List<WatchedDirectory>();
					}
					return Children;
				}
			}

			internal string GetPath(bool relativeToRoot, string additionalName = null)
			{
				StringBuilder stringBuilder = t_builder;
				if (stringBuilder == null)
				{
					stringBuilder = (t_builder = new StringBuilder());
				}
				stringBuilder.Clear();
				Write(stringBuilder, relativeToRoot);
				if (additionalName != null)
				{
					AppendSeparatorIfNeeded(stringBuilder);
					stringBuilder.Append(additionalName);
				}
				return stringBuilder.ToString();
			}

			private void Write(StringBuilder builder, bool relativeToRoot)
			{
				if (Parent != null)
				{
					Parent.Write(builder, relativeToRoot);
					AppendSeparatorIfNeeded(builder);
				}
				if (Parent != null || !relativeToRoot)
				{
					builder.Append(Name);
				}
			}

			private static void AppendSeparatorIfNeeded(StringBuilder builder)
			{
				if (builder.Length > 0)
				{
					char c = builder[builder.Length - 1];
					if (c != System.IO.Path.DirectorySeparatorChar && c != System.IO.Path.AltDirectorySeparatorChar)
					{
						builder.Append(System.IO.Path.DirectorySeparatorChar);
					}
				}
			}
		}

		private const int c_INotifyEventSize = 16;

		private readonly WeakReference<FileSystemWatcher> _weakWatcher;

		private readonly string _directoryPath;

		private readonly SafeFileHandle _inotifyHandle;

		private readonly byte[] _buffer;

		private int _bufferAvailable;

		private int _bufferPos;

		private readonly NotifyFilters _notifyFilters;

		private readonly global::Interop.Sys.NotifyEvents _watchFilters;

		private readonly bool _includeSubdirectories;

		private readonly CancellationToken _cancellationToken;

		private readonly Dictionary<int, WatchedDirectory> _wdToPathMap = new Dictionary<int, WatchedDirectory>();

		private const int NAME_MAX = 255;

		private object SyncObj => _wdToPathMap;

		internal RunningInstance(FileSystemWatcher watcher, SafeFileHandle inotifyHandle, string directoryPath, bool includeSubdirectories, NotifyFilters notifyFilters, CancellationToken cancellationToken)
		{
			_weakWatcher = new WeakReference<FileSystemWatcher>(watcher);
			_inotifyHandle = inotifyHandle;
			_directoryPath = directoryPath;
			_buffer = watcher.AllocateBuffer();
			_includeSubdirectories = includeSubdirectories;
			_notifyFilters = notifyFilters;
			_watchFilters = TranslateFilters(notifyFilters);
			_cancellationToken = cancellationToken;
			AddDirectoryWatchUnlocked(null, directoryPath);
		}

		internal void Start()
		{
			Task.Factory.StartNew(delegate(object obj)
			{
				((RunningInstance)obj).ProcessEvents();
			}, this, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
		}

		private void AddDirectoryWatch(WatchedDirectory parent, string directoryName)
		{
			lock (SyncObj)
			{
				if (parent == null || _wdToPathMap.Count > 0)
				{
					AddDirectoryWatchUnlocked(parent, directoryName);
				}
			}
		}

		private void AddDirectoryWatchUnlocked(WatchedDirectory parent, string directoryName)
		{
			string text = ((parent != null) ? parent.GetPath(relativeToRoot: false, directoryName) : directoryName);
			global::Interop.Sys.FileStatus output = default(global::Interop.Sys.FileStatus);
			if (global::Interop.Sys.LStat(text, out output) == 0 && ((ulong)output.Mode & 0xF000uL) == 40960)
			{
				return;
			}
			int num = global::Interop.Sys.INotifyAddWatch(_inotifyHandle, text, (uint)(_watchFilters | global::Interop.Sys.NotifyEvents.IN_DONT_FOLLOW | global::Interop.Sys.NotifyEvents.IN_EXCL_UNLINK));
			if (num == -1)
			{
				global::Interop.ErrorInfo lastErrorInfo = global::Interop.Sys.GetLastErrorInfo();
				Exception exception;
				if (lastErrorInfo.Error == global::Interop.Error.ENOSPC)
				{
					string text2 = ReadMaxUserLimit("/proc/sys/fs/inotify/max_user_watches");
					exception = new IOException((!string.IsNullOrEmpty(text2)) ? global::SR.Format("The configured user limit ({0}) on the number of inotify watches has been reached.", text2) : "The configured user limit on the number of inotify watches has been reached.", lastErrorInfo.RawErrno);
				}
				else
				{
					exception = global::Interop.GetExceptionForIoErrno(lastErrorInfo, text);
				}
				if (_weakWatcher.TryGetTarget(out var target))
				{
					target.OnError(new ErrorEventArgs(exception));
				}
				return;
			}
			bool flag = false;
			if (_wdToPathMap.TryGetValue(num, out var value))
			{
				if (value.Parent != parent)
				{
					if (value.Parent != null)
					{
						value.Parent.Children.Remove(value);
					}
					value.Parent = parent;
					parent?.InitializedChildren.Add(value);
				}
				value.Name = directoryName;
			}
			else
			{
				value = new WatchedDirectory
				{
					Parent = parent,
					WatchDescriptor = num,
					Name = directoryName
				};
				parent?.InitializedChildren.Add(value);
				_wdToPathMap.Add(num, value);
				flag = true;
			}
			if (!flag || !_includeSubdirectories)
			{
				return;
			}
			foreach (string item in Directory.EnumerateDirectories(text))
			{
				AddDirectoryWatchUnlocked(value, System.IO.Path.GetFileName(item));
			}
		}

		private void RemoveWatchedDirectory(WatchedDirectory directoryEntry, bool removeInotify = true)
		{
			lock (SyncObj)
			{
				if (directoryEntry.Parent != null)
				{
					directoryEntry.Parent.Children.Remove(directoryEntry);
				}
				RemoveWatchedDirectoryUnlocked(directoryEntry, removeInotify);
			}
		}

		private void RemoveWatchedDirectoryUnlocked(WatchedDirectory directoryEntry, bool removeInotify)
		{
			if (directoryEntry.Children != null)
			{
				foreach (WatchedDirectory child in directoryEntry.Children)
				{
					RemoveWatchedDirectoryUnlocked(child, removeInotify);
				}
				directoryEntry.Children = null;
			}
			_wdToPathMap.Remove(directoryEntry.WatchDescriptor);
			if (removeInotify)
			{
				global::Interop.Sys.INotifyRemoveWatch(_inotifyHandle, directoryEntry.WatchDescriptor);
			}
		}

		private void CancellationCallback()
		{
			lock (SyncObj)
			{
				foreach (int key in _wdToPathMap.Keys)
				{
					global::Interop.Sys.INotifyRemoveWatch(_inotifyHandle, key);
				}
				_wdToPathMap.Clear();
			}
		}

		private void ProcessEvents()
		{
			CancellationTokenRegistration cancellationTokenRegistration = _cancellationToken.Register(delegate(object obj)
			{
				((RunningInstance)obj).CancellationCallback();
			}, this);
			try
			{
				string text = null;
				WatchedDirectory watchedDirectory = null;
				uint num = 0u;
				NotifyEvent notifyEvent;
				FileSystemWatcher target;
				while (!_cancellationToken.IsCancellationRequested && TryReadEvent(out notifyEvent) && _weakWatcher.TryGetTarget(out target))
				{
					uint mask = notifyEvent.mask;
					string text2 = null;
					WatchedDirectory value = null;
					if ((mask & 0x4000) != 0)
					{
						target.NotifyInternalBufferOverflowEvent();
						if (_includeSubdirectories)
						{
							target.Restart();
						}
						break;
					}
					lock (SyncObj)
					{
						if (!_wdToPathMap.TryGetValue(notifyEvent.wd, out value))
						{
							continue;
						}
					}
					text2 = value.GetPath(relativeToRoot: true, notifyEvent.name);
					if (string.IsNullOrEmpty(text2))
					{
						target = null;
						continue;
					}
					bool flag = (mask & 0x40008000) != 0;
					if (text != null && ((mask & 0x80) == 0 || num != notifyEvent.cookie))
					{
						if (watchedDirectory != null && watchedDirectory.Children != null)
						{
							foreach (WatchedDirectory child in watchedDirectory.Children)
							{
								if (child.Name == text)
								{
									RemoveWatchedDirectory(child);
									break;
								}
							}
						}
						target.NotifyFileSystemEventArgs(WatcherChangeTypes.Deleted, text);
						text = null;
						watchedDirectory = null;
						num = 0u;
					}
					if ((mask & 0x180) != 0 && flag && _includeSubdirectories)
					{
						AddDirectoryWatch(value, notifyEvent.name);
					}
					if ((0x3C0 & mask) != 0 && ((flag && (_notifyFilters & NotifyFilters.DirectoryName) == 0) || (!flag && (_notifyFilters & NotifyFilters.FileName) == 0)))
					{
						continue;
					}
					switch ((global::Interop.Sys.NotifyEvents)(mask & 0x83C7))
					{
					case global::Interop.Sys.NotifyEvents.IN_CREATE:
						target.NotifyFileSystemEventArgs(WatcherChangeTypes.Created, text2);
						break;
					case global::Interop.Sys.NotifyEvents.IN_IGNORED:
						RemoveWatchedDirectory(value, removeInotify: false);
						break;
					case global::Interop.Sys.NotifyEvents.IN_DELETE:
						target.NotifyFileSystemEventArgs(WatcherChangeTypes.Deleted, text2);
						break;
					case global::Interop.Sys.NotifyEvents.IN_ACCESS:
					case global::Interop.Sys.NotifyEvents.IN_MODIFY:
					case global::Interop.Sys.NotifyEvents.IN_ATTRIB:
						target.NotifyFileSystemEventArgs(WatcherChangeTypes.Changed, text2);
						break;
					case global::Interop.Sys.NotifyEvents.IN_MOVED_FROM:
						if (_bufferPos == _bufferAvailable)
						{
							global::Interop.Sys.Poll(_inotifyHandle, global::Interop.Sys.PollEvents.POLLIN, 2, out var triggered);
							if (triggered == global::Interop.Sys.PollEvents.POLLNONE)
							{
								target.NotifyFileSystemEventArgs(WatcherChangeTypes.Deleted, text2);
								break;
							}
						}
						text = text2;
						watchedDirectory = (flag ? value : null);
						num = notifyEvent.cookie;
						break;
					case global::Interop.Sys.NotifyEvents.IN_MOVED_TO:
						if (text != null)
						{
							target.NotifyRenameEventArgs(WatcherChangeTypes.Renamed, text2, text);
						}
						else
						{
							target.NotifyFileSystemEventArgs(WatcherChangeTypes.Created, text2);
						}
						text = null;
						watchedDirectory = null;
						num = 0u;
						break;
					}
					target = null;
				}
			}
			catch (Exception exception)
			{
				if (_weakWatcher.TryGetTarget(out var target2))
				{
					target2.OnError(new ErrorEventArgs(exception));
				}
			}
			finally
			{
				cancellationTokenRegistration.Dispose();
				_inotifyHandle.Dispose();
			}
		}

		private unsafe bool TryReadEvent(out NotifyEvent notifyEvent)
		{
			if (_bufferAvailable == 0 || _bufferPos == _bufferAvailable)
			{
				try
				{
					fixed (byte* buffer = &_buffer[0])
					{
						_bufferAvailable = global::Interop.CheckIo(global::Interop.Sys.Read(_inotifyHandle, buffer, _buffer.Length), null, isDirectory: true);
					}
				}
				catch (ArgumentException)
				{
					_bufferAvailable = 0;
				}
				if (_bufferAvailable == 0)
				{
					notifyEvent = default(NotifyEvent);
					return false;
				}
				_bufferPos = 0;
			}
			NotifyEvent notifyEvent2 = default(NotifyEvent);
			notifyEvent2.wd = BitConverter.ToInt32(_buffer, _bufferPos);
			notifyEvent2.mask = BitConverter.ToUInt32(_buffer, _bufferPos + 4);
			notifyEvent2.cookie = BitConverter.ToUInt32(_buffer, _bufferPos + 8);
			int num = (int)BitConverter.ToUInt32(_buffer, _bufferPos + 12);
			notifyEvent2.name = ReadName(_bufferPos + 16, num);
			_bufferPos += 16 + num;
			notifyEvent = notifyEvent2;
			return true;
		}

		private string ReadName(int position, int nameLength)
		{
			int num = nameLength;
			for (int i = 0; i < nameLength; i++)
			{
				if (_buffer[position + i] == 0)
				{
					num = i;
					break;
				}
			}
			if (num <= 0)
			{
				return string.Empty;
			}
			return Encoding.UTF8.GetString(_buffer, position, num);
		}
	}

	private sealed class NormalizedFilterCollection : Collection<string>
	{
		private sealed class ImmutableStringList : IList<string>, ICollection<string>, IEnumerable<string>, IEnumerable
		{
			public string[] Items = Array.Empty<string>();

			public string this[int index]
			{
				get
				{
					string[] items = Items;
					if ((uint)index >= (uint)items.Length)
					{
						throw new ArgumentOutOfRangeException("index");
					}
					return items[index];
				}
				set
				{
					string[] array = (string[])Items.Clone();
					array[index] = value;
					Items = array;
				}
			}

			public int Count => Items.Length;

			public bool IsReadOnly => false;

			public void Add(string item)
			{
				throw new NotSupportedException();
			}

			public void Clear()
			{
				Items = Array.Empty<string>();
			}

			public bool Contains(string item)
			{
				return Array.IndexOf(Items, item) != -1;
			}

			public void CopyTo(string[] array, int arrayIndex)
			{
				Items.CopyTo(array, arrayIndex);
			}

			public IEnumerator<string> GetEnumerator()
			{
				return ((IEnumerable<string>)Items).GetEnumerator();
			}

			public int IndexOf(string item)
			{
				return Array.IndexOf(Items, item);
			}

			public void Insert(int index, string item)
			{
				string[] items = Items;
				string[] array = new string[items.Length + 1];
				items.AsSpan(0, index).CopyTo(array);
				items.AsSpan(index).CopyTo(array.AsSpan(index + 1));
				array[index] = item;
				Items = array;
			}

			public bool Remove(string item)
			{
				throw new NotSupportedException();
			}

			public void RemoveAt(int index)
			{
				string[] items = Items;
				string[] array = new string[items.Length - 1];
				items.AsSpan(0, index).CopyTo(array);
				items.AsSpan(index + 1).CopyTo(array.AsSpan(index));
				Items = array;
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

		internal NormalizedFilterCollection()
			: base((IList<string>)new ImmutableStringList())
		{
		}

		protected override void InsertItem(int index, string item)
		{
			base.InsertItem(index, (string.IsNullOrEmpty(item) || item == "*.*") ? "*" : item);
		}

		protected override void SetItem(int index, string item)
		{
			base.SetItem(index, (string.IsNullOrEmpty(item) || item == "*.*") ? "*" : item);
		}

		internal string[] GetFilters()
		{
			return ((ImmutableStringList)base.Items).Items;
		}
	}

	private const string MaxUserInstancesPath = "/proc/sys/fs/inotify/max_user_instances";

	private const string MaxUserWatchesPath = "/proc/sys/fs/inotify/max_user_watches";

	private CancellationTokenSource _cancellation;

	private readonly NormalizedFilterCollection _filters = new NormalizedFilterCollection();

	private string _directory;

	private const NotifyFilters c_defaultNotifyFilters = NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastWrite;

	private NotifyFilters _notifyFilters = NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastWrite;

	private bool _includeSubdirectories;

	private bool _enabled;

	private bool _initializing;

	private uint _internalBufferSize = 8192u;

	private bool _disposed;

	private FileSystemEventHandler _onChangedHandler;

	private FileSystemEventHandler _onCreatedHandler;

	private FileSystemEventHandler _onDeletedHandler;

	private RenamedEventHandler _onRenamedHandler;

	private ErrorEventHandler _onErrorHandler;

	private static readonly char[] s_wildcards = new char[2] { '?', '*' };

	private const int c_notifyFiltersValidMask = 383;

	public NotifyFilters NotifyFilter
	{
		get
		{
			return _notifyFilters;
		}
		set
		{
			if ((value & ~(NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size)) != 0)
			{
				throw new ArgumentException(global::SR.Format("The value of argument '{0}' ({1}) is invalid for Enum type '{2}'.", "value", (int)value, "NotifyFilters"));
			}
			if (_notifyFilters != value)
			{
				_notifyFilters = value;
				Restart();
			}
		}
	}

	public Collection<string> Filters => _filters;

	public bool EnableRaisingEvents
	{
		get
		{
			return _enabled;
		}
		set
		{
			if (_enabled != value)
			{
				if (IsSuspended())
				{
					_enabled = value;
				}
				else if (value)
				{
					StartRaisingEventsIfNotDisposed();
				}
				else
				{
					StopRaisingEvents();
				}
			}
		}
	}

	public string Filter
	{
		get
		{
			if (Filters.Count != 0)
			{
				return Filters[0];
			}
			return "*";
		}
		set
		{
			Filters.Clear();
			Filters.Add(value);
		}
	}

	public bool IncludeSubdirectories
	{
		get
		{
			return _includeSubdirectories;
		}
		set
		{
			if (_includeSubdirectories != value)
			{
				_includeSubdirectories = value;
				Restart();
			}
		}
	}

	public int InternalBufferSize
	{
		get
		{
			return (int)_internalBufferSize;
		}
		set
		{
			if (_internalBufferSize != value)
			{
				if (value < 4096)
				{
					_internalBufferSize = 4096u;
				}
				else
				{
					_internalBufferSize = (uint)value;
				}
				Restart();
			}
		}
	}

	public string Path
	{
		get
		{
			return _directory;
		}
		set
		{
			value = ((value == null) ? string.Empty : value);
			if (!string.Equals(_directory, value, PathInternal.StringComparison))
			{
				if (value.Length == 0)
				{
					throw new ArgumentException(global::SR.Format("The directory name {0} is invalid.", value), "Path");
				}
				if (!Directory.Exists(value))
				{
					throw new ArgumentException(global::SR.Format("The directory name '{0}' does not exist.", value), "Path");
				}
				_directory = value;
				Restart();
			}
		}
	}

	public override ISite Site
	{
		get
		{
			return base.Site;
		}
		set
		{
			base.Site = value;
			if (Site != null && Site.DesignMode)
			{
				EnableRaisingEvents = true;
			}
		}
	}

	public ISynchronizeInvoke SynchronizingObject { get; set; }

	public event FileSystemEventHandler Changed
	{
		add
		{
			_onChangedHandler = (FileSystemEventHandler)Delegate.Combine(_onChangedHandler, value);
		}
		remove
		{
			_onChangedHandler = (FileSystemEventHandler)Delegate.Remove(_onChangedHandler, value);
		}
	}

	public event FileSystemEventHandler Created
	{
		add
		{
			_onCreatedHandler = (FileSystemEventHandler)Delegate.Combine(_onCreatedHandler, value);
		}
		remove
		{
			_onCreatedHandler = (FileSystemEventHandler)Delegate.Remove(_onCreatedHandler, value);
		}
	}

	public event FileSystemEventHandler Deleted
	{
		add
		{
			_onDeletedHandler = (FileSystemEventHandler)Delegate.Combine(_onDeletedHandler, value);
		}
		remove
		{
			_onDeletedHandler = (FileSystemEventHandler)Delegate.Remove(_onDeletedHandler, value);
		}
	}

	public event ErrorEventHandler Error
	{
		add
		{
			_onErrorHandler = (ErrorEventHandler)Delegate.Combine(_onErrorHandler, value);
		}
		remove
		{
			_onErrorHandler = (ErrorEventHandler)Delegate.Remove(_onErrorHandler, value);
		}
	}

	public event RenamedEventHandler Renamed
	{
		add
		{
			_onRenamedHandler = (RenamedEventHandler)Delegate.Combine(_onRenamedHandler, value);
		}
		remove
		{
			_onRenamedHandler = (RenamedEventHandler)Delegate.Remove(_onRenamedHandler, value);
		}
	}

	private void StartRaisingEvents()
	{
		if (IsSuspended())
		{
			_enabled = true;
		}
		else
		{
			if (_cancellation != null)
			{
				return;
			}
			SafeFileHandle safeFileHandle;
			try
			{
			}
			finally
			{
				safeFileHandle = global::Interop.Sys.INotifyInit();
				if (safeFileHandle.IsInvalid)
				{
					global::Interop.ErrorInfo lastErrorInfo = global::Interop.Sys.GetLastErrorInfo();
					switch (lastErrorInfo.Error)
					{
					case global::Interop.Error.EMFILE:
					{
						string text = ReadMaxUserLimit("/proc/sys/fs/inotify/max_user_instances");
						throw new IOException((!string.IsNullOrEmpty(text)) ? global::SR.Format("The configured user limit ({0}) on the number of inotify instances has been reached.", text) : "The configured user limit on the number of inotify instances has been reached.", lastErrorInfo.RawErrno);
					}
					case global::Interop.Error.ENFILE:
						throw new IOException("The system limit on the number of inotify instances has been reached.", lastErrorInfo.RawErrno);
					default:
						throw global::Interop.GetExceptionForIoErrno(lastErrorInfo);
					}
				}
			}
			try
			{
				CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
				RunningInstance runningInstance = new RunningInstance(this, safeFileHandle, _directory, IncludeSubdirectories, NotifyFilter, cancellationTokenSource.Token);
				_cancellation = cancellationTokenSource;
				_enabled = true;
				runningInstance.Start();
			}
			catch
			{
				safeFileHandle.Dispose();
				throw;
			}
		}
	}

	private void StopRaisingEvents()
	{
		_enabled = false;
		if (!IsSuspended())
		{
			CancellationTokenSource cancellation = _cancellation;
			if (cancellation != null)
			{
				_cancellation = null;
				cancellation.Cancel();
			}
		}
	}

	private void FinalizeDispose()
	{
		StopRaisingEvents();
	}

	private static string ReadMaxUserLimit(string path)
	{
		try
		{
			return File.ReadAllText(path).Trim();
		}
		catch
		{
			return null;
		}
	}

	private static global::Interop.Sys.NotifyEvents TranslateFilters(NotifyFilters filters)
	{
		global::Interop.Sys.NotifyEvents notifyEvents = (global::Interop.Sys.NotifyEvents)0;
		notifyEvents |= global::Interop.Sys.NotifyEvents.IN_ONLYDIR | global::Interop.Sys.NotifyEvents.IN_EXCL_UNLINK;
		notifyEvents |= global::Interop.Sys.NotifyEvents.IN_CREATE | global::Interop.Sys.NotifyEvents.IN_DELETE;
		if ((filters & NotifyFilters.LastAccess) != 0)
		{
			notifyEvents |= global::Interop.Sys.NotifyEvents.IN_ACCESS;
		}
		if ((filters & (NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size)) != 0)
		{
			notifyEvents |= global::Interop.Sys.NotifyEvents.IN_MODIFY;
		}
		if ((filters & (NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size)) != 0)
		{
			notifyEvents |= global::Interop.Sys.NotifyEvents.IN_ATTRIB;
		}
		if ((filters & (NotifyFilters.DirectoryName | NotifyFilters.FileName)) != 0)
		{
			notifyEvents |= global::Interop.Sys.NotifyEvents.IN_MOVED_FROM | global::Interop.Sys.NotifyEvents.IN_MOVED_TO;
		}
		return notifyEvents;
	}

	public FileSystemWatcher()
	{
		_directory = string.Empty;
	}

	public FileSystemWatcher(string path)
	{
		CheckPathValidity(path);
		_directory = path;
	}

	public FileSystemWatcher(string path, string filter)
	{
		CheckPathValidity(path);
		_directory = path;
		Filter = filter ?? throw new ArgumentNullException("filter");
	}

	private byte[] AllocateBuffer()
	{
		try
		{
			return new byte[_internalBufferSize];
		}
		catch (OutOfMemoryException)
		{
			throw new OutOfMemoryException(global::SR.Format("The specified buffer size is too large. FileSystemWatcher cannot allocate {0} bytes for the internal buffer.", _internalBufferSize));
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (disposing)
			{
				StopRaisingEvents();
				_onChangedHandler = null;
				_onCreatedHandler = null;
				_onDeletedHandler = null;
				_onRenamedHandler = null;
				_onErrorHandler = null;
			}
			else
			{
				FinalizeDispose();
			}
		}
		finally
		{
			_disposed = true;
			base.Dispose(disposing);
		}
	}

	private static void CheckPathValidity(string path)
	{
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (path.Length == 0)
		{
			throw new ArgumentException(global::SR.Format("The directory name {0} is invalid.", path), "path");
		}
		if (!Directory.Exists(path))
		{
			throw new ArgumentException(global::SR.Format("The directory name '{0}' does not exist.", path), "path");
		}
	}

	private bool MatchPattern(ReadOnlySpan<char> relativePath)
	{
		if (relativePath.IsWhiteSpace())
		{
			return false;
		}
		ReadOnlySpan<char> fileName = System.IO.Path.GetFileName(relativePath);
		if (fileName.Length == 0)
		{
			return false;
		}
		string[] filters = _filters.GetFilters();
		if (filters.Length == 0)
		{
			return true;
		}
		string[] array = filters;
		for (int i = 0; i < array.Length; i++)
		{
			if (FileSystemName.MatchesSimpleExpression(array[i], fileName, !PathInternal.IsCaseSensitive))
			{
				return true;
			}
		}
		return false;
	}

	private void NotifyInternalBufferOverflowEvent()
	{
		_onErrorHandler?.Invoke(this, new ErrorEventArgs(new InternalBufferOverflowException(global::SR.Format("Too many changes at once in directory:{0}.", _directory))));
	}

	private void NotifyRenameEventArgs(WatcherChangeTypes action, ReadOnlySpan<char> name, ReadOnlySpan<char> oldName)
	{
		RenamedEventHandler onRenamedHandler = _onRenamedHandler;
		if (onRenamedHandler != null && (MatchPattern(name) || MatchPattern(oldName)))
		{
			onRenamedHandler(this, new RenamedEventArgs(action, _directory, name.IsEmpty ? null : name.ToString(), oldName.IsEmpty ? null : oldName.ToString()));
		}
	}

	private FileSystemEventHandler GetHandler(WatcherChangeTypes changeType)
	{
		return changeType switch
		{
			WatcherChangeTypes.Created => _onCreatedHandler, 
			WatcherChangeTypes.Deleted => _onDeletedHandler, 
			WatcherChangeTypes.Changed => _onChangedHandler, 
			_ => null, 
		};
	}

	private void NotifyFileSystemEventArgs(WatcherChangeTypes changeType, ReadOnlySpan<char> name)
	{
		FileSystemEventHandler handler = GetHandler(changeType);
		if (handler != null && MatchPattern(name.IsEmpty ? ((ReadOnlySpan<char>)_directory) : name))
		{
			handler(this, new FileSystemEventArgs(changeType, _directory, name.IsEmpty ? null : name.ToString()));
		}
	}

	private void NotifyFileSystemEventArgs(WatcherChangeTypes changeType, string name)
	{
		FileSystemEventHandler handler = GetHandler(changeType);
		if (handler != null && MatchPattern(string.IsNullOrEmpty(name) ? _directory : name))
		{
			handler(this, new FileSystemEventArgs(changeType, _directory, name));
		}
	}

	protected void OnChanged(FileSystemEventArgs e)
	{
		InvokeOn(e, _onChangedHandler);
	}

	protected void OnCreated(FileSystemEventArgs e)
	{
		InvokeOn(e, _onCreatedHandler);
	}

	protected void OnDeleted(FileSystemEventArgs e)
	{
		InvokeOn(e, _onDeletedHandler);
	}

	private void InvokeOn(FileSystemEventArgs e, FileSystemEventHandler handler)
	{
		if (handler != null)
		{
			ISynchronizeInvoke synchronizingObject = SynchronizingObject;
			if (synchronizingObject != null && synchronizingObject.InvokeRequired)
			{
				synchronizingObject.BeginInvoke(handler, new object[2] { this, e });
			}
			else
			{
				handler(this, e);
			}
		}
	}

	protected void OnError(ErrorEventArgs e)
	{
		ErrorEventHandler onErrorHandler = _onErrorHandler;
		if (onErrorHandler != null)
		{
			ISynchronizeInvoke synchronizingObject = SynchronizingObject;
			if (synchronizingObject != null && synchronizingObject.InvokeRequired)
			{
				synchronizingObject.BeginInvoke(onErrorHandler, new object[2] { this, e });
			}
			else
			{
				onErrorHandler(this, e);
			}
		}
	}

	protected void OnRenamed(RenamedEventArgs e)
	{
		RenamedEventHandler onRenamedHandler = _onRenamedHandler;
		if (onRenamedHandler != null)
		{
			ISynchronizeInvoke synchronizingObject = SynchronizingObject;
			if (synchronizingObject != null && synchronizingObject.InvokeRequired)
			{
				synchronizingObject.BeginInvoke(onRenamedHandler, new object[2] { this, e });
			}
			else
			{
				onRenamedHandler(this, e);
			}
		}
	}

	public WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType)
	{
		return WaitForChanged(changeType, -1);
	}

	public WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, int timeout)
	{
		TaskCompletionSource<WaitForChangedResult> tcs = new TaskCompletionSource<WaitForChangedResult>();
		FileSystemEventHandler fileSystemEventHandler = null;
		RenamedEventHandler renamedEventHandler = null;
		if ((changeType & (WatcherChangeTypes.Changed | WatcherChangeTypes.Created | WatcherChangeTypes.Deleted)) != 0)
		{
			fileSystemEventHandler = delegate(object s, FileSystemEventArgs e)
			{
				if ((e.ChangeType & changeType) != 0)
				{
					tcs.TrySetResult(new WaitForChangedResult(e.ChangeType, e.Name, null, timedOut: false));
				}
			};
			if ((changeType & WatcherChangeTypes.Created) != 0)
			{
				Created += fileSystemEventHandler;
			}
			if ((changeType & WatcherChangeTypes.Deleted) != 0)
			{
				Deleted += fileSystemEventHandler;
			}
			if ((changeType & WatcherChangeTypes.Changed) != 0)
			{
				Changed += fileSystemEventHandler;
			}
		}
		if ((changeType & WatcherChangeTypes.Renamed) != 0)
		{
			renamedEventHandler = delegate(object s, RenamedEventArgs e)
			{
				if ((e.ChangeType & changeType) != 0)
				{
					tcs.TrySetResult(new WaitForChangedResult(e.ChangeType, e.Name, e.OldName, timedOut: false));
				}
			};
			Renamed += renamedEventHandler;
		}
		try
		{
			bool enableRaisingEvents = EnableRaisingEvents;
			if (!enableRaisingEvents)
			{
				EnableRaisingEvents = true;
			}
			tcs.Task.Wait(timeout);
			EnableRaisingEvents = enableRaisingEvents;
		}
		finally
		{
			if (renamedEventHandler != null)
			{
				Renamed -= renamedEventHandler;
			}
			if (fileSystemEventHandler != null)
			{
				if ((changeType & WatcherChangeTypes.Changed) != 0)
				{
					Changed -= fileSystemEventHandler;
				}
				if ((changeType & WatcherChangeTypes.Deleted) != 0)
				{
					Deleted -= fileSystemEventHandler;
				}
				if ((changeType & WatcherChangeTypes.Created) != 0)
				{
					Created -= fileSystemEventHandler;
				}
			}
		}
		if (tcs.Task.Status != TaskStatus.RanToCompletion)
		{
			return WaitForChangedResult.TimedOutResult;
		}
		return tcs.Task.Result;
	}

	private void Restart()
	{
		if (!IsSuspended() && _enabled)
		{
			StopRaisingEvents();
			StartRaisingEventsIfNotDisposed();
		}
	}

	private void StartRaisingEventsIfNotDisposed()
	{
		if (_disposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		StartRaisingEvents();
	}

	public void BeginInit()
	{
		bool enabled = _enabled;
		StopRaisingEvents();
		_enabled = enabled;
		_initializing = true;
	}

	public void EndInit()
	{
		_initializing = false;
		if (_directory.Length != 0 && _enabled)
		{
			StartRaisingEvents();
		}
	}

	private bool IsSuspended()
	{
		if (!_initializing)
		{
			return base.DesignMode;
		}
		return true;
	}
}
