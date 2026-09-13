using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;

namespace System.Threading;

[Serializable]
public sealed class CompressedStack : ISerializable
{
	private ArrayList _list;

	internal IList List => _list;

	internal CompressedStack(int length)
	{
		if (length > 0)
		{
			_list = new ArrayList(length);
		}
	}

	internal CompressedStack(CompressedStack cs)
	{
		if (cs != null && cs._list != null)
		{
			_list = (ArrayList)cs._list.Clone();
		}
	}

	[ComVisible(false)]
	public CompressedStack CreateCopy()
	{
		return new CompressedStack(this);
	}

	public static CompressedStack Capture()
	{
		throw new NotSupportedException();
	}

	[SecurityCritical]
	public static CompressedStack GetCompressedStack()
	{
		throw new NotSupportedException();
	}

	[MonoTODO("incomplete")]
	[SecurityCritical]
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
	}

	[SecurityCritical]
	public static void Run(CompressedStack compressedStack, ContextCallback callback, object state)
	{
		throw new NotSupportedException();
	}

	internal bool Equals(CompressedStack cs)
	{
		if (IsEmpty())
		{
			return cs.IsEmpty();
		}
		if (cs.IsEmpty())
		{
			return false;
		}
		if (_list.Count != cs._list.Count)
		{
			return false;
		}
		return true;
	}

	internal bool IsEmpty()
	{
		if (_list != null)
		{
			return _list.Count == 0;
		}
		return true;
	}
}
