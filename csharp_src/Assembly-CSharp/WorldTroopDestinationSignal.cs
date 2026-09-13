using System.Collections;
using UnityEngine;

public class WorldTroopDestinationSignal : MonoBehaviour
{
	[SerializeField]
	private Transform _transContainer;

	[SerializeField]
	private Transform _transCircleEffContainer;

	[SerializeField]
	private GameObject[] _goCircleEffs;

	[SerializeField]
	private SimpleAnimation[] _animCircleEffs;

	[SerializeField]
	private GameObject _goPointEff;

	[SerializeField]
	private GameObject _goHeadInfo;

	[SerializeField]
	private GameObject _goOperateIcon;

	[SerializeField]
	private SuperTextMesh _txtDistance;

	[SerializeField]
	private GameObject _imgDistance;

	[SerializeField]
	private SpriteRenderer _sprOperateType;

	private EnumDestinationSignalType _signalType;

	private Vector3 _signalPos;

	private int _cacheLastTile = -1;

	private Coroutine _corPlaySelectAnim;

	private Coroutine _corPlayPointAnim;

	private float _distance;

	private const string TARGET_TYPE_ICON_PATH = "Assets/Main/Sprites/UI/LWCommon/Sprite/{0}.png";

	public void SetDestinationOver()
	{
		_goHeadInfo.SetActive(value: false);
		_transContainer.position = _signalPos;
		if (_signalType == EnumDestinationSignalType.EmptyGround)
		{
			ShowCircleEff();
			if (_distance != 0f)
			{
				PlayPointAnim();
			}
		}
		else
		{
			base.gameObject.SetActive(value: false);
		}
	}

	private void ShowDestinationCircleEff(Vector3 targetPos, EnumDestinationSignalType signalType, int tileSize)
	{
		_signalType = signalType;
		_signalPos = GetFianlPos(targetPos, tileSize);
		if (_signalType == EnumDestinationSignalType.None)
		{
			base.gameObject.SetActive(value: false);
		}
		else if (_signalType == EnumDestinationSignalType.EmptyGround)
		{
			_transContainer.position = targetPos;
			ShowCircleEff();
		}
		else if (_signalType == EnumDestinationSignalType.UnReachAble)
		{
			base.gameObject.SetActive(value: false);
			_transContainer.position = targetPos;
			ShowCircleEff();
		}
		else if (_signalType == EnumDestinationSignalType.EnemyMarch)
		{
			_transContainer.position = targetPos;
			int circleEffIndex = GetCircleEffIndex(_signalType);
			ShowCircleEff(circleEffIndex);
			PlaySelectAnim(circleEffIndex);
		}
		else
		{
			_transContainer.position = _signalPos;
			int circleEffIndex2 = GetCircleEffIndex(_signalType);
			ShowCircleEff(circleEffIndex2);
			PlaySelectAnim(circleEffIndex2);
		}
	}

	public void SetDestinationForMarch(Vector3 localDestination, EnumDestinationSignalType signalType, int tileSize)
	{
		base.gameObject.SetActive(value: true);
		_goPointEff.SetActive(value: false);
		_goHeadInfo.SetActive(value: false);
		ShowDestinationCircleEff(localDestination, signalType, tileSize);
	}

	public void SetDestination(Vector3 localDestination, EnumDestinationSignalType signalType, MarchTargetType targetType, int tileSize, float distance, bool isTower = false)
	{
		int num = SceneManager.World.WorldToTileIndex(localDestination);
		if (num != _cacheLastTile)
		{
			_cacheLastTile = -1;
			base.gameObject.SetActive(value: true);
			_goPointEff.SetActive(value: false);
			if (isTower && targetType != MarchTargetType.ATTACK_ARMY)
			{
				_goHeadInfo.SetActive(value: false);
			}
			else
			{
				_goHeadInfo.SetActive(value: true);
			}
			ShowDestinationCircleEff(localDestination, signalType, tileSize);
			if (_signalType != EnumDestinationSignalType.None && _signalType != EnumDestinationSignalType.EmptyGround)
			{
				_cacheLastTile = num;
			}
			_distance = distance;
			_txtDistance.gameObject.SetActive(distance != 0f);
			_imgDistance.SetActive(distance != 0f);
			_txtDistance.text = (int)distance + GameEntry.Localization.GetString("100204");
			_goOperateIcon.SetActive(value: true);
			string operateIcon = GetOperateIcon(targetType);
			_sprOperateType.LoadSprite(operateIcon);
		}
	}

