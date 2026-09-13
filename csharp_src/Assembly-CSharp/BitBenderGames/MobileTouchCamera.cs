using System;
using System.Collections.Generic;
using BitBenderGames.CameraState;
using GameFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

namespace BitBenderGames;

[RequireComponent(typeof(Camera))]
public class MobileTouchCamera : MonoBehaviour
{
	public enum State
	{
		Idle,
		FreeLook,
		MoveTo,
		Focus,
		QuitFocus,
		Follow,
		SyncWithTimeline,
		Lock
	}

	[Serializable]
	public class ZoomParam
	{
		public float posY;

		public float offsetZ;

		public float sensitivity;
	}

	public delegate void OnForceChangePosition();

	private List<CameraStateBase> _stateList;

	private State _currentState;

	private CameraStateBase _cameraState;

	public TouchInputController touchInput;

	private Camera camera;

	private UniversalAdditionalCameraData cameraData;

	private Plane refPlaneXZ = new Plane(new Vector3(0f, 1f, 0f), 0f);

	private bool showHorizonError = true;

	private float maxHorizonFallbackDistance = 10000f;

	private bool _use45XYCamera;

	private bool _use45XCamera;

	private int _hideCameraRefCount;

	[SerializeField]
	[Tooltip("This value defines how quickly the camera comes to a halt when auto scrolling.")]
	private float dampFactorTimeMultiplier = 2f;

	[SerializeField]
	[Tooltip("When swiping over the screen the camera will keep scrolling a while before coming to a halt. This variable limits the maximum velocity of the auto scroll.")]
	private float autoScrollVelocityMax = 60f;

	[SerializeField]
	[Tooltip("When dragging quickly, the camera will keep autoscrolling in the last direction. The autoscrolling will slowly come to a halt. This value defines how fast the camera will come to a halt.")]
	private float autoScrollDamp = 300f;

	[SerializeField]
	private List<float> autoScrollDamps = new List<float>();

	[SerializeField]
	[Tooltip("This curve allows to modulate the auto scroll damp value over time.")]
	private AnimationCurve autoScrollDampCurve = new AnimationCurve(new Keyframe(0f, 1f, 0f, 0f), new Keyframe(0.7f, 0.9f, -0.5f, -0.5f), new Keyframe(1f, 0.01f, -0.85f, -0.85f));

	[SerializeField]
	[Tooltip("The lower the value, the slower the camera will follow. The higher the value, the more direct the camera will follow movement updates. Necessary for keeping the camera smooth when the framerate is not in sync with the touch input update rate.")]
	private float camFollowFactor = 15f;

	[SerializeField]
	private float camZoomInit;

	[SerializeField]
	private float camZoomCityInit;

	[SerializeField]
	private float camZoomWorldInit;

	[SerializeField]
	private float camZoomMin = 10f;

	[SerializeField]
	private float camZoomMinCity = 50f;

	[SerializeField]
	private float camZoomMinWorld = 20f;

	[SerializeField]
	private float camZoomMax = 160f;

	[SerializeField]
	private float camZoomMaxCity = 50f;

	[SerializeField]
	private float camZoomMaxWorld = 220f;

	[SerializeField]
	private float camZoomFarmPlant;

	[SerializeField]
	private float camZoomFarmPlantRotation = 60f;

	[SerializeField]
	private float camZoomBuild;

	[SerializeField]
	private float camZoomFocusRotation;

	[SerializeField]
	private float camZoomEarthOrder;

	[SerializeField]
	private float camZoomDome;

	[SerializeField]
	private float camZoomMoveCity;

	[SerializeField]
	private float camZoomFormation;

	[SerializeField]
	private float camZoomFocusEarthOrderRotation;

	[SerializeField]
	private float camZoomFocusMoveCityRotation;

	[SerializeField]
	private float camZoomFocusFormationRotation;

	[SerializeField]
	private float camZoomInitCityRotation;

	[SerializeField]
	private float camZoomInitWorldRotation;

	[SerializeField]
	private float camOverzoomMargin = 1f;

	[SerializeField]
	private List<ZoomParam> zoomParams;

	[SerializeField]
	private AnimationCurve cameraFocusCurve;

	[SerializeField]
	private AnimationCurve cameraFocusEarthCurve;

	[SerializeField]
	private AnimationCurve cameraFocusDomeCurve;

	[SerializeField]
	private AnimationCurve cameraFocusMoveCityCurve;

