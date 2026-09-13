using UnityEngine;

public class UICityMeteorite : MonoBehaviour
{
	public SuperTextMesh crystalCount;

	public SuperTextMesh nucleusCount;

	public GameObject goCrystal;

	public GameObject goNucleus;

	public Vector3 leftPosition;

	public AutoFaceToCamera faceToCamera;

	public void Refresh(int crystal, int nucleus)
	{
		if (crystal > 0)
		{
			crystalCount.text = $"x{crystal}";
			goCrystal.transform.localPosition = ((nucleus > 0) ? leftPosition : Vector3.zero);
			goCrystal.TryActive(active: true);
		}
		else
		{
			goCrystal.TryActive(active: false);
		}
		if (nucleus > 0)
		{
			nucleusCount.text = $"x{nucleus}";
			goNucleus.transform.localPosition = ((crystal > 0) ? (-leftPosition) : Vector3.zero);
			goNucleus.TryActive(active: true);
		}
		else
		{
			goNucleus.TryActive(active: false);
		}
		if ((crystal > 0 || nucleus > 0) && faceToCamera != null)
		{
			faceToCamera.enabled = true;
			faceToCamera.IgnoreCacheRotation = true;
		}
	}
}
