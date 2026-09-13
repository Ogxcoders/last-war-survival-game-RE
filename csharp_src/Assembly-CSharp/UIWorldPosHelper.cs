using UnityEngine;

public class UIWorldPosHelper
{
	private float scaleFactor = 1f;

	public void Init(float scale)
	{
		scaleFactor = scale;
	}

	public void CalcConstructMilePointerSimple(float leftPadding, float topPadding, Vector3 mainWorldPos, out bool show, out float dist, out float posX, out float posY, out float eulerAngleZ)
	{
		show = false;
		dist = 0f;
		posX = 0f;
		posY = 0f;
		eulerAngleZ = 0f;
		Vector3 vector = Camera.main.WorldToScreenPoint(mainWorldPos);
		float num = Screen.width;
		float num2 = Screen.height;
		if (!(vector.x > 0f) || !(vector.x < num) || !(vector.y > 0f) || !(vector.y < num2))
		{
			Vector2 vector2 = new Vector2(vector.x, vector.y);
			show = true;
			leftPadding *= scaleFactor;
			topPadding *= scaleFactor;
			if (vector.x < leftPadding)
			{
				vector.x = leftPadding;
			}
			else if (vector.x > num - leftPadding)
			{
				vector.x = num - leftPadding;
			}
			if (vector.y < topPadding)
			{
				vector.y = topPadding;
			}
			else if (vector.y > num2 - topPadding)
			{
				vector.y = num2 - topPadding;
			}
			Vector2 vector3 = vector2 - new Vector2(num / 2f, num2 / 2f);
			eulerAngleZ = Quaternion.FromToRotation(toDirection: new Vector3(vector3.x, vector3.y, 0f), fromDirection: Vector3.up).eulerAngles.z;
			Vector3 vector4 = GameEntry.UICamera.ScreenToWorldPoint(new Vector3(vector.x, vector.y, 0f));
			posX = vector4.x;
			posY = vector4.y;
		}
	}
}
