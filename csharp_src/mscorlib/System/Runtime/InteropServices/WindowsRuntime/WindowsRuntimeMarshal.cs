using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Threading;

namespace System.Runtime.InteropServices.WindowsRuntime;

public static class WindowsRuntimeMarshal
{
	internal struct EventRegistrationTokenList
	{
		private EventRegistrationToken firstToken;

		private List<EventRegistrationToken> restTokens;

		internal EventRegistrationTokenList(EventRegistrationToken token)
		{
			firstToken = token;
			restTokens = null;
		}

		internal EventRegistrationTokenList(EventRegistrationTokenList list)
		{
			firstToken = list.firstToken;
			restTokens = list.restTokens;
		}

		public bool Push(EventRegistrationToken token)
		{
			bool result = false;
			if (restTokens == null)
			{
				restTokens = new List<EventRegistrationToken>();
				result = true;
			}
			restTokens.Add(token);
			return result;
		}

		public bool Pop(out EventRegistrationToken token)
		{
			if (restTokens == null || restTokens.Count == 0)
			{
				token = firstToken;
				return false;
			}
			int index = restTokens.Count - 1;
			token = restTokens[index];
			restTokens.RemoveAt(index);
			return true;
		}

		public void CopyTo(List<EventRegistrationToken> tokens)
		{
			tokens.Add(firstToken);
			if (restTokens != null)
			{
				tokens.AddRange(restTokens);
			}
		}
	}

	internal static class ManagedEventRegistrationImpl
	{
		internal static volatile ConditionalWeakTable<object, Dictionary<MethodInfo, Dictionary<object, EventRegistrationTokenList>>> s_eventRegistrations = new ConditionalWeakTable<object, Dictionary<MethodInfo, Dictionary<object, EventRegistrationTokenList>>>();

		[SecurityCritical]
		internal static void AddEventHandler<T>(Func<T, EventRegistrationToken> addMethod, Action<EventRegistrationToken> removeMethod, T handler)
		{
			Dictionary<object, EventRegistrationTokenList> eventRegistrationTokenTable = GetEventRegistrationTokenTable(removeMethod.Target, removeMethod);
			EventRegistrationToken token = addMethod(handler);
			lock (eventRegistrationTokenTable)
			{
				if (!eventRegistrationTokenTable.TryGetValue(handler, out var value))
				{
					value = new EventRegistrationTokenList(token);
					eventRegistrationTokenTable[handler] = value;
				}
				else if (value.Push(token))
				{
					eventRegistrationTokenTable[handler] = value;
				}
			}
		}

		private static Dictionary<object, EventRegistrationTokenList> GetEventRegistrationTokenTable(object instance, Action<EventRegistrationToken> removeMethod)
		{
			lock (s_eventRegistrations)
			{
				Dictionary<MethodInfo, Dictionary<object, EventRegistrationTokenList>> value = null;
				if (!s_eventRegistrations.TryGetValue(instance, out value))
				{
					value = new Dictionary<MethodInfo, Dictionary<object, EventRegistrationTokenList>>();
					s_eventRegistrations.Add(instance, value);
				}
				Dictionary<object, EventRegistrationTokenList> value2 = null;
				if (!value.TryGetValue(removeMethod.Method, out value2))
				{
					value2 = new Dictionary<object, EventRegistrationTokenList>();
					value.Add(removeMethod.Method, value2);
				}
				return value2;
			}
		}

		[SecurityCritical]
		internal static void RemoveEventHandler<T>(Action<EventRegistrationToken> removeMethod, T handler)
		{
			Dictionary<object, EventRegistrationTokenList> eventRegistrationTokenTable = GetEventRegistrationTokenTable(removeMethod.Target, removeMethod);
			EventRegistrationToken token;
			lock (eventRegistrationTokenTable)
			{
				if (!eventRegistrationTokenTable.TryGetValue(handler, out var value))
				{
					return;
				}
				if (!value.Pop(out token))
				{
					eventRegistrationTokenTable.Remove(handler);
				}
			}
			removeMethod(token);
		}

