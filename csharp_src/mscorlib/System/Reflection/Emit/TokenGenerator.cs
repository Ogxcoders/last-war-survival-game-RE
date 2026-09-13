namespace System.Reflection.Emit;

internal interface TokenGenerator
{
	int GetToken(string str);

	int GetToken(MemberInfo member, bool create_open_instance);

	int GetToken(MethodBase method, Type[] opt_param_types);

	int GetToken(SignatureHelper helper);
}
