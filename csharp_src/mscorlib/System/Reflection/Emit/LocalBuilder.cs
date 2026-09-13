using System.Runtime.InteropServices;

namespace System.Reflection.Emit;

[StructLayout(LayoutKind.Sequential)]
public sealed class LocalBuilder : LocalVariableInfo
{
	private string name;

	internal ILGenerator ilgen;

	private int startOffset;

	private int endOffset;

	public override Type LocalType => type;

	public override bool IsPinned => is_pinned;

	public override int LocalIndex => position;

	internal string Name => name;

	internal int StartOffset => startOffset;

	internal int EndOffset => endOffset;

	internal LocalBuilder(Type t, ILGenerator ilgen)
	{
		type = t;
		this.ilgen = ilgen;
	}

	public void SetLocalSymInfo(string name, int startOffset, int endOffset)
	{
		this.name = name;
		this.startOffset = startOffset;
		this.endOffset = endOffset;
	}

	public void SetLocalSymInfo(string name)
	{
		SetLocalSymInfo(name, 0, 0);
	}
}
