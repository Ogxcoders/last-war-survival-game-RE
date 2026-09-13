#define UNITY_ASSERTIONS
using System.Collections.Generic;
using Unity.Collections;
using Unity.Profiling;

namespace UnityEngine.UIElements.UIR.Implementation;

internal static class RenderEvents
{
	private static readonly float VisibilityTreshold = Mathf.Epsilon;

	private static readonly ProfilerMarker k_NudgeVerticesMarker = new ProfilerMarker("UIR.NudgeVertices");

	internal static Shader ResolveShader(Shader shader)
	{
		if (shader == null)
		{
			shader = Shader.Find(UIRUtility.k_DefaultShaderName);
		}
		Debug.Assert(shader != null, "Failed to load the shader UIRDefault shader");
		shader.hideFlags |= HideFlags.DontSaveInEditor;
		return shader;
	}

	internal static void ProcessOnClippingChanged(RenderChain renderChain, VisualElement ve, uint dirtyID, UIRenderDevice device, ref ChainBuilderStats stats)
	{
		bool flag = (ve.renderChainData.dirtiedValues & RenderDataDirtyTypes.ClippingHierarchy) != 0;
		if (flag)
		{
			stats.recursiveClipUpdates++;
		}
		else
		{
			stats.nonRecursiveClipUpdates++;
		}
		DepthFirstOnClippingChanged(renderChain, ve.hierarchy.parent, ve, dirtyID, flag, isRootOfChange: true, isPendingHierarchicalRepaint: false, inheritedClipRectIDChanged: false, inheritedStencilClippedChanged: false, device, ref stats);
	}

	internal static void ProcessOnOpacityChanged(RenderChain renderChain, VisualElement ve, uint dirtyID, ref ChainBuilderStats stats)
	{
		bool hierarchical = (ve.renderChainData.dirtiedValues & RenderDataDirtyTypes.OpacityHierarchy) != 0;
		stats.recursiveOpacityUpdates++;
		DepthFirstOnOpacityChanged(renderChain, (ve.hierarchy.parent != null) ? ve.hierarchy.parent.renderChainData.compositeOpacity : 1f, ve, dirtyID, hierarchical, ref stats);
	}

	internal static void ProcessOnTransformOrSizeChanged(RenderChain renderChain, VisualElement ve, uint dirtyID, UIRenderDevice device, ref ChainBuilderStats stats)
	{
		stats.recursiveTransformUpdates++;
		DepthFirstOnTransformOrSizeChanged(renderChain, ve.hierarchy.parent, ve, dirtyID, device, isAncestorOfChangeSkinned: false, transformChanged: false, ref stats);
	}

	internal static void ProcessOnVisualsChanged(RenderChain renderChain, VisualElement ve, uint dirtyID, ref ChainBuilderStats stats)
	{
		bool flag = (ve.renderChainData.dirtiedValues & RenderDataDirtyTypes.VisualsHierarchy) != 0;
		if (flag)
		{
			stats.recursiveVisualUpdates++;
		}
		else
		{
			stats.nonRecursiveVisualUpdates++;
		}
		VisualElement parent = ve.hierarchy.parent;
		DepthFirstOnVisualsChanged(renderChain, ve, dirtyID, parent != null && (parent.renderChainData.isHierarchyHidden || IsElementHierarchyHidden(parent)), flag, ref stats);
	}

	internal static void ProcessRegenText(RenderChain renderChain, VisualElement ve, UIRTextUpdatePainter painter, UIRenderDevice device, ref ChainBuilderStats stats)
	{
		stats.textUpdates++;
		painter.Begin(ve, device);
		ve.InvokeGenerateVisualContent(painter.meshGenerationContext);
		painter.End();
	}

	private static Matrix4x4 GetTransformIDTransformInfo(VisualElement ve)
	{
		Debug.Assert(RenderChainVEData.AllocatesID(ve.renderChainData.transformID) || (ve.renderHints & RenderHints.GroupTransform) != 0);
		Matrix4x4 result = ((ve.renderChainData.groupTransformAncestor == null) ? ve.worldTransform : (ve.renderChainData.groupTransformAncestor.worldTransform.inverse * ve.worldTransform));
		result.m22 = (result.m33 = 1f);
		return result;
	}

	private static Vector4 GetClipRectIDClipInfo(VisualElement ve)
	{
		Debug.Assert(RenderChainVEData.AllocatesID(ve.renderChainData.clipRectID));
		if (ve.renderChainData.groupTransformAncestor == null)
		{
			return ve.worldClip.ToVector4();
		}
		Rect worldClipMinusGroup = ve.worldClipMinusGroup;
		Matrix4x4 inverse = ve.renderChainData.groupTransformAncestor.worldTransform.inverse;
		Vector3 vector = inverse.MultiplyPoint3x4(new Vector3(worldClipMinusGroup.xMin, worldClipMinusGroup.yMin, 0f));
		Vector3 vector2 = inverse.MultiplyPoint3x4(new Vector3(worldClipMinusGroup.xMax, worldClipMinusGroup.yMax, 0f));
		return new Vector4(Mathf.Min(vector.x, vector2.x), Mathf.Min(vector.y, vector2.y), Mathf.Max(vector.x, vector2.x), Mathf.Max(vector.y, vector2.y));
	}

	private static void GetVerticesTransformInfo(VisualElement ve, out Matrix4x4 transform)
	{
		if (RenderChainVEData.AllocatesID(ve.renderChainData.transformID) || (ve.renderHints & RenderHints.GroupTransform) != RenderHints.None)
		{
			transform = Matrix4x4.identity;
		}
		else if (ve.renderChainData.boneTransformAncestor != null)
		{
			transform = ve.renderChainData.boneTransformAncestor.worldTransform.inverse * ve.worldTransform;
		}
		else if (ve.renderChainData.groupTransformAncestor != null)
		{
			transform = ve.renderChainData.groupTransformAncestor.worldTransform.inverse * ve.worldTransform;
		}
		else
		{
			transform = ve.worldTransform;
		}
		transform.m22 = (transform.m33 = 1f);
	}

