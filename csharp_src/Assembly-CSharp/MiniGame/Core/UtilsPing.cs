using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace MiniGame.Core;

public static class UtilsPing
{
	public class PingResult
	{
		public string Url { get; set; }

		public long RttMs { get; set; }

		public string Error { get; set; }
	}

	public class PingRequest
	{
		public string UdpUrl { get; set; }

		public string HttpUrl { get; set; }
	}

	private class PartialPingResult
	{
		public string Url { get; set; }

		public long? HttpRtt { get; set; }

		public string HttpError { get; set; }

		public long? UdpRtt { get; set; }

		public string UdpError { get; set; }
	}

	private static readonly HttpClient _sharedHttpClient = new HttpClient();

	private static readonly SemaphoreSlim _httpConcurrencySemaphore = new SemaphoreSlim(4, 4);

	private static Action<Action> _sharedPostMainThread;

	private static ArraySegment<byte> _udpSendBuffer = new ArraySegment<byte>(new byte[4] { 112, 105, 110, 103 });

	private static ArraySegment<byte> _udpRecvBuffer = new ArraySegment<byte>(new byte[1024]);

	public static void SetPostMainThreadDelegate(Action<Action> postToMainThread)
	{
		_sharedPostMainThread = postToMainThread ?? throw new ArgumentNullException("postToMainThread");
	}

	public static CancellationTokenSource PingAll(IReadOnlyList<PingRequest> requests, Action<List<PingResult>> callback, int timeoutMs = 3000, int sampleCount = 2, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (callback == null)
		{
			throw new ArgumentNullException("callback");
		}
		if (_sharedPostMainThread == null)
		{
			throw new InvalidOperationException("Must call SetPostMainThreadDelegate first.");
		}
		CancellationTokenSource combinedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
		System.Threading.Tasks.Task.Run(async delegate
		{
			try
			{
				List<PingResult> results = await PingAllAsync(requests, sampleCount, timeoutMs, combinedCts.Token).ConfigureAwait(continueOnCapturedContext: false);
				_sharedPostMainThread(delegate
				{
					callback?.Invoke(results);
				});
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				Exception ex3 = ex2;
				List<PingResult> results2 = requests.Select((PingRequest r) => new PingResult
				{
					Url = (r.HttpUrl ?? r.UdpUrl ?? ""),
					RttMs = 9999L,
					Error = ex3.Message
				}).ToList();
				_sharedPostMainThread(delegate
				{
					callback?.Invoke(results2);
				});
			}
		}, CancellationToken.None);
		return combinedCts;
	}

	public static async Task<List<PingResult>> PingAllAsync(IReadOnlyList<PingRequest> requests, int sampleCount = 2, int timeoutMs = 3000, CancellationToken ct = default(CancellationToken))
	{
		if (requests == null)
		{
			throw new ArgumentNullException("requests");
		}
		PartialPingResult[] partialResults = new PartialPingResult[requests.Count];
		Task<PingResult>[] httpTasks = new Task<PingResult>[requests.Count];
		Task<PingResult>[] udpTasks = new Task<PingResult>[requests.Count];
		for (int i = 0; i < requests.Count; i++)
		{
			PingRequest pingRequest = requests[i];
			partialResults[i] = new PartialPingResult
			{
				Url = (pingRequest.HttpUrl ?? pingRequest.UdpUrl ?? $"(index:{i})")
			};
		}
		List<System.Threading.Tasks.Task> list = new List<System.Threading.Tasks.Task>();
		for (int j = 0; j < requests.Count; j++)
		{
			PingRequest pingRequest2 = requests[j];
			if (!string.IsNullOrEmpty(pingRequest2.HttpUrl))
			{
				httpTasks[j] = HttpPingAsync(pingRequest2.HttpUrl, sampleCount, ct);
				list.Add(httpTasks[j]);
			}
			if (!string.IsNullOrEmpty(pingRequest2.UdpUrl))
			{
				udpTasks[j] = UdpPingAsync(pingRequest2.UdpUrl, sampleCount, ct);
				list.Add(udpTasks[j]);
			}
		}
		if (list.Count == 0)
		{
			return BuildFinalResults(partialResults);
		}
		using (CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(ct))
		{
			System.Threading.Tasks.Task task = System.Threading.Tasks.Task.Delay(timeoutMs, cts.Token);
			await System.Threading.Tasks.Task.WhenAny(System.Threading.Tasks.Task.WhenAll(list), task).ConfigureAwait(continueOnCapturedContext: false);
			cts.Cancel();
		}
		for (int k = 0; k < requests.Count; k++)
		{
			if (httpTasks[k] != null && httpTasks[k].IsCompleted && !httpTasks[k].IsFaulted && !httpTasks[k].IsCanceled)
			{
				PingResult pingResult = await httpTasks[k].ConfigureAwait(continueOnCapturedContext: false);
				if (pingResult.RttMs < 9999)
				{
					partialResults[k].HttpRtt = pingResult.RttMs;
				}
				else
				{
					partialResults[k].HttpError = pingResult.Error;
				}
			}
			if (udpTasks[k] != null && udpTasks[k].IsCompleted && !udpTasks[k].IsFaulted && !udpTasks[k].IsCanceled)
			{
				PingResult pingResult2 = await udpTasks[k].ConfigureAwait(continueOnCapturedContext: false);
				if (pingResult2.RttMs < 9999)
				{
					partialResults[k].UdpRtt = pingResult2.RttMs;
				}
				else
				{
					partialResults[k].UdpError = pingResult2.Error;
				}
			}
		}
		return BuildFinalResults(partialResults);
	}

