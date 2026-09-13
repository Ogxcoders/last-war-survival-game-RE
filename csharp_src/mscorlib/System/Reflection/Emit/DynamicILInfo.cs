using System.Runtime.InteropServices;

namespace System.Reflection.Emit;

[ComVisible(true)]
public class DynamicILInfo
{
	private DynamicMethod method;

	public DynamicMethod DynamicMethod => method;

	internal DynamicILInfo()
	{
	}

	internal DynamicILInfo(DynamicMethod method)
	{
		this.method = method;
	}

	[MonoTODO]
	public int GetTokenFor(byte[] signature)
	{
		throw new NotImplementedException();
	}

	public int GetTokenFor(DynamicMethod method)
	{
		return this.method.GetILGenerator().TokenGenerator.GetToken(method, create_open_instance: false);
	}

	public int GetTokenFor(RuntimeFieldHandle field)
	{
		return method.GetILGenerator().TokenGenerator.GetToken(FieldInfo.GetFieldFromHandle(field), create_open_instance: false);
	}

	public int GetTokenFor(RuntimeMethodHandle method)
	{
		MethodBase methodFromHandle = MethodBase.GetMethodFromHandle(method);
		return this.method.GetILGenerator().TokenGenerator.GetToken(methodFromHandle, create_open_instance: false);
	}

	public int GetTokenFor(RuntimeTypeHandle type)
	{
		Type typeFromHandle = Type.GetTypeFromHandle(type);
		return method.GetILGenerator().TokenGenerator.GetToken(typeFromHandle, create_open_instance: false);
	}

	public int GetTokenFor(string literal)
	{
		return method.GetILGenerator().TokenGenerator.GetToken(literal);
	}

	[MonoTODO]
	public int GetTokenFor(RuntimeMethodHandle method, RuntimeTypeHandle contextType)
	{
		throw new NotImplementedException();
	}

	[MonoTODO]
	public int GetTokenFor(RuntimeFieldHandle field, RuntimeTypeHandle contextType)
	{
		throw new NotImplementedException();
	}

	public void SetCode(byte[] code, int maxStackSize)
	{
		if (code == null)
		{
			throw new ArgumentNullException("code");
		}
		method.GetILGenerator().SetCode(code, maxStackSize);
	}

	[CLSCompliant(false)]
	public unsafe void SetCode(byte* code, int codeSize, int maxStackSize)
	{
		if (code == null)
		{
			throw new ArgumentNullException("code");
		}
		method.GetILGenerator().SetCode(code, codeSize, maxStackSize);
	}

	[MonoTODO]
	public void SetExceptions(byte[] exceptions)
	{
		throw new NotImplementedException();
	}

	[MonoTODO]
	[CLSCompliant(false)]
	public unsafe void SetExceptions(byte* exceptions, int exceptionsSize)
	{
		throw new NotImplementedException();
	}

	[MonoTODO]
	public void SetLocalSignature(byte[] localSignature)
	{
		throw new NotImplementedException();
	}

	[CLSCompliant(false)]
	public unsafe void SetLocalSignature(byte* localSignature, int signatureSize)
	{
		byte[] array = new byte[signatureSize];
		for (int i = 0; i < signatureSize; i++)
		{
			array[i] = localSignature[i];
		}
	}
}