	internal static uint DepthFirstOnChildAdded(RenderChain renderChain, VisualElement parent, VisualElement ve, int index, bool resetState)
	{
		Debug.Assert(ve.panel != null);
		if (ve.renderChainData.isInChain)
		{
			return 0u;
		}
		if (resetState)
		{
			ve.renderChainData = default(RenderChainVEData);
		}
		ve.renderChainData.isInChain = true;
		ve.renderChainData.verticesSpace = Matrix4x4.identity;
		ve.renderChainData.transformID = UIRVEShaderInfoAllocator.identityTransform;
		ve.renderChainData.clipRectID = UIRVEShaderInfoAllocator.infiniteClipRect;
		ve.renderChainData.opacityID = UIRVEShaderInfoAllocator.fullOpacity;
		ve.renderChainData.compositeOpacity = float.MaxValue;
		if (parent != null)
		{
			if ((parent.renderHints & RenderHints.GroupTransform) != RenderHints.None)
			{
				ve.renderChainData.groupTransformAncestor = parent;
			}
			else
			{
				ve.renderChainData.groupTransformAncestor = parent.renderChainData.groupTransformAncestor;
			}
			ve.renderChainData.hierarchyDepth = parent.renderChainData.hierarchyDepth + 1;
		}
		else
		{
			ve.renderChainData.groupTransformAncestor = null;
			ve.renderChainData.hierarchyDepth = 0;
		}
		renderChain.EnsureFitsDepth(ve.renderChainData.hierarchyDepth);
		if (index > 0)
		{
			Debug.Assert(parent != null);
			ve.renderChainData.prev = GetLastDeepestChild(parent.hierarchy[index - 1]);
		}
		else
		{
			ve.renderChainData.prev = parent;
		}
		ve.renderChainData.next = ((ve.renderChainData.prev != null) ? ve.renderChainData.prev.renderChainData.next : null);
		if (ve.renderChainData.prev != null)
		{
			ve.renderChainData.prev.renderChainData.next = ve;
		}
		if (ve.renderChainData.next != null)
		{
			ve.renderChainData.next.renderChainData.prev = ve;
		}
		Debug.Assert(!RenderChainVEData.AllocatesID(ve.renderChainData.transformID));
		if (NeedsTransformID(ve))
		{
			ve.renderChainData.transformID = renderChain.shaderInfoAllocator.AllocTransform();
		}
		else
		{
			ve.renderChainData.transformID = BMPAlloc.Invalid;
		}
		ve.renderChainData.boneTransformAncestor = null;
		if (!RenderChainVEData.AllocatesID(ve.renderChainData.transformID))
		{
			if (parent != null && (ve.renderHints & RenderHints.GroupTransform) == 0)
			{
				if (RenderChainVEData.AllocatesID(parent.renderChainData.transformID))
				{
					ve.renderChainData.boneTransformAncestor = parent;
				}
				else
				{
					ve.renderChainData.boneTransformAncestor = parent.renderChainData.boneTransformAncestor;
				}
				ve.renderChainData.transformID = parent.renderChainData.transformID;
				ve.renderChainData.transformID.owned = 0;
			}
			else
			{
				ve.renderChainData.transformID = UIRVEShaderInfoAllocator.identityTransform;
			}
		}
		else
		{
			renderChain.shaderInfoAllocator.SetTransformValue(ve.renderChainData.transformID, GetTransformIDTransformInfo(ve));
		}
		int childCount = ve.hierarchy.childCount;
		uint num = 0u;
		for (int i = 0; i < childCount; i++)
		{
			num += DepthFirstOnChildAdded(renderChain, ve, ve.hierarchy[i], i, resetState);
		}
		return 1 + num;
	}

	internal static uint DepthFirstOnChildRemoving(RenderChain renderChain, VisualElement ve)
	{
		int num = ve.hierarchy.childCount - 1;
		uint num2 = 0u;
		while (num >= 0)
		{
			num2 += DepthFirstOnChildRemoving(renderChain, ve.hierarchy[num--]);
		}
		if ((ve.renderHints & RenderHints.GroupTransform) != RenderHints.None)
		{
			renderChain.StopTrackingGroupTransformElement(ve);
		}
		if (ve.renderChainData.isInChain)
		{
			renderChain.ChildWillBeRemoved(ve);
			ResetCommands(renderChain, ve);
			ve.renderChainData.isInChain = false;
			ve.renderChainData.clipMethod = ClipMethod.Undetermined;
			if (ve.renderChainData.next != null)
			{
				ve.renderChainData.next.renderChainData.prev = ve.renderChainData.prev;
			}
			if (ve.renderChainData.prev != null)
			{
				ve.renderChainData.prev.renderChainData.next = ve.renderChainData.next;
			}
			if (RenderChainVEData.AllocatesID(ve.renderChainData.opacityID))
			{
				renderChain.shaderInfoAllocator.FreeOpacity(ve.renderChainData.opacityID);
				ve.renderChainData.opacityID = UIRVEShaderInfoAllocator.fullOpacity;
			}
			if (RenderChainVEData.AllocatesID(ve.renderChainData.clipRectID))
			{
				renderChain.shaderInfoAllocator.FreeClipRect(ve.renderChainData.clipRectID);
				ve.renderChainData.clipRectID = UIRVEShaderInfoAllocator.infiniteClipRect;
			}
			if (RenderChainVEData.AllocatesID(ve.renderChainData.transformID))
			{
				renderChain.shaderInfoAllocator.FreeTransform(ve.renderChainData.transformID);
				ve.renderChainData.transformID = UIRVEShaderInfoAllocator.identityTransform;
			}
			ve.renderChainData.boneTransformAncestor = (ve.renderChainData.groupTransformAncestor = null);
			if (ve.renderChainData.closingData != null)
			{
				renderChain.device.Free(ve.renderChainData.closingData);
				ve.renderChainData.closingData = null;
			}
			if (ve.renderChainData.data != null)
			{
				renderChain.device.Free(ve.renderChainData.data);
				ve.renderChainData.data = null;
			}
		}
		return num2 + 1;
	}

