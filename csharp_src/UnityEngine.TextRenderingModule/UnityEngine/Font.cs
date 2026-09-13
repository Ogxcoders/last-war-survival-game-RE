using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine;

[NativeHeader("Modules/TextRendering/Public/Font.h")]
[NativeClass("TextRendering::Font")]
[NativeHeader("Modules/TextRendering/Public/FontImpl.h")]
[StaticAccessor("TextRenderingPrivate", StaticAccessorType.DoubleColon)]
public sealed class Font : Object
{
	public delegate void FontTextureRebuildCallback();

	public extern Material material
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern string[] fontNames
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool dynamic
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern int ascent
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern int fontSize
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern CharacterInfo[] characterInfo
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction("TextRenderingPrivate::GetFontCharacterInfo", HasExplicitThis = true)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction("TextRenderingPrivate::SetFontCharacterInfo", HasExplicitThis = true)]
		set;
	}

	[NativeProperty("LineSpacing", false, TargetType.Function)]
	public extern int lineHeight
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	[Obsolete("Font.textureRebuildCallback has been deprecated. Use Font.textureRebuilt instead.")]
	public FontTextureRebuildCallback textureRebuildCallback
	{
		get
		{
			return this.m_FontTextureRebuildCallback;
		}
		set
		{
			this.m_FontTextureRebuildCallback = value;
		}
	}

	public static event Action<Font> textureRebuilt;

	private event FontTextureRebuildCallback m_FontTextureRebuildCallback;

	public Font()
	{
		Internal_CreateFont(this, null);
	}

	public Font(string name)
	{
		if (Path.GetDirectoryName(name) == string.Empty)
		{
			Internal_CreateFont(this, name);
		}
		else
		{
			Internal_CreateFontFromPath(this, name);
		}
	}

	private Font(string[] names, int size)
	{
		Internal_CreateDynamicFont(this, names, size);
	}

	public static Font CreateDynamicFontFromOSFont(string fontname, int size)
	{
		return new Font(new string[1] { fontname }, size);
	}

	public static Font CreateDynamicFontFromOSFont(string[] fontnames, int size)
	{
		return new Font(fontnames, size);
	}

	[RequiredByNativeCode]
	internal static void InvokeTextureRebuilt_Internal(Font font)
	{
		Font.textureRebuilt?.Invoke(font);
		font.m_FontTextureRebuildCallback?.Invoke();
	}

	public static int GetMaxVertsForString(string str)
	{
		return str.Length * 4 + 4;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern Font GetDefault();

	public bool HasCharacter(char c)
	{
		return HasCharacter((int)c);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern bool HasCharacter(int c);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern string[] GetOSInstalledFontNames();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern string[] GetPathsToOSFonts();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Internal_CreateFont([Writable] Font self, string name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Internal_CreateFontFromPath([Writable] Font self, string fontPath);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Internal_CreateDynamicFont([Writable] Font self, string[] _names, int size);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("TextRenderingPrivate::GetCharacterInfo", HasExplicitThis = true)]
	public extern bool GetCharacterInfo(char ch, out CharacterInfo info, [DefaultValue("0")] int size, [DefaultValue("FontStyle.Normal")] FontStyle style);

	[ExcludeFromDocs]
	public bool GetCharacterInfo(char ch, out CharacterInfo info, int size)
	{
		return GetCharacterInfo(ch, out info, size, FontStyle.Normal);
	}

	[ExcludeFromDocs]
	public bool GetCharacterInfo(char ch, out CharacterInfo info)
	{
		return GetCharacterInfo(ch, out info, 0, FontStyle.Normal);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void RequestCharactersInTexture(string characters, [DefaultValue("0")] int size, [DefaultValue("FontStyle.Normal")] FontStyle style);

	[ExcludeFromDocs]
	public void RequestCharactersInTexture(string characters, int size)
	{
		RequestCharactersInTexture(characters, size, FontStyle.Normal);
	}

	[ExcludeFromDocs]
	public void RequestCharactersInTexture(string characters)
	{
		RequestCharactersInTexture(characters, 0, FontStyle.Normal);
	}
}
