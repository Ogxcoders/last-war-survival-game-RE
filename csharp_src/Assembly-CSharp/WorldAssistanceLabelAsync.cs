using UnityEngine;

public class WorldAssistanceLabelAsync : AsyncMono<WorldAssistanceLabelAsync>
{
	public SpriteRenderer icon;

	public TextMeshProEx tmpCount;

	public SpriteRenderer bg;

	public Transform root;

	public float gap = 0.1f;

	public float border;

	public Color normalColor;

	public Color maxColor;

	public float anchorX = 0.5f;

	private string iconPath = string.Empty;

	public void SetAssistance(int count, int maxCount, bool showMax, bool myAssistance)
	{
		tmpCount.text = (showMax ? $"{count}/{maxCount}" : $"{count}");
		Color color = ((count >= maxCount) ? maxColor : normalColor);
		tmpCount.color = color;
		string text = (myAssistance ? "Assets/Main/Sprites/LodIcon/wxy_dashijie_zhufang_zhushouwo.png" : "Assets/Main/Sprites/LodIcon/wxy_dashijie_zhufang_zhushou.png");
		if ((object)iconPath != text)
		{
			iconPath = text;
			icon.LoadSprite(iconPath);
		}
		RefreshLayout();
	}

	private void RefreshLayout()
	{
		float preferredWidth = tmpCount.preferredWidth;
		float x = icon.size.x;
		float num = preferredWidth + x + gap;
		float x2 = x + gap + preferredWidth * anchorX - num * anchorX;
		float x3 = x * anchorX - num * anchorX;
		Vector3 localPosition = tmpCount.transform.localPosition;
		localPosition.x = x2;
		tmpCount.transform.localPosition = localPosition;
		Vector3 localPosition2 = icon.transform.localPosition;
		localPosition2.x = x3;
		icon.transform.localPosition = localPosition2;
		Vector2 size = bg.size;
		size.x = num + border;
		bg.size = size;
	}
}
