using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace System.Runtime.CompilerServices;

public abstract class DebugInfoGenerator
{
	public static DebugInfoGenerator CreatePdbGenerator()
	{
		throw new PlatformNotSupportedException();
	}

	public abstract void MarkSequencePoint(LambdaExpression method, int ilOffset, DebugInfoExpression sequencePoint);

	internal virtual void MarkSequencePoint(LambdaExpression method, MethodBase methodBase, ILGenerator ilg, DebugInfoExpression sequencePoint)
	{
		MarkSequencePoint(method, ilg.ILOffset, sequencePoint);
	}

	internal virtual void SetLocalName(LocalBuilder localBuilder, string name)
	{
	}
}
