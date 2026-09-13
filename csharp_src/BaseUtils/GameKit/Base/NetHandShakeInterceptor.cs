using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using GameFramework;

namespace GameKit.Base;

public class NetHandShakeInterceptor
{
	private class HandShakeReceiveContext
	{
		private static Stack<HandShakeReceiveContext> _contextPools = new Stack<HandShakeReceiveContext>();

		public byte[] buffer;

		public StringBuilder sb;

		public StringBuilder totalSb;

		public StreamReader reader;

		public List<string> rawHeaders;

		private MemoryStream _cachedStream;

		public HandShakeReceiveContext()
		{
			Log.Error("Instancing handshake receive context");
		}

		public HandShakeReceiveContext Reset()
		{
			if (sb == null)
			{
				sb = new StringBuilder();
			}
			else
			{
				sb.Length = 0;
			}
			if (totalSb == null)
			{
				totalSb = new StringBuilder();
			}
			else
			{
				totalSb.Length = 0;
			}
			if (_cachedStream == null)
			{
				_cachedStream = new MemoryStream();
			}
			else
			{
				_cachedStream.Seek(0L, SeekOrigin.Begin);
				_cachedStream.SetLength(0L);
			}
			reader = new StreamReader(_cachedStream);
			if (rawHeaders == null)
			{
				rawHeaders = new List<string>();
			}
			else
			{
				rawHeaders.Clear();
			}
			if (buffer == null)
			{
				buffer = new byte[512];
			}
			return this;
		}

		public static HandShakeReceiveContext Allocate()
		{
			HandShakeReceiveContext handShakeReceiveContext;
			lock (_contextPools)
			{
				handShakeReceiveContext = ((_contextPools.Count > 0) ? _contextPools.Pop() : new HandShakeReceiveContext());
			}
			return handShakeReceiveContext.Reset();
		}

		public static void Recycle(HandShakeReceiveContext context)
		{
			lock (_contextPools)
			{
				_contextPools.Push(context);
			}
		}
	}

	private class HandShakeContext
	{
		public Socket socket;

		public Action<Exception> callback;

		public HandShakeContext(Socket pSocket, Action<Exception> pCallback)
		{
			socket = pSocket;
			callback = pCallback;
		}
	}

	private const int BUFFER_LEN = 512;

	private static readonly string _secWebSocketKey = Convert.ToBase64String(Encoding.UTF8.GetBytes("random"));

