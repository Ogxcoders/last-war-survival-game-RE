using System;
using System.Collections;
using UnityEngine;

public class BasementTower : CityBuilding, ITouchObjectBeginDragHandler, ITouchObject, ITouchObjectDragHandler, ITouchObjectEndDragHandler, ITouchObjectPointerDownHandler, ITouchObjectEndLongTabHandler
{
	private AnimationState animState;

	private GameObject projectile;

	private long defMarchIUuid;

	private InstanceRequest normalAttackInst;

	private Coroutine _corTowerRoate;

	private bool isUninit;

	private const float EdgeRateX = 0.08f;

	private const float EdgeRateY = 0.1f;

	private WorldTroop _troop;

	private InstanceRequest _dragTroopLineInst;

	private WorldTroopLine _dragTroopLine;

	private InstanceRequest _troopDestinationInst;

	private WorldTroopDestinationSignal _troopDestination;

	private static GameObject AttackRangeEffect;

	private static InstanceRequest AttackRangeEffectReq;

	private static bool AttackRangeEffectVisible;

	private Action scanToTarget;

	public Vector2Int guideCityTilePos;

	public new WorldPreviewType PreviewType => WorldPreviewType.Default;

	protected internal override void CSInit(object userData)
	{
		base.CSInit(userData);
		isUninit = false;
	}

	protected internal override void CSUninit()
	{
		base.CSUninit();
		isUninit = true;
		DestroyDragLine();
		DestroyTroopDestination();
		if (AttackRangeEffectReq != null)
		{
			AttackRangeEffectReq.Destroy();
			AttackRangeEffectReq = null;
		}
	}

	public void ReigsterCallBack(Action scanToTarget)
	{
		this.scanToTarget = scanToTarget;
	}

	public void ScanPosRot()
	{
	}

	public void SetFireState(bool state)
	{
	}

	public void SetAnimEnableState(bool state)
	{
		animState.speed = (state ? 1 : 0);
	}

	public void SetAnimState(bool state)
	{
		if (!state)
		{
			SetAnimEnableState(state: true);
			SetFireState(state: false);
		}
	}

	protected internal override void CSUpdate(float elapseSeconds)
	{
		base.CSUpdate(elapseSeconds);
		ScanPosRot();
	}

	public override void OnBattleAtkUpdate(long targetUuid)
	{
		if (targetUuid != 0L)
		{
			defMarchIUuid = targetUuid;
			WorldTroop troop = SceneManager.World.GetTroop(defMarchIUuid);
			if (_corTowerRoate != null)
			{
				StopCoroutine(_corTowerRoate);
			}
			_corTowerRoate = StartCoroutine(RotateToTarget(troop));
			if (troop != null)
			{
				_ = projectile != null;
			}
		}
	}

	public void ShowAttack()
	{
		InstanceRequest requestInst = GameEntry.Resource.InstantiateAsync("Assets/_Art/Effect/prefab/scene/VFX_putonggongji.prefab");
		normalAttackInst = requestInst;
		requestInst.completed += delegate
		{
			Transform transform = base.transform.Find("ModelGo/Normal/A_build_pt/A_build@pt_skin/To_unity/Root/main/up");
			if (transform != null)
			{
				requestInst.gameObject.transform.SetParent(transform.transform);
			}
			else
			{
				requestInst.gameObject.transform.SetParent(base.transform);
			}
			requestInst.gameObject.transform.localPosition = Vector3.zero;
			requestInst.gameObject.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
			YieldUtils.DelayActionWithOutContext(delegate
			{
				if (requestInst != null)
				{
					requestInst.gameObject.Destroy();
				}
			}, 1f);
		};
	}

