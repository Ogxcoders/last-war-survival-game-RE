using TMPro;
using UnityEngine;

public class MeteoriteWorldCountdown : MonoBehaviour
{
	public SpriteRenderer srMeteoriteIcon;

	public TextMeshPro tmpCountdown;

	public TextMeshPro tmpNotice;

	public AutoAdjustScale scaler;

	public string Notice
	{
		set
		{
			tmpNotice.text = value;
		}
	}

	public string Countdown
	{
		set
		{
			tmpCountdown.text = value;
		}
	}

	private void Start()
	{
		if ((bool)scaler)
		{
			scaler.SetZScalable(zscalable: true);
		}
	}
}
