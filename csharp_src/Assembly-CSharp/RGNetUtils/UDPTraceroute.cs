using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace RGNetUtils;

public class UDPTraceroute
{
	public delegate void OnTracerouteResultCallback(string resultStr);

	private const int MaxTtl = 30;

	private const int MaxAttempts = 3;

	private const int UdpPort = 33434;

	private const int BufferSize = 100;

	private const int TimeoutMilliseconds = 500;

	private string mHostNameOrAddress;

	private int mAttemptsPerTtl;

	private int mTimeout;

	private string mErrorMsg;

	private int mTimeToLive;

	private UDPTracerouteErrorCallbackDelegate mErrorCallback;

	private static string sLastTracerouteHostNameOrAddress;

	public UDPTracerouteResult Start(string hostNameOrAddress, int ttl = 30, int attempts = 3, int timeout = 500)
	{
		mErrorMsg = "";
		mHostNameOrAddress = hostNameOrAddress;
		mAttemptsPerTtl = Math.Max(attempts, 1);
		mTimeout = Math.Max(timeout, 1);
		int microSeconds = mTimeout * 1000;
		mTimeToLive = Math.Max(ttl, 1);
		IPAddress[] array = ResolveIPAddress(mHostNameOrAddress);
		if (array == null)
		{
			return null;
		}
		UDPTracerouteResult uDPTracerouteResult = new UDPTracerouteResult();
		uDPTracerouteResult.IpAddress = array[0];
		IPEndPoint iPEndPoint = new IPEndPoint(uDPTracerouteResult.IpAddress, 33434);
		try
		{
			using (Socket socket = new Socket(uDPTracerouteResult.IpAddress.AddressFamily, SocketType.Dgram, ProtocolType.Icmp))
			{
				using Socket socket2 = new Socket(uDPTracerouteResult.IpAddress.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
				int i = 1;
				bool flag = false;
				socket2.ReceiveTimeout = mTimeout;
				for (; i <= mTimeToLive; i++)
				{
					socket2.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, i);
					EndPoint remoteEP = new IPEndPoint(0L, 0);
					List<UDPTracerouteHopResult> list = new List<UDPTracerouteHopResult>(mAttemptsPerTtl);
					for (int j = 0; j < mAttemptsPerTtl; j++)
					{
						UDPTracerouteHopResult uDPTracerouteHopResult = new UDPTracerouteHopResult();
						byte[] bytes = Encoding.ASCII.GetBytes("GET / HTTP/1.1\r\n\r\n");
						DateTime now = DateTime.Now;
						socket2.SendTo(bytes, iPEndPoint);
						if (socket.Poll(microSeconds, SelectMode.SelectRead))
						{
							bytes = new byte[100];
							try
							{
								uDPTracerouteHopResult.BytesRead = socket.ReceiveFrom(bytes, ref remoteEP);
								IPEndPoint iPEndPoint2 = remoteEP as IPEndPoint;
								if (uDPTracerouteHopResult.BytesRead > 2)
								{
									uDPTracerouteHopResult.IsSuccess = true;
									uDPTracerouteHopResult.FromEndPoint = new IPEndPoint(iPEndPoint2.Address, iPEndPoint2.Port);
									int num = (bytes[0] & 0xF) * 4;
									uDPTracerouteHopResult.IcmpType = bytes[num];
									uDPTracerouteHopResult.IcmpCode = bytes[num + 1];
								}
								else
								{
									uDPTracerouteHopResult.IsSuccess = false;
									uDPTracerouteHopResult.ErrorStr = $"Received too little data from {iPEndPoint2.Address}";
								}
							}
							catch (SocketException ex)
							{
								if (ex.SocketErrorCode == SocketError.TimedOut)
								{
									uDPTracerouteHopResult.SetTimeout();
								}
								else
								{
									uDPTracerouteHopResult.IsSuccess = false;
									uDPTracerouteHopResult.ErrorStr = ex.Message;
								}
							}
							uDPTracerouteHopResult.ArriveTime = (DateTime.Now - now).TotalMilliseconds;
							if (remoteEP.Equals(iPEndPoint) || (uDPTracerouteHopResult.IcmpType == 3 && uDPTracerouteHopResult.IcmpCode == 3))
							{
								flag = true;
							}
						}
						else
						{
							uDPTracerouteHopResult.SetTimeout();
						}
						list.Add(uDPTracerouteHopResult);
					}
					uDPTracerouteResult.HopResults.Add(list);
					if (flag)
					{
						break;
					}
				}
			}
			return uDPTracerouteResult;
		}
		catch (SocketException ex2)
		{
			if (ex2.SocketErrorCode != SocketError.NetworkUnreachable)
			{
				MarkError("SocketException: " + ex2.Message);
			}
		}
		catch (WebException ex3)
		{
			if (ex3.Status != WebExceptionStatus.ConnectFailure && ex3.Status != WebExceptionStatus.NameResolutionFailure)
			{
				MarkError("WebException: " + ex3.Message);
			}
		}
		catch (Exception ex4)
		{
			MarkError("Exception: " + ex4.Message);
		}
		return null;
	}

