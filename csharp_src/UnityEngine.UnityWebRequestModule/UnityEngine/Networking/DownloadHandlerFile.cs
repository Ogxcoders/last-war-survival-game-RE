using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace UnityEngine.Networking;

[StructLayout(LayoutKind.Sequential)]
[NativeHeader("Modules/UnityWebRequest/Public/DownloadHandler/DownloadHandlerVFS.h")]
public sealed class DownloadHandlerFile : DownloadHandler
{
	public extern bool removeFileOnAbort
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern IntPtr Create(DownloadHandlerFile obj, string path, bool append);

	private void InternalCreateVFS(string path, bool append)
	{
		string directoryName = Path.GetDirectoryName(path);
		if (!Directory.Exists(directoryName))
		{
			Directory.CreateDirectory(directoryName);
		}
		m_Ptr = Create(this, path, append);
	}

	public DownloadHandlerFile(string path)
	{
		InternalCreateVFS(path, append: false);
	}

	public DownloadHandlerFile(string path, bool append)
	{
		InternalCreateVFS(path, append);
	}

	protected override byte[] GetData()
	{
		throw new NotSupportedException("Raw data access is not supported");
	}

	protected override string GetText()
	{
		throw new NotSupportedException("String access is not supported");
	}
}
