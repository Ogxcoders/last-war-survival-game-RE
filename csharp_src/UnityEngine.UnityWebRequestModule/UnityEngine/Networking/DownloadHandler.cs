using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Networking;

[StructLayout(LayoutKind.Sequential)]
[NativeHeader("Modules/UnityWebRequest/Public/DownloadHandler/DownloadHandler.h")]
public class DownloadHandler : IDisposable
{
	[NonSerialized]
	[VisibleToOtherModules]
	internal IntPtr m_Ptr;

	public bool isDone => IsDone();

	public byte[] data => GetData();

	public string text => GetText();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true)]
	private extern void Release();

	[VisibleToOtherModules]
	internal DownloadHandler()
	{
	}

	~DownloadHandler()
	{
		Dispose();
	}

	public void Dispose()
	{
		if (m_Ptr != IntPtr.Zero)
		{
			Release();
			m_Ptr = IntPtr.Zero;
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern bool IsDone();

	protected virtual byte[] GetData()
	{
		return null;
	}

	protected virtual string GetText()
	{
		byte[] array = GetData();
		if (array != null && array.Length != 0)
		{
			return GetTextEncoder().GetString(array, 0, array.Length);
		}
		return "";
	}

	private Encoding GetTextEncoder()
	{
		string contentType = GetContentType();
		if (!string.IsNullOrEmpty(contentType))
		{
			int num = contentType.IndexOf("charset", StringComparison.OrdinalIgnoreCase);
			if (num > -1)
			{
				int num2 = contentType.IndexOf('=', num);
				if (num2 > -1)
				{
					string text = contentType.Substring(num2 + 1).Trim().Trim('\'', '"')
						.Trim();
					int num3 = text.IndexOf(';');
					if (num3 > -1)
					{
						text = text.Substring(0, num3);
					}
					try
					{
						return Encoding.GetEncoding(text);
					}
					catch (ArgumentException ex)
					{
						Debug.LogWarning($"Unsupported encoding '{text}': {ex.Message}");
					}
					catch (NotSupportedException ex2)
					{
						Debug.LogWarning($"Unsupported encoding '{text}': {ex2.Message}");
					}
				}
			}
		}
		return Encoding.UTF8;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern string GetContentType();

	[UsedByNativeCode]
	protected virtual bool ReceiveData(byte[] data, int dataLength)
	{
		return true;
	}

	[RequiredByNativeCode]
	protected virtual void ReceiveContentLengthHeader(ulong contentLength)
	{
		ReceiveContentLength((int)contentLength);
	}

	[Obsolete("Use ReceiveContentLengthHeader")]
	protected virtual void ReceiveContentLength(int contentLength)
	{
	}

	[UsedByNativeCode]
	protected virtual void CompleteContent()
	{
	}

	[UsedByNativeCode]
	protected virtual float GetProgress()
	{
		return 0f;
	}

	protected static T GetCheckedDownloader<T>(UnityWebRequest www) where T : DownloadHandler
	{
		if (www == null)
		{
			throw new NullReferenceException("Cannot get content from a null UnityWebRequest object");
		}
		if (!www.isDone)
		{
			throw new InvalidOperationException("Cannot get content from an unfinished UnityWebRequest object");
		}
		if (www.isNetworkError)
		{
			throw new InvalidOperationException(www.error);
		}
		return (T)www.downloadHandler;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[VisibleToOtherModules]
	[NativeThrows]
	internal static extern byte[] InternalGetByteArray(DownloadHandler dh);
}
