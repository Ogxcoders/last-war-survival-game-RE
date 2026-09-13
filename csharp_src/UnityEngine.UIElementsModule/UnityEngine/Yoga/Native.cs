using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Yoga;

[NativeHeader("Modules/UIElements/YogaNative.bindings.h")]
internal static class Native
{
	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	private static extern IntPtr YGNodeNew();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern IntPtr YGNodeNewWithConfig(IntPtr config);

	public static void YGNodeFree(IntPtr ygNode)
	{
		if (!(ygNode == IntPtr.Zero))
		{
			YGNodeFreeInternal(ygNode);
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "YGNodeFree", IsThreadSafe = true)]
	private static extern void YGNodeFreeInternal(IntPtr ygNode);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeReset(IntPtr node);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGSetManagedObject(IntPtr ygNode, YogaNode node);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern IntPtr YGConfigGetDefault();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern IntPtr YGConfigNew();

	public static void YGConfigFree(IntPtr config)
	{
		if (!(config == IntPtr.Zero))
		{
			YGConfigFreeInternal(config);
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "YGConfigFree", IsThreadSafe = true)]
	private static extern void YGConfigFreeInternal(IntPtr config);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern int YGNodeGetInstanceCount();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern int YGConfigGetInstanceCount();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGConfigSetExperimentalFeatureEnabled(IntPtr config, YogaExperimentalFeature feature, bool enabled);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern bool YGConfigIsExperimentalFeatureEnabled(IntPtr config, YogaExperimentalFeature feature);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGConfigSetUseWebDefaults(IntPtr config, bool useWebDefaults);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern bool YGConfigGetUseWebDefaults(IntPtr config);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGConfigSetPointScaleFactor(IntPtr config, float pixelsInPoint);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeInsertChild(IntPtr node, IntPtr child, uint index);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeRemoveChild(IntPtr node, IntPtr child);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeCalculateLayout(IntPtr node, float availableWidth, float availableHeight, YogaDirection parentDirection);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeMarkDirty(IntPtr node);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern bool YGNodeIsDirty(IntPtr node);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodePrint(IntPtr node, YogaPrintOptions options);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeCopyStyle(IntPtr dstNode, IntPtr srcNode);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "YogaCallback::SetMeasureFunc")]
	public static extern void YGNodeSetMeasureFunc(IntPtr node);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "YogaCallback::RemoveMeasureFunc")]
	public static extern void YGNodeRemoveMeasureFunc(IntPtr node);

	[RequiredByNativeCode]
	public unsafe static void YGNodeMeasureInvoke(YogaNode node, float width, YogaMeasureMode widthMode, float height, YogaMeasureMode heightMode, IntPtr returnValueAddress)
	{
		*(YogaSize*)(void*)returnValueAddress = YogaNode.MeasureInternal(node, width, widthMode, height, heightMode);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "YogaCallback::SetBaselineFunc")]
	public static extern void YGNodeSetBaselineFunc(IntPtr node);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "YogaCallback::RemoveBaselineFunc")]
	public static extern void YGNodeRemoveBaselineFunc(IntPtr node);

	[RequiredByNativeCode]
	public unsafe static void YGNodeBaselineInvoke(YogaNode node, float width, float height, IntPtr returnValueAddress)
	{
		*(float*)(void*)returnValueAddress = YogaNode.BaselineInternal(node, width, height);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeSetHasNewLayout(IntPtr node, bool hasNewLayout);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern bool YGNodeGetHasNewLayout(IntPtr node);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetDirection(IntPtr node, YogaDirection direction);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern YogaDirection YGNodeStyleGetDirection(IntPtr node);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetFlexDirection(IntPtr node, YogaFlexDirection flexDirection);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern YogaFlexDirection YGNodeStyleGetFlexDirection(IntPtr node);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetJustifyContent(IntPtr node, YogaJustify justifyContent);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern YogaJustify YGNodeStyleGetJustifyContent(IntPtr node);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetAlignContent(IntPtr node, YogaAlign alignContent);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern YogaAlign YGNodeStyleGetAlignContent(IntPtr node);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetAlignItems(IntPtr node, YogaAlign alignItems);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern YogaAlign YGNodeStyleGetAlignItems(IntPtr node);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetAlignSelf(IntPtr node, YogaAlign alignSelf);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern YogaAlign YGNodeStyleGetAlignSelf(IntPtr node);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetPositionType(IntPtr node, YogaPositionType positionType);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern YogaPositionType YGNodeStyleGetPositionType(IntPtr node);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetFlexWrap(IntPtr node, YogaWrap flexWrap);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern YogaWrap YGNodeStyleGetFlexWrap(IntPtr node);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetOverflow(IntPtr node, YogaOverflow flexWrap);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern YogaOverflow YGNodeStyleGetOverflow(IntPtr node);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetDisplay(IntPtr node, YogaDisplay display);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern YogaDisplay YGNodeStyleGetDisplay(IntPtr node);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetFlex(IntPtr node, float flex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetFlexGrow(IntPtr node, float flexGrow);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern float YGNodeStyleGetFlexGrow(IntPtr node);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetFlexShrink(IntPtr node, float flexShrink);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern float YGNodeStyleGetFlexShrink(IntPtr node);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetFlexBasis(IntPtr node, float flexBasis);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetFlexBasisPercent(IntPtr node, float flexBasis);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetFlexBasisAuto(IntPtr node);

	[FreeFunction]
	public static YogaValue YGNodeStyleGetFlexBasis(IntPtr node)
	{
		YGNodeStyleGetFlexBasis_Injected(node, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern float YGNodeGetComputedFlexBasis(IntPtr node);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetWidth(IntPtr node, float width);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetWidthPercent(IntPtr node, float width);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetWidthAuto(IntPtr node);

	[FreeFunction]
	public static YogaValue YGNodeStyleGetWidth(IntPtr node)
	{
		YGNodeStyleGetWidth_Injected(node, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetHeight(IntPtr node, float height);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetHeightPercent(IntPtr node, float height);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetHeightAuto(IntPtr node);

	[FreeFunction]
	public static YogaValue YGNodeStyleGetHeight(IntPtr node)
	{
		YGNodeStyleGetHeight_Injected(node, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetMinWidth(IntPtr node, float minWidth);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetMinWidthPercent(IntPtr node, float minWidth);

	[FreeFunction]
	public static YogaValue YGNodeStyleGetMinWidth(IntPtr node)
	{
		YGNodeStyleGetMinWidth_Injected(node, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetMinHeight(IntPtr node, float minHeight);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetMinHeightPercent(IntPtr node, float minHeight);

	[FreeFunction]
	public static YogaValue YGNodeStyleGetMinHeight(IntPtr node)
	{
		YGNodeStyleGetMinHeight_Injected(node, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetMaxWidth(IntPtr node, float maxWidth);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetMaxWidthPercent(IntPtr node, float maxWidth);

	[FreeFunction]
	public static YogaValue YGNodeStyleGetMaxWidth(IntPtr node)
	{
		YGNodeStyleGetMaxWidth_Injected(node, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetMaxHeight(IntPtr node, float maxHeight);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetMaxHeightPercent(IntPtr node, float maxHeight);

	[FreeFunction]
	public static YogaValue YGNodeStyleGetMaxHeight(IntPtr node)
	{
		YGNodeStyleGetMaxHeight_Injected(node, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetAspectRatio(IntPtr node, float aspectRatio);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern float YGNodeStyleGetAspectRatio(IntPtr node);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetPosition(IntPtr node, YogaEdge edge, float position);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetPositionPercent(IntPtr node, YogaEdge edge, float position);

	[FreeFunction]
	public static YogaValue YGNodeStyleGetPosition(IntPtr node, YogaEdge edge)
	{
		YGNodeStyleGetPosition_Injected(node, edge, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetMargin(IntPtr node, YogaEdge edge, float margin);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetMarginPercent(IntPtr node, YogaEdge edge, float margin);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetMarginAuto(IntPtr node, YogaEdge edge);

	[FreeFunction]
	public static YogaValue YGNodeStyleGetMargin(IntPtr node, YogaEdge edge)
	{
		YGNodeStyleGetMargin_Injected(node, edge, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetPadding(IntPtr node, YogaEdge edge, float padding);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetPaddingPercent(IntPtr node, YogaEdge edge, float padding);

	[FreeFunction]
	public static YogaValue YGNodeStyleGetPadding(IntPtr node, YogaEdge edge)
	{
		YGNodeStyleGetPadding_Injected(node, edge, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void YGNodeStyleSetBorder(IntPtr node, YogaEdge edge, float border);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern float YGNodeStyleGetBorder(IntPtr node, YogaEdge edge);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern float YGNodeLayoutGetLeft(IntPtr node);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern float YGNodeLayoutGetTop(IntPtr node);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern float YGNodeLayoutGetRight(IntPtr node);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern float YGNodeLayoutGetBottom(IntPtr node);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern float YGNodeLayoutGetWidth(IntPtr node);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern float YGNodeLayoutGetHeight(IntPtr node);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern float YGNodeLayoutGetMargin(IntPtr node, YogaEdge edge);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern float YGNodeLayoutGetPadding(IntPtr node, YogaEdge edge);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern YogaDirection YGNodeLayoutGetDirection(IntPtr node);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void YGNodeStyleGetFlexBasis_Injected(IntPtr node, out YogaValue ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void YGNodeStyleGetWidth_Injected(IntPtr node, out YogaValue ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void YGNodeStyleGetHeight_Injected(IntPtr node, out YogaValue ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void YGNodeStyleGetMinWidth_Injected(IntPtr node, out YogaValue ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void YGNodeStyleGetMinHeight_Injected(IntPtr node, out YogaValue ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void YGNodeStyleGetMaxWidth_Injected(IntPtr node, out YogaValue ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void YGNodeStyleGetMaxHeight_Injected(IntPtr node, out YogaValue ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void YGNodeStyleGetPosition_Injected(IntPtr node, YogaEdge edge, out YogaValue ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void YGNodeStyleGetMargin_Injected(IntPtr node, YogaEdge edge, out YogaValue ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void YGNodeStyleGetPadding_Injected(IntPtr node, YogaEdge edge, out YogaValue ret);
}