	private static void DepthFirstOnClippingChanged(RenderChain renderChain, VisualElement parent, VisualElement ve, uint dirtyID, bool hierarchical, bool isRootOfChange, bool isPendingHierarchicalRepaint, bool inheritedClipRectIDChanged, bool inheritedStencilClippedChanged, UIRenderDevice device, ref ChainBuilderStats stats)
	{
		if (dirtyID == ve.renderChainData.dirtyID && !inheritedClipRectIDChanged && !inheritedStencilClippedChanged)
		{
			return;
		}
		ve.renderChainData.dirtyID = dirtyID;
		if (!isRootOfChange)
		{
			stats.recursiveClipUpdatesExpanded++;
		}
		isPendingHierarchicalRepaint |= (ve.renderChainData.dirtiedValues & RenderDataDirtyTypes.VisualsHierarchy) != 0;
		bool flag = hierarchical || isRootOfChange || inheritedClipRectIDChanged;
		bool flag2 = hierarchical || isRootOfChange;
		bool flag3 = hierarchical || isRootOfChange || inheritedStencilClippedChanged;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		bool flag7 = hierarchical;
		ClipMethod clipMethod = ve.renderChainData.clipMethod;
		ClipMethod clipMethod2 = (flag2 ? DetermineSelfClipMethod(ve) : clipMethod);
		bool flag8 = false;
		if (flag)
		{
			BMPAlloc bMPAlloc = ve.renderChainData.clipRectID;
			if (clipMethod2 == ClipMethod.ShaderDiscard)
			{
				if (!RenderChainVEData.AllocatesID(ve.renderChainData.clipRectID))
				{
					bMPAlloc = renderChain.shaderInfoAllocator.AllocClipRect();
					if (!bMPAlloc.IsValid())
					{
						clipMethod2 = ClipMethod.Scissor;
						bMPAlloc = UIRVEShaderInfoAllocator.infiniteClipRect;
					}
				}
			}
			else
			{
				if (RenderChainVEData.AllocatesID(ve.renderChainData.clipRectID))
				{
					renderChain.shaderInfoAllocator.FreeClipRect(ve.renderChainData.clipRectID);
				}
				if ((ve.renderHints & RenderHints.GroupTransform) == 0)
				{
					bMPAlloc = ((clipMethod2 != ClipMethod.Scissor && parent != null) ? parent.renderChainData.clipRectID : UIRVEShaderInfoAllocator.infiniteClipRect);
					bMPAlloc.owned = 0;
				}
			}
			flag8 = !ve.renderChainData.clipRectID.Equals(bMPAlloc);
			Debug.Assert((ve.renderHints & RenderHints.GroupTransform) == 0 || !flag8);
			ve.renderChainData.clipRectID = bMPAlloc;
		}
		if (clipMethod != clipMethod2)
		{
			ve.renderChainData.clipMethod = clipMethod2;
			if (clipMethod == ClipMethod.Stencil || clipMethod2 == ClipMethod.Stencil)
			{
				flag3 = true;
				flag5 = true;
			}
			if (clipMethod == ClipMethod.Scissor || clipMethod2 == ClipMethod.Scissor)
			{
				flag4 = true;
			}
			if (clipMethod2 == ClipMethod.ShaderDiscard || (clipMethod == ClipMethod.ShaderDiscard && RenderChainVEData.AllocatesID(ve.renderChainData.clipRectID)))
			{
				flag6 = true;
			}
		}
		if (flag8)
		{
			flag7 = true;
			flag5 = true;
		}
		bool inheritedStencilClippedChanged2 = false;
		if (flag3)
		{
			bool isStencilClipped = ve.renderChainData.isStencilClipped;
			bool flag9 = clipMethod2 == ClipMethod.Stencil || (parent?.renderChainData.isStencilClipped ?? false);
			ve.renderChainData.isStencilClipped = flag9;
			if (isStencilClipped != flag9)
			{
				inheritedStencilClippedChanged2 = true;
				flag7 = true;
			}
		}
		if ((flag4 || flag5) && !isPendingHierarchicalRepaint)
		{
			renderChain.UIEOnVisualsChanged(ve, flag5);
			isPendingHierarchicalRepaint = true;
		}
		if (flag6)
		{
			renderChain.UIEOnTransformOrSizeChanged(ve, transformChanged: false, clipRectSizeChanged: true);
		}
		if (flag7)
		{
			int childCount = ve.hierarchy.childCount;
			for (int i = 0; i < childCount; i++)
			{
				DepthFirstOnClippingChanged(renderChain, ve, ve.hierarchy[i], dirtyID, hierarchical, isRootOfChange: false, isPendingHierarchicalRepaint, flag8, inheritedStencilClippedChanged2, device, ref stats);
			}
		}
	}

	private static void DepthFirstOnOpacityChanged(RenderChain renderChain, float parentCompositeOpacity, VisualElement ve, uint dirtyID, bool hierarchical, ref ChainBuilderStats stats)
	{
		if (dirtyID == ve.renderChainData.dirtyID)
		{
			return;
		}
		ve.renderChainData.dirtyID = dirtyID;
		stats.recursiveOpacityUpdatesExpanded++;
		float compositeOpacity = ve.renderChainData.compositeOpacity;
		float num = ve.resolvedStyle.opacity * parentCompositeOpacity;
		bool flag = (compositeOpacity < VisibilityTreshold) ^ (num < VisibilityTreshold);
		bool flag2 = compositeOpacity < VisibilityTreshold && num >= VisibilityTreshold;
		bool flag3 = Mathf.Abs(compositeOpacity - num) > 0.0001f || flag;
		if (flag3)
		{
			ve.renderChainData.compositeOpacity = num;
		}
		bool flag4 = false;
		if (num < parentCompositeOpacity - 0.0001f)
		{
			if (ve.renderChainData.opacityID.owned == 0)
			{
				flag4 = true;
				ve.renderChainData.opacityID = renderChain.shaderInfoAllocator.AllocOpacity();
			}
			if ((flag4 || flag3) && ve.renderChainData.opacityID.IsValid())
			{
				renderChain.shaderInfoAllocator.SetOpacityValue(ve.renderChainData.opacityID, num);
			}
		}
		else if (ve.renderChainData.opacityID.owned == 0)
		{
			if (ve.hierarchy.parent != null && !ve.renderChainData.opacityID.Equals(ve.hierarchy.parent.renderChainData.opacityID))
			{
				flag4 = true;
				ve.renderChainData.opacityID = ve.hierarchy.parent.renderChainData.opacityID;
				ve.renderChainData.opacityID.owned = 0;
			}
		}
		else if (flag3 && ve.renderChainData.opacityID.IsValid())
		{
			renderChain.shaderInfoAllocator.SetOpacityValue(ve.renderChainData.opacityID, num);
		}
		if (flag2)
		{
			renderChain.UIEOnVisualsChanged(ve, hierarchical: true);
		}
		else if (flag4 && (ve.renderChainData.dirtiedValues & RenderDataDirtyTypes.Visuals) == 0)
		{
			renderChain.UIEOnVisualsChanged(ve, hierarchical: false);
		}
		if (flag3 || flag4 || hierarchical)
		{
			int childCount = ve.hierarchy.childCount;
			for (int i = 0; i < childCount; i++)
			{
				DepthFirstOnOpacityChanged(renderChain, num, ve.hierarchy[i], dirtyID, hierarchical, ref stats);
			}
		}
	}

