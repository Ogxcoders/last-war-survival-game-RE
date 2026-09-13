using System.Runtime.InteropServices;
using System.Text;

namespace System.Reflection.Emit;

[StructLayout(LayoutKind.Sequential)]
internal class ArrayType : SymbolType
{
	private int rank;

	internal ArrayType(Type elementType, int rank)
		: base(elementType)
	{
		this.rank = rank;
	}

	internal int GetEffectiveRank()
	{
		return rank;
	}

	internal override Type InternalResolve()
	{
		Type type = m_baseType.InternalResolve();
		if (rank == 0)
		{
			return type.MakeArrayType();
		}
		return type.MakeArrayType(rank);
	}

	internal override Type RuntimeResolve()
	{
		Type type = m_baseType.RuntimeResolve();
		if (rank == 0)
		{
			return type.MakeArrayType();
		}
		return type.MakeArrayType(rank);
	}

	protected override bool IsArrayImpl()
	{
		return true;
	}

	public override int GetArrayRank()
	{
		if (rank != 0)
		{
			return rank;
		}
		return 1;
	}

	internal override string FormatName(string elementName)
	{
		if (elementName == null)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder(elementName);
		stringBuilder.Append("[");
		for (int i = 1; i < rank; i++)
		{
			stringBuilder.Append(",");
		}
		if (rank == 1)
		{
			stringBuilder.Append("*");
		}
		stringBuilder.Append("]");
		return stringBuilder.ToString();
	}
}