		[SecurityCritical]
		internal static void RemoveAllEventHandlers(Action<EventRegistrationToken> removeMethod)
		{
			Dictionary<object, EventRegistrationTokenList> eventRegistrationTokenTable = GetEventRegistrationTokenTable(removeMethod.Target, removeMethod);
			List<EventRegistrationToken> list = new List<EventRegistrationToken>();
			lock (eventRegistrationTokenTable)
			{
				foreach (EventRegistrationTokenList value in eventRegistrationTokenTable.Values)
				{
					value.CopyTo(list);
				}
				eventRegistrationTokenTable.Clear();
			}
			CallRemoveMethods(removeMethod, list);
		}
	}

	internal static class NativeOrStaticEventRegistrationImpl
	{
		internal struct EventCacheKey
		{
			internal object target;

			internal MethodInfo method;

			public override string ToString()
			{
				return "(" + target?.ToString() + ", " + method?.ToString() + ")";
			}
		}

		internal class EventCacheKeyEqualityComparer : IEqualityComparer<EventCacheKey>
		{
			public bool Equals(EventCacheKey lhs, EventCacheKey rhs)
			{
				if (object.Equals(lhs.target, rhs.target))
				{
					return object.Equals(lhs.method, rhs.method);
				}
				return false;
			}

			public int GetHashCode(EventCacheKey key)
			{
				return key.target.GetHashCode() ^ key.method.GetHashCode();
			}
		}

		internal class EventRegistrationTokenListWithCount
		{
			private TokenListCount _tokenListCount;

			private EventRegistrationTokenList _tokenList;

			internal EventRegistrationTokenListWithCount(TokenListCount tokenListCount, EventRegistrationToken token)
			{
				_tokenListCount = tokenListCount;
				_tokenListCount.Inc();
				_tokenList = new EventRegistrationTokenList(token);
			}

			~EventRegistrationTokenListWithCount()
			{
				_tokenListCount.Dec();
			}

			public void Push(EventRegistrationToken token)
			{
				_tokenList.Push(token);
			}

			public bool Pop(out EventRegistrationToken token)
			{
				return _tokenList.Pop(out token);
			}

			public void CopyTo(List<EventRegistrationToken> tokens)
			{
				_tokenList.CopyTo(tokens);
			}
		}

		internal class TokenListCount
		{
			private int _count;

			private EventCacheKey _key;

			internal EventCacheKey Key => _key;

			internal TokenListCount(EventCacheKey key)
			{
				_key = key;
			}

			internal void Inc()
			{
				Interlocked.Increment(ref _count);
			}

			internal void Dec()
			{
				s_eventCacheRWLock.AcquireWriterLock(-1);
				try
				{
					if (Interlocked.Decrement(ref _count) == 0)
					{
						CleanupCache();
					}
				}
				finally
				{
					s_eventCacheRWLock.ReleaseWriterLock();
				}
			}

			private void CleanupCache()
			{
				s_eventRegistrations.Remove(_key);
			}
		}

		internal struct EventCacheEntry
		{
			internal ConditionalWeakTable<object, EventRegistrationTokenListWithCount> registrationTable;

			internal TokenListCount tokenListCount;
		}

		internal class ReaderWriterLockTimedOutException : ApplicationException
		{
		}

		internal class MyReaderWriterLock
		{
			private int myLock;

			private int owners;

			private uint numWriteWaiters;

			private uint numReadWaiters;

			private EventWaitHandle writeEvent;

			private EventWaitHandle readEvent;

			internal MyReaderWriterLock()
			{
			}

			internal void AcquireReaderLock(int millisecondsTimeout)
			{
				EnterMyLock();
				while (owners < 0 || numWriteWaiters != 0)
				{
					if (readEvent == null)
					{
						LazyCreateEvent(ref readEvent, makeAutoResetEvent: false);
					}
					else
					{
						WaitOnEvent(readEvent, ref numReadWaiters, millisecondsTimeout);
					}
				}
				owners++;
				ExitMyLock();
			}

			internal void AcquireWriterLock(int millisecondsTimeout)
			{
				EnterMyLock();
				while (owners != 0)
				{
					if (writeEvent == null)
					{
						LazyCreateEvent(ref writeEvent, makeAutoResetEvent: true);
					}
					else
					{
						WaitOnEvent(writeEvent, ref numWriteWaiters, millisecondsTimeout);
					}
				}
				owners = -1;
				ExitMyLock();
			}