	private bool isCamZoomChangeBlock;

	private Vector2 cameraWorldRotateXRange = new Vector2(37f, 54.5f);

	private Vector2 newCameraWorldRotateXRange;

	private bool cameraRotRangeOverride;

	private Vector2 newCameraWorldZoomRange;

	private bool cameraZoomRangeOverride;

	private bool canMoveing = true;

	private Action beforeUpdate;

	private Action afterUpdate;

	public State CurrentState => _currentState;

	public CameraStateBase CurrentCameraState => _cameraState;

	public Plane RefPlane => refPlaneXZ;

	public int LodLevel { get; set; }

	public bool use45XYCamera
	{
		get
		{
			return _use45XYCamera;
		}
		set
		{
			_use45XYCamera = value;
			if (_use45XYCamera)
			{
				base.transform.rotation = Quaternion.Euler(45f, -45f, 0f);
			}
		}
	}

	public bool use45XCamera
	{
		get
		{
			return _use45XCamera;
		}
		set
		{
			_use45XCamera = value;
			if (_use45XCamera)
			{
				base.transform.rotation = Quaternion.Euler(45f, 0f, 0f);
			}
		}
	}

	public float CamZoomInit
	{
		get
		{
			if (SceneManager.CurrSceneID == 2)
			{
				camZoomInit = camZoomWorldInit;
			}
			else
			{
				camZoomInit = camZoomCityInit;
			}
			return camZoomInit;
		}
		set
		{
			camZoomInit = value;
		}
	}

	public float CamZoomMin
	{
		get
		{
			if (cameraZoomRangeOverride)
			{
				return Mathf.Max(camZoomMin, newCameraWorldZoomRange.x);
			}
			return camZoomMin;
		}
		set
		{
			camZoomMin = value;
		}
	}

	public float CamZoomMax
	{
		get
		{
			if (cameraZoomRangeOverride)
			{
				return Mathf.Min(camZoomMax, newCameraWorldZoomRange.y);
			}
			return camZoomMax;
		}
		set
		{
			camZoomMax = value;
		}
	}

	public float CamZoomMaxCity => camZoomMaxCity;

	public float CamZoomMinCity => camZoomMinCity;

	public float CamZoomMaxWorld => camZoomMaxWorld;

	public float CamZoomMinWorld => camZoomMinWorld;

	public float CamOverZoomMargin => camOverzoomMargin;

	public float CamZoom
	{
		get
		{
			return base.transform.position.y;
		}
		set
		{
			if (!isCamZoomChangeBlock && CalcZoom(value, base.transform.position, out var outPos, out var outRot))
			{
				base.transform.position = outPos;
				base.transform.rotation = outRot;
			}
		}
	}

	private Vector2 CameraWorldRotateXRange
	{
		get
		{
			if (cameraRotRangeOverride)
			{
				return newCameraWorldRotateXRange;
			}
			return cameraWorldRotateXRange;
		}
	}

	public bool CanMoveing
	{
		get
		{
			return canMoveing;
		}
		set
		{
			_ = canMoveing;
			canMoveing = value;
		}
	}

	public float DampFactorTimeMultiplier => dampFactorTimeMultiplier;

	public float AutoScrollVelocityMax => autoScrollVelocityMax;

	public float AutoScrollDamp => autoScrollDamp;

	public List<float> AutoScrollDamps => autoScrollDamps;

	public AnimationCurve AutoScrollDampCurve => autoScrollDampCurve;

	public float CamFollowFactor => camFollowFactor;

	public float CamZoomFarmPlant
	{
		get
		{
			return camZoomFarmPlant;
		}
		set
		{
			camZoomFarmPlant = value;
		}
	}

	public float CamZoomFarmPlantRotation
	{
		get
		{
			return camZoomFarmPlantRotation;
		}
		set
		{
			camZoomFarmPlantRotation = value;
		}
	}

	public float CamZoomBuild
	{
		get
		{
			return camZoomBuild;
		}
		set
		{
			camZoomBuild = value;
		}
	}

	public float CamZoomFocusRotation
	{
		get
		{
			return camZoomFocusRotation;
		}
		set
		{
			camZoomFocusRotation = value;
		}
	}

	public float CamZoomFormation
	{
		get
		{
			return camZoomFormation;
		}
		set
		{
			camZoomFormation = value;
		}
	}

