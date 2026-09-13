using UnityEngine;

public class RenderSettingAuto : MonoBehaviour
{
	[SerializeField]
	private RenderSettingData _renderSettingData;

	private void OnEnable()
	{
		RenderSettingManager.PushSetting(_renderSettingData);
	}

	private void OnDisable()
	{
		RenderSettingManager.PopSetting();
	}

	public RenderSettingData GetRenderSettingData()
	{
		return _renderSettingData;
	}
}