			internal void ReleaseReaderLock()
			{
				EnterMyLock();
				owners--;
				ExitAndWakeUpAppropriateWaiters();
			}

			internal void ReleaseWriterLock()
			{
				EnterMyLock();
				owners++;
				ExitAndWakeUpAppropriateWaiters();
			}

			private void LazyCreateEvent(ref EventWaitHandle waitEvent, bool makeAutoResetEvent)
			{
				ExitMyLock();
				EventWaitHandle eventWaitHandle = ((!makeAutoResetEvent) ? ((EventWaitHandle)new ManualResetEvent(initialState: false)) : ((EventWaitHandle)new AutoResetEvent(initialState: false)));
				EnterMyLock();
				if (waitEvent == null)
				{
					waitEvent = eventWaitHandle;
				}
			}

			private void WaitOnEvent(EventWaitHandle waitEvent, ref uint numWaiters, int millisecondsTimeout)
			{
				waitEvent.Reset();
				numWaiters++;
				bool flag = false;
				ExitMyLock();
				try
				{
					if (!waitEvent.WaitOne(millisecondsTimeout, exitContext: false))
					{
						throw new ReaderWriterLockTimedOutException();
					}
					flag = true;
				}
				finally
				{
					EnterMyLock();
					numWaiters--;
					if (!flag)
					{
						ExitMyLock();
					}
				}
			}

			private void ExitAndWakeUpAppropriateWaiters()
			{
				if (owners == 0 && numWriteWaiters != 0)
				{
					ExitMyLock();
					writeEvent.Set();
				}
				else if (owners >= 0 && numReadWaiters != 0)
				{
					ExitMyLock();
					readEvent.Set();
				}
				else
				{
					ExitMyLock();
				}
			}

			private void EnterMyLock()
			{
				if (Interlocked.CompareExchange(ref myLock, 1, 0) != 0)
				{
					EnterMyLockSpin();
				}
			}

			private void EnterMyLockSpin()
			{
				int num = 0;
				while (true)
				{
					if (num < 3 && Environment.ProcessorCount > 1)
					{
						Thread.SpinWait(20);
					}
					else
					{
						Thread.Sleep(0);
					}
					if (Interlocked.CompareExchange(ref myLock, 1, 0) == 0)
					{
						break;
					}
					num++;
				}
			}

			private void ExitMyLock()
			{
				myLock = 0;
			}
		}

		internal static volatile Dictionary<EventCacheKey, EventCacheEntry> s_eventRegistrations = new Dictionary<EventCacheKey, EventCacheEntry>(new EventCacheKeyEqualityComparer());

		private static volatile MyReaderWriterLock s_eventCacheRWLock = new MyReaderWriterLock();

		[SecuritySafeCritical]
		private static object GetInstanceKey(Action<EventRegistrationToken> removeMethod)
		{
			object target = removeMethod.Target;
			if (target == null)
			{
				return removeMethod.Method.DeclaringType;
			}
			return Marshal.GetRawIUnknownForComObjectNoAddRef(target);
		}

		[SecurityCritical]
		internal static void AddEventHandler<T>(Func<T, EventRegistrationToken> addMethod, Action<EventRegistrationToken> removeMethod, T handler)
		{
			object instanceKey = GetInstanceKey(removeMethod);
			EventRegistrationToken eventRegistrationToken = addMethod(handler);
			bool flag = false;
			try
			{
				s_eventCacheRWLock.AcquireReaderLock(-1);
				try
				{
					TokenListCount tokenListCount;
					ConditionalWeakTable<object, EventRegistrationTokenListWithCount> orCreateEventRegistrationTokenTable = GetOrCreateEventRegistrationTokenTable(instanceKey, removeMethod, out tokenListCount);
					lock (orCreateEventRegistrationTokenTable)
					{
						if (orCreateEventRegistrationTokenTable.FindEquivalentKeyUnsafe(handler, out var value) == null)
						{
							value = new EventRegistrationTokenListWithCount(tokenListCount, eventRegistrationToken);
							orCreateEventRegistrationTokenTable.Add(handler, value);
						}
						else
						{
							value.Push(eventRegistrationToken);
						}
						flag = true;
					}
				}
				finally
				{
					s_eventCacheRWLock.ReleaseReaderLock();
				}
			}
			catch (Exception)
			{
				if (!flag)
				{
					removeMethod(eventRegistrationToken);
				}
				throw;
			}
		}

