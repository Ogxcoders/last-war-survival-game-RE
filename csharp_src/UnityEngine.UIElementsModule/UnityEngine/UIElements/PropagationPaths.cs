using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements;

internal class PropagationPaths
{
	[Flags]
	public enum Type
	{
		None = 0,
		TrickleDown = 1,
		BubbleUp = 2
	}

	private static readonly ObjectPool<PropagationPaths> s_Pool = new ObjectPool<PropagationPaths>();

	public readonly List<VisualElement> trickleDownPath;

	public readonly List<VisualElement> targetElements;

	public readonly List<VisualElement> bubbleUpPath;

	private const int k_DefaultPropagationDepth = 16;

	private const int k_DefaultTargetCount = 4;

	public PropagationPaths()
	{
		trickleDownPath = new List<VisualElement>(16);
		targetElements = new List<VisualElement>(4);
		bubbleUpPath = new List<VisualElement>(16);
	}

	public PropagationPaths(PropagationPaths paths)
	{
		trickleDownPath = new List<VisualElement>(paths.trickleDownPath);
		targetElements = new List<VisualElement>(paths.targetElements);
		bubbleUpPath = new List<VisualElement>(paths.bubbleUpPath);
	}

	internal static PropagationPaths Copy(PropagationPaths paths)
	{
		PropagationPaths propagationPaths = s_Pool.Get();
		propagationPaths.trickleDownPath.AddRange(paths.trickleDownPath);
		propagationPaths.targetElements.AddRange(paths.targetElements);
		propagationPaths.bubbleUpPath.AddRange(paths.bubbleUpPath);
		return propagationPaths;
	}

	public static PropagationPaths Build(VisualElement elem, Type pathTypesRequested)
	{
		if (elem == null || pathTypesRequested == Type.None)
		{
			return null;
		}
		PropagationPaths propagationPaths = s_Pool.Get();
		propagationPaths.targetElements.Add(elem);
		while (elem.hierarchy.parent != null)
		{
			if (elem.hierarchy.parent.enabledInHierarchy)
			{
				if (elem.hierarchy.parent.isCompositeRoot)
				{
					propagationPaths.targetElements.Add(elem.hierarchy.parent);
				}
				else
				{
					if ((pathTypesRequested & Type.TrickleDown) == Type.TrickleDown && elem.hierarchy.parent.HasTrickleDownHandlers())
					{
						propagationPaths.trickleDownPath.Add(elem.hierarchy.parent);
					}
					if ((pathTypesRequested & Type.BubbleUp) == Type.BubbleUp && elem.hierarchy.parent.HasBubbleUpHandlers())
					{
						propagationPaths.bubbleUpPath.Add(elem.hierarchy.parent);
					}
				}
			}
			elem = elem.hierarchy.parent;
		}
		return propagationPaths;
	}

	public void Release()
	{
		bubbleUpPath.Clear();
		targetElements.Clear();
		trickleDownPath.Clear();
		s_Pool.Release(this);
	}
}
