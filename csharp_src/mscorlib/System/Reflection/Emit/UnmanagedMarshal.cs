using System.Runtime.InteropServices;

namespace System.Reflection.Emit;

[Serializable]
[StructLayout(LayoutKind.Sequential)]
[Obsolete("An alternate API is available: Emit the MarshalAs custom attribute instead.")]
[ComVisible(true)]
public sealed class UnmanagedMarshal
{
	private int count;

	private UnmanagedType t;

	private UnmanagedType tbase;

	private string guid;

	private string mcookie;

	private string marshaltype;

	internal Type marshaltyperef;

	private int param_num;

	private bool has_size;

	public UnmanagedType BaseType
	{
		get
		{
			if (t == UnmanagedType.LPArray)
			{
				throw new ArgumentException();
			}
			if (t == UnmanagedType.SafeArray)
			{
				throw new ArgumentException();
			}
			return tbase;
		}
	}

	public int ElementCount => count;

	public UnmanagedType GetUnmanagedType => t;

	public Guid IIDGuid => new Guid(guid);

	private UnmanagedMarshal(UnmanagedType maint, int cnt)
	{
		count = cnt;
		t = maint;
		tbase = maint;
	}

	private UnmanagedMarshal(UnmanagedType maint, UnmanagedType elemt)
	{
		count = 0;
		t = maint;
		tbase = elemt;
	}

	public static UnmanagedMarshal DefineByValArray(int elemCount)
	{
		return new UnmanagedMarshal(UnmanagedType.ByValArray, elemCount);
	}

	public static UnmanagedMarshal DefineByValTStr(int elemCount)
	{
		return new UnmanagedMarshal(UnmanagedType.ByValTStr, elemCount);
	}

	public static UnmanagedMarshal DefineLPArray(UnmanagedType elemType)
	{
		return new UnmanagedMarshal(UnmanagedType.LPArray, elemType);
	}

	public static UnmanagedMarshal DefineSafeArray(UnmanagedType elemType)
	{
		return new UnmanagedMarshal(UnmanagedType.SafeArray, elemType);
	}

	public static UnmanagedMarshal DefineUnmanagedMarshal(UnmanagedType unmanagedType)
	{
		return new UnmanagedMarshal(unmanagedType, unmanagedType);
	}

	internal static UnmanagedMarshal DefineCustom(Type typeref, string cookie, string mtype, Guid id)
	{
		UnmanagedMarshal unmanagedMarshal = new UnmanagedMarshal(UnmanagedType.CustomMarshaler, UnmanagedType.CustomMarshaler);
		unmanagedMarshal.mcookie = cookie;
		unmanagedMarshal.marshaltype = mtype;
		unmanagedMarshal.marshaltyperef = typeref;
		if (id == Guid.Empty)
		{
			unmanagedMarshal.guid = string.Empty;
		}
		else
		{
			unmanagedMarshal.guid = id.ToString();
		}
		return unmanagedMarshal;
	}

	internal static UnmanagedMarshal DefineLPArrayInternal(UnmanagedType elemType, int sizeConst, int sizeParamIndex)
	{
		return new UnmanagedMarshal(UnmanagedType.LPArray, elemType)
		{
			count = sizeConst,
			param_num = sizeParamIndex,
			has_size = true
		};
	}
}