		private static ConditionalWeakTable<object, EventRegistrationTokenListWithCount> GetEventRegistrationTokenTableNoCreate(object instance, Action<EventRegistrationToken> removeMethod, out TokenListCount tokenListCount)
		{
			return GetEventRegistrationTokenTableInternal(instance, removeMethod, out tokenListCount, createIfNotFound: false);
		}

		private static ConditionalWeakTable<object, EventRegistrationTokenListWithCount> GetOrCreateEventRegistrationTokenTable(object instance, Action<EventRegistrationToken> removeMethod, out TokenListCount tokenListCount)
		{
			return GetEventRegistrationTokenTableInternal(instance, removeMethod, out tokenListCount, createIfNotFound: true);
		}

		private static ConditionalWeakTable<object, EventRegistrationTokenListWithCount> GetEventRegistrationTokenTableInternal(object instance, Action<EventRegistrationToken> removeMethod, out TokenListCount tokenListCount, bool createIfNotFound)
		{
			EventCacheKey key = default(EventCacheKey);
			key.target = instance;
			key.method = removeMethod.Method;
			lock (s_eventRegistrations)
			{
				if (!s_eventRegistrations.TryGetValue(key, out var value))
				{
					if (!createIfNotFound)
					{
						tokenListCount = null;
						return null;
					}
					value = new EventCacheEntry
					{
						registrationTable = new ConditionalWeakTable<object, EventRegistrationTokenListWithCount>(),
						tokenListCount = new TokenListCount(key)
					};
					s_eventRegistrations.Add(key, value);
				}
				tokenListCount = value.tokenListCount;
				return value.registrationTable;
			}
		}

		[SecurityCritical]
		internal static void RemoveEventHandler<T>(Action<EventRegistrationToken> removeMethod, T handler)
		{
			object instanceKey = GetInstanceKey(removeMethod);
			s_eventCacheRWLock.AcquireReaderLock(-1);
			EventRegistrationToken token;
			try
			{
				TokenListCount tokenListCount;
				ConditionalWeakTable<object, EventRegistrationTokenListWithCount> eventRegistrationTokenTableNoCreate = GetEventRegistrationTokenTableNoCreate(instanceKey, removeMethod, out tokenListCount);
				if (eventRegistrationTokenTableNoCreate == null)
				{
					return;
				}
				lock (eventRegistrationTokenTableNoCreate)
				{
					EventRegistrationTokenListWithCount value;
					object key = eventRegistrationTokenTableNoCreate.FindEquivalentKeyUnsafe(handler, out value);
					if (value == null)
					{
						return;
					}
					if (!value.Pop(out token))
					{
						eventRegistrationTokenTableNoCreate.Remove(key);
					}
				}
			}
			finally
			{
				s_eventCacheRWLock.ReleaseReaderLock();
			}
			removeMethod(token);
		}

		[SecurityCritical]
		internal static void RemoveAllEventHandlers(Action<EventRegistrationToken> removeMethod)
		{
			object instanceKey = GetInstanceKey(removeMethod);
			List<EventRegistrationToken> list = new List<EventRegistrationToken>();
			s_eventCacheRWLock.AcquireReaderLock(-1);
			try
			{
				TokenListCount tokenListCount;
				ConditionalWeakTable<object, EventRegistrationTokenListWithCount> eventRegistrationTokenTableNoCreate = GetEventRegistrationTokenTableNoCreate(instanceKey, removeMethod, out tokenListCount);
				if (eventRegistrationTokenTableNoCreate == null)
				{
					return;
				}
				lock (eventRegistrationTokenTableNoCreate)
				{
					foreach (EventRegistrationTokenListWithCount value in eventRegistrationTokenTableNoCreate.Values)
					{
						value.CopyTo(list);
					}
					eventRegistrationTokenTableNoCreate.Clear();
				}
			}
			finally
			{
				s_eventCacheRWLock.ReleaseReaderLock();
			}
			CallRemoveMethods(removeMethod, list);
		}
	}

