using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.NiceVibrations;

public class WobbleDemoManager : DemoManager
{
	public Camera ButtonCamera;

	public RectTransform ContentZone;

	public WobbleButton WobbleButtonPrefab;

	public Vector2 PrefabSize = new Vector2(200f, 200f);

	public float Margin = 20f;

	public float Padding = 20f;

	protected List<WobbleButton> Buttons;

	protected Canvas _canvas;

	protected Vector3 _position = Vector3.zero;

	protected virtual void Start()
	{
		_canvas = GetComponentInParent<Canvas>();
		float f = (ContentZone.rect.width - 2f * Padding) / (PrefabSize.x + Margin);
		float f2 = (ContentZone.rect.height - 2f * Padding) / (PrefabSize.y + Margin);
		int num = Mathf.FloorToInt(f);
		int num2 = Mathf.FloorToInt(f2);
		float num3 = (ContentZone.rect.width - Padding * 2f - (float)num * PrefabSize.x - (float)(num - 1) * Margin) / 2f;
		float num4 = (ContentZone.rect.height - Padding * 2f - (float)num2 * PrefabSize.x - (float)(num2 - 1) * Margin) / 2f;
		Buttons = new List<WobbleButton>();
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				_position.x = num3 + Padding + PrefabSize.x / 2f + (float)i * (PrefabSize.x + Margin);
				_position.y = num4 + Padding + PrefabSize.y / 2f + (float)j * (PrefabSize.y + Margin);
				_position.z = 0f;
				WobbleButton wobbleButton = Object.Instantiate(WobbleButtonPrefab);
				wobbleButton.transform.SetParent(ContentZone.transform);
				Buttons.Add(wobbleButton);
				RectTransform component = wobbleButton.GetComponent<RectTransform>();
				component.anchorMin = Vector2.zero;
				component.anchorMax = Vector2.zero;
				wobbleButton.name = "WobbleButton" + i + j;
				wobbleButton.transform.localScale = Vector3.one;
				component.anchoredPosition3D = _position;
				wobbleButton.TargetCamera = ButtonCamera;
				wobbleButton.Initialization();
			}
		}
		int num5 = 0;
		foreach (WobbleButton button in Buttons)
		{
			float pitch = NiceVibrationsDemoHelpers.Remap(num5, 0f, Buttons.Count, 0.3f, 1f);
			button.SetPitch(pitch);
			num5++;
		}
	}
}
