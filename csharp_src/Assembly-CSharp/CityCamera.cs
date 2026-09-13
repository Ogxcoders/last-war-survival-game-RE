using System;
using System.Collections.Generic;
using BitBenderGames;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using XLua;

public class CityCamera : CityManagerBase
{
	public const float PointMoveDetal = 0.1f;

	private MobileTouchCamera touchCamera;

	private TouchInputController touchInput;

	public Camera camera;

	private long trackMarchId;

	private float prevLodDist = -1f;

	private Vector3 _lastTarget;

	private Vector3 _focusTarget;

	private static Vector2 LodScaleKey0 = new Vector2(0f, 1f);

	private static Vector2 LodScaleKey1 = new Vector2(220f, 22f);

	public static LookAtFocusState[] NeedLoadFocusCurve = new LookAtFocusState[1] { LookAtFocusState.BuildRoad };

	private Dictionary<LookAtFocusState, FocusCurveStruct> _saveCurve;

	private static int[] lodArray;

	private Vector3[] frustumPoints = new Vector3[4];

	public static int[] LodArray
	{
		get
		{
			if (lodArray == null)
			{
				LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetCityLodArray");
				List<int> list = new List<int>();
				for (int i = 0; i <= luaTable.Length; i++)
				{
					list.Add(luaTable.Get<int>(i));
				}
				lodArray = list.ToArray();
			}
			return lodArray;
		}
	}

	public float InitZoom
	{
		get
		{
			return touchCamera.CamZoomInit;
		}
		set
		{
			touchCamera.CamZoomInit = value;
		}
	}

	public float ZoomMin
	{
		get
		{
			return touchCamera.CamZoomMin;
		}
		set
		{
			touchCamera.CamZoomMin = value;
		}
	}

	public float ZoomMax
	{
		get
		{
			return touchCamera.CamZoomMax;
		}
		set
		{
			touchCamera.CamZoomMax = value;
		}
	}

	public Vector3 CurTarget => GetCameraTargetPos();

	public Vector2Int CurTilePos => scene.WorldToTile(GetCameraTargetPos());

	public Vector2Int CurTilePosClamped => scene.ClampTilePos(CurTilePos);

	public float Zoom
	{
		get
		{
			return touchCamera.CamZoom;
		}
		set
		{
			touchCamera.CamZoom = value;
		}
	}

	public bool AutoMove
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	public bool IsFocus
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public bool CanMoving
	{
		get
		{
			if (touchCamera != null)
			{
				return touchCamera.CanMoveing;
			}
			return false;
		}
		set
		{
			if (touchCamera != null)
			{
				touchCamera.CanMoveing = value;
			}
		}
	}

	public bool Enabled
	{
		get
		{
			return camera.gameObject.activeSelf;
		}
		set
		{
			camera.gameObject.SetActive(value);
		}
	}

	public TouchInputController TouchInputController => touchInput;

	public event Action AfterUpdate;

	public void SetRotRangeOverride(bool overrideRange, Vector2 range)
	{
		touchCamera?.SetRotRangeOverride(overrideRange, range);
	}

	public CityCamera(CityScene scene)
		: base(scene)
	{
		camera = Camera.main;
		_saveCurve = new Dictionary<LookAtFocusState, FocusCurveStruct>();
	}

	public override void Init()
	{
		touchCamera = camera.GetComponent<MobileTouchCamera>();
		touchCamera.ResetCamera();
		SetTouchInputControllerEnable(able: false);
		touchCamera.use45XYCamera = true;
		SetFOV(10f);
		touchInput = touchCamera.touchInput;
		touchCamera.AfterUpdate = OnAfterCameraUpdate;
		touchCamera.BeforeUpdate = OnBeforeCameraUpdate;
		touchCamera.CamZoomMax = touchCamera.CamZoomMaxCity;
		touchCamera.CamZoomMin = touchCamera.CamZoomMinCity;
		touchCamera.CamZoom = touchCamera.CamZoomInit;
		touchCamera.SetRotRangeOverride(overrideValue: false, Vector2.zero);
		touchCamera.SetZoomRangeOverride(overrideValue: false, Vector2.zero);
		touchCamera.LodLevel = 1;
		touchCamera.GetComponent<UniversalAdditionalCameraData>().renderShadows = false;
		LoadCurve();
		CameraUtil.DefaultTransparencyAxis(camera);
	}

