using UnityEngine;

public class ScreenEdgeAlign : MonoBehaviour
{
	private const int DefaultScreenWidth = 810;

	private const int DefaultScreenHeight = 1440;

	public Transform TopNode;

	public Transform BottomNode;

	public Transform LeftNode;

	public Transform RightNode;

	[SerializeField]
	private float PositionChangeRatio = 1f;

	[SerializeField]
	private int MaxEffectWidth = 810;

	[SerializeField]
	private int MaxEffectHeight = 1440;

	private bool _Dirty = true;

	public void Awake()
	{
		_Dirty = true;
	}

	private void OnRectTransformDimensionsChange()
	{
		_Dirty = true;
	}

	private void Update()
	{
		if (_Dirty)
		{
			_Dirty = false;
			Refresh();
		}
	}

	private void Refresh()
	{
		GetScreenSize(out var width, out var height);
		UpdateNodePositions(width, height);
	}

	private void GetScreenSize(out int width, out int height)
	{
		if ((bool)GameEntry.UIContainer)
		{
			RectTransform component = GameEntry.UIContainer.GetComponent<RectTransform>();
			if (component != null)
			{
				width = (int)component.rect.width;
				height = (int)component.rect.height;
				return;
			}
		}
		width = Screen.width;
		height = Screen.height;
	}

	private void UpdateNodePositions(int currentScreenWidth, int currentScreenHeight)
	{
		float num = (float)(currentScreenWidth - 810) * PositionChangeRatio;
		float num2 = (float)(currentScreenHeight - 1440) * PositionChangeRatio;
		if (TopNode != null)
		{
			TopNode.localPosition = new Vector3(0f, num2, 0f);
		}
		if (BottomNode != null)
		{
			BottomNode.localPosition = new Vector3(0f, 0f - num2, 0f);
		}
		if (LeftNode != null)
		{
			LeftNode.localPosition = new Vector3(0f - num, 0f, 0f);
		}
		if (RightNode != null)
		{
			RightNode.localPosition = new Vector3(num, 0f, 0f);
		}
		AdjustScaleForEdges(currentScreenWidth, currentScreenHeight);
	}

	private void AdjustScaleForEdges(int currentScreenWidth, int currentScreenHeight)
	{
		base.transform.localScale = Vector3.one;
		float x = 1f;
		if (currentScreenWidth > MaxEffectWidth)
		{
			x = (float)currentScreenWidth / (float)MaxEffectWidth;
		}
		if (TopNode != null)
		{
			TopNode.localScale = new Vector3(x, TopNode.localScale.y, TopNode.localScale.z);
		}
		if (BottomNode != null)
		{
			BottomNode.localScale = new Vector3(x, BottomNode.localScale.y, BottomNode.localScale.z);
		}
		x = 1f;
		if (currentScreenHeight > MaxEffectHeight)
		{
			x = (float)currentScreenHeight / (float)MaxEffectHeight;
		}
		if (LeftNode != null)
		{
			LeftNode.localScale = new Vector3(LeftNode.localScale.x, x, LeftNode.localScale.z);
		}
		if (RightNode != null)
		{
			RightNode.localScale = new Vector3(RightNode.localScale.x, x, RightNode.localScale.z);
		}
	}
}