	private static void DepthFirstOnTransformOrSizeChanged(RenderChain renderChain, VisualElement parent, VisualElement ve, uint dirtyID, UIRenderDevice device, bool isAncestorOfChangeSkinned, bool transformChanged, ref ChainBuilderStats stats)
	{
		if (dirtyID == ve.renderChainData.dirtyID)
		{
			return;
		}
		stats.recursiveTransformUpdatesExpanded++;
		transformChanged |= (ve.renderChainData.dirtiedValues & RenderDataDirtyTypes.Transform) != 0;
		if (RenderChainVEData.AllocatesID(ve.renderChainData.clipRectID))
		{
			renderChain.shaderInfoAllocator.SetClipRectValue(ve.renderChainData.clipRectID, GetClipRectIDClipInfo(ve));
		}
		bool flag = true;
		if (RenderChainVEData.AllocatesID(ve.renderChainData.transformID))
		{
			renderChain.shaderInfoAllocator.SetTransformValue(ve.renderChainData.transformID, GetTransformIDTransformInfo(ve));
			isAncestorOfChangeSkinned = true;
			stats.boneTransformed++;
		}
		else if (transformChanged)
		{
			if ((ve.renderHints & RenderHints.GroupTransform) != RenderHints.None)
			{
				stats.groupTransformElementsChanged++;
			}
			else if (isAncestorOfChangeSkinned)
			{
				Debug.Assert(RenderChainVEData.InheritsID(ve.renderChainData.transformID));
				flag = false;
				stats.skipTransformed++;
			}
			else if ((ve.renderChainData.dirtiedValues & (RenderDataDirtyTypes.Visuals | RenderDataDirtyTypes.VisualsHierarchy)) == 0 && ve.renderChainData.data != null)
			{
				if (!ve.renderChainData.disableNudging && NudgeVerticesToNewSpace(ve, device))
				{
					stats.nudgeTransformed++;
				}
				else
				{
					renderChain.UIEOnVisualsChanged(ve, hierarchical: false);
					stats.visualUpdateTransformed++;
				}
			}
		}
		if (flag)
		{
			ve.renderChainData.dirtyID = dirtyID;
		}
		if ((ve.renderHints & RenderHints.GroupTransform) == 0)
		{
			int childCount = ve.hierarchy.childCount;
			for (int i = 0; i < childCount; i++)
			{
				DepthFirstOnTransformOrSizeChanged(renderChain, ve, ve.hierarchy[i], dirtyID, device, isAncestorOfChangeSkinned, transformChanged, ref stats);
			}
		}
		else
		{
			renderChain.OnGroupTransformElementChangedTransform(ve);
		}
	}

	private static void DepthFirstOnVisualsChanged(RenderChain renderChain, VisualElement ve, uint dirtyID, bool parentHierarchyHidden, bool hierarchical, ref ChainBuilderStats stats)
	{
		if (dirtyID == ve.renderChainData.dirtyID)
		{
			return;
		}
		ve.renderChainData.dirtyID = dirtyID;
		if (hierarchical)
		{
			stats.recursiveVisualUpdatesExpanded++;
		}
		bool isHierarchyHidden = ve.renderChainData.isHierarchyHidden;
		ve.renderChainData.isHierarchyHidden = parentHierarchyHidden || IsElementHierarchyHidden(ve);
		if (isHierarchyHidden != ve.renderChainData.isHierarchyHidden)
		{
			hierarchical = true;
		}
		Debug.Assert(ve.renderChainData.clipMethod != ClipMethod.Undetermined);
		Debug.Assert(RenderChainVEData.AllocatesID(ve.renderChainData.transformID) || ve.hierarchy.parent == null || ve.renderChainData.transformID.Equals(ve.hierarchy.parent.renderChainData.transformID) || (ve.renderHints & RenderHints.GroupTransform) != 0);
		UIRStylePainter.ClosingInfo closingInfo = PaintElement(renderChain, ve, ref stats);
		if (hierarchical)
		{
			int childCount = ve.hierarchy.childCount;
			for (int i = 0; i < childCount; i++)
			{
				DepthFirstOnVisualsChanged(renderChain, ve.hierarchy[i], dirtyID, ve.renderChainData.isHierarchyHidden, hierarchical: true, ref stats);
			}
		}
		if (closingInfo.needsClosing)
		{
			ClosePaintElement(ve, closingInfo, renderChain, ref stats);
		}
	}

	private static bool IsElementHierarchyHidden(VisualElement ve)
	{
		return ve.resolvedStyle.opacity < Mathf.Epsilon || ve.resolvedStyle.display == DisplayStyle.None;
	}

	private static bool IsElementSelfHidden(VisualElement ve)
	{
		return ve.resolvedStyle.visibility == Visibility.Hidden;
	}

	private static VisualElement GetLastDeepestChild(VisualElement ve)
	{
		for (int childCount = ve.hierarchy.childCount; childCount > 0; childCount = ve.hierarchy.childCount)
		{
			ve = ve.hierarchy[childCount - 1];
		}
		return ve;
	}

	private static VisualElement GetNextDepthFirst(VisualElement ve)
	{
		for (VisualElement parent = ve.hierarchy.parent; parent != null; parent = parent.hierarchy.parent)
		{
			int num = parent.hierarchy.IndexOf(ve);
			int childCount = parent.hierarchy.childCount;
			if (num < childCount - 1)
			{
				return parent.hierarchy[num + 1];
			}
			ve = parent;
		}
		return null;
	}

	private static bool IsParentOrAncestorOf(this VisualElement ve, VisualElement child)
	{
		while (child.hierarchy.parent != null)
		{
			if (child.hierarchy.parent == ve)
			{
				return true;
			}
			child = child.hierarchy.parent;
		}
		return false;
	}

	private static ClipMethod DetermineSelfClipMethod(VisualElement ve)
	{
		if (!ve.ShouldClip())
		{
			return ClipMethod.NotClipped;
		}
		ClipMethod result = (((ve.renderHints & (RenderHints.GroupTransform | RenderHints.ClipWithScissors)) != RenderHints.None) ? ClipMethod.Scissor : ClipMethod.ShaderDiscard);
		if (!UIRUtility.IsRoundRect(ve) && !UIRUtility.IsVectorImageBackground(ve))
		{
			return result;
		}
		VisualElement parent = ve.hierarchy.parent;
		if (parent != null && parent.renderChainData.isStencilClipped)
		{
			return result;
		}
		return ClipMethod.Stencil;
	}

	private static bool NeedsTransformID(VisualElement ve)
	{
		return (ve.renderHints & RenderHints.GroupTransform) == 0 && (ve.renderHints & RenderHints.BoneTransform) == RenderHints.BoneTransform;
	}

	private static bool TransformIDHasChanged(Alloc before, Alloc after)
	{
		if (before.size == 0 && after.size == 0)
		{
			return false;
		}
		if (before.size != after.size || before.start != after.start)
		{
			return true;
		}
		return false;
	}