	public override void UnInit()
	{
		touchCamera.use45XYCamera = false;
		touchCamera.SetRotation(Quaternion.Euler(45f, 0f, 0f));
		touchCamera.AfterUpdate = null;
		touchCamera.BeforeUpdate = null;
		touchCamera.OnDestroy();
		this.AfterUpdate = null;
		DestroyCurve();
		base.UnInit();
	}

	public void UnInit_withoutMove()
	{
		touchCamera.AfterUpdate = null;
		touchCamera.BeforeUpdate = null;
		touchCamera.OnDestroy();
		this.AfterUpdate = null;
		DestroyCurve();
		base.UnInit();
	}

	public void UnInit_withMove()
	{
		touchCamera.use45XYCamera = false;
		touchCamera.SetRotation(Quaternion.Euler(45f, 0f, 0f));
	}

	public void AutoLookat(Vector3 lookat, float zoom = -1f, float time = 0.2f, Action onComplete = null)
	{
		touchCamera.AutoLookat(lookat, zoom, time, onComplete);
	}

	public void AutoZoom(float zoom, float time = 0.2f, Action onComplete = null)
	{
		touchCamera.AutoZoom(zoom, time, onComplete);
	}

	public void AutoFocus(Vector3 lookat, LookAtFocusState state, float time, bool focusToCenter, bool lockView, Action onComplete = null)
	{
		float zoom = -1f;
		float rotation = 0f;
		AnimationCurve curve = null;
		if (_saveCurve.ContainsKey(state))
		{
			if (_saveCurve[state].focusCurve != null)
			{
				zoom = _saveCurve[state].focusCurve.camZoom;
				rotation = _saveCurve[state].focusCurve.camZoomFocusRotation;
				curve = _saveCurve[state].focusCurve.enterCurve;
			}
			else
			{
				zoom = touchCamera.CamZoomBuild;
				rotation = touchCamera.CamZoomFocusRotation;
				curve = touchCamera.CameraFocusCurve;
			}
		}
		else
		{
			switch (state)
			{
			case LookAtFocusState.FarmPlant:
				zoom = touchCamera.CamZoomFarmPlant;
				rotation = touchCamera.CamZoomFarmPlantRotation;
				curve = touchCamera.CameraFocusCurve;
				break;
			case LookAtFocusState.PlaceBuild:
				zoom = touchCamera.CamZoomBuild;
				rotation = touchCamera.CamZoomFocusRotation;
				curve = touchCamera.CameraFocusCurve;
				break;
			case LookAtFocusState.EarthOrder:
				zoom = touchCamera.CamZoomEarthOrder;
				rotation = touchCamera.CamZoomFocusEarthOrderRotation;
				curve = touchCamera.CameraFocusEarthCurve;
				break;
			case LookAtFocusState.Dome:
				zoom = touchCamera.CamZoomDome;
				rotation = touchCamera.CamZoomInitRotation;
				curve = touchCamera.CameraFocusDomeCurve;
				break;
			case LookAtFocusState.MoveCity:
				zoom = touchCamera.CamZoomMoveCity;
				rotation = touchCamera.CamZoomFocusMoveCityRotation;
				curve = touchCamera.CameraFocusMoveCityCurve;
				break;
			case LookAtFocusState.Formation:
				zoom = touchCamera.CamZoomFormation;
				rotation = touchCamera.CamZoomFocusFormationRotation;
				curve = touchCamera.CameraFocusCurve;
				break;
			}
		}
		touchCamera.AutoFocus(lookat, zoom, time, rotation, focusToCenter, lockView, curve, onComplete);
	}

	public void QuitFocus(float time)
	{
		touchCamera.QuitFocus(time);
	}

	public void StopMove()
	{
		touchCamera?.StopMove();
	}

	private Vector3 GetCameraTargetPos()
	{
		return touchCamera.GetCameraTargetPos();
	}

	public void Lookat(Vector3 lookWorldPosition)
	{
		touchCamera.LookAt(lookWorldPosition);
	}

	public override void OnUpdate(float deltaTime)
	{
		_ = Enabled;
	}

	public Vector3 GetRaycastGroundPoint(Vector3 screenPos)
	{
		Vector3 hitPoint = Vector3.zero;
		if (camera != null && touchCamera != null)
		{
			touchCamera.RaycastGround(ScreenPointToRay(screenPos), out hitPoint);
		}
		return hitPoint;
	}

