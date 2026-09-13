using UnityEngine;

public class BasementDome : MonoBehaviour
{
	private enum DomeState
	{
		Normal,
		Upgrade,
		Extend
	}

	private enum GlassAlphaState
	{
		Show,
		WaitAnim,
		ShowAnim,
		HideAnim,
		Hide
	}

	[SerializeField]
	private GameObject extendEffect;

	[SerializeField]
	private BaseGlassShield _shield;

	[SerializeField]
	private SimpleAnimation _simpleAnim;

	[SerializeField]
	private GameObject _glassGo;

	private CityBuilding parentBuilding;

	private long mainBuildUuid;

	private BuildingGrowEffect grow;

	private DomeState curState;

	private int startTime;

	private int endTime;

	private GlassAlphaState _glassAlphaState;

	public static float WaitAnimTime = 1f;

	public static float ShowAnimTime = 1f;

	public static float HideAnimTime = 1f;

	private float alphaAnimTime;

	private bool _needExpand;

	private static int ExtandAnimTime = 2;

	private int _offer_range;

	private bool _showClickAnim;

	protected internal void CSInit(object userData)
	{
		alphaAnimTime = 0f;
		_showClickAnim = false;
		if (_simpleAnim != null)
		{
			_simpleAnim.Stop();
		}
		parentBuilding = userData as CityBuilding;
		if (parentBuilding != null)
		{
			mainBuildUuid = parentBuilding.Uuid;
		}
		extendEffect.SetActive(value: false);
		grow = GetComponent<BuildingGrowEffect>();
		curState = DomeState.Normal;
		_glassAlphaState = GlassAlphaState.WaitAnim;
		grow.SetAlphaValue(1f);
		GameEntry.Event.Subscribe(EventId.OnMeteoriteHitGlass, OnShowGlassShield);
		GameEntry.Event.Subscribe(EventId.FarmGuideFakePlantShowState, FarmGuideFakePlantShowStateSignal);
		SetShieldActive(isShow: false);
		_needExpand = false;
		RefreshData();
	}

	protected internal void CSUnInit()
	{
		GameEntry.Event.Unsubscribe(EventId.OnMeteoriteHitGlass, OnShowGlassShield);
		GameEntry.Event.Unsubscribe(EventId.FarmGuideFakePlantShowState, FarmGuideFakePlantShowStateSignal);
	}

	public void UpdatePos(Vector3 pos)
	{
		base.gameObject.transform.position = pos;
	}

	private void Update()
	{
		if (curState != DomeState.Normal)
		{
			if (GameEntry.Timer.GetServerTimeSeconds() < endTime)
			{
				return;
			}
			if (curState == DomeState.Upgrade)
			{
				SceneManager.World.AutoLookat(SceneManager.World.CurTarget, SceneManager.World.InitZoom, 0.4f);
				ShowNormal();
				if (_needExpand)
				{
					ShowExtend();
					return;
				}
				Vector2Int curTilePos = SceneManager.World.CurTilePos;
				SceneManager.World.SendViewRequest(curTilePos, SceneManager.World.GetLodLevel(), GameEntry.Data.Player.GetSelfServerId());
				curState = DomeState.Normal;
				_glassAlphaState = GlassAlphaState.WaitAnim;
			}
			else if (curState == DomeState.Extend)
			{
				extendEffect.SetActive(value: false);
				Vector2Int curTilePos2 = SceneManager.World.CurTilePos;
				SceneManager.World.SendViewRequest(curTilePos2, SceneManager.World.GetLodLevel(), GameEntry.Data.Player.GetSelfServerId());
				curState = DomeState.Normal;
				_glassAlphaState = GlassAlphaState.WaitAnim;
			}
			return;
		}
		CheckGlassAlpha();
		if (_glassAlphaState == GlassAlphaState.WaitAnim)
		{
			alphaAnimTime += Time.deltaTime;
			if (alphaAnimTime > WaitAnimTime)
			{
				alphaAnimTime = 0f;
				_glassAlphaState = GlassAlphaState.Show;
			}
		}
		else if (_glassAlphaState == GlassAlphaState.ShowAnim)
		{
			alphaAnimTime += Time.deltaTime;
			if (alphaAnimTime > ShowAnimTime)
			{
				alphaAnimTime = 0f;
				grow.SetAlphaValue(1f);
				_glassAlphaState = GlassAlphaState.Show;
			}
		}
		else if (_glassAlphaState == GlassAlphaState.HideAnim)
		{
			alphaAnimTime += Time.deltaTime;
			if (alphaAnimTime > HideAnimTime)
			{
				alphaAnimTime = 0f;
				_glassAlphaState = GlassAlphaState.Hide;
			}
		}
	}