	internal static UIRStylePainter.ClosingInfo PaintElement(RenderChain renderChain, VisualElement ve, ref ChainBuilderStats stats)
	{
		bool flag = ve.renderChainData.clipMethod == ClipMethod.Stencil;
		bool flag2 = ve.renderChainData.clipMethod == ClipMethod.Scissor;
		if ((IsElementSelfHidden(ve) && !flag && !flag2) || ve.renderChainData.isHierarchyHidden)
		{
			if (ve.renderChainData.data != null)
			{
				renderChain.painter.device.Free(ve.renderChainData.data);
				ve.renderChainData.data = null;
			}
			if (ve.renderChainData.firstCommand != null)
			{
				ResetCommands(renderChain, ve);
			}
			return default(UIRStylePainter.ClosingInfo);
		}
		RenderChainCommand renderChainCommand = ve.renderChainData.firstCommand?.prev;
		RenderChainCommand renderChainCommand2 = ve.renderChainData.lastCommand?.next;
		bool flag3 = ve.renderChainData.firstClosingCommand != null && renderChainCommand2 == ve.renderChainData.firstClosingCommand;
		RenderChainCommand renderChainCommand3;
		RenderChainCommand renderChainCommand4;
		if (flag3)
		{
			renderChainCommand2 = ve.renderChainData.lastClosingCommand.next;
			renderChainCommand3 = (renderChainCommand4 = null);
		}
		else
		{
			renderChainCommand3 = ve.renderChainData.firstClosingCommand?.prev;
			renderChainCommand4 = ve.renderChainData.lastClosingCommand?.next;
		}
		Debug.Assert(renderChainCommand?.owner != ve);
		Debug.Assert(renderChainCommand2?.owner != ve);
		Debug.Assert(renderChainCommand3?.owner != ve);
		Debug.Assert(renderChainCommand4?.owner != ve);
		ResetCommands(renderChain, ve);
		UIRStylePainter painter = renderChain.painter;
		painter.Begin(ve);
		if (ve.visible)
		{
			painter.DrawVisualElementBackground();
			painter.DrawVisualElementBorder();
			painter.ApplyVisualElementClipping();
			ve.InvokeGenerateVisualContent(painter.meshGenerationContext);
		}
		else if (flag2 || flag)
		{
			painter.ApplyVisualElementClipping();
		}
		MeshHandle data = ve.renderChainData.data;
		if (painter.totalVertices > renderChain.device.maxVerticesPerPage)
		{
			Debug.LogError(string.Format("A {0} must not allocate more than {1} vertices.", "VisualElement", renderChain.device.maxVerticesPerPage));
			if (data != null)
			{
				painter.device.Free(data);
				data = null;
			}
			painter.Reset();
			painter.Begin(ve);
		}
		List<UIRStylePainter.Entry> entries = painter.entries;
		if (entries.Count > 0)
		{
			NativeSlice<Vertex> verts = default(NativeSlice<Vertex>);
			NativeSlice<ushort> indices = default(NativeSlice<ushort>);
			ushort indexOffset = 0;
			if (painter.totalVertices > 0)
			{
				UpdateOrAllocate(ref data, painter.totalVertices, painter.totalIndices, painter.device, out verts, out indices, out indexOffset, ref stats);
			}
			int num = 0;
			int num2 = 0;
			RenderChainCommand prev = renderChainCommand;
			RenderChainCommand next = renderChainCommand2;
			if (renderChainCommand == null && renderChainCommand2 == null)
			{
				FindCommandInsertionPoint(ve, out prev, out next);
			}
			bool flag4 = false;
			Matrix4x4 transform = Matrix4x4.identity;
			Color32 xformClipPages = new Color32(0, 0, 0, 0);
			Color32 idsAddFlags = new Color32(0, 0, 0, 0);
			Color32 opacityPage = new Color32(0, 0, 0, 0);
			int num3 = -1;
			int num4 = -1;
			foreach (UIRStylePainter.Entry entry in painter.entries)
			{
				NativeSlice<Vertex> vertices = entry.vertices;
				if (vertices.Length > 0)
				{
					NativeSlice<ushort> indices2 = entry.indices;
					if (indices2.Length > 0)
					{
						if (!flag4)
						{
							flag4 = true;
							GetVerticesTransformInfo(ve, out transform);
							ve.renderChainData.verticesSpace = transform;
							Color32 color = renderChain.shaderInfoAllocator.TransformAllocToVertexData(ve.renderChainData.transformID);
							Color32 color2 = renderChain.shaderInfoAllocator.OpacityAllocToVertexData(ve.renderChainData.opacityID);
							xformClipPages.r = color.r;
							xformClipPages.g = color.g;
							idsAddFlags.r = color.b;
							opacityPage.r = color2.r;
							opacityPage.g = color2.g;
							idsAddFlags.b = color2.b;
						}
						Color32 color3 = renderChain.shaderInfoAllocator.ClipRectAllocToVertexData(entry.clipRectID);
						xformClipPages.b = color3.r;
						xformClipPages.a = color3.g;
						idsAddFlags.g = color3.b;
						idsAddFlags.a = (byte)entry.addFlags;
						NativeSlice<Vertex> thisSlice = verts;
						int start = num;
						vertices = entry.vertices;
						NativeSlice<Vertex> nativeSlice = thisSlice.Slice(start, vertices.Length);
						if (entry.uvIsDisplacement)
						{
							if (num3 < 0)
							{
								num3 = num;
								int num5 = num;
								vertices = entry.vertices;
								num4 = num5 + vertices.Length;
							}
							else if (num4 == num)
							{
								int num6 = num4;
								vertices = entry.vertices;
								num4 = num6 + vertices.Length;
							}
							else
							{
								ve.renderChainData.disableNudging = true;
							}
							CopyTransformVertsPosAndVec(entry.vertices, nativeSlice, transform, xformClipPages, idsAddFlags, opacityPage);
						}
						else
						{
							CopyTransformVertsPos(entry.vertices, nativeSlice, transform, xformClipPages, idsAddFlags, opacityPage);
						}
						indices2 = entry.indices;
						int length = indices2.Length;
						int indexOffset2 = num + indexOffset;
						NativeSlice<ushort> nativeSlice2 = indices.Slice(num2, length);
						if (entry.isClipRegisterEntry || !entry.isStencilClipped)
						{
							CopyTriangleIndices(entry.indices, nativeSlice2, indexOffset2);
						}
						else
						{
							CopyTriangleIndicesFlipWindingOrder(entry.indices, nativeSlice2, indexOffset2);
						}
						if (entry.isClipRegisterEntry)
						{
							painter.LandClipRegisterMesh(nativeSlice, nativeSlice2, indexOffset2);
						}
						RenderChainCommand command = InjectMeshDrawCommand(renderChain, ve, ref prev, ref next, data, length, num2, entry.material, entry.custom, entry.font);
						if (entry.isTextEntry && ve.renderChainData.usesLegacyText)
						{
							if (ve.renderChainData.textEntries == null)
							{
								ve.renderChainData.textEntries = new List<RenderChainTextEntry>(1);
							}
							List<RenderChainTextEntry> textEntries = ve.renderChainData.textEntries;
							RenderChainTextEntry item = new RenderChainTextEntry
							{
								command = command,
								firstVertex = num
							};
							vertices = entry.vertices;
							item.vertexCount = vertices.Length;
							textEntries.Add(item);
						}
						int num7 = num;
						vertices = entry.vertices;
						num = num7 + vertices.Length;
						num2 += length;
						continue;
					}
				}
				if (entry.customCommand != null)
				{
					InjectCommandInBetween(renderChain, entry.customCommand, ref prev, ref next);
				}
				else
				{
					Debug.Assert(condition: false);
				}
			}
			if (!ve.renderChainData.disableNudging && num3 >= 0)
			{
				ve.renderChainData.displacementUVStart = num3;
				ve.renderChainData.displacementUVEnd = num4;
			}
		}
		else if (data != null)
		{
			painter.device.Free(data);
			data = null;
		}
		ve.renderChainData.data = data;
		if (ve.renderChainData.usesLegacyText)
		{
			renderChain.AddTextElement(ve);
		}
		if (painter.closingInfo.clipperRegisterIndices.Length == 0 && ve.renderChainData.closingData != null)
		{
			painter.device.Free(ve.renderChainData.closingData);
			ve.renderChainData.closingData = null;
		}
		if (painter.closingInfo.needsClosing)
		{
			RenderChainCommand prev2 = renderChainCommand3;
			RenderChainCommand next2 = renderChainCommand4;
			if (flag3)
			{
				prev2 = ve.renderChainData.lastCommand;
				next2 = prev2.next;
			}
			else if (prev2 == null && next2 == null)
			{
				FindClosingCommandInsertionPoint(ve, out prev2, out next2);
			}
			if (painter.closingInfo.clipperRegisterIndices.Length > 0)
			{
				painter.LandClipUnregisterMeshDrawCommand(InjectClosingMeshDrawCommand(renderChain, ve, ref prev2, ref next2, null, 0, 0, null, null, null));
			}
			if (painter.closingInfo.popViewMatrix)
			{
				RenderChainCommand renderChainCommand5 = renderChain.AllocCommand();
				renderChainCommand5.type = CommandType.PopView;
				renderChainCommand5.closing = true;
				renderChainCommand5.owner = ve;
				InjectClosingCommandInBetween(renderChainCommand5, ref prev2, ref next2);
			}
			if (painter.closingInfo.popScissorClip)
			{
				RenderChainCommand renderChainCommand6 = renderChain.AllocCommand();
				renderChainCommand6.type = CommandType.PopScissor;
				renderChainCommand6.closing = true;
				renderChainCommand6.owner = ve;
				InjectClosingCommandInBetween(renderChainCommand6, ref prev2, ref next2);
			}
		}
		Debug.Assert(ve.renderChainData.closingData == null || ve.renderChainData.data != null);
		UIRStylePainter.ClosingInfo closingInfo = painter.closingInfo;
		painter.Reset();
		return closingInfo;
	}

