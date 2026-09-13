using UnityEngine;

public class ScreenSafeArea : MonoBehaviour
{
	public Rect iphoneDelta = new Rect(50f, 0f, -100f, 0f);

	[SerializeField]
	private RectTransform Panel;

	private Rect LastSafeArea = new Rect(0f, 0f, 0f, 0f);

	private float ScreenWidth;

	private float ScreenHeight;

	private void Awake()
	{
		Panel = GetComponent<RectTransform>();
		Refresh();
		ScreenHeight = Screen.height;
		ScreenWidth = Screen.width;
	}

	private void Update()
	{
		CheckSize();
	}

	private void CheckSize()
	{
		if (ScreenHeight != (float)Screen.height || ScreenWidth != (float)Screen.width)
		{
			Refresh();
			ScreenHeight = Screen.height;
			ScreenWidth = Screen.width;
			GameEntry.Event.Fire(EventId.SCREEN_SIZE_CHANGE);
		}
	}

	private void Refresh()
	{
		Rect safeArea = GetSafeArea();
		if (safeArea != LastSafeArea)
		{
			SetSafeArea(safeArea);
		}
	}

	public static Rect GetSafeArea()
	{
		Rect rect = Screen.safeArea;
		string androidScreenNotch = GameEntry.Sdk.AndroidScreenNotch;
		if (!androidScreenNotch.IsNullOrEmpty())
		{
			string[] array = androidScreenNotch.Split(new char[1] { ';' });
			if (array.Length > 3)
			{
				array[0].ToFloat();
				array[1].ToFloat();
				float num = array[2].ToFloat();
				array[3].ToFloat();
				rect = new Rect(0f, 0f, Screen.width, (float)Screen.height - num);
			}
		}
		float num2 = (float)Screen.height - rect.yMax;
		return new Rect(0f, 0f, Screen.width, (float)Screen.height - num2);
	}

	private void SetSafeArea(Rect r)
	{
		LastSafeArea = r;
		Vector2 position = r.position;
		Vector2 anchorMax = r.position + r.size;
		position.x /= Screen.width;
		position.y /= Screen.height;
		anchorMax.x /= Screen.width;
		anchorMax.y /= Screen.height;
		Panel.anchorMin = position;
		Panel.anchorMax = anchorMax;
	}
}
