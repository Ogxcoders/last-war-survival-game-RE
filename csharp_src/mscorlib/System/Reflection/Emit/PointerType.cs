using System.Runtime.InteropServices;

namespace System.Reflection.Emit;

[StructLayout(LayoutKind.Sequential)]
internal class PointerType : SymbolType
{
	internal PointerType(Type elementType)
		: base(elementType)
	{
	}

	internal override Type InternalResolve()
	{
		return m_baseType.InternalResolve().MakePointerType();
	}

	protected override bool IsPointerImpl()
	{
		return true;
	}

	internal override string FormatName(string elementName)
	{
		if (elementName == null)
		{
			return null;
		}
		return elementName + "*";
	}
}
