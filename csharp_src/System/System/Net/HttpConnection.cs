using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace System.Net;

internal sealed class HttpConnection
{
	private enum InputState
	{
		RequestLine,
		Headers
	}

	private enum LineState
	{
		None,
		CR,
		LF
	}

	private static AsyncCallback onread_cb = OnRead;

	private const int BufferSize = 8192;

	private Socket sock;

	private Stream stream;

	private EndPointListener epl;

	private MemoryStream ms;

	private byte[] buffer;

	private HttpListenerContext context;

	private StringBuilder current_line;

	private ListenerPrefix prefix;

	private RequestStream i_stream;

	private ResponseStream o_stream;

	private bool chunked;

	private int reuses;

	private bool context_bound;

	private bool secure;

	private X509Certificate cert;

	private int s_timeout = 90000;

	private Timer timer;

	private IPEndPoint local_ep;

	private HttpListener last_listener;

	private int[] client_cert_errors;

	private X509Certificate2 client_cert;

	private SslStream ssl_stream;

	private InputState input_state;

	private LineState line_state;

	private int position;

	internal SslStream SslStream => ssl_stream;

	internal int[] ClientCertificateErrors => client_cert_errors;

	internal X509Certificate2 ClientCertificate => client_cert;

	public bool IsClosed => sock == null;

	public int Reuses => reuses;

	public IPEndPoint LocalEndPoint
	{
		get
		{
			if (local_ep != null)
			{
				return local_ep;
			}
			local_ep = (IPEndPoint)sock.LocalEndPoint;
			return local_ep;
		}
	}

	public IPEndPoint RemoteEndPoint => (IPEndPoint)sock.RemoteEndPoint;

	public bool IsSecure => secure;

	public ListenerPrefix Prefix
	{
		get
		{
			return prefix;
		}
		set
		{
			prefix = value;
		}
	}

	public HttpConnection(Socket sock, EndPointListener epl, bool secure, X509Certificate cert)
	{
		this.sock = sock;
		this.epl = epl;
		this.secure = secure;
		this.cert = cert;
		if (!secure)
		{
			stream = new NetworkStream(sock, ownsSocket: false);
		}
		else
		{
			ssl_stream = epl.Listener.CreateSslStream(new NetworkStream(sock, ownsSocket: false), ownsStream: false, delegate(object t, X509Certificate c, X509Chain ch, SslPolicyErrors e)
			{
				if (c == null)
				{
					return true;
				}
				X509Certificate2 x509Certificate = c as X509Certificate2;
				if (x509Certificate == null)
				{
					x509Certificate = new X509Certificate2(c.GetRawCertData());
				}
				client_cert = x509Certificate;
				client_cert_errors = new int[1] { (int)e };
				return true;
			});
			stream = ssl_stream;
		}
		timer = new Timer(OnTimeout, null, -1, -1);
		if (ssl_stream != null)
		{
			ssl_stream.AuthenticateAsServer(cert, clientCertificateRequired: true, (SslProtocols)ServicePointManager.SecurityProtocol, checkCertificateRevocation: false);
		}
		Init();
	}

	private void Init()
	{
		context_bound = false;
		i_stream = null;
		o_stream = null;
		prefix = null;
		chunked = false;
		ms = new MemoryStream();
		position = 0;
		input_state = InputState.RequestLine;
		line_state = LineState.None;
		context = new HttpListenerContext(this);
	}

	private void OnTimeout(object unused)
	{
		CloseSocket();
		Unbind();
	}

	public void BeginReadRequest()
	{
		if (buffer == null)
		{
			buffer = new byte[8192];
		}
		try
		{
			if (reuses == 1)
			{
				s_timeout = 15000;
			}
			timer.Change(s_timeout, -1);
			stream.BeginRead(buffer, 0, 8192, onread_cb, this);
		}
		catch
		{
			timer.Change(-1, -1);
			CloseSocket();
			Unbind();
		}
	}

	public RequestStream GetRequestStream(bool chunked, long contentlength)
	{
		if (i_stream == null)
		{
			byte[] array = ms.GetBuffer();
			int num = (int)ms.Length;
			ms = null;
			if (chunked)
			{
				this.chunked = true;
				context.Response.SendChunked = true;
				i_stream = new ChunkedInputStream(context, stream, array, position, num - position);
			}
			else
			{
				i_stream = new RequestStream(stream, array, position, num - position, contentlength);
			}
		}
		return i_stream;
	}

	public ResponseStream GetResponseStream()
	{
		if (o_stream == null)
		{
			HttpListener listener = context.Listener;
			if (listener == null)
			{
				return new ResponseStream(stream, context.Response, ignore_errors: true);
			}
			o_stream = new ResponseStream(stream, context.Response, listener.IgnoreWriteExceptions);
		}
		return o_stream;
	}

	private static void OnRead(IAsyncResult ares)
	{
		((HttpConnection)ares.AsyncState).OnReadInternal(ares);
	}