	public float CamZoomFocusFormationRotation
	{
		get
		{
			return camZoomFocusFormationRotation;
		}
		set
		{
			camZoomFocusFormationRotation = value;
		}
	}

	public float CamZoomEarthOrder => camZoomEarthOrder;

	public float CamZoomDome => camZoomDome;

	public float CamZoomMoveCity => camZoomMoveCity;

	public float CamZoomFocusEarthOrderRotation => camZoomFocusEarthOrderRotation;

	public float CamZoomFocusMoveCityRotation => camZoomFocusMoveCityRotation;

	public float CamZoomInitRotation
	{
		get
		{
			if (SceneManager.CurrSceneID == 2)
			{
				return camZoomInitWorldRotation;
			}
			return camZoomInitCityRotation;
		}
	}

	public AnimationCurve CameraFocusCurve => cameraFocusCurve;

	public AnimationCurve CameraFocusEarthCurve => cameraFocusEarthCurve;

	public AnimationCurve CameraFocusDomeCurve => cameraFocusDomeCurve;

	public AnimationCurve CameraFocusMoveCityCurve => cameraFocusMoveCityCurve;

	public Action BeforeUpdate
	{
		set
		{
			beforeUpdate = value;
		}
	}

	public Action AfterUpdate
	{
		set
		{
			afterUpdate = value;
		}
	}

	public event OnForceChangePosition OnForceChangePositionEvent;

	public void HideCamera()
	{
		_hideCameraRefCount++;
		if (_hideCameraRefCount == 1 && (bool)cameraData)
		{
			cameraData.disableRender = true;
			GameEntry.Event.Fire(EventId.OnSceneCameraDisableRender, true);
		}
	}

	public void ShowCamera()
	{
		if (_hideCameraRefCount > 0)
		{
			_hideCameraRefCount--;
			if (_hideCameraRefCount == 0 && (bool)cameraData)
			{
				cameraData.disableRender = false;
				GameEntry.Event.Fire(EventId.OnSceneCameraDisableRender, false);
			}
		}
	}

	public void SetRotRangeOverride(bool overrideValue, Vector2 range)
	{
		cameraRotRangeOverride = overrideValue;
		newCameraWorldRotateXRange = range;
	}

	public void SetZoomRangeOverride(bool overrideValue, Vector2 zoomRange)
	{
		cameraZoomRangeOverride = overrideValue;
		newCameraWorldZoomRange = zoomRange;
		if (overrideValue)
		{
			CamZoom = Mathf.Clamp(CamZoom, CamZoomMin, CamZoomMax);
		}
	}

	private float GetLerpRotateX(float zoomValue)
	{
		if (zoomValue >= camZoomInit)
		{
			float f = (zoomValue - camZoomInit) / (camZoomMax - camZoomInit);
			f = Mathf.Pow(f, 0.28f);
			return Mathf.Lerp(camZoomInitWorldRotation, CameraWorldRotateXRange.y, f);
		}
		if (zoomValue < camZoomInit)
		{
			float f2 = (camZoomInit - zoomValue) / (camZoomInit - camZoomMin);
			f2 = Mathf.Pow(f2, 1.5f);
			return Mathf.Lerp(camZoomInitWorldRotation, CameraWorldRotateXRange.x, f2);
		}
		return camZoomInitWorldRotation;
	}

	public bool CalcZoom(float cameraY, Vector3 pos, out Vector3 outPos, out Quaternion outRot)
	{
		Vector3 cameraTargetPos = GetCameraTargetPos();
		GetZoomInterval(cameraY, out var beg, out var end);
		if (beg >= 0 && end >= 0 && beg < zoomParams.Count && end < zoomParams.Count)
		{
			_ = CameraWorldRotateXRange;
			float t = (cameraY - zoomParams[beg].posY) / (zoomParams[end].posY - zoomParams[beg].posY);
			if (use45XCamera)
			{
				float num = Mathf.Lerp(zoomParams[beg].posY, zoomParams[end].posY, t);
				float lerpRotateX = GetLerpRotateX(cameraY);
				lerpRotateX = Mathf.Clamp(lerpRotateX, cameraWorldRotateXRange.x, cameraWorldRotateXRange.y);
				outRot = Quaternion.Euler(lerpRotateX, 0f, 0f);
				Vector3 vector = outRot * Vector3.forward;
				outPos = cameraTargetPos - vector * (num / Mathf.Sin(lerpRotateX * (MathF.PI / 180f)));
			}
			else if (use45XYCamera)
			{
				float num2 = Mathf.Lerp(zoomParams[beg].posY, zoomParams[end].posY, t);
				float num3 = num2 - pos.y;
				float num4 = num3 * 0.7071f;
				float num5 = 0f - num3 * 0.7071f;
				outPos = new Vector3(pos.x + num4, num2, pos.z + num5);
				outRot = Quaternion.LookRotation(cameraTargetPos - outPos);
			}
			else
			{
				Vector3 a = new Vector3(pos.x, zoomParams[beg].posY, cameraTargetPos.z - zoomParams[beg].offsetZ);
				Vector3 b = new Vector3(pos.x, zoomParams[end].posY, cameraTargetPos.z - zoomParams[end].offsetZ);
				outPos = Vector3.Lerp(a, b, t);
				outRot = Quaternion.LookRotation(cameraTargetPos - outPos);
			}
			return true;
		}
		outPos = Vector3.zero;
		outRot = Quaternion.identity;
		return false;
	}