	private IEnumerator RotateToTarget(WorldTroop targetTroop)
	{
		Transform tower = base.transform.Find("ModelGo/Normal/A_build_pt/A_build@pt_skin/To_unity/Root/main/up");
		Vector3 position = targetTroop.GetPosition();
		Vector3 position2 = tower.transform.position;
		Quaternion rotateAngle = Quaternion.LookRotation(position - position2, Vector3.up);
		while (tower.transform.rotation != rotateAngle)
		{
			tower.transform.rotation = Quaternion.Lerp(tower.transform.rotation, rotateAngle, 0.5f);
			yield return null;
		}
		ShowAttack();
	}

	private float GetDistanceToTarget(WorldTroop selfInfo, Vector3 targetPos)
	{
		Vector3 position = selfInfo.GetPosition();
		Vector2Int a = SceneManager.World.WorldToTile(position);
		Vector2Int b = SceneManager.World.WorldToTile(targetPos);
		return SceneManager.World.TileDistance(a, b);
	}

	private InstanceRequest CreateTroopDestination()
	{
		if (_troopDestinationInst == null)
		{
			_troopDestinationInst = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/March/TroopDestinationSignal.prefab");
			_troopDestinationInst.completed += delegate
			{
				_troopDestinationInst.gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
				_troopDestination = _troopDestinationInst.gameObject.GetComponent<WorldTroopDestinationSignal>();
			};
		}
		return _troopDestinationInst;
	}

	private void DestroyTroopDestination()
	{
		if (_troopDestinationInst != null)
		{
			_troopDestinationInst.Destroy();
			_troopDestinationInst = null;
			_troopDestination = null;
		}
	}

	private InstanceRequest CreateDragLine()
	{
		if (_dragTroopLineInst == null)
		{
			_dragTroopLineInst = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/March/TroopLineDrag.prefab");
			_dragTroopLineInst.completed += delegate
			{
				_dragTroopLineInst.gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
				_dragTroopLine = _dragTroopLineInst.gameObject.GetComponent<WorldTroopLine>();
			};
		}
		return _dragTroopLineInst;
	}

	private void DestroyDragLine()
	{
		if (_dragTroopLineInst != null)
		{
			_dragTroopLineInst.Destroy();
			_dragTroopLineInst = null;
			_dragTroopLineInst = null;
		}
	}

	private void EdgeDragUpdate(Vector3 dragPosCurrent)
	{
		float num = dragPosCurrent.x / (float)Screen.width;
		float num2 = dragPosCurrent.y / (float)Screen.height;
		if (num < 0.08f || num > 0.92f || num2 < 0.1f || num2 > 0.9f)
		{
			Vector3 touchPoint = SceneManager.World.GetTouchPoint(dragPosCurrent);
			Vector3 curTarget = SceneManager.World.CurTarget;
			Vector3 vector = touchPoint - curTarget;
			float magnitude = vector.magnitude;
			if (magnitude > 0.1f)
			{
				Vector3 vector2 = vector / magnitude;
				float num3 = 1f * Time.deltaTime * SceneManager.World.GetLodDistance();
				Vector3 lookWorldPosition = curTarget + vector2 * num3;
				SceneManager.World.Lookat(lookWorldPosition);
			}
		}
	}

	private static void ShowBatteryEffect(int pointIndex)
	{
		AttackRangeEffectVisible = true;
		if (AttackRangeEffectReq == null)
		{
			AttackRangeEffectReq = GameEntry.Resource.InstantiateAsync("Assets/_Art/Effect/prefab/scene/Build/V_paota_fanwei.prefab");
			AttackRangeEffectReq.completed += delegate
			{
				AttackRangeEffect = AttackRangeEffectReq.gameObject;
				if (AttackRangeEffectVisible)
				{
					AttackRangeEffect.SetActive(value: true);
					AttackRangeEffect.transform.SetParent(SceneManager.World.BuildBubbleNode);
					AttackRangeEffect.name = "TurretAttackRangeEffect";
					AttackRangeEffect.transform.position = SceneManager.World.TileIndexToWorld(pointIndex);
				}
				else
				{
					AttackRangeEffect.SetActive(value: false);
				}
			};
		}
		else if (AttackRangeEffect != null)
		{
			AttackRangeEffect.SetActive(value: true);
			AttackRangeEffect.transform.position = SceneManager.World.TileIndexToWorld(pointIndex);
		}
	}

