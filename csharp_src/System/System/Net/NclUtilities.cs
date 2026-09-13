using System.Collections.Generic;
using System.Threading;

namespace System.Net;

internal static class NclUtilities
{
	private static volatile ContextCallback s_ContextRelativeDemandCallback;

	private static volatile IPAddress[] _LocalAddresses;

	private static object _LocalAddressesLock;

	private const int HostNameBufferLength = 256;

	internal static string _LocalDomainName;

	internal static bool HasShutdownStarted
	{
		get
		{
			if (!Environment.HasShutdownStarted)
			{
				return AppDomain.CurrentDomain.IsFinalizingForUnload();
			}
			return true;
		}
	}

	internal static ContextCallback ContextRelativeDemandCallback
	{
		get
		{
			if (s_ContextRelativeDemandCallback == null)
			{
				s_ContextRelativeDemandCallback = DemandCallback;
			}
			return s_ContextRelativeDemandCallback;
		}
	}

	internal static IPAddress[] LocalAddresses
	{
		get
		{
			IPAddress[] localAddresses = _LocalAddresses;
			if (localAddresses != null)
			{
				return localAddresses;
			}
			lock (LocalAddressesLock)
			{
				localAddresses = _LocalAddresses;
				if (localAddresses != null)
				{
					return localAddresses;
				}
				List<IPAddress> list = new List<IPAddress>();
				try
				{
					IPHostEntry localHost = GetLocalHost();
					if (localHost != null)
					{
						if (localHost.HostName != null)
						{
							int num = localHost.HostName.IndexOf('.');
							if (num != -1)
							{
								_LocalDomainName = localHost.HostName.Substring(num);
							}
						}
						IPAddress[] addressList = localHost.AddressList;
						if (addressList != null)
						{
							IPAddress[] array = addressList;
							foreach (IPAddress item in array)
							{
								list.Add(item);
							}
						}
					}
				}
				catch
				{
				}
				localAddresses = new IPAddress[list.Count];
				int num2 = 0;
				foreach (IPAddress item2 in list)
				{
					localAddresses[num2] = item2;
					num2++;
				}
				_LocalAddresses = localAddresses;
				return localAddresses;
			}
		}
	}

	private static object LocalAddressesLock
	{
		get
		{
			if (_LocalAddressesLock == null)
			{
				Interlocked.CompareExchange(ref _LocalAddressesLock, new object(), null);
			}
			return _LocalAddressesLock;
		}
	}

	internal static bool IsThreadPoolLow()
	{
		ThreadPool.GetAvailableThreads(out var workerThreads, out var _);
		return workerThreads < 2;
	}

	internal static bool IsCredentialFailure(SecurityStatus error)
	{
		if (error != SecurityStatus.LogonDenied && error != SecurityStatus.UnknownCredentials && error != SecurityStatus.NoImpersonation && error != SecurityStatus.NoAuthenticatingAuthority && error != SecurityStatus.UntrustedRoot && error != SecurityStatus.CertExpired && error != SecurityStatus.SmartcardLogonRequired)
		{
			return error == SecurityStatus.BadBinding;
		}
		return true;
	}

	internal static bool IsClientFault(SecurityStatus error)
	{
		if (error != SecurityStatus.InvalidToken && error != SecurityStatus.CannotPack && error != SecurityStatus.QopNotSupported && error != SecurityStatus.NoCredentials && error != SecurityStatus.MessageAltered && error != SecurityStatus.OutOfSequence && error != SecurityStatus.IncompleteMessage && error != SecurityStatus.IncompleteCredentials && error != SecurityStatus.WrongPrincipal && error != SecurityStatus.TimeSkew && error != SecurityStatus.IllegalMessage && error != SecurityStatus.CertUnknown && error != SecurityStatus.AlgorithmMismatch && error != SecurityStatus.SecurityQosFailed)
		{
			return error == SecurityStatus.UnsupportedPreauth;
		}
		return true;
	}

	private static void DemandCallback(object state)
	{
	}

	internal static bool GuessWhetherHostIsLoopback(string host)
	{
		string text = host.ToLowerInvariant();
		if (text == "localhost" || text == "loopback")
		{
			return true;
		}
		return false;
	}

	internal static bool IsFatal(Exception exception)
	{
		if (exception != null)
		{
			if (!(exception is OutOfMemoryException) && !(exception is StackOverflowException))
			{
				return exception is ThreadAbortException;
			}
			return true;
		}
		return false;
	}

	internal static bool IsAddressLocal(IPAddress ipAddress)
	{
		IPAddress[] localAddresses = LocalAddresses;
		for (int i = 0; i < localAddresses.Length; i++)
		{
			if (ipAddress.Equals(localAddresses[i], compareScopeId: false))
			{
				return true;
			}
		}
		return false;
	}

	private static IPHostEntry GetLocalHost()
	{
		return Dns.GetHostByName(Dns.GetHostName());
	}
}