	public void SetIsCamZoomChangeBlock(bool value)
	{
		isCamZoomChangeBlock = value;
	}

	public bool GetIsCamZoomChangeBlock()
	{
		return isCamZoomChangeBlock;
	}

	public List<ZoomParam> GetZoomParams()
	{
		return new List<ZoomParam>(zoomParams);
	}

	public void SetZoomParams(List<ZoomParam> zoomParams)
	{
		this.zoomParams = zoomParams;
	}

	public void SetZoomParams(int level, float y, float offsetZ, float sensitivity)
	{
		if (level > 0 && level < zoomParams.Count)
		{
			zoomParams[level].posY = y;
			zoomParams[level].offsetZ = offsetZ;
			zoomParams[level].sensitivity = sensitivity;
			CamZoom = base.transform.position.y;
		}
	}

	public void Awake()
	{
		camera = GetComponent<Camera>();
		cameraData = GetComponent<UniversalAdditionalCameraData>();
		CamZoom = CamZoomInit;
		SetRotRangeOverride(overrideValue: false, Vector2.zero);
		touchInput = new TouchInputController();
		_stateList = new List<CameraStateBase>();
		_stateList.Add(new CameraStateIdle(this));
		_stateList.Add(new CameraStateFreeLook(this));
		_stateList.Add(new CameraStateMoveTo(this));
		_stateList.Add(new CameraStateFocus(this));
		_stateList.Add(new CameraStateQuitFocus(this));
		_stateList.Add(new CameraStateFollow(this));
		_stateList.Add(new CameraStateSyncWithTimeline(this));
		_stateList.Add(new CameraStateLock(this));
		_currentState = State.Idle;
		_cameraState = GetState(_currentState);
		_cameraState.OnEnter();
	}

	public void ResetCamera()
	{
		touchInput = new TouchInputController();
		_currentState = State.Idle;
		_cameraState = GetState(_currentState);
		_cameraState.OnEnter();
	}

	public void OnDestroy()
	{
		SetState(State.Idle, false);
		beforeUpdate = null;
		afterUpdate = null;
	}

	public void Update()
	{
		if (touchInput.enabled)
		{
			touchInput.OnUpdate();
		}
		beforeUpdate?.Invoke();
		_cameraState.OnUpdate();
		afterUpdate?.Invoke();
	}

	public void Follow(GameObject go, float time, float offset = 0f)
	{
		CameraStateFollow cameraStateFollow = GetState(State.Follow) as CameraStateFollow;
		GameObject leaderObj = cameraStateFollow._leaderObj;
		cameraStateFollow._leaderObj = go;
		cameraStateFollow.Offset = new Vector3(0f, 0f, offset);
		cameraStateFollow._moveToLeaderTime = ((leaderObj != null && go != null && leaderObj == go) ? (-1f) : time);
		if (_currentState == State.Follow)
		{
			if (go == null)
			{
				SetState(State.Idle, false);
			}
			else if (leaderObj != go)
			{
				cameraStateFollow.Reset();
			}
		}
		else if (go != null)
		{
			SetState(State.Follow, false);
		}
	}

	public void BeginSyncWithTimeline(Camera camInTimeline, float transitionTime)
	{
		if (camInTimeline == null)
		{
			Log.Error("SyncWithTimeline camInTimeline is null");
			return;
		}
		SetState(State.SyncWithTimeline, true, camInTimeline, transitionTime);
	}

