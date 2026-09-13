using UnityEngine;
using UnityEngine.EventSystems;

namespace RuntimeInspectorNamespace;

public class TooltipArea : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private TooltipListener tooltipListener;

	private ITooltipContent tooltipContent;

	public void Initialize(TooltipListener tooltipListener, ITooltipContent tooltipContent)
	{
		this.tooltipListener = tooltipListener;
		this.tooltipContent = tooltipContent;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!eventData.dragging)
		{
			tooltipListener.OnDrawerHovered(tooltipContent, eventData, isHovering: true);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		tooltipListener.OnDrawerHovered(tooltipContent, eventData, isHovering: false);
	}
}