	public static void StartAndReportAsync(string hostNameOrAddress, string reportKey, int ttl = 30, int attempts = 3, int timeout = 500)
	{
		if (sLastTracerouteHostNameOrAddress == hostNameOrAddress)
		{
			return;
		}
		sLastTracerouteHostNameOrAddress = hostNameOrAddress;
		MainThreadDispatcher mainThreadDispatcher = MainThreadDispatcher.Instance;
		ThreadPool.QueueUserWorkItem(delegate
		{
			UDPTraceroute uDPTraceroute = new UDPTraceroute();
			Dictionary<string, object> reportMap = null;
			UDPTracerouteResult uDPTracerouteResult = uDPTraceroute.Start(hostNameOrAddress, ttl, attempts, timeout);
			if (uDPTracerouteResult == null)
			{
				string lastErrorStr = uDPTraceroute.GetLastErrorStr();
				if (string.IsNullOrEmpty(lastErrorStr))
				{
					return;
				}
				reportMap = new Dictionary<string, object> { 
				{
					"Summary",
					"traceroute to " + hostNameOrAddress + " error : " + lastErrorStr
				} };
			}
			else
			{
				reportMap = uDPTraceroute.SerializeToMap(uDPTracerouteResult);
			}
			mainThreadDispatcher?.Enqueue(delegate
			{
				if (reportMap != null)
				{
					PostEventLog.TrackMap(reportKey, reportMap);
					string text = (reportMap?["Summary"])?.ToString();
					Debug.LogWarning("Traceroute ReportKey: " + reportKey + ", Result : " + text);
				}
			});
		});
	}

	public static void StartAndReportAsync(string[] hostNameOrAddresses, string reportKey, int ttl = 30, int attempts = 3, int timeout = 500)
	{
		if (hostNameOrAddresses == null || hostNameOrAddresses.Length == 0)
		{
			return;
		}
		MainThreadDispatcher mainThreadDispatcher = MainThreadDispatcher.Instance;
		ThreadPool.QueueUserWorkItem(delegate
		{
			StringBuilder result = new StringBuilder();
			foreach (string text in hostNameOrAddresses)
			{
				UDPTraceroute uDPTraceroute = new UDPTraceroute();
				UDPTracerouteResult uDPTracerouteResult = uDPTraceroute.Start(text, ttl, attempts, timeout);
				if (uDPTracerouteResult == null)
				{
					string lastErrorStr = uDPTraceroute.GetLastErrorStr();
					if (!string.IsNullOrEmpty(lastErrorStr))
					{
						result.Append("traceroute to " + text + " error : " + lastErrorStr + " \n");
					}
				}
				else
				{
					result.Append(uDPTraceroute.SerializeToString(uDPTracerouteResult));
				}
			}
			if (result.Length != 0)
			{
				mainThreadDispatcher?.Enqueue(delegate
				{
					if (result.Length > 0)
					{
						string text2 = result.ToString();
						Dictionary<string, object> prop = new Dictionary<string, object> { { "Summary", text2 } };
						PostEventLog.TrackMap(reportKey, prop);
						Debug.LogWarning("Traceroute ReportKey: " + reportKey + ", Result : " + text2);
					}
				});
			}
		});
	}

