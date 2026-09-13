using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering;

[UsedByNativeCode]
[NativeHeader("Runtime/Shaders/ShaderKeywords.h")]
[NativeHeader("Runtime/Graphics/ShaderScriptBindings.h")]
public struct ShaderKeyword
{
	internal const int k_MaxShaderKeywords = 448;

	private const int k_InvalidKeyword = -1;

	internal int m_KeywordIndex;

	public int index => m_KeywordIndex;

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::GetGlobalKeywordIndex")]
	internal static extern int GetGlobalKeywordIndex(string keyword);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ShaderScripting::GetKeywordIndex")]
	internal static extern int GetKeywordIndex(Shader shader, string keyword);

	[FreeFunction("ShaderScripting::GetGlobalKeywordName")]
	public static string GetGlobalKeywordName(ShaderKeyword index)
	{
		return GetGlobalKeywordName_Injected(ref index);
	}

	[FreeFunction("ShaderScripting::GetGlobalKeywordType")]
	public static ShaderKeywordType GetGlobalKeywordType(ShaderKeyword index)
	{
		return GetGlobalKeywordType_Injected(ref index);
	}

	[FreeFunction("ShaderScripting::IsKeywordLocal")]
	public static bool IsKeywordLocal(ShaderKeyword index)
	{
		return IsKeywordLocal_Injected(ref index);
	}

	[FreeFunction("ShaderScripting::GetKeywordName")]
	public static string GetKeywordName(Shader shader, ShaderKeyword index)
	{
		return GetKeywordName_Injected(shader, ref index);
	}

	[FreeFunction("ShaderScripting::GetKeywordType")]
	public static ShaderKeywordType GetKeywordType(Shader shader, ShaderKeyword index)
	{
		return GetKeywordType_Injected(shader, ref index);
	}

	internal ShaderKeyword(int keywordIndex)
	{
		m_KeywordIndex = keywordIndex;
	}

	public ShaderKeyword(string keywordName)
	{
		m_KeywordIndex = GetGlobalKeywordIndex(keywordName);
	}

	public ShaderKeyword(Shader shader, string keywordName)
	{
		m_KeywordIndex = GetKeywordIndex(shader, keywordName);
	}

	public bool IsValid()
	{
		return m_KeywordIndex >= 0 && m_KeywordIndex < 448 && m_KeywordIndex != -1;
	}

	[Obsolete("GetKeywordType is deprecated. Use ShaderKeyword.GetGlobalKeywordType instead.")]
	public ShaderKeywordType GetKeywordType()
	{
		return GetGlobalKeywordType(this);
	}

	[Obsolete("GetKeywordName is deprecated. Use ShaderKeyword.GetGlobalKeywordName instead.")]
	public string GetKeywordName()
	{
		return GetGlobalKeywordName(this);
	}

	[Obsolete("GetName() has been deprecated. Use ShaderKeyword.GetGlobalKeywordName instead.")]
	public string GetName()
	{
		return GetKeywordName();
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern string GetGlobalKeywordName_Injected(ref ShaderKeyword index);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern ShaderKeywordType GetGlobalKeywordType_Injected(ref ShaderKeyword index);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool IsKeywordLocal_Injected(ref ShaderKeyword index);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern string GetKeywordName_Injected(Shader shader, ref ShaderKeyword index);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern ShaderKeywordType GetKeywordType_Injected(Shader shader, ref ShaderKeyword index);
}
