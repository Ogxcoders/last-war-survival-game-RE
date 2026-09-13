using UnityEngine;

public class DominatorMainScene : MonoBehaviour
{
	[SerializeField]
	private RenderSettingAuto _renderSetting;

	private void Awake()
	{
		if (_renderSetting != null)
		{
			RenderSettingData renderSettingData = _renderSetting.GetRenderSettingData();
			if (renderSettingData != null)
			{
				renderSettingData.fog = !IsPC();
			}
		}
	}

	private bool IsPC()
	{
		return false;
	}
}