	private static bool s_haveBlueErrorApis = true;

	private static Guid s_iidIErrorInfo = new Guid(485667104, 21629, 4123, 142, 101, 8, 0, 43, 43, 209, 25);

	[SecurityCritical]
	public static void AddEventHandler<T>(Func<T, EventRegistrationToken> addMethod, Action<EventRegistrationToken> removeMethod, T handler)
	{
		if (addMethod == null)
		{
			throw new ArgumentNullException("addMethod");
		}
		if (removeMethod == null)
		{
			throw new ArgumentNullException("removeMethod");
		}
		if (handler != null)
		{
			object target = removeMethod.Target;
			if (target == null || Marshal.IsComObject(target))
			{
				NativeOrStaticEventRegistrationImpl.AddEventHandler(addMethod, removeMethod, handler);
			}
			else
			{
				ManagedEventRegistrationImpl.AddEventHandler(addMethod, removeMethod, handler);
			}
		}
	}

	[SecurityCritical]
	public static void RemoveEventHandler<T>(Action<EventRegistrationToken> removeMethod, T handler)
	{
		if (removeMethod == null)
		{
			throw new ArgumentNullException("removeMethod");
		}
		if (handler != null)
		{
			object target = removeMethod.Target;
			if (target == null || Marshal.IsComObject(target))
			{
				NativeOrStaticEventRegistrationImpl.RemoveEventHandler(removeMethod, handler);
			}
			else
			{
				ManagedEventRegistrationImpl.RemoveEventHandler(removeMethod, handler);
			}
		}
	}

	[SecurityCritical]
	public static void RemoveAllEventHandlers(Action<EventRegistrationToken> removeMethod)
	{
		if (removeMethod == null)
		{
			throw new ArgumentNullException("removeMethod");
		}
		object target = removeMethod.Target;
		if (target == null || Marshal.IsComObject(target))
		{
			NativeOrStaticEventRegistrationImpl.RemoveAllEventHandlers(removeMethod);
		}
		else
		{
			ManagedEventRegistrationImpl.RemoveAllEventHandlers(removeMethod);
		}
	}

	internal static int GetRegistrationTokenCacheSize()
	{
		int num = 0;
		if (ManagedEventRegistrationImpl.s_eventRegistrations != null)
		{
			lock (ManagedEventRegistrationImpl.s_eventRegistrations)
			{
				num += ManagedEventRegistrationImpl.s_eventRegistrations.Keys.Count;
			}
		}
		if (NativeOrStaticEventRegistrationImpl.s_eventRegistrations != null)
		{
			lock (NativeOrStaticEventRegistrationImpl.s_eventRegistrations)
			{
				num += NativeOrStaticEventRegistrationImpl.s_eventRegistrations.Count;
			}
		}
		return num;
	}

	internal static void CallRemoveMethods(Action<EventRegistrationToken> removeMethod, List<EventRegistrationToken> tokensToRemove)
	{
		List<Exception> list = new List<Exception>();
		foreach (EventRegistrationToken item2 in tokensToRemove)
		{
			try
			{
				removeMethod(item2);
			}
			catch (Exception item)
			{
				list.Add(item);
			}
		}
		if (list.Count > 0)
		{
			throw new AggregateException(list.ToArray());
		}
	}

	[SecurityCritical]
	internal unsafe static string HStringToString(IntPtr hstring)
	{
		if (hstring == IntPtr.Zero)
		{
			return string.Empty;
		}
		uint num = default(uint);
		return new string(UnsafeNativeMethods.WindowsGetStringRawBuffer(hstring, &num), 0, checked((int)num));
	}

	internal static Exception GetExceptionForHR(int hresult, Exception innerException, string messageResource)
	{
		Exception ex = null;
		if (innerException != null)
		{
			string text = innerException.Message;
			if (text == null && messageResource != null)
			{
				text = Environment.GetResourceString(messageResource);
			}
			ex = new Exception(text, innerException);
		}
		else
		{
			ex = new Exception((messageResource != null) ? Environment.GetResourceString(messageResource) : null);
		}
		ex.SetErrorCode(hresult);
		return ex;
	}