	private void OnReadInternal(IAsyncResult ares)
	{
		timer.Change(-1, -1);
		int num = -1;
		try
		{
			num = stream.EndRead(ares);
			ms.Write(buffer, 0, num);
			if (ms.Length > 32768)
			{
				SendError("Bad request", 400);
				Close(force_close: true);
				return;
			}
		}
		catch
		{
			if (ms != null && ms.Length > 0)
			{
				SendError();
			}
			if (sock != null)
			{
				CloseSocket();
				Unbind();
			}
			return;
		}
		if (num == 0)
		{
			CloseSocket();
			Unbind();
		}
		else if (ProcessInput(ms))
		{
			if (!context.HaveError && !context.Request.FinishInitialization())
			{
				Close(force_close: true);
				return;
			}
			if (context.HaveError)
			{
				SendError();
				Close(force_close: true);
				return;
			}
			if (!epl.BindContext(context))
			{
				SendError("Invalid host", 400);
				Close(force_close: true);
				return;
			}
			HttpListener listener = context.Listener;
			if (last_listener != listener)
			{
				RemoveConnection();
				listener.AddConnection(this);
				last_listener = listener;
			}
			context_bound = true;
			listener.RegisterContext(context);
		}
		else
		{
			stream.BeginRead(buffer, 0, 8192, onread_cb, this);
		}
	}

	private void RemoveConnection()
	{
		if (last_listener == null)
		{
			epl.RemoveConnection(this);
		}
		else
		{
			last_listener.RemoveConnection(this);
		}
	}

	private bool ProcessInput(MemoryStream ms)
	{
		byte[] array = ms.GetBuffer();
		int num = (int)ms.Length;
		int used = 0;
		while (true)
		{
			if (context.HaveError)
			{
				return true;
			}
			if (position >= num)
			{
				break;
			}
			string text;
			try
			{
				text = ReadLine(array, position, num - position, ref used);
				position += used;
			}
			catch
			{
				context.ErrorMessage = "Bad request";
				context.ErrorStatus = 400;
				return true;
			}
			if (text == null)
			{
				break;
			}
			if (text == "")
			{
				if (input_state != InputState.RequestLine)
				{
					current_line = null;
					ms = null;
					return true;
				}
				continue;
			}
			if (input_state == InputState.RequestLine)
			{
				context.Request.SetRequestLine(text);
				input_state = InputState.Headers;
				continue;
			}
			try
			{
				context.Request.AddHeader(text);
			}
			catch (Exception ex)
			{
				context.ErrorMessage = ex.Message;
				context.ErrorStatus = 400;
				return true;
			}
		}
		if (used == num)
		{
			ms.SetLength(0L);
			position = 0;
		}
		return false;
	}

	private string ReadLine(byte[] buffer, int offset, int len, ref int used)
	{
		if (current_line == null)
		{
			current_line = new StringBuilder(128);
		}
		int num = offset + len;
		used = 0;
		for (int i = offset; i < num; i++)
		{
			if (line_state == LineState.LF)
			{
				break;
			}
			used++;
			byte b = buffer[i];
			switch (b)
			{
			case 13:
				line_state = LineState.CR;
				break;
			case 10:
				line_state = LineState.LF;
				break;
			default:
				current_line.Append((char)b);
				break;
			}
		}
		string result = null;
		if (line_state == LineState.LF)
		{
			line_state = LineState.None;
			result = current_line.ToString();
			current_line.Length = 0;
		}
		return result;
	}

	public void SendError(string msg, int status)
	{
		try
		{
			HttpListenerResponse response = context.Response;
			response.StatusCode = status;
			response.ContentType = "text/html";
			string arg = HttpStatusDescription.Get(status);
			string s = ((msg == null) ? $"<h1>{arg}</h1>" : $"<h1>{arg} ({msg})</h1>");
			byte[] bytes = context.Response.ContentEncoding.GetBytes(s);
			response.Close(bytes, willBlock: false);
		}
		catch
		{
		}
	}

	public void SendError()
	{
		SendError(context.ErrorMessage, context.ErrorStatus);
	}

	private void Unbind()
	{
		if (context_bound)
		{
			epl.UnbindContext(context);
			context_bound = false;
		}
	}

	public void Close()
	{
		Close(force_close: false);
	}

	private void CloseSocket()
	{
		if (sock == null)
		{
			return;
		}
		try
		{
			sock.Close();
		}
		catch
		{
		}
		finally
		{
			sock = null;
		}
		RemoveConnection();
	}

	internal void Close(bool force_close)
	{
		if (sock != null)
		{
			GetResponseStream()?.Close();
			o_stream = null;
		}
		if (sock == null)
		{
			return;
		}
		force_close |= !context.Request.KeepAlive;
		if (!force_close)
		{
			force_close = context.Response.Headers["connection"] == "close";
		}
		if (!force_close && context.Request.FlushInput())
		{
			if (chunked && !context.Response.ForceCloseChunked)
			{
				reuses++;
				Unbind();
				Init();
				BeginReadRequest();
			}
			else
			{
				reuses++;
				Unbind();
				Init();
				BeginReadRequest();
			}
			return;
		}
		Socket socket = sock;
		sock = null;
		try
		{
			socket?.Shutdown(SocketShutdown.Both);
		}
		catch
		{
		}
		finally
		{
			socket?.Close();
		}
		Unbind();
		RemoveConnection();
	}
}