	private static void HideBatteryEffect()
	{
		AttackRangeEffectVisible = false;
		if (AttackRangeEffect != null)
		{
			AttackRangeEffect.SetActive(value: false);
		}
	}

	bool ITouchObjectPointerDownHandler.OnPointerDown()
	{
		BuildPointInfo buildInfo = GetBuildInfo();
		if (buildInfo != null && buildInfo.pointType == WorldPointType.PlayerBuilding && buildInfo.ownerUid == GameEntry.Data.Player.Uid)
		{
			BuildPointInfo buildPointInfo = buildInfo;
			if (buildPointInfo != null && buildPointInfo.itemId == 418000)
			{
				LuaBuildData buildingDataByUuid = GameEntry.Data.Building.GetBuildingDataByUuid(buildPointInfo.uuid);
				if (buildingDataByUuid != null && buildingDataByUuid.IsActive())
				{
					ShowBatteryEffect(buildInfo.pointIndex);
				}
			}
		}
		return true;
	}

	bool ITouchObjectBeginDragHandler.OnBeginDrag(Vector3 dragStartPos)
	{
		return true;
	}

	bool ITouchObjectDragHandler.OnDrag(Vector3 dragStartPos, Vector3 dragCurrPos)
	{
		if (isUninit)
		{
			return true;
		}
		if (!IsSelf())
		{
			return true;
		}
		long raycastHitMarch = SceneManager.World.GetRaycastHitMarch(dragCurrPos);
		SceneManager.World.CanMoving = false;
		EdgeDragUpdate(dragCurrPos);
		CreateDragLine();
		if (_dragTroopLine != null)
		{
			_dragTroopLine.SetDragPath(base.transform.position, SceneManager.World.GetTouchPoint());
		}
		Vector3 realPos = SceneManager.World.GetTouchPoint();
		int tileSize = 1;
		int num = SceneManager.World.WorldToTileIndex(SceneManager.World.GetTouchPoint());
		MarchTargetType targetType = SceneManager.World.GetTargetType(raycastHitMarch, num);
		EnumDestinationSignalType destinationType = SceneManager.World.GetDestinationType(0L, raycastHitMarch, num, targetType, isFormation: false, ref realPos, ref tileSize);
		CreateTroopDestination();
		if (_troopDestination != null)
		{
			_troopDestination.SetDestination(realPos, destinationType, targetType, tileSize, 0f, isTower: true);
		}
		return true;
	}

	bool ITouchObjectEndDragHandler.OnEndDrag(Vector3 dragStopPos)
	{
		if (isUninit)
		{
			return true;
		}
		long num = 0L;
		long raycastHitMarch = SceneManager.World.GetRaycastHitMarch(dragStopPos);
		if (raycastHitMarch > 0)
		{
			WorldMarch march = SceneManager.World.GetMarch(raycastHitMarch);
			if (march != null && march.ownerUid != GameEntry.Data.Player.Uid && !march.GetIsBroken() && !march.IsMonsterOrOrdinaryBoss())
			{
				num = raycastHitMarch;
			}
		}
		DestroyDragLine();
		if (num != 0L)
		{
			UserTowerChangeListenMessage.Instance.Send(new UserTowerChangeListenMessage.Request
			{
				towerUuid = base.Uuid,
				targetUuid = num
			});
		}
		if (_troopDestination != null)
		{
			_troopDestination.SetDestinationOver();
		}
		HideBatteryEffect();
		return true;
	}

	bool ITouchObjectEndLongTabHandler.OnEndLongTap()
	{
		OnEndLongTap();
		HideBatteryEffect();
		return true;
	}
}