	public void EndSyncWithTimeline()
	{
		SetState(State.Idle, false);
	}

	public void LockCamera(Vector3 pos, float duration)
	{
		SetState(State.Lock, true, pos, duration);
	}

	public void FreeCamera()
	{
		SetState(State.Idle, false);
	}

	public void AutoLookat(Vector3 target, float zoom, float time, Action onCompete)
	{
		Vector3 cameraTargetPos = GetCameraTargetPos();
		if (target == cameraTargetPos && Mathf.Approximately(zoom, CamZoom))
		{
			onCompete?.Invoke();
			return;
		}
		target = AdjustTarget(target);
		CameraStateMoveTo obj = GetState(State.MoveTo) as CameraStateMoveTo;
		obj._target = target;
		obj._zoom = ((zoom < 0f) ? CamZoom : zoom);
		obj._time = time;
		obj._onComplete = onCompete;
		obj._moveType = CameraStateMoveTo.MoveType.MoveAndZoom;
		SetState(State.MoveTo, false);
	}

	public void AutoZoom(float zoom, float time, Action onComplete)
	{
		if (Mathf.Approximately(zoom, CamZoom))
		{
			onComplete?.Invoke();
			return;
		}
		CameraStateMoveTo cameraStateMoveTo = GetState(State.MoveTo) as CameraStateMoveTo;
		cameraStateMoveTo._target = Vector3.zero;
		cameraStateMoveTo._zoom = ((zoom < 0f) ? CamZoom : zoom);
		cameraStateMoveTo._time = time;
		cameraStateMoveTo._onComplete = onComplete;
		cameraStateMoveTo._moveType = CameraStateMoveTo.MoveType.ZoomOnly;
		if (_currentState == State.MoveTo)
		{
			cameraStateMoveTo.TransToSelf();
		}
		else
		{
			SetState(State.MoveTo, false);
		}
	}

	public void AutoFocus(Vector3 target, float zoom, float time, float rotation, bool focusToCenter, bool lockView, AnimationCurve curve, Action onCompete)
	{
		Vector3 cameraTargetPos = GetCameraTargetPos();
		if (target == cameraTargetPos && Mathf.Approximately(zoom, CamZoom))
		{
			onCompete?.Invoke();
			return;
		}
		SetState(State.Idle, false);
		target = AdjustTarget(target);
		CameraStateFocus obj = GetState(State.Focus) as CameraStateFocus;
		obj._target = target;
		obj._zoom = ((zoom < 0f) ? CamZoom : zoom);
		obj._time = time;
		obj._lerpCurve = curve;
		obj._rotation = rotation;
		obj._onComplete = onCompete;
		obj._focusToCenter = focusToCenter;
		obj._lockView = lockView;
		SetState(State.Focus, false);
	}

	public void QuitFocus(float time)
	{
		if (_currentState == State.Focus)
		{
			CameraStateFocus cameraStateFocus = GetState(State.Focus) as CameraStateFocus;
			CameraStateQuitFocus cameraStateQuitFocus = GetState(State.QuitFocus) as CameraStateQuitFocus;
			if (cameraStateFocus._lockView && !cameraStateFocus._focusToCenter)
			{
				cameraStateQuitFocus._target = cameraStateFocus._target;
			}
			else
			{
				cameraStateQuitFocus._target = GetCameraTargetPos();
			}
			cameraStateQuitFocus._focusToCenter = cameraStateFocus._focusToCenter;
			cameraStateQuitFocus._time = time;
			SetState(State.QuitFocus, false);
		}
	}

	public void StopMove()
	{
		if (_currentState == State.Follow || _currentState == State.FreeLook || _currentState == State.MoveTo)
		{
			SetState(State.Idle, false);
		}
	}

	public void LookAt(Vector3 target)
	{
		Vector3 cameraTargetPos = GetCameraTargetPos();
		target = AdjustTarget(target);
		Vector3 vector = target - cameraTargetPos;
		SetCameraPos(base.transform.position + vector);
		this.OnForceChangePositionEvent?.Invoke();
	}

	public CameraStateBase GetState(State state)
	{
		return _stateList[(int)state];
	}

	public void SetState(State newState, bool canReEnterCurrState = false, params object[] enterParams)
	{
		if (_currentState != newState || canReEnterCurrState)
		{
			_currentState = newState;
			CameraStateBase cameraState = _cameraState;
			_cameraState = GetState(_currentState);
			cameraState.OnLeave();
			_cameraState.OnEnter(enterParams);
			_cameraState.OnUpdate();
		}
	}