	private static List<PingResult> BuildFinalResults(PartialPingResult[] partialResults)
	{
		List<PingResult> list = new List<PingResult>();
		foreach (PartialPingResult partialPingResult in partialResults)
		{
			long? num = null;
			if (partialPingResult.HttpRtt.HasValue && partialPingResult.HttpRtt.Value < 9999)
			{
				num = partialPingResult.HttpRtt.Value;
			}
			if (partialPingResult.UdpRtt.HasValue && partialPingResult.UdpRtt.Value < 9999)
			{
				num = (num.HasValue ? Math.Min(num.Value, partialPingResult.UdpRtt.Value) : partialPingResult.UdpRtt.Value);
			}
			if (num.HasValue)
			{
				list.Add(new PingResult
				{
					Url = partialPingResult.Url,
					RttMs = num.Value,
					Error = ""
				});
				continue;
			}
			string text = "";
			if (!string.IsNullOrEmpty(partialPingResult.HttpError))
			{
				text = text + "Http: " + partialPingResult.HttpError + "; ";
			}
			if (!string.IsNullOrEmpty(partialPingResult.UdpError))
			{
				text = text + "Udp: " + partialPingResult.UdpError + "; ";
			}
			if (string.IsNullOrEmpty(text))
			{
				text = "Timeout";
			}
			list.Add(new PingResult
			{
				Url = partialPingResult.Url,
				RttMs = 9999L,
				Error = text.Trim().TrimEnd(new char[1] { ';' })
			});
		}
		return list;
	}

	public static async Task<PingResult> PingAsync(PingRequest req, int sampleCount, CancellationToken ct = default(CancellationToken))
	{
		bool hasHttp = !string.IsNullOrEmpty(req.HttpUrl);
		bool hasUdp = !string.IsNullOrEmpty(req.UdpUrl);
		if (!hasHttp && !hasUdp)
		{
			return new PingResult
			{
				Url = "(empty)",
				RttMs = 9999L,
				Error = "No URL provided"
			};
		}
		Task<PingResult> httpTask = (hasHttp ? HttpPingAsync(req.HttpUrl, sampleCount, ct) : System.Threading.Tasks.Task.FromResult<PingResult>(null));
		Task<PingResult> udpTask = (hasUdp ? UdpPingAsync(req.UdpUrl, sampleCount, ct) : System.Threading.Tasks.Task.FromResult<PingResult>(null));
		await System.Threading.Tasks.Task.WhenAll(hasHttp ? httpTask : System.Threading.Tasks.Task.CompletedTask, hasUdp ? udpTask : System.Threading.Tasks.Task.CompletedTask).ConfigureAwait(continueOnCapturedContext: false);
		PingResult best = null;
		string error = "";
		if (hasHttp)
		{
			PingResult pingResult = await httpTask.ConfigureAwait(continueOnCapturedContext: false);
			if (pingResult != null && pingResult.RttMs < 9999)
			{
				best = ((best == null || pingResult.RttMs < best.RttMs) ? pingResult : best);
			}
			else
			{
				error = error + "HttpError(" + pingResult?.Error + ") ";
			}
		}
		if (hasUdp)
		{
			PingResult pingResult2 = await udpTask.ConfigureAwait(continueOnCapturedContext: false);
			if (pingResult2 != null && pingResult2.RttMs < 9999)
			{
				best = ((best == null || pingResult2.RttMs < best.RttMs) ? pingResult2 : best);
			}
			else
			{
				error = error + "UdpError(" + pingResult2?.Error + ") ";
			}
		}
		if (best != null)
		{
			return best;
		}
		return new PingResult
		{
			Url = (req.HttpUrl ?? req.UdpUrl),
			RttMs = 9999L,
			Error = error.Trim()
		};
	}

