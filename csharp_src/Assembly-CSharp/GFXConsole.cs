using BitBenderGames;
using UnityEngine;

public class GFXConsole : BaseGFXConsole
{
	private GameObject _uiContainerGO;

	private GameObject _gfxBg;

	private MobileTouchCamera _touchCamera;

	protected override void Initialize()
	{
		AddPanel(new PostProcessGFXPanel());
		AddPanel(new ScreenGFXPanel());
		AddPanel(new ShaderGFXPanel());
		AddPanel(new QualitySettingGFXPanel());
		AddPanel(new CameraGFXPanel());
		AddPanel(new SceneViewerGFXPanel());
		AddPanel(new ProfilerGFXPanel());
		_uiContainerGO = GameObject.Find("UIContainer");
		_touchCamera = GameObject.Find("Main Camera").GetComponent<MobileTouchCamera>();
		_gfxBg = GameObject.Find("GameFramework/UI/UIContainer/GfxProfilerBg");
		if ((bool)_gfxBg)
		{
			_gfxBg.transform.SetAsLastSibling();
		}
	}

	protected override void OnShowConsole()
	{
		_gfxBg.gameObject.SetActive(value: true);
	}

	protected override void OnHideConsole()
	{
		_gfxBg.gameObject.SetActive(value: false);
	}
}
