using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine;

[NativeHeader("Modules/UI/UIStructs.h")]
[NativeHeader("Modules/UI/Canvas.h")]
[RequireComponent(typeof(RectTransform))]
[NativeClass("UI::Canvas")]
public sealed class Canvas : Behaviour
{
	public delegate void WillRenderCanvases();

	public extern RenderMode renderMode
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool isRootCanvas
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public Rect pixelRect
	{
		get
		{
			get_pixelRect_Injected(out var ret);
			return ret;
		}
	}

	public extern float scaleFactor
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float referencePixelsPerUnit
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool overridePixelPerfect
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool pixelPerfect
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float planeDistance
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern int renderOrder
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern bool overrideSorting
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern int sortingOrder
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern int targetDisplay
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern int sortingLayerID
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern int cachedSortingLayerValue
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern AdditionalCanvasShaderChannels additionalShaderChannels
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern string sortingLayerName
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern Canvas rootCanvas
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public Vector2 renderingDisplaySize
	{
		get
		{
			get_renderingDisplaySize_Injected(out var ret);
			return ret;
		}
	}

	[NativeProperty("Camera", false, TargetType.Function)]
	public extern Camera worldCamera
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeProperty("SortingBucketNormalizedSize", false, TargetType.Function)]
	public extern float normalizedSortingGridSize
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[Obsolete("Setting normalizedSize via a int is not supported. Please use normalizedSortingGridSize", false)]
	[NativeProperty("SortingBucketNormalizedSize", false, TargetType.Function)]
	public extern int sortingGridNormalizedSize
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public static event WillRenderCanvases preWillRenderCanvases;

	public static event WillRenderCanvases willRenderCanvases;

	[MethodImpl(MethodImplOptions.InternalCall)]
	[Obsolete("Shared default material now used for text and general UI elements, call Canvas.GetDefaultCanvasMaterial()", false)]
	[FreeFunction("UI::GetDefaultUIMaterial")]
	public static extern Material GetDefaultCanvasTextMaterial();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("UI::GetDefaultUIMaterial")]
	public static extern Material GetDefaultCanvasMaterial();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("UI::GetETC1SupportedCanvasMaterial")]
	public static extern Material GetETC1SupportedCanvasMaterial();

	public static void ForceUpdateCanvases()
	{
		SendPreWillRenderCanvases();
		SendWillRenderCanvases();
	}

	[RequiredByNativeCode]
	private static void SendPreWillRenderCanvases()
	{
		Canvas.preWillRenderCanvases?.Invoke();
	}

	[RequiredByNativeCode]
	private static void SendWillRenderCanvases()
	{
		Canvas.willRenderCanvases?.Invoke();
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_pixelRect_Injected(out Rect ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_renderingDisplaySize_Injected(out Vector2 ret);
}