	public bool InputDonwOnUI(Vector3 pos)
	{
		if (TouchWrapper.TouchCount > 0)
		{
			foreach (WrappedTouch touch in TouchWrapper.Touches)
			{
				if (EventSystem.current.IsPointerOverGameObject(touch.FingerId))
				{
					return true;
				}
			}
		}
		return false;
	}

	public Vector3 GetIntersectionPoint(Ray ray)
	{
		float enter = 0f;
		if (!RefPlane.Raycast(ray, out enter) || enter > maxHorizonFallbackDistance)
		{
			if (showHorizonError)
			{
				Debug.LogError("Failed to compute intersection between camera ray and reference plane. Make sure the camera Axes are set up correctly.");
				showHorizonError = false;
			}
			return UnprojectVector2(ProjectVector3(ray.origin)) + UnprojectVector2(ProjectVector3(ray.direction)).normalized * maxHorizonFallbackDistance;
		}
		return ray.origin + ray.direction * enter;
	}

	public bool RaycastGround(Ray ray, out Vector3 hitPoint)
	{
		hitPoint = Vector3.zero;
		float enter = 0f;
		bool num = RefPlane.Raycast(ray, out enter);
		if (num)
		{
			hitPoint = ray.GetPoint(enter);
		}
		return num;
	}

	public Vector3 UnprojectVector2(Vector2 v2, float offset = 0f)
	{
		return new Vector3(v2.x, offset, v2.y);
	}

	public Vector2 ProjectVector3(Vector3 v3)
	{
		return new Vector2(v3.x, v3.z);
	}

	public Vector3 GetCameraTargetPos()
	{
		return GetIntersectionPoint(GetCamCenterRay());
	}

	public Vector3 GetCameraPos()
	{
		return base.transform.position;
	}

	public void SetCameraPos(Vector3 pos)
	{
		base.transform.position = pos;
	}

	public Quaternion GetRotation()
	{
		return base.transform.rotation;
	}

	public void SetRotation(Quaternion rot)
	{
		base.transform.rotation = rot;
	}

	public void SetFov(float fov)
	{
		camera.fieldOfView = fov;
	}

	public float GetFov()
	{
		return camera.fieldOfView;
	}

	private Ray GetCamCenterRay()
	{
		return new Ray(base.transform.position, base.transform.forward);
	}

	public Ray ScreenPointToRay(Vector3 pos)
	{
		Vector3 position = base.transform.position;
		base.transform.position = Vector3.zero;
		Ray ray = camera.ScreenPointToRay(pos);
		base.transform.position = position;
		return new Ray(position, ray.direction);
	}

	public bool GetTouchTerrainPos(float x, float y, out Vector3 pos)
	{
		Ray ray = ScreenPointToRay(new Vector3(x, y, 0f));
		float enter = 0f;
		if (RefPlane.Raycast(ray, out enter))
		{
			pos = ray.origin + ray.direction * enter;
			return true;
		}
		pos = Vector3.zero;
		return false;
	}

	public Vector3 WorldToScreenPoint(Vector3 position)
	{
		return camera.WorldToScreenPoint(position);
	}

	private void GetZoomInterval(float cameraY, out int beg, out int end)
	{
		beg = -1;
		end = -1;
		for (int i = 0; i < zoomParams.Count; i++)
		{
			if (cameraY < zoomParams[i].posY)
			{
				beg = i - 1;
				end = i;
				break;
			}
		}
	}

	public float GetZoomSensitivity()
	{
		GetZoomInterval(base.transform.position.y, out var beg, out var end);
		if (beg >= 0 && end >= 0 && beg < zoomParams.Count && end < zoomParams.Count)
		{
			float t = (base.transform.position.y - zoomParams[beg].posY) / (zoomParams[end].posY - zoomParams[beg].posY);
			return Mathf.Lerp(zoomParams[beg].sensitivity, zoomParams[end].sensitivity, t);
		}
		return 1f;
	}

	public Vector3 AdjustTarget(Vector3 target)
	{
		if (Mathf.Abs(target.y) > Mathf.Epsilon)
		{
			target = GetIntersectionPoint(new Ray(target, (target.y >= 0f) ? base.transform.forward : (-1f * base.transform.forward)));
		}
		return target;
	}

	private void OnDrawGizmos()
	{
	}
}