	public Vector3 WorldToViewportPoint(Vector3 position)
	{
		return camera.WorldToViewportPoint(position);
	}

	public Vector3 WorldToScreenPoint(Vector3 worldPos)
	{
		Vector3 forward = camera.transform.forward;
		Vector3 vector = worldPos - camera.transform.position;
		float num = Vector3.Dot(forward, vector);
		if (num <= 0f)
		{
			Vector3 vector2 = forward * num * 1.01f;
			worldPos = camera.transform.position + (vector - vector2);
		}
		Vector2 vector3 = RectTransformUtility.WorldToScreenPoint(camera, worldPos);
		return new Vector3(vector3.x, vector3.y, 0f);
	}

	public Vector3 ScreenPointToWorld(Vector3 worldPos, float disPlane = 0f)
	{
		Vector3 result = Vector3.zero;
		if (touchCamera != null)
		{
			Ray ray = ScreenPointToRay(worldPos);
			if (touchCamera.RefPlane.Raycast(ray, out var enter) && enter > disPlane)
			{
				result = ray.GetPoint(enter - disPlane);
			}
		}
		return result;
	}

	public Ray ScreenPointToRay(Vector3 pos)
	{
		return camera.ScreenPointToRay(pos);
	}

	public void TrackMarch(long marchId)
	{
		CityTroop cityTroop = SceneManager.World.GetCityTroop();
		if (cityTroop != null && marchId != 0L)
		{
			touchCamera.Follow(cityTroop.GetTransform().gameObject, 0.4f);
		}
		else
		{
			touchCamera.Follow(null, 0f);
		}
	}

	public void BeginSyncWithTimeline(Camera camInTimeline, float transitionTime)
	{
		touchCamera.BeginSyncWithTimeline(camInTimeline, transitionTime);
	}

	public void EndSyncWithTimeline()
	{
		touchCamera.EndSyncWithTimeline();
	}

	public void LockCamera(Vector3 pos, float duration)
	{
		touchCamera.LockCamera(pos, duration);
	}

	public void FreeCamera()
	{
		touchCamera.FreeCamera();
	}

	private void OnBeforeCameraUpdate()
	{
	}

	private void OnAfterCameraUpdate()
	{
		this.AfterUpdate?.Invoke();
		if ((CurTarget - _lastTarget).sqrMagnitude > 0.1f)
		{
			_lastTarget = CurTarget;
			GameEntry.Event.Fire(EventId.WORLD_CAMERA_CHANGE_POINT);
		}
		if (IsLodChanged())
		{
			int lodLevel = GetLodLevel();
			touchCamera.LodLevel = lodLevel;
			GameEntry.Event.Fire(EventId.ChangeCameraLod, lodLevel);
		}
		prevLodDist = GetLodDistance();
	}

	public void ClampToEdge()
	{
		Vector2 tilePos = scene.WorldToTileFloat(CurTarget);
		bool flag = false;
		if (tilePos.x < (float)scene.CurTileCountXMin)
		{
			tilePos.x = scene.CurTileCountXMin;
			flag = true;
		}
		if (tilePos.x >= (float)scene.CurTileCountXMax)
		{
			tilePos.x = (float)scene.CurTileCountXMax - 0.01f;
			flag = true;
		}
		if (tilePos.y < (float)scene.CurTileCountYMin)
		{
			tilePos.y = scene.CurTileCountYMin;
			flag = true;
		}
		if (tilePos.y >= (float)scene.CurTileCountYMax)
		{
			tilePos.y = (float)scene.CurTileCountYMax - 0.01f;
			flag = true;
		}
		if (flag)
		{
			touchCamera.LookAt(scene.TileFloatToWorld(tilePos));
		}
	}

	public Quaternion GetRotation()
	{
		return camera.transform.rotation;
	}

	public Vector3 GetPosition()
	{
		return camera.transform.position;
	}

	public float GetMapIconScale()
	{
		float t = (camera.transform.position.y - LodScaleKey0.x) / (LodScaleKey1.x - LodScaleKey0.x);
		return Mathf.Lerp(LodScaleKey0.y, LodScaleKey1.y, t);
	}

	public void EnablePostProcess()
	{
		camera.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = true;
	}

	public void DisablePostProcess()
	{
		camera.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = false;
	}