	private static void ClosePaintElement(VisualElement ve, UIRStylePainter.ClosingInfo closingInfo, RenderChain renderChain, ref ChainBuilderStats stats)
	{
		if (closingInfo.clipperRegisterIndices.Length > 0)
		{
			NativeSlice<Vertex> verts = default(NativeSlice<Vertex>);
			NativeSlice<ushort> indices = default(NativeSlice<ushort>);
			ushort indexOffset = 0;
			UpdateOrAllocate(ref ve.renderChainData.closingData, closingInfo.clipperRegisterVertices.Length, closingInfo.clipperRegisterIndices.Length, renderChain.painter.device, out verts, out indices, out indexOffset, ref stats);
			verts.CopyFrom(closingInfo.clipperRegisterVertices);
			CopyTriangleIndicesFlipWindingOrder(closingInfo.clipperRegisterIndices, indices, indexOffset - closingInfo.clipperRegisterIndexOffset);
			closingInfo.clipUnregisterDrawCommand.mesh = ve.renderChainData.closingData;
			closingInfo.clipUnregisterDrawCommand.indexCount = indices.Length;
		}
	}

	private static void UpdateOrAllocate(ref MeshHandle data, int vertexCount, int indexCount, UIRenderDevice device, out NativeSlice<Vertex> verts, out NativeSlice<ushort> indices, out ushort indexOffset, ref ChainBuilderStats stats)
	{
		if (data != null)
		{
			if (data.allocVerts.size >= vertexCount && data.allocIndices.size >= indexCount)
			{
				device.Update(data, (uint)vertexCount, (uint)indexCount, out verts, out indices, out indexOffset);
				stats.updatedMeshAllocations++;
			}
			else
			{
				device.Free(data);
				data = device.Allocate((uint)vertexCount, (uint)indexCount, out verts, out indices, out indexOffset);
				stats.newMeshAllocations++;
			}
		}
		else
		{
			data = device.Allocate((uint)vertexCount, (uint)indexCount, out verts, out indices, out indexOffset);
			stats.newMeshAllocations++;
		}
	}

	private static void CopyTransformVertsPos(NativeSlice<Vertex> source, NativeSlice<Vertex> target, Matrix4x4 mat, Color32 xformClipPages, Color32 idsAddFlags, Color32 opacityPage)
	{
		int length = source.Length;
		for (int i = 0; i < length; i++)
		{
			Vertex value = source[i];
			value.position = mat.MultiplyPoint3x4(value.position);
			value.xformClipPages = xformClipPages;
			value.idsFlags.r = idsAddFlags.r;
			value.idsFlags.g = idsAddFlags.g;
			value.idsFlags.b = idsAddFlags.b;
			value.idsFlags.a += idsAddFlags.a;
			value.opacityPageSVGSettingIndex.r = opacityPage.r;
			value.opacityPageSVGSettingIndex.g = opacityPage.g;
			target[i] = value;
		}
	}

	private static void CopyTransformVertsPosAndVec(NativeSlice<Vertex> source, NativeSlice<Vertex> target, Matrix4x4 mat, Color32 xformClipPages, Color32 idsAddFlags, Color32 opacityPage)
	{
		int length = source.Length;
		Vector3 vector = new Vector3(0f, 0f, 0.995f);
		for (int i = 0; i < length; i++)
		{
			Vertex value = source[i];
			value.position = mat.MultiplyPoint3x4(value.position);
			vector.x = value.uv.x;
			vector.y = value.uv.y;
			value.uv = mat.MultiplyVector(vector);
			value.xformClipPages = xformClipPages;
			value.idsFlags.r = idsAddFlags.r;
			value.idsFlags.g = idsAddFlags.g;
			value.idsFlags.b = idsAddFlags.b;
			value.idsFlags.a += idsAddFlags.a;
			value.opacityPageSVGSettingIndex.r = opacityPage.r;
			value.opacityPageSVGSettingIndex.g = opacityPage.g;
			target[i] = value;
		}
	}