	public static void StartAsync(string hostNameOrAddress, string reportKey, OnTracerouteResultCallback onResult, int ttl = 30, int attempts = 3, int timeout = 500)
	{
		if (sLastTracerouteHostNameOrAddress == hostNameOrAddress)
		{
			return;
		}
		sLastTracerouteHostNameOrAddress = hostNameOrAddress;
		MainThreadDispatcher mainThreadDispatcher = MainThreadDispatcher.Instance;
		ThreadPool.QueueUserWorkItem(delegate
		{
			UDPTraceroute uDPTraceroute = new UDPTraceroute();
			UDPTracerouteResult uDPTracerouteResult = uDPTraceroute.Start(hostNameOrAddress, ttl, attempts, timeout);
			string resultStr;
			if (uDPTracerouteResult == null)
			{
				string lastErrorStr = uDPTraceroute.GetLastErrorStr();
				if (string.IsNullOrEmpty(lastErrorStr))
				{
					return;
				}
				resultStr = "traceroute to " + hostNameOrAddress + " error : " + lastErrorStr;
			}
			else
			{
				resultStr = uDPTraceroute.SerializeToString(uDPTracerouteResult);
			}
			mainThreadDispatcher?.Enqueue(delegate
			{
				if (!string.IsNullOrEmpty(resultStr) && onResult != null)
				{
					onResult(resultStr);
				}
			});
		});
	}

