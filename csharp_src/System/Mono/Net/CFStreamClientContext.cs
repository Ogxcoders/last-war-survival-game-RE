using System;

namespace Mono.Net;

internal struct CFStreamClientContext
{
	public IntPtr Version;

	public IntPtr Info;

	public IntPtr Retain;

	public IntPtr Release;

	public IntPtr CopyDescription;
}
