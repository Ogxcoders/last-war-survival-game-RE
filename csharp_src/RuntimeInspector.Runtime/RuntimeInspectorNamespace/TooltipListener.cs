using UnityEngine;
using UnityEngine.EventSystems;

namespace RuntimeInspectorNamespace;

public class TooltipListener : MonoBehaviour
{
	private ITooltipManager manager;

	private ITooltipContent hoveredDrawer;

	private PointerEventData hoveringPointer;

	private float hoveredDrawerTooltipShowTime;

	public void Initialize(ITooltipManager manager)
	{
		this.manager = manager;
	}

	private void Update()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (hoveringPointer == null)
		{
			return;
		}
		Vector2 delta = hoveringPointer.delta;
		if (delta.x != 0f || delta.y != 0f)
		{
			hoveredDrawerTooltipShowTime = realtimeSinceStartup + manager.TooltipDelay;
		}
		else if (realtimeSinceStartup > hoveredDrawerTooltipShowTime)
		{
			if (!hoveredDrawer.IsActive)
			{
				hoveredDrawer = null;
				hoveringPointer = null;
			}
			else
			{
				RuntimeInspectorUtils.ShowTooltip(hoveredDrawer.TooltipText, hoveringPointer, manager.Skin, manager.Canvas);
				hoveredDrawerTooltipShowTime = float.PositiveInfinity;
			}
		}
	}

	internal void OnDrawerHovered(ITooltipContent drawer, PointerEventData pointer, bool isHovering)
	{
		RuntimeInspectorUtils.HideTooltip();
		if (isHovering)
		{
			hoveredDrawer = drawer;
			hoveringPointer = pointer;
			hoveredDrawerTooltipShowTime = Time.realtimeSinceStartup + manager.TooltipDelay;
		}
		else if (drawer == null || hoveredDrawer == drawer)
		{
			hoveredDrawer = null;
			hoveringPointer = null;
		}
	}
}