	private string GetOperateIcon(MarchTargetType tempType)
	{
		string text = "";
		switch (tempType)
		{
		case MarchTargetType.ATTACK_MONSTER:
		case MarchTargetType.ATTACK_BUILDING:
		case MarchTargetType.ATTACK_ARMY:
		case MarchTargetType.ATTACK_ARMY_COLLECT:
		case MarchTargetType.ATTACK_CITY:
		case MarchTargetType.ATTACK_ROAD:
		case MarchTargetType.ATTACK_ALLIANCE_CITY:
		case MarchTargetType.DIRECT_ATTACK_ACT_BOSS:
		case MarchTargetType.ATTACK_THRONE:
		case MarchTargetType.ATTACK_CITY_STRONGHOLD:
		case MarchTargetType.ATTACK_WINTER_STORM_CITY:
			text = "Common_icon_march_battle";
			break;
		case MarchTargetType.COLLECT:
		case MarchTargetType.SAMPLE:
		case MarchTargetType.PICK_GARBAGE:
			text = "Common_icon_march_collect";
			break;
		case MarchTargetType.ASSISTANCE_BUILD:
		case MarchTargetType.ASSISTANCE_CITY:
		case MarchTargetType.ASSISTANCE_THRONE:
		case MarchTargetType.ASSISTANCE_CITY_STRONGHOLD:
		case MarchTargetType.ASSISTANCE_WINTER_STORM_CITY:
		case MarchTargetType.ASSISTANCE_EPIDEMIC_CITY:
		case MarchTargetType.ASSISTANCE_OUTPOST_BUILDING:
			text = "Common_icon_march_assistance";
			break;
		case MarchTargetType.BACK_HOME:
			text = "Common_icon_march_return";
			break;
		default:
			text = "Common_icon_march_station";
			break;
		}
		return $"Assets/Main/Sprites/UI/LWCommon/Sprite/{text}.png";
	}

	private int GetCircleEffIndex(EnumDestinationSignalType tempType)
	{
		int result = 0;
		switch (tempType)
		{
		case EnumDestinationSignalType.My:
		case EnumDestinationSignalType.Other:
			result = 0;
			break;
		case EnumDestinationSignalType.Alliance:
			result = 1;
			break;
		case EnumDestinationSignalType.EnemyBuild:
		case EnumDestinationSignalType.EnemyMarch:
			result = 2;
			break;
		}
		return result;
	}

	private void ShowCircleEff(int effType = -1)
	{
		for (int i = 0; i < _goCircleEffs.Length; i++)
		{
			_goCircleEffs[i].SetActive(effType == i);
		}
	}

	private void PlayPointAnim(int effIndex = -1)
	{
		if (_corPlayPointAnim != null)
		{
			StopCoroutine(_corPlayPointAnim);
		}
		if (base.gameObject.activeSelf && effIndex == -1)
		{
			_corPlayPointAnim = StartCoroutine(CorPlayPointAnim(effIndex));
		}
	}

	private IEnumerator CorPlayPointAnim(int effIndex)
	{
		if (effIndex == -1)
		{
			_goPointEff.SetActive(value: true);
		}
		yield return new WaitForSeconds(3.5f);
		base.gameObject.SetActive(value: false);
	}

	private void PlaySelectAnim(int effIndex)
	{
		_animCircleEffs[effIndex].Play();
	}

	public void HideDestination()
	{
		base.gameObject.SetActive(value: false);
	}

	private Vector3 GetFianlPos(Vector3 localPos, int tileSize)
	{
		int num = SceneManager.World.WorldToTileIndex(localPos);
		Vector3 vector = SceneManager.World.TileIndexToWorld(num);
		if (tileSize > 1 && _signalType != EnumDestinationSignalType.Other && _signalType != EnumDestinationSignalType.EnemyMarch)
		{
			switch (tileSize)
			{
			case 2:
				vector -= new Vector3(1f, 0f, 1f);
				break;
			case 3:
			{
				int index = GameEntry.Lua.CallWithReturn<int, int, int>("CSharpCallLuaInterface.GetBuildModelCenter", num, tileSize);
				vector = SceneManager.World.TileIndexToWorld(index);
				break;
			}
			case 7:
				vector = vector;
				break;
			}
		}
		float num2 = 0.6f;
		if (tileSize > 1 && tileSize <= 3)
		{
			num2 = 1f;
		}
		else if (tileSize > 3)
		{
			num2 = (float)tileSize / 2f;
		}
		_transCircleEffContainer.localScale = Vector3.one * num2;
		return vector;
	}
}
