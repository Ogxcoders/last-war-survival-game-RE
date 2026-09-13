using UnityEngine.EventSystems;

namespace UnityEngine.UI;

internal static class MultipleDisplayUtilities
{
	public static bool GetRelativeMousePositionForDrag(PointerEventData eventData, ref Vector2 position)
	{
		int displayIndex = eventData.pointerPressRaycast.displayIndex;
		Vector3 vector = RelativeMouseAtScaled(eventData.position);
		if ((int)vector.z != displayIndex)
		{
			return false;
		}
		position = ((displayIndex != 0) ? ((Vector2)vector) : eventData.position);
		return true;
	}

	public static Vector3 RelativeMouseAtScaled(Vector2 position)
	{
		if (Display.main.renderingWidth != Display.main.systemWidth || Display.main.renderingHeight != Display.main.systemHeight)
		{
			int num = (Screen.fullScreen ? Display.main.renderingWidth : ((int)((float)Display.main.renderingHeight * ((float)Display.main.systemWidth / (float)Display.main.systemHeight))));
			int num2 = ((!Screen.fullScreen) ? ((int)((float)(num - Display.main.renderingWidth) * 0.5f)) : 0);
			int num3 = num - num2;
			if (position.y < 0f || position.y > (float)Display.main.renderingHeight || position.x < 0f || position.x > (float)num3)
			{
				if (!Screen.fullScreen)
				{
					position.x -= (float)(Display.main.renderingWidth - Display.main.systemWidth) * 0.5f;
					position.y -= (float)(Display.main.renderingHeight - Display.main.systemHeight) * 0.5f;
				}
				else
				{
					position.x += num2;
					float num4 = (float)Display.main.systemWidth / (float)num;
					float num5 = (float)Display.main.systemHeight / (float)Display.main.renderingHeight;
					position.x *= num4;
					position.y *= num5;
				}
				return Display.RelativeMouseAt(position);
			}
			return new Vector3(position.x, position.y, 0f);
		}
		return Display.RelativeMouseAt(position);
	}
}