	internal static Exception GetExceptionForHR(int hresult, Exception innerException)
	{
		return GetExceptionForHR(hresult, innerException, null);
	}

	[SecurityCritical]
	private static bool RoOriginateLanguageException(int error, string message, IntPtr languageException)
	{
		if (s_haveBlueErrorApis)
		{
			try
			{
				return UnsafeNativeMethods.RoOriginateLanguageException(error, message, languageException);
			}
			catch (EntryPointNotFoundException)
			{
				s_haveBlueErrorApis = false;
			}
		}
		return false;
	}

	[SecurityCritical]
	private static void RoReportUnhandledError(IRestrictedErrorInfo error)
	{
		if (s_haveBlueErrorApis)
		{
			try
			{
				UnsafeNativeMethods.RoReportUnhandledError(error);
			}
			catch (EntryPointNotFoundException)
			{
				s_haveBlueErrorApis = false;
			}
		}
	}

	[FriendAccessAllowed]
	[SecuritySafeCritical]
	internal static bool ReportUnhandledError(Exception e)
	{
		if (!AppDomain.IsAppXModel())
		{
			return false;
		}
		if (!s_haveBlueErrorApis)
		{
			return false;
		}
		if (e != null)
		{
			IntPtr intPtr = IntPtr.Zero;
			IntPtr ppv = IntPtr.Zero;
			try
			{
				intPtr = Marshal.GetIUnknownForObject(e);
				if (intPtr != IntPtr.Zero)
				{
					Marshal.QueryInterface(intPtr, ref s_iidIErrorInfo, out ppv);
					if (ppv != IntPtr.Zero && RoOriginateLanguageException(Marshal.GetHRForException_WinRT(e), e.Message, ppv))
					{
						IRestrictedErrorInfo restrictedErrorInfo = UnsafeNativeMethods.GetRestrictedErrorInfo();
						if (restrictedErrorInfo != null)
						{
							RoReportUnhandledError(restrictedErrorInfo);
							return true;
						}
					}
				}
			}
			finally
			{
				if (ppv != IntPtr.Zero)
				{
					Marshal.Release(ppv);
				}
				if (intPtr != IntPtr.Zero)
				{
					Marshal.Release(intPtr);
				}
			}
		}
		return false;
	}

	[SecurityCritical]
	public static IActivationFactory GetActivationFactory(Type type)
	{
		if (type == null)
		{
			throw new ArgumentNullException("type");
		}
		if (type.IsWindowsRuntimeObject && type.IsImport)
		{
			return (IActivationFactory)Marshal.GetNativeActivationFactory(type);
		}
		throw new NotSupportedException();
	}

	[SecurityCritical]
	public unsafe static IntPtr StringToHString(string s)
	{
		if (!Environment.IsWinRTSupported)
		{
			throw new PlatformNotSupportedException(Environment.GetResourceString("Windows Runtime is not supported on this operating system."));
		}
		if (s == null)
		{
			throw new ArgumentNullException("s");
		}
		IntPtr result = default(IntPtr);
		Marshal.ThrowExceptionForHR(UnsafeNativeMethods.WindowsCreateString(s, s.Length, &result), new IntPtr(-1));
		return result;
	}

	[SecurityCritical]
	public static string PtrToStringHString(IntPtr ptr)
	{
		if (!Environment.IsWinRTSupported)
		{
			throw new PlatformNotSupportedException(Environment.GetResourceString("Windows Runtime is not supported on this operating system."));
		}
		return HStringToString(ptr);
	}

	[SecurityCritical]
	public static void FreeHString(IntPtr ptr)
	{
		if (!Environment.IsWinRTSupported)
		{
			throw new PlatformNotSupportedException(Environment.GetResourceString("Windows Runtime is not supported on this operating system."));
		}
		if (ptr != IntPtr.Zero)
		{
			UnsafeNativeMethods.WindowsDeleteString(ptr);
		}
	}
}