	public static async Task<PingResult> HttpPingAsync(string url, int sampleCount = 2, CancellationToken ct = default(CancellationToken))
	{
		if (string.IsNullOrWhiteSpace(url))
		{
			return new PingResult
			{
				Url = (url ?? ""),
				RttMs = 9999L,
				Error = "URL is null or empty"
			};
		}
		sampleCount = Math.Max(1, sampleCount);
		long minRtt = long.MaxValue;
		string lastError = null;
		bool success = false;
		await _httpConcurrencySemaphore.WaitAsync(ct).ConfigureAwait(continueOnCapturedContext: false);
		CancellationTokenSource ctSend = null;
		try
		{
			ctSend = new CancellationTokenSource();
			ctSend.CancelAfter(TimeSpan.FromSeconds(5.0));
			for (int i = 0; i < sampleCount && !ct.IsCancellationRequested; i++)
			{
				HttpResponseMessage response = null;
				try
				{
					HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Head, url);
					Stopwatch sw = Stopwatch.StartNew();
					Task<HttpResponseMessage> sendTask = _sharedHttpClient.SendAsync(request, ctSend.Token);
					System.Threading.Tasks.Task timeoutTask = System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(2.0), ct);
					System.Threading.Tasks.Task obj = await System.Threading.Tasks.Task.WhenAny(sendTask, timeoutTask).ConfigureAwait(continueOnCapturedContext: false);
					sw.Stop();
					if (obj == timeoutTask)
					{
						lastError = "HTTP request timed out (hard limit)";
						continue;
					}
					response = await sendTask.ConfigureAwait(continueOnCapturedContext: false);
					success = true;
					long elapsedMilliseconds = sw.ElapsedMilliseconds;
					if (elapsedMilliseconds >= 0 && elapsedMilliseconds < minRtt)
					{
						minRtt = elapsedMilliseconds;
					}
				}
				catch (OperationCanceledException) when (ct.IsCancellationRequested)
				{
					lastError = "Cancelled";
					break;
				}
				catch (OperationCanceledException)
				{
					lastError = "HTTP request cancelled or internal timeout";
					continue;
				}
				catch (HttpRequestException ex3)
				{
					lastError = "Network Error: " + ex3.Message;
				}
				catch (Exception ex4)
				{
					lastError = "Unexpected Error: " + ex4.Message;
				}
				finally
				{
					response?.Dispose();
				}
				if (i < sampleCount - 1)
				{
					if (ct.IsCancellationRequested)
					{
						break;
					}
					await System.Threading.Tasks.Task.Delay(30, ct).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
		}
		finally
		{
			ctSend?.Dispose();
			_httpConcurrencySemaphore.Release();
		}
		return new PingResult
		{
			Url = url,
			RttMs = (success ? minRtt : 9999),
			Error = (success ? null : lastError)
		};
	}