	public void OnDrawGizmos()
	{
		Vector3 curTarget = CurTarget;
		Gizmos.color = new Color(0f, 0f, 0.8f, 1f);
		Gizmos.DrawSphere(curTarget, 1f);
		Ray ray = camera.ScreenPointToRay(new Vector3(0f, 0f, 0f));
		Ray ray2 = camera.ScreenPointToRay(new Vector3(camera.pixelWidth, 0f, 0f));
		Ray ray3 = camera.ScreenPointToRay(new Vector3(camera.pixelWidth, camera.pixelHeight, 0f));
		Ray ray4 = camera.ScreenPointToRay(new Vector3(0f, camera.pixelHeight, 0f));
		Plane refPlane = touchCamera.RefPlane;
		if (refPlane.Raycast(ray, out var enter))
		{
			frustumPoints[0] = ray.GetPoint(enter);
		}
		if (refPlane.Raycast(ray2, out enter))
		{
			frustumPoints[1] = ray2.GetPoint(enter);
		}
		if (refPlane.Raycast(ray3, out enter))
		{
			frustumPoints[2] = ray3.GetPoint(enter);
		}
		if (refPlane.Raycast(ray4, out enter))
		{
			frustumPoints[3] = ray4.GetPoint(enter);
		}
		Gizmos.DrawLine(frustumPoints[0], frustumPoints[1]);
		Gizmos.DrawLine(frustumPoints[1], frustumPoints[2]);
		Gizmos.DrawLine(frustumPoints[2], frustumPoints[3]);
		Gizmos.DrawLine(frustumPoints[3], frustumPoints[0]);
	}

	public void SetTouchInputControllerEnable(bool able)
	{
		if (touchInput != null)
		{
			touchInput.enabled = able;
		}
	}

	private void LoadCurve()
	{
		for (int i = 0; i < NeedLoadFocusCurve.Length; i++)
		{
			FocusCurveStruct curve = new FocusCurveStruct();
			curve.instanceRequest = GameEntry.Resource.InstantiateAsync($"Assets/Main/Prefabs/CityScene/FocusCurve{(int)NeedLoadFocusCurve[i]}.prefab");
			curve.instanceRequest.completed += delegate
			{
				GameObject gameObject = curve.instanceRequest.gameObject;
				if (gameObject != null)
				{
					gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
					curve.focusCurve = gameObject.GetComponent<FocusCurve>();
				}
			};
			_saveCurve.Add(NeedLoadFocusCurve[i], curve);
		}
	}

	private void DestroyCurve()
	{
		foreach (FocusCurveStruct value in _saveCurve.Values)
		{
			if (value != null && value.instanceRequest != null)
			{
				value.instanceRequest.Destroy();
			}
		}
		_saveCurve = null;
	}

	public void SetZoomParams(int level, float y, float offsetZ, float sensitivity)
	{
		touchCamera.SetZoomParams(level, y, offsetZ, sensitivity);
	}

	public List<MobileTouchCamera.ZoomParam> GetZoomParams()
	{
		return touchCamera.GetZoomParams();
	}

	public void SetFOV(float fov)
	{
		camera.fieldOfView = fov;
		camera.transform.Find("HudCamera").GetComponent<Camera>().fieldOfView = fov;
	}

	public float GetFOV()
	{
		return camera.fieldOfView;
	}

	public int GetLodLevel()
	{
		float lodDistance = GetLodDistance();
		for (int i = 0; i < LodArray.Length; i++)
		{
			if (lodDistance <= (float)LodArray[i])
			{
				return i;
			}
		}
		return LodArray.Length - 1;
	}

	public float GetLodDistance()
	{
		return camera.transform.position.y;
	}

	public float GetPreviousLodDistance()
	{
		return prevLodDist;
	}

	public float GetMinLodDistance()
	{
		return ZoomMin * 0.5f;
	}

	public void MarkLodChanged()
	{
		prevLodDist = -1f;
	}

	private bool IsLodChanged()
	{
		float previousLodDistance = GetPreviousLodDistance();
		float lodDistance = GetLodDistance();
		for (int i = 0; i < LodArray.Length; i++)
		{
			if (((float)LodArray[i] < lodDistance && previousLodDistance <= (float)LodArray[i]) || (lodDistance <= (float)LodArray[i] && previousLodDistance > (float)LodArray[i]))
			{
				return true;
			}
		}
		return false;
	}
}