	public void Execute(string host, int port, string path, Socket socket, Action<Exception> callback)
	{
		HandShakeContext handShakeContext = new HandShakeContext(socket, callback);
		HandShakeReceiveContext handShakeReceiveContext = HandShakeReceiveContext.Allocate();
		socket.BeginReceive(handShakeReceiveContext.buffer, 0, 512, SocketFlags.None, ReceiveHandShakeCallback, (handShakeContext, handShakeReceiveContext));
		StringWriter stringWriter = new StringWriter();
		stringWriter.Write("GET " + path + " HTTP/1.1\r\n");
		stringWriter.Write("Host: " + host + "\r\n");
		stringWriter.Write("Upgrade: websocket\r\n");
		stringWriter.Write("Connection: Upgrade\r\n");
		stringWriter.Write("Sec-WebSocket-Key: " + _secWebSocketKey + "\r\n");
		stringWriter.Write("Sec-WebSocket-Version: 13\r\n");
		stringWriter.Write("Cache-Control: no-cache\r\n");
		stringWriter.Write("Pragma: no-cache\r\n");
		stringWriter.Write("Accept-Encoding: identity\r\n");
		stringWriter.Write("TE: identity\r\n");
		stringWriter.Write("User-Agent: LastWarWebSocket\r\n");
		stringWriter.Write("Content-Length: 0\r\n");
		stringWriter.Write("\r\n");
		string s = stringWriter.ToString();
		byte[] bytes = Encoding.UTF8.GetBytes(s);
		socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, SendHandShakeCallback, (bytes, 0, handShakeContext));
	}

	private void SendHandShakeCallback(IAsyncResult ar)
	{
		(byte[], int, HandShakeContext) tuple = ((byte[], int, HandShakeContext))ar.AsyncState;
		HandShakeContext item = tuple.Item3;
		try
		{
			int num = item.socket.EndSend(ar);
			if (num != tuple.Item1.Length)
			{
				int num2 = tuple.Item2 + num;
				item.socket.BeginSend(tuple.Item1, num2, tuple.Item1.Length - num2, SocketFlags.None, SendHandShakeCallback, (tuple.Item1, num2, item));
			}
		}
		catch (Exception e)
		{
			DoCallback(item, e);
		}
	}

	private static string GetReadHeadersLog(HandShakeReceiveContext context)
	{
		try
		{
			StringBuilder sb = context.sb;
			List<string> rawHeaders = context.rawHeaders;
			if (rawHeaders.Count > 0 || sb.Length > 0)
			{
				return string.Format("ErrorHeaders => {0}; {1}", (rawHeaders.Count > 0) ? string.Join(";", rawHeaders) : "", sb);
			}
			return "NullErrorHeaders";
		}
		catch (Exception ex)
		{
			return ex.Message + "\n" + ex.StackTrace;
		}
	}

	private void ReceiveHandShakeCallback(IAsyncResult asyncResult)
	{
		(HandShakeContext, HandShakeReceiveContext) obj = ((HandShakeContext, HandShakeReceiveContext))asyncResult.AsyncState;
		HandShakeContext item = obj.Item1;
		HandShakeReceiveContext item2 = obj.Item2;
		bool flag = false;
		try
		{
			Socket socket = item.socket;
			StreamReader reader = item2.reader;
			Stream baseStream = reader.BaseStream;
			StringBuilder sb = item2.sb;
			StringBuilder totalSb = item2.totalSb;
			List<string> rawHeaders = item2.rawHeaders;
			int num = socket.EndReceive(asyncResult);
			if (num > 0)
			{
				long position = baseStream.Position;
				baseStream.Position = baseStream.Length;
				baseStream.Write(item2.buffer, 0, num);
				baseStream.Position = position;
				bool flag2 = false;
				int num2;
				while ((num2 = reader.Read()) != -1)
				{
					totalSb.Append((char)num2);
					if ((ushort)num2 == 10)
					{
						string text = sb.ToString();
						if (text.IsNullOrEmpty() || (!text.Contains(":") && !text.Contains("HTTP/") && !text.Contains("http/")))
						{
							flag2 = true;
							break;
						}
						rawHeaders.Add(text);
						sb.Length = 0;
					}
					sb.Append((char)num2);
				}
				if (flag2)
				{
					int num3 = -1;
					bool flag3 = false;
					int num4 = -1;
					flag = true;
					string text2 = totalSb.ToString();
					Log.Info("RawHeaders : " + text2);
					foreach (string item3 in rawHeaders)
					{
						if (item3.Contains("HTTP/") || item3.Contains("http/"))
						{
							string[] array = item3.Split(new char[1] { ' ' });
							if (array.Length >= 2 && int.TryParse(array[1].Trim(), out var result))
							{
								num3 = result;
							}
							else
							{
								Log.Error("Parse status code failed " + item3);
							}
							if (num3 == 101 || (num3 >= 200 && num3 < 300) || num3 == 304)
							{
								break;
							}
						}
						if (item3.Contains("content-type") || item3.Contains("Content-Type"))
						{
							string[] array2 = item3.Split(new char[1] { ' ' });
							if (array2.Length >= 2 && array2[1].ToLower().Contains("text"))
							{
								flag3 = true;
							}
							else
							{
								Log.Error("Parse content type failed " + item3);
							}
						}
						if (item3.Contains("content-length") || item3.Contains("Content-Length"))
						{
							string[] array3 = item3.Split(new char[1] { ' ' });
							if (array3.Length >= 2 && int.TryParse(array3[1].Trim(), out var result2))
							{
								num4 = result2;
							}
							else
							{
								Log.Error("Parse content length failed " + item3);
							}
						}
					}
					if (num3 == 101 || (num3 >= 200 && num3 < 300) || num3 == 304)
					{
						DoCallback(item, null);
					}
					else
					{
						string text3 = null;
						if (flag3 && num4 > 0)
						{
							byte[] array4 = new byte[num4];
							int num5 = num4;
							int byteCount = Encoding.UTF8.GetByteCount(text2);
							Log.Info($"Websocket contentLength:{num4}, remainingLength:{num5}, receivedLength:{baseStream.Length}");
							try
							{
								if (byteCount < baseStream.Length)
								{
									int num6 = (int)(baseStream.Length - byteCount);
									num5 -= num6;
									baseStream.Position = byteCount;
									baseStream.Read(array4, 0, num6);
								}
								while (num5 > 0)
								{
									int num7 = socket.Receive(array4, num4 - num5, num5, SocketFlags.None);
									if (num7 > 0)
									{
										num5 -= num7;
										continue;
									}
									break;
								}
							}
							catch (Exception)
							{
							}
							if (num5 == 0)
							{
								text3 = Encoding.UTF8.GetString(array4);
							}
						}
						if (text3 != null)
						{
							Log.Info("Failed Content : " + text3);
						}
						DoCallback(item, new Exception("Websocket handshake status error " + num3));
					}
					HandShakeReceiveContext.Recycle(item2);
				}
				else
				{
					socket.BeginReceive(item2.buffer, 0, 512, SocketFlags.None, ReceiveHandShakeCallback, (item, item2));
				}
			}
			else
			{
				DoCallback(item, new Exception("Websocket transBytesNum is 0"));
				HandShakeReceiveContext.Recycle(item2);
			}
		}
		catch (Exception ex2)
		{
			string text4 = ((!flag) ? GetReadHeadersLog(item2) : null);
			DoCallback(item, (text4 == null) ? ex2 : new Exception("HeaderDetail : " + text4 + "\n" + ex2.ToString()));
			HandShakeReceiveContext.Recycle(item2);
		}
	}

	private void DoCallback(HandShakeContext context, Exception e)
	{
		Action<Exception> action = null;
		lock (context)
		{
			action = context.callback;
			context.callback = null;
			context.socket = null;
		}
		action?.Invoke(e);
	}
}