	public static async Task<PingResult> UdpPingAsync(string udpUrl, int sampleCount = 2, CancellationToken ct = default(CancellationToken))
	{
		if (string.IsNullOrWhiteSpace(udpUrl))
		{
			return new PingResult
			{
				Url = (udpUrl ?? ""),
				RttMs = 9999L,
				Error = "URL is null or empty"
			};
		}
		IPEndPoint endpoint = await TryParseUdpUrlAsync(udpUrl, ct).ConfigureAwait(continueOnCapturedContext: false);
		if (endpoint == null)
		{
			return new PingResult
			{
				Url = udpUrl,
				RttMs = 9999L,
				Error = "Invalid UDP URL or DNS failed"
			};
		}
		sampleCount = Math.Max(1, sampleCount);
		long minRtt = long.MaxValue;
		string lastError = null;
		bool success = false;
		byte[] sendBuffer = new byte[1] { 1 };
		byte[] recvBuffer = new byte[64];
		for (int i = 0; i < sampleCount; i++)
		{
			if (ct.IsCancellationRequested)
			{
				break;
			}
			Socket socket = null;
			try
			{
				socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
				{
					Blocking = false
				};
				try
				{
					socket.SendTo(sendBuffer, endpoint);
				}
				catch (SocketException ex) when (ex.SocketErrorCode == SocketError.WouldBlock)
				{
				}
				Stopwatch sw = Stopwatch.StartNew();
				EndPoint remoteEp = endpoint;
				bool received = false;
				while (sw.ElapsedMilliseconds < 2000 && !ct.IsCancellationRequested)
				{
					if (socket.Poll(0, SelectMode.SelectRead))
					{
						try
						{
							if (socket.ReceiveFrom(recvBuffer, ref remoteEp) > 0)
							{
								received = true;
								sw.Stop();
								break;
							}
						}
						catch (SocketException ex2)
						{
							lastError = $"Socket error: {ex2.SocketErrorCode}";
							break;
						}
					}
					await System.Threading.Tasks.Task.Delay(10, ct).ConfigureAwait(continueOnCapturedContext: false);
				}
				if (received && !ct.IsCancellationRequested)
				{
					success = true;
					long elapsedMilliseconds = sw.ElapsedMilliseconds;
					if (elapsedMilliseconds < minRtt)
					{
						minRtt = elapsedMilliseconds;
					}
				}
				else
				{
					lastError = "UDP Timeout";
				}
			}
			catch (OperationCanceledException)
			{
				lastError = "Cancelled";
				break;
			}
			catch (SocketException ex4)
			{
				lastError = $"SocketError: {ex4.SocketErrorCode}";
			}
			catch (Exception ex5)
			{
				lastError = "Unexpected: " + ex5.Message;
			}
			finally
			{
				socket?.Dispose();
			}
			if (i < sampleCount - 1 && !ct.IsCancellationRequested)
			{
				await System.Threading.Tasks.Task.Delay(10, ct).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		return new PingResult
		{
			Url = endpoint.ToString(),
			RttMs = (success ? minRtt : 9999),
			Error = (success ? null : lastError)
		};
	}

	private static async Task<IPEndPoint> TryParseUdpUrlAsync(string url, CancellationToken ct = default(CancellationToken), int dnsTimeoutMs = 2000)
	{
		if (string.IsNullOrWhiteSpace(url))
		{
			return null;
		}
		string[] array = url.Split(new char[1] { ':' });
		if (array.Length != 2)
		{
			return null;
		}
		string text = array[0];
		if (!int.TryParse(array[1], out var port) || port <= 0 || port > 65535)
		{
			return null;
		}
		if (IPAddress.TryParse(text, out var address))
		{
			return new IPEndPoint(address, port);
		}
		try
		{
			Task<IPAddress[]> dnsTask = Dns.GetHostAddressesAsync(text);
			System.Threading.Tasks.Task delayTask = System.Threading.Tasks.Task.Delay(dnsTimeoutMs, ct);
			if (await System.Threading.Tasks.Task.WhenAny(dnsTask, delayTask).ConfigureAwait(continueOnCapturedContext: false) == delayTask)
			{
				return null;
			}
			IPAddress[] array2 = await dnsTask.ConfigureAwait(continueOnCapturedContext: false);
			if (array2.Length != 0)
			{
				return new IPEndPoint(array2[0], port);
			}
		}
		catch (OperationCanceledException)
		{
			return null;
		}
		catch (TimeoutException)
		{
			return null;
		}
		catch
		{
		}
		return null;
	}
}
