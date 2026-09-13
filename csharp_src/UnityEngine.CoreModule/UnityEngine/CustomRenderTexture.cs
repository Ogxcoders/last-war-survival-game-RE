using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine;

[NativeHeader("Runtime/Graphics/CustomRenderTexture.h")]
[UsedByNativeCode]
public sealed class CustomRenderTexture : RenderTexture
{
	public extern Material material
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern Material initializationMaterial
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern Texture initializationTexture
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern CustomRenderTextureInitializationSource initializationSource
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public Color initializationColor
	{
		get
		{
			get_initializationColor_Injected(out var ret);
			return ret;
		}
		set
		{
			set_initializationColor_Injected(ref value);
		}
	}

	public extern CustomRenderTextureUpdateMode updateMode
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern CustomRenderTextureUpdateMode initializationMode
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern CustomRenderTextureUpdateZoneSpace updateZoneSpace
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern int shaderPass
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern uint cubemapFaceMask
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool doubleBuffered
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool wrapUpdateZones
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "CustomRenderTextureScripting::Create")]
	private static extern void Internal_CreateCustomRenderTexture([Writable] CustomRenderTexture rt);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("TriggerUpdate")]
	public extern void Update(int count);

	public void Update()
	{
		Update(1);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("TriggerInitialization")]
	public extern void Initialize();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void ClearUpdateZones();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "CustomRenderTextureScripting::GetUpdateZonesInternal", HasExplicitThis = true)]
	internal extern void GetUpdateZonesInternal([NotNull] object updateZones);

	public void GetUpdateZones(List<CustomRenderTextureUpdateZone> updateZones)
	{
		GetUpdateZonesInternal(updateZones);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "CustomRenderTextureScripting::SetUpdateZonesInternal", HasExplicitThis = true)]
	private extern void SetUpdateZonesInternal(CustomRenderTextureUpdateZone[] updateZones);

	public void SetUpdateZones(CustomRenderTextureUpdateZone[] updateZones)
	{
		if (updateZones == null)
		{
			throw new ArgumentNullException("updateZones");
		}
		SetUpdateZonesInternal(updateZones);
	}

	public CustomRenderTexture(int width, int height, RenderTextureFormat format, RenderTextureReadWrite readWrite)
		: this(width, height, RenderTexture.GetCompatibleFormat(format, readWrite))
	{
	}

	public CustomRenderTexture(int width, int height, RenderTextureFormat format)
		: this(width, height, RenderTexture.GetCompatibleFormat(format, RenderTextureReadWrite.Default))
	{
	}

	public CustomRenderTexture(int width, int height)
		: this(width, height, SystemInfo.GetGraphicsFormat(DefaultFormat.LDR))
	{
	}

	public CustomRenderTexture(int width, int height, DefaultFormat defaultFormat)
		: this(width, height, SystemInfo.GetGraphicsFormat(defaultFormat))
	{
	}

	public CustomRenderTexture(int width, int height, GraphicsFormat format)
	{
		if (ValidateFormat(format, FormatUsage.Render))
		{
			Internal_CreateCustomRenderTexture(this);
			this.width = width;
			this.height = height;
			base.graphicsFormat = format;
			SetSRGBReadWrite(GraphicsFormatUtility.IsSRGBFormat(format));
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_initializationColor_Injected(out Color ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_initializationColor_Injected(ref Color value);
}
