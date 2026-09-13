using Tayx.Graphy.Utils.NumString;
using UnityEngine;
using UnityEngine.UI;

namespace Tayx.Graphy.Battery;

public class BatteryText : MonoBehaviour
{
	public Text capacity;

	public Text voltage;

	public Text electricity;

	public Text power;

	public Text time;

	private float lastTime;

	private void Update()
	{
		float num = Time.time;
		if (num - lastTime > 10f)
		{
			lastTime = num;
			float num2 = Battery.capacity;
			float num3 = Battery.voltage;
			float num4 = Battery.electricity;
			int value = (int)(num4 * num3 * 0.001f);
			float value2 = num2 / num4;
			capacity.text = num2.ToStringNonAlloc();
			voltage.text = num3.ToStringNonAlloc();
			electricity.text = num4.ToStringNonAlloc();
			power.text = value.ToStringNonAlloc();
			time.text = value2.ToStringNonAlloc();
		}
	}
}
