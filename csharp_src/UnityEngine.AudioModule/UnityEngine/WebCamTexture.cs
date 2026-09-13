using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine;

[NativeHeader("Runtime/Video/BaseWebCamTexture.h")]
public sealed class WebCamTexture : Texture
{
	public Vector2? autoFocusPoint
	{
		get
		{
			return (internalAutoFocusPoint.x < 0f) ? ((Vector2?)null) : new Vector2?(internalAutoFocusPoint);
		}
		set
		{
			internalAutoFocusPoint = ((!value.HasValue) ? new Vector2(-1f, -1f) : value.Value);
		}
	}

	internal Vector2 internalAutoFocusPoint
	{
		get
		{
			get_internalAutoFocusPoint_Injected(out var ret);
			return ret;
		}
		set
		{
			set_internalAutoFocusPoint_Injected(ref value);
		}
	}

	public extern bool isDepth
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern bool isPlaying
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		get;
	}

	public extern string deviceName
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		set;
	}

	public extern float requestedFPS
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		set;
	}

	public extern int requestedWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		set;
	}

	public extern int requestedHeight
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		set;
	}

	public static extern WebCamDevice[] devices
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		get;
	}

	public extern int videoRotationAngle
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		get;
	}

	public extern bool videoVerticallyMirrored
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		get;
	}

	public extern bool didUpdateThisFrame
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[GeneratedByOldBindingsGenerator]
	private static extern void Internal_CreateWebCamTexture([Writable] WebCamTexture self, string scriptingDevice, int requestedWidth, int requestedHeight, int maxFramerate);

	public WebCamTexture(string deviceName, int requestedWidth, int requestedHeight, int requestedFPS)
	{
		Internal_CreateWebCamTexture(this, deviceName, requestedWidth, requestedHeight, requestedFPS);
	}

	public WebCamTexture(string deviceName, int requestedWidth, int requestedHeight)
	{
		Internal_CreateWebCamTexture(this, deviceName, requestedWidth, requestedHeight, 0);
	}

	public WebCamTexture(string deviceName)
	{
		Internal_CreateWebCamTexture(this, deviceName, 0, 0, 0);
	}

	public WebCamTexture(int requestedWidth, int requestedHeight, int requestedFPS)
	{
		Internal_CreateWebCamTexture(this, "", requestedWidth, requestedHeight, requestedFPS);
	}

	public WebCamTexture(int requestedWidth, int requestedHeight)
	{
		Internal_CreateWebCamTexture(this, "", requestedWidth, requestedHeight, 0);
	}

	public WebCamTexture()
	{
		Internal_CreateWebCamTexture(this, "", 0, 0, 0);
	}

	public void Play()
	{
		INTERNAL_CALL_Play(this);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[GeneratedByOldBindingsGenerator]
	private static extern void INTERNAL_CALL_Play(WebCamTexture self);

	public void Pause()
	{
		INTERNAL_CALL_Pause(this);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[GeneratedByOldBindingsGenerator]
	private static extern void INTERNAL_CALL_Pause(WebCamTexture self);

	public void Stop()
	{
		INTERNAL_CALL_Stop(this);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[GeneratedByOldBindingsGenerator]
	private static extern void INTERNAL_CALL_Stop(WebCamTexture self);

	public Color GetPixel(int x, int y)
	{
		INTERNAL_CALL_GetPixel(this, x, y, out var value);
		return value;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[GeneratedByOldBindingsGenerator]
	private static extern void INTERNAL_CALL_GetPixel(WebCamTexture self, int x, int y, out Color value);

	public Color[] GetPixels()
	{
		return GetPixels(0, 0, width, height);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[GeneratedByOldBindingsGenerator]
	public extern Color[] GetPixels(int x, int y, int blockWidth, int blockHeight);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[GeneratedByOldBindingsGenerator]
	public extern Color32[] GetPixels32([DefaultValue("null")] Color32[] colors);

	[ExcludeFromDocs]
	public Color32[] GetPixels32()
	{
		Color32[] colors = null;
		return GetPixels32(colors);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_internalAutoFocusPoint_Injected(out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_internalAutoFocusPoint_Injected(ref Vector2 value);
}