	public string SerializeToString(UDPTracerouteResult result, bool printHostName = false)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (result != null)
		{
			int count = result.HopResults.Count;
			int num;
			for (num = result.HopResults.Count; num > 0; num--)
			{
				List<UDPTracerouteHopResult> list = result.HopResults[num - 1];
				bool flag = true;
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsSuccess)
					{
						flag = false;
						break;
					}
				}
				if (!flag)
				{
					break;
				}
			}
			if (printHostName)
			{
				stringBuilder.AppendFormat("Traceroute to {0}({1}) all hops {2} valid hops {3}. ", GetHostnameFromIpAddress(result.IpAddress), result.IpAddress, count, num);
			}
			else
			{
				stringBuilder.AppendFormat("Traceroute to {0} all hops {1} valid hops {2}. ", result.IpAddress, count, num);
			}
			stringBuilder.AppendLine();
			for (int j = 0; j < num; j++)
			{
				List<UDPTracerouteHopResult> list2 = result.HopResults[j];
				stringBuilder.Append($"{j}: ");
				IPAddress iPAddress = null;
				for (int k = 0; k < list2.Count; k++)
				{
					UDPTracerouteHopResult uDPTracerouteHopResult = list2[k];
					if (uDPTracerouteHopResult.IsSuccess)
					{
						if (iPAddress == null || !iPAddress.Equals(uDPTracerouteHopResult.FromEndPoint.Address))
						{
							if (printHostName)
							{
								stringBuilder.AppendFormat(" {0} ({1})", GetHostnameFromIpAddress(uDPTracerouteHopResult.FromEndPoint.Address), uDPTracerouteHopResult.FromEndPoint.Address);
							}
							else
							{
								stringBuilder.AppendFormat(" {0}", uDPTracerouteHopResult.FromEndPoint.Address);
							}
							iPAddress = uDPTracerouteHopResult.FromEndPoint.Address;
						}
						stringBuilder.AppendFormat(" {0}", uDPTracerouteHopResult.ArriveTime);
						if (uDPTracerouteHopResult.IcmpType == 3)
						{
							if (uDPTracerouteHopResult.IcmpCode == 3)
							{
								stringBuilder.Append(" !");
							}
							else if (uDPTracerouteHopResult.IcmpCode == 0)
							{
								stringBuilder.Append(" !N");
							}
							else if (uDPTracerouteHopResult.IcmpCode == 1)
							{
								stringBuilder.Append(" !H");
							}
							else if (uDPTracerouteHopResult.IcmpCode == 2)
							{
								stringBuilder.Append(" !P");
							}
							else if (uDPTracerouteHopResult.IcmpCode == 4)
							{
								stringBuilder.Append(" !F");
							}
							else if (uDPTracerouteHopResult.IcmpCode == 5)
							{
								stringBuilder.Append(" !S");
							}
						}
					}
					else
					{
						stringBuilder.Append(" * ");
					}
				}
				stringBuilder.AppendLine();
			}
		}
		return stringBuilder.ToString();
	}

	public Dictionary<string, object> SerializeToMap(UDPTracerouteResult result)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		if (result == null)
		{
			return dictionary;
		}
		int count = result.HopResults.Count;
		int num;
		for (num = result.HopResults.Count; num > 0; num--)
		{
			List<UDPTracerouteHopResult> list = result.HopResults[num - 1];
			bool flag = true;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].IsSuccess)
				{
					flag = false;
					break;
				}
			}
			if (!flag)
			{
				break;
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("Traceroute to {0} all hops {1} valid hops {2}. ", result.IpAddress, count, num);
		stringBuilder.AppendLine();
		for (int j = 0; j < num; j++)
		{
			List<UDPTracerouteHopResult> list2 = result.HopResults[j];
			stringBuilder.Append($"{j}: ");
			IPAddress iPAddress = null;
			for (int k = 0; k < list2.Count; k++)
			{
				UDPTracerouteHopResult uDPTracerouteHopResult = list2[k];
				if (uDPTracerouteHopResult.IsSuccess)
				{
					if (iPAddress == null || !iPAddress.Equals(uDPTracerouteHopResult.FromEndPoint.Address))
					{
						stringBuilder.AppendFormat(" {0}", uDPTracerouteHopResult.FromEndPoint.Address);
						iPAddress = uDPTracerouteHopResult.FromEndPoint.Address;
					}
					stringBuilder.AppendFormat(" {0}", uDPTracerouteHopResult.ArriveTime);
					if (uDPTracerouteHopResult.IcmpType == 3)
					{
						if (uDPTracerouteHopResult.IcmpCode == 3)
						{
							stringBuilder.Append(" !");
						}
						else if (uDPTracerouteHopResult.IcmpCode == 0)
						{
							stringBuilder.Append(" !N");
						}
						else if (uDPTracerouteHopResult.IcmpCode == 1)
						{
							stringBuilder.Append(" !H");
						}
						else if (uDPTracerouteHopResult.IcmpCode == 2)
						{
							stringBuilder.Append(" !P");
						}
						else if (uDPTracerouteHopResult.IcmpCode == 4)
						{
							stringBuilder.Append(" !F");
						}
						else if (uDPTracerouteHopResult.IcmpCode == 5)
						{
							stringBuilder.Append(" !S");
						}
					}
				}
				else
				{
					stringBuilder.Append(" *");
				}
			}
			stringBuilder.AppendLine();
		}
		dictionary.Add("Summary", stringBuilder.ToString());
		return dictionary;
	}

	public string GetLastErrorStr()
	{
		return mErrorMsg;
	}

	private IPAddress[] ResolveIPAddress(string hostOrIpAddress, string dnsResolver = "system")
	{
		if (IPAddress.TryParse(hostOrIpAddress, out var address))
		{
			return new IPAddress[1] { address };
		}
		if (dnsResolver == "system")
		{
			try
			{
				return Dns.GetHostAddresses(hostOrIpAddress);
			}
			catch (Exception ex)
			{
				MarkError("Error resolving host name:" + ex.Message);
				return null;
			}
		}
		MarkError("Resolve IP Address from dns service is not supported!");
		return null;
	}

	private void MarkError(string errMsg)
	{
		mErrorMsg = errMsg;
		if (mErrorCallback != null)
		{
			mErrorCallback(mErrorMsg);
		}
	}

	private string GetHostnameFromIpAddress(IPAddress ipAddress)
	{
		try
		{
			return Dns.GetHostEntry(ipAddress).HostName;
		}
		catch (Exception)
		{
			return ipAddress.ToString();
		}
	}
}
