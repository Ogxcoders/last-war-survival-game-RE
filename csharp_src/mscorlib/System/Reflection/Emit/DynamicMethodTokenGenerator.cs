namespace System.Reflection.Emit;

internal class DynamicMethodTokenGenerator : TokenGenerator
{
	private DynamicMethod m;

	public DynamicMethodTokenGenerator(DynamicMethod m)
	{
		this.m = m;
	}

	public int GetToken(string str)
	{
		return m.AddRef(str);
	}

	public int GetToken(MethodBase method, Type[] opt_param_types)
	{
		throw new InvalidOperationException();
	}

	public int GetToken(MemberInfo member, bool create_open_instance)
	{
		return m.AddRef(member);
	}

	public int GetToken(SignatureHelper helper)
	{
		return m.AddRef(helper);
	}
}
