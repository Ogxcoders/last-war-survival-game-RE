using UnityEngine;

public class BuildLightHouse : MonoBehaviour
{
	[SerializeField]
	private GameObject lightOn;

	[SerializeField]
	private GameObject lightOff;

	[SerializeField]
	private GameObject lightOnL1;

	[SerializeField]
	private GameObject lightOnL2;

	[SerializeField]
	private GameObject lightOnL3;

	[SerializeField]
	private GameObject lightOnL4;

	[SerializeField]
	private GameObject line1;

	[SerializeField]
	private GameObject line2;

	[SerializeField]
	private GameObject line3;

	[SerializeField]
	private GameObject line4;

	[SerializeField]
	private GameObject lineWork1;

	[SerializeField]
	private GameObject lineWork2;

	[SerializeField]
	private GameObject lineWork3;

	[SerializeField]
	private GameObject lineWork4;

	[SerializeField]
	private GameObject switchButtonOn;

	[SerializeField]
	private GameObject switchButtonOff;

	public void UpdateData(int brightnessLevel, bool activePower1, bool activePower2, bool activePower3, bool activePower4)
	{
		lightOn.SetActive(brightnessLevel > 0);
		lightOff.SetActive(brightnessLevel == 0);
		switchButtonOn.SetActive(brightnessLevel > 0);
		switchButtonOff.SetActive(brightnessLevel == 0);
		if (brightnessLevel > 0)
		{
			lightOnL1.SetActive(brightnessLevel == 1);
			lightOnL2.SetActive(brightnessLevel == 2);
			lightOnL3.SetActive(brightnessLevel == 3);
			lightOnL4.SetActive(brightnessLevel == 4);
		}
		line1.SetActive(activePower1);
		line2.SetActive(activePower2);
		line3.SetActive(activePower3);
		line4.SetActive(activePower4);
	}

	public void UpdateLineStatus(bool activePower1, bool activePower2, bool activePower3, bool activePower4)
	{
		lineWork1.SetActive(activePower1);
		lineWork2.SetActive(activePower2);
		lineWork3.SetActive(activePower3);
		lineWork4.SetActive(activePower4);
	}
}
