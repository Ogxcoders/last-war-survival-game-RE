using System.Collections.Generic;
using System.Reflection;

namespace FibMatrix.Rendering;

public static class RenderStateRegisterCenter
{
	public static readonly Dictionary<string, RenderStateRegister> RenderStateRegisters;

	public static RenderStateRegister DepthTextureStateRegister;

	public static RenderStateRegister DepthNormalTextureStateRegister;

	public static RenderStateRegister OpaqueTextureStateRegister;

	public static RenderStateRegister PostProcessStateRegister;

	static RenderStateRegisterCenter()
	{
		RenderStateRegisters = new Dictionary<string, RenderStateRegister>();
		DepthTextureStateRegister = new DepthTextureStateRegister();
		DepthNormalTextureStateRegister = new DepthNormalTextureStateRegister();
		OpaqueTextureStateRegister = new OpaqueTextureStateRegister();
		PostProcessStateRegister = new PostProcessStateRegister();
		FieldInfo[] fields = typeof(RenderStateRegisterCenter).GetFields(BindingFlags.Static | BindingFlags.Public);
		foreach (FieldInfo fieldInfo in fields)
		{
			if (fieldInfo.FieldType.IsSubclassOf(typeof(RenderStateRegister)) || fieldInfo.FieldType.IsEquivalentTo(typeof(RenderStateRegister)))
			{
				RenderStateRegisters.Add(fieldInfo.Name, fieldInfo.GetValue(null) as RenderStateRegister);
			}
		}
	}
}
