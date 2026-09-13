using System.Runtime.InteropServices;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net.Security;

internal class SecurityBuffer
{
	public int size;

	public SecurityBufferType type;

	public byte[] token;

	public SafeHandle unmanagedToken;

	public int offset;

	public SecurityBuffer(byte[] data, int offset, int size, SecurityBufferType tokentype)
	{
		if (offset < 0 || offset > ((data != null) ? data.Length : 0))
		{
			NetEventSource.Fail(this, $"'offset' out of range.  [{offset}]", ".ctor");
		}
		if (size < 0 || size > ((data != null) ? (data.Length - offset) : 0))
		{
			NetEventSource.Fail(this, $"'size' out of range.  [{size}]", ".ctor");
		}
		this.offset = ((data != null && offset >= 0) ? Math.Min(offset, data.Length) : 0);
		this.size = ((data != null && size >= 0) ? Math.Min(size, data.Length - this.offset) : 0);
		type = tokentype;
		token = ((size == 0) ? null : data);
	}

	public SecurityBuffer(byte[] data, SecurityBufferType tokentype)
	{
		size = ((data != null) ? data.Length : 0);
		type = tokentype;
		token = ((size == 0) ? null : data);
	}

	public SecurityBuffer(int size, SecurityBufferType tokentype)
	{
		if (size < 0)
		{
			NetEventSource.Fail(this, $"'size' out of range.  [{size}]", ".ctor");
		}
		this.size = size;
		type = tokentype;
		token = ((size == 0) ? null : new byte[size]);
	}

	public SecurityBuffer(ChannelBinding binding)
	{
		size = binding?.Size ?? 0;
		type = SecurityBufferType.SECBUFFER_CHANNEL_BINDINGS;
		unmanagedToken = binding;
	}
}
