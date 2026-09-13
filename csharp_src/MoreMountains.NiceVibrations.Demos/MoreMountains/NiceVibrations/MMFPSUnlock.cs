using UnityEngine;

namespace MoreMountains.NiceVibrations;

public class MMFPSUnlock : MonoBehaviour
{
	public int TargetFPS;

	[Range(0f, 2f)]
	public int VSyncCount;

	protected virtual void Start()
	{
		UpdateSettings();
	}

	protected virtual void OnValidate()
	{
		UpdateSettings();
	}

	protected virtual void UpdateSettings()
	{
		QualitySettings.vSyncCount = VSyncCount;
		Application.targetFrameRate = TargetFPS;
	}
}