	private static void CopyTriangleIndicesFlipWindingOrder(NativeSlice<ushort> source, NativeSlice<ushort> target)
	{
		Debug.Assert(source != target);
		int length = source.Length;
		for (int i = 0; i < length; i += 3)
		{
			ushort value = source[i];
			target[i] = source[i + 1];
			target[i + 1] = value;
			target[i + 2] = source[i + 2];
		}
	}

	private static void CopyTriangleIndicesFlipWindingOrder(NativeSlice<ushort> source, NativeSlice<ushort> target, int indexOffset)
	{
		Debug.Assert(source != target);
		int length = source.Length;
		for (int i = 0; i < length; i += 3)
		{
			ushort value = (ushort)(source[i] + indexOffset);
			target[i] = (ushort)(source[i + 1] + indexOffset);
			target[i + 1] = value;
			target[i + 2] = (ushort)(source[i + 2] + indexOffset);
		}
	}

	private static void CopyTriangleIndices(NativeSlice<ushort> source, NativeSlice<ushort> target, int indexOffset)
	{
		int length = source.Length;
		for (int i = 0; i < length; i++)
		{
			target[i] = (ushort)(source[i] + indexOffset);
		}
	}

	private static bool NudgeVerticesToNewSpace(VisualElement ve, UIRenderDevice device)
	{
		Debug.Assert(!ve.renderChainData.disableNudging);
		GetVerticesTransformInfo(ve, out var transform);
		Matrix4x4 nudgeTransform = transform * ve.renderChainData.verticesSpace.inverse;
		Matrix4x4 matrix4x = nudgeTransform * ve.renderChainData.verticesSpace;
		float num = Mathf.Abs(transform.m00 - matrix4x.m00);
		num += Mathf.Abs(transform.m01 - matrix4x.m01);
		num += Mathf.Abs(transform.m02 - matrix4x.m02);
		num += Mathf.Abs(transform.m03 - matrix4x.m03);
		num += Mathf.Abs(transform.m10 - matrix4x.m10);
		num += Mathf.Abs(transform.m11 - matrix4x.m11);
		num += Mathf.Abs(transform.m12 - matrix4x.m12);
		num += Mathf.Abs(transform.m13 - matrix4x.m13);
		num += Mathf.Abs(transform.m20 - matrix4x.m20);
		num += Mathf.Abs(transform.m21 - matrix4x.m21);
		num += Mathf.Abs(transform.m22 - matrix4x.m22);
		num += Mathf.Abs(transform.m23 - matrix4x.m23);
		if (num > 0.0001f)
		{
			return false;
		}
		ve.renderChainData.verticesSpace = transform;
		DoNudgeVertices(ve, device, ve.renderChainData.data, ref nudgeTransform, isClosingMesh: false);
		if (ve.renderChainData.closingData != null)
		{
			DoNudgeVertices(ve, device, ve.renderChainData.closingData, ref nudgeTransform, isClosingMesh: true);
		}
		return true;
	}

	private static void DoNudgeVertices(VisualElement ve, UIRenderDevice device, MeshHandle mesh, ref Matrix4x4 nudgeTransform, bool isClosingMesh)
	{
		int size = (int)mesh.allocVerts.size;
		NativeSlice<Vertex> nativeSlice = mesh.allocPage.vertices.cpuData.Slice((int)mesh.allocVerts.start, size);
		device.Update(mesh, (uint)size, out var vertexData);
		if (isClosingMesh)
		{
			for (int i = 0; i < size; i++)
			{
				Vertex value = nativeSlice[i];
				value.position = nudgeTransform.MultiplyPoint3x4(value.position);
				vertexData[i] = value;
			}
			return;
		}
		int displacementUVStart = ve.renderChainData.displacementUVStart;
		int displacementUVEnd = ve.renderChainData.displacementUVEnd;
		for (int j = 0; j < displacementUVStart; j++)
		{
			Vertex value2 = nativeSlice[j];
			value2.position = nudgeTransform.MultiplyPoint3x4(value2.position);
			vertexData[j] = value2;
		}
		for (int k = displacementUVStart; k < displacementUVEnd; k++)
		{
			Vertex value3 = nativeSlice[k];
			value3.position = nudgeTransform.MultiplyPoint3x4(value3.position);
			value3.uv = nudgeTransform.MultiplyVector(value3.uv);
			vertexData[k] = value3;
		}
		for (int l = displacementUVEnd; l < size; l++)
		{
			Vertex value4 = nativeSlice[l];
			value4.position = nudgeTransform.MultiplyPoint3x4(value4.position);
			vertexData[l] = value4;
		}
	}

	private static RenderChainCommand InjectMeshDrawCommand(RenderChain renderChain, VisualElement ve, ref RenderChainCommand cmdPrev, ref RenderChainCommand cmdNext, MeshHandle mesh, int indexCount, int indexOffset, Material material, Texture custom, Texture font)
	{
		RenderChainCommand renderChainCommand = renderChain.AllocCommand();
		renderChainCommand.type = CommandType.Draw;
		renderChainCommand.state = new State
		{
			material = material,
			custom = custom,
			font = font
		};
		renderChainCommand.mesh = mesh;
		renderChainCommand.indexOffset = indexOffset;
		renderChainCommand.indexCount = indexCount;
		renderChainCommand.owner = ve;
		InjectCommandInBetween(renderChain, renderChainCommand, ref cmdPrev, ref cmdNext);
		return renderChainCommand;
	}

	private static RenderChainCommand InjectClosingMeshDrawCommand(RenderChain renderChain, VisualElement ve, ref RenderChainCommand cmdPrev, ref RenderChainCommand cmdNext, MeshHandle mesh, int indexCount, int indexOffset, Material material, Texture custom, Texture font)
	{
		RenderChainCommand renderChainCommand = renderChain.AllocCommand();
		renderChainCommand.type = CommandType.Draw;
		renderChainCommand.closing = true;
		renderChainCommand.state = new State
		{
			material = material,
			custom = custom,
			font = font
		};
		renderChainCommand.mesh = mesh;
		renderChainCommand.indexOffset = indexOffset;
		renderChainCommand.indexCount = indexCount;
		renderChainCommand.owner = ve;
		InjectClosingCommandInBetween(renderChainCommand, ref cmdPrev, ref cmdNext);
		return renderChainCommand;
	}

