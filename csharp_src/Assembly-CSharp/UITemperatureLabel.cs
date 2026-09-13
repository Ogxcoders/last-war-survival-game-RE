using GameFramework;
using UnityEngine;

public class UITemperatureLabel : MonoBehaviour
{
	[SerializeField]
	private GameObject temp;

	[SerializeField]
	private SpriteRenderer black1;

	[SerializeField]
	private SpriteRenderer tempBg;

	[SerializeField]
	private SuperTextMesh tempTxt;

	[SerializeField]
	private GameObject phase;

	[SerializeField]
	private SpriteRenderer black2;

	[SerializeField]
	private SpriteRenderer slider;

	[SerializeField]
	private SuperTextMesh timeTxt;

	[SerializeField]
	private SpriteRenderer phaseBg;

	[SerializeField]
	private SpriteRenderer phaseIcon;

	private float updateCd;

	private bool needShowTemp;

	private bool needShowPhaseChange;

	private bool hide;

	private ThermalConductor conductor;

	private string uuid;

	private bool listen;

	private void AddListener()
	{
		if (!listen)
		{
			listen = true;
			GameEntry.Event.Subscribe(EventId.HideBaseTemperature, SetHide);
			GameEntry.Event.Subscribe(EventId.OtherBaseThermalRefresh, OnOtherBaseThermalRefresh);
		}
	}

	private void RemoveListener()
	{
		if (listen)
		{
			listen = false;
			GameEntry.Event.Unsubscribe(EventId.HideBaseTemperature, SetHide);
			GameEntry.Event.Unsubscribe(EventId.OtherBaseThermalRefresh, OnOtherBaseThermalRefresh);
		}
	}

	private void OnOtherBaseThermalRefresh(object param)
	{
		if (hide || !(param is string text) || !(text == uuid))
		{
			return;
		}
		if (SceneManager.World != null && long.TryParse(text, out var result))
		{
			PointInfo pointInfoByUuid = SceneManager.World.GetPointInfoByUuid(result);
			if (pointInfoByUuid != null && pointInfoByUuid.thermalConductor != null)
			{
				conductor = pointInfoByUuid.thermalConductor;
			}
		}
		RefreshView();
	}

	public void SetHide(object param)
	{
		if (param is bool flag)
		{
			hide = flag;
			RefreshView();
		}
	}

	public void SetConductor(ThermalConductor conductor, bool needShowTemp)
	{
		if (conductor != null)
		{
			this.needShowTemp = needShowTemp;
			this.conductor = conductor;
			uuid = conductor.uuid;
			hide = GameEntry.Setting.GetBool("HIDE_BASE_TEMPERATURE", defaultValue: false);
			RefreshView();
			AddListener();
		}
	}

	public void Dispose()
	{
		RemoveListener();
		conductor = null;
		uuid = null;
	}

	private void RefreshView()
	{
		if (hide)
		{
			phase.SetActive(value: false);
			temp.SetActive(value: false);
		}
		else if (conductor.IsPhaseChanging())
		{
			needShowPhaseChange = true;
			phase.SetActive(value: true);
			temp.SetActive(value: false);
			RefreshPhaseChange();
		}
		else if (needShowTemp)
		{
			phase.SetActive(value: false);
			temp.SetActive(value: true);
			RefreshTemperature();
		}
		else
		{
			phase.SetActive(value: false);
			temp.SetActive(value: false);
		}
	}

	private void RefreshPhaseChange()
	{
		long serverTime = GameEntry.Timer.GetServerTime();
		if (serverTime < conductor.nextPhaseEndTime)
		{
			float curTemperature = conductor.GetCurTemperature();
			long num = conductor.nextPhaseEndTime - serverTime;
			timeTxt.text = GameEntry.Timer.MillisecondsToStringWithoutHour(num, ":");
			double num2 = (double)num / (double)conductor.changePhaseDuration;
			Vector2 size = slider.size;
			size.x = 1.52f * (float)num2;
			slider.size = size;
			Color temperatureColor = WorldCityColor.GetTemperatureColor(curTemperature);
			phaseBg.color = temperatureColor;
			slider.color = temperatureColor;
			if (conductor.nextPhase == 2)
			{
				phaseIcon.LoadSprite("Assets/Main/Sprites/UI/UISeason/UISeason2/WorldPoint/FX_S2saiji_wendu_bing.png");
			}
			else if (conductor.nextPhase == 3)
			{
				phaseIcon.LoadSprite("Assets/Main/Sprites/UI/UISeason/UISeason2/WorldPoint/FX_S2saiji_wendu_huo.png");
			}
			else if (conductor.phase == 2)
			{
				phaseIcon.LoadSprite("Assets/Main/Sprites/UI/UISeason/UISeason2/WorldPoint/FX_S2saiji_wendu_ronghua.png");
			}
			else if (conductor.phase == 3)
			{
				phaseIcon.LoadSprite("Assets/Main/Sprites/UI/UISeason/UISeason2/WorldPoint/FX_S2saiji_wendu_mie.png");
			}
			else
			{
				Log.Error($"phase change error, curPhase={conductor.phase},nextPhase={conductor.nextPhase}");
			}
		}
		else
		{
			needShowPhaseChange = false;
			phase.SetActive(value: false);
			if (needShowTemp)
			{
				temp.SetActive(value: true);
				RefreshTemperature();
			}
		}
	}

	private void RefreshTemperature()
	{
		float curTemperature = conductor.GetCurTemperature();
		tempTxt.text = conductor.GetCurTemperatureString();
		tempBg.color = WorldCityColor.GetTemperatureColor(curTemperature);
	}

	private void Update()
	{
		if (hide)
		{
			return;
		}
		if (needShowPhaseChange)
		{
			updateCd += Time.deltaTime;
			if (updateCd >= 1f)
			{
				updateCd -= 1f;
				RefreshPhaseChange();
			}
		}
		else if (needShowTemp)
		{
			updateCd += Time.deltaTime;
			if (updateCd >= 1f)
			{
				updateCd -= 1f;
				RefreshTemperature();
			}
		}
	}
}
