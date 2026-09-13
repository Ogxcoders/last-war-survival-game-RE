using System.Runtime.InteropServices;

namespace System.Reflection.Emit;

[StructLayout(LayoutKind.Sequential)]
internal class ByRefType : SymbolType
{
	internal ByRefType(Type elementType)
		: base(elementType)
	{
	}

	internal override Type InternalResolve()
	{
		return m_baseType.InternalResolve().MakeByRefType();
	}

	protected override bool IsByRefImpl()
	{
		return true;
	}

	internal override string FormatName(string elementName)
	{
		if (elementName == null)
		{
			return null;
		}
		return elementName + "&";
	}

	public override Type MakeArrayType()
	{
		throw new ArgumentException("Cannot create an array type of a byref type");
	}

	public override Type MakeArrayType(int rank)
	{
		throw new ArgumentException("Cannot create an array type of a byref type");
	}

	public override Type MakeByRefType()
	{
		throw new ArgumentException("Cannot create a byref type of an already byref type");
	}

	public override Type MakePointerType()
	{
		throw new ArgumentException("Cannot create a pointer type of a byref type");
	}
}