	public void ShowNormal()
	{
		if (!(grow == null))
		{
			grow.EndAnim();
			grow.ShowNormal();
			grow.enabled = false;
		}
	}

	public void ShowExtend()
	{
		extendEffect.SetActive(value: true);
		curState = DomeState.Extend;
		startTime = GameEntry.Timer.GetServerTimeSeconds();
		endTime = startTime + ExtandAnimTime;
	}

	private void OnShowGlassShield(object userData)
	{
		if ((long)userData == mainBuildUuid && !HasDomeProtect())
		{
			SetShieldActive(isShow: true);
			_shield.Hit();
		}
	}

	private void SetShieldActive(bool isShow)
	{
		_shield.gameObject.SetActive(isShow);
	}

	private void CheckGlassAlpha()
	{
		if (curState != DomeState.Normal || (_glassAlphaState != GlassAlphaState.Show && _glassAlphaState != GlassAlphaState.Hide))
		{
			return;
		}
		if (SceneManager.World.GetLodDistance() > GetHideY())
		{
			if (_glassAlphaState == GlassAlphaState.Hide)
			{
				_glassAlphaState = GlassAlphaState.ShowAnim;
				GameEntry.Event.Fire(EventId.CityDomeShow, parentBuilding.Uuid);
			}
		}
		else if (_glassAlphaState == GlassAlphaState.Show)
		{
			_glassAlphaState = GlassAlphaState.HideAnim;
			GameEntry.Event.Fire(EventId.CityDomeHide, parentBuilding.Uuid);
		}
	}

	private float GetHideY()
	{
		if (SceneManager.World.GetPointInfoByUuid(mainBuildUuid) is BuildPointInfo buildPointInfo)
		{
			switch (buildPointInfo.level)
			{
			case 1:
				return 35f;
			case 2:
				return 40f;
			case 3:
				return 50f;
			}
		}
		return 35f;
	}

	public void RefreshData()
	{
		BuildPointInfo buildInfo = parentBuilding.GetBuildInfo();
		if (buildInfo == null)
		{
			return;
		}
		int buildId = buildInfo.itemId + buildInfo.level;
		int buildOffsetRangeByBuildId = SceneManager.World.GetBuildOffsetRangeByBuildId(buildId);
		if (buildOffsetRangeByBuildId > 0)
		{
			int num = buildOffsetRangeByBuildId;
			if (num != _offer_range && _offer_range <= 0)
			{
				_offer_range = num;
			}
		}
	}

	public void ShowClickAnim()
	{
		if (_simpleAnim != null)
		{
			_simpleAnim.Play("saoguang");
		}
		_showClickAnim = true;
	}

	public void HideClickAnim()
	{
		if (_simpleAnim != null)
		{
			_simpleAnim.Stop();
		}
		_showClickAnim = false;
	}

	private void FarmGuideFakePlantShowStateSignal(object userData)
	{
		bool active = (bool)userData;
		if (_glassGo != null)
		{
			_glassGo.gameObject.SetActive(active);
		}
	}

	private bool HasDomeProtect()
	{
		if (SceneManager.World.GetPointInfoByUuid(mainBuildUuid) is BuildPointInfo buildPointInfo)
		{
			int serverTimeSeconds = GameEntry.Timer.GetServerTimeSeconds();
			return buildPointInfo.protectEndTime > serverTimeSeconds;
		}
		return false;
	}
}