	private static void FindCommandInsertionPoint(VisualElement ve, out RenderChainCommand prev, out RenderChainCommand next)
	{
		VisualElement prev2 = ve.renderChainData.prev;
		while (prev2 != null && prev2.renderChainData.lastCommand == null)
		{
			prev2 = prev2.renderChainData.prev;
		}
		if (prev2 != null && prev2.renderChainData.lastCommand != null)
		{
			if (prev2.hierarchy.parent == ve.hierarchy.parent)
			{
				prev = prev2.renderChainData.lastClosingOrLastCommand;
			}
			else if (prev2.IsParentOrAncestorOf(ve))
			{
				prev = prev2.renderChainData.lastCommand;
			}
			else
			{
				RenderChainCommand renderChainCommand = prev2.renderChainData.lastClosingOrLastCommand;
				do
				{
					prev = renderChainCommand;
					renderChainCommand = renderChainCommand.next;
				}
				while (renderChainCommand != null && renderChainCommand.owner != ve && renderChainCommand.closing && !renderChainCommand.owner.IsParentOrAncestorOf(ve));
			}
			next = prev.next;
		}
		else
		{
			VisualElement next2 = ve.renderChainData.next;
			while (next2 != null && next2.renderChainData.firstCommand == null)
			{
				next2 = next2.renderChainData.next;
			}
			next = next2?.renderChainData.firstCommand;
			prev = null;
			Debug.Assert(next == null || next.prev == null);
		}
	}

	private static void FindClosingCommandInsertionPoint(VisualElement ve, out RenderChainCommand prev, out RenderChainCommand next)
	{
		VisualElement visualElement = ve.renderChainData.next;
		while (visualElement != null && visualElement.renderChainData.firstCommand == null)
		{
			visualElement = visualElement.renderChainData.next;
		}
		if (visualElement != null && visualElement.renderChainData.firstCommand != null)
		{
			if (visualElement.hierarchy.parent == ve.hierarchy.parent)
			{
				next = visualElement.renderChainData.firstCommand;
				prev = next.prev;
			}
			else if (ve.IsParentOrAncestorOf(visualElement))
			{
				do
				{
					prev = visualElement.renderChainData.lastClosingOrLastCommand;
					visualElement = prev.next?.owner;
				}
				while (visualElement != null && ve.IsParentOrAncestorOf(visualElement));
				next = prev.next;
			}
			else
			{
				prev = ve.renderChainData.lastCommand;
				next = prev.next;
			}
		}
		else
		{
			prev = ve.renderChainData.lastCommand;
			next = prev.next;
		}
	}

	private static void InjectCommandInBetween(RenderChain renderChain, RenderChainCommand cmd, ref RenderChainCommand prev, ref RenderChainCommand next)
	{
		if (prev != null)
		{
			cmd.prev = prev;
			prev.next = cmd;
		}
		if (next != null)
		{
			cmd.next = next;
			next.prev = cmd;
		}
		VisualElement owner = cmd.owner;
		owner.renderChainData.lastCommand = cmd;
		if (owner.renderChainData.firstCommand == null)
		{
			owner.renderChainData.firstCommand = cmd;
		}
		renderChain.OnRenderCommandAdded(cmd);
		prev = cmd;
		next = cmd.next;
	}

	private static void InjectClosingCommandInBetween(RenderChainCommand cmd, ref RenderChainCommand prev, ref RenderChainCommand next)
	{
		Debug.Assert(cmd.closing);
		if (prev != null)
		{
			cmd.prev = prev;
			prev.next = cmd;
		}
		if (next != null)
		{
			cmd.next = next;
			next.prev = cmd;
		}
		VisualElement owner = cmd.owner;
		owner.renderChainData.lastClosingCommand = cmd;
		if (owner.renderChainData.firstClosingCommand == null)
		{
			owner.renderChainData.firstClosingCommand = cmd;
		}
		prev = cmd;
		next = cmd.next;
	}

	private static void ResetCommands(RenderChain renderChain, VisualElement ve)
	{
		if (ve.renderChainData.firstCommand != null)
		{
			renderChain.OnRenderCommandRemoved(ve.renderChainData.firstCommand, ve.renderChainData.lastCommand);
		}
		RenderChainCommand renderChainCommand = ((ve.renderChainData.firstCommand != null) ? ve.renderChainData.firstCommand.prev : null);
		RenderChainCommand renderChainCommand2 = ((ve.renderChainData.lastCommand != null) ? ve.renderChainData.lastCommand.next : null);
		Debug.Assert(renderChainCommand == null || renderChainCommand.owner != ve);
		Debug.Assert(renderChainCommand2 == null || renderChainCommand2 == ve.renderChainData.firstClosingCommand || renderChainCommand2.owner != ve);
		if (renderChainCommand != null)
		{
			renderChainCommand.next = renderChainCommand2;
		}
		if (renderChainCommand2 != null)
		{
			renderChainCommand2.prev = renderChainCommand;
		}
		if (ve.renderChainData.firstCommand != null)
		{
			RenderChainCommand renderChainCommand3 = ve.renderChainData.firstCommand;
			while (renderChainCommand3 != ve.renderChainData.lastCommand)
			{
				RenderChainCommand next = renderChainCommand3.next;
				renderChain.FreeCommand(renderChainCommand3);
				renderChainCommand3 = next;
			}
			renderChain.FreeCommand(renderChainCommand3);
		}
		ve.renderChainData.firstCommand = (ve.renderChainData.lastCommand = null);
		renderChainCommand = ((ve.renderChainData.firstClosingCommand != null) ? ve.renderChainData.firstClosingCommand.prev : null);
		renderChainCommand2 = ((ve.renderChainData.lastClosingCommand != null) ? ve.renderChainData.lastClosingCommand.next : null);
		Debug.Assert(renderChainCommand == null || renderChainCommand.owner != ve);
		Debug.Assert(renderChainCommand2 == null || renderChainCommand2.owner != ve);
		if (renderChainCommand != null)
		{
			renderChainCommand.next = renderChainCommand2;
		}
		if (renderChainCommand2 != null)
		{
			renderChainCommand2.prev = renderChainCommand;
		}
		if (ve.renderChainData.firstClosingCommand != null)
		{
			renderChain.OnRenderCommandRemoved(ve.renderChainData.firstClosingCommand, ve.renderChainData.lastClosingCommand);
			RenderChainCommand renderChainCommand4 = ve.renderChainData.firstClosingCommand;
			while (renderChainCommand4 != ve.renderChainData.lastClosingCommand)
			{
				RenderChainCommand next2 = renderChainCommand4.next;
				renderChain.FreeCommand(renderChainCommand4);
				renderChainCommand4 = next2;
			}
			renderChain.FreeCommand(renderChainCommand4);
		}
		ve.renderChainData.firstClosingCommand = (ve.renderChainData.lastClosingCommand = null);
		if (ve.renderChainData.usesLegacyText)
		{
			Debug.Assert(ve.renderChainData.textEntries.Count > 0);
			renderChain.RemoveTextElement(ve);
			ve.renderChainData.textEntries.Clear();
			ve.renderChainData.usesLegacyText = false;
		}
	}
}
