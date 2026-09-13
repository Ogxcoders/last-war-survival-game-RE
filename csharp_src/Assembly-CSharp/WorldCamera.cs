using System;
using System.Collections.Generic;
using BitBenderGames;
using BitBenderGames.CameraState;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using XLua;

public class WorldCamera : WorldManagerBase
{
	public const float MoveTime = 0.3f;

	public const float MainBuildingUpgradeZoom = 54.96f;

	public const float ShowBaseFireCameraZoom = 62.07f;

	public const float PointMoveDetal = 0.1f;

	public static LookAtFocusState[] NeedLoadFocusCurve = new LookAtFocusState[1] { LookAtFocusState.BuildRoad };

	private MobileTouchCamera touchCamera;

	private TouchInputController touchInput;

	private Camera camera;

	private long trackMarchId;

	private float prevLodDist = -1f;

	private Vector3 _lastTarget;

	private Vector3 _focusTarget;

	public static readonly float MAX_POS_Y = 4000f;

	public static readonly int MAX_PADDING_Y = 100;

	private static Vector2 LodScaleKey0 = new Vector2(0f, 1f);

	private static Vector2 LodScaleKey1 = new Vector2(MAX_POS_Y, MAX_POS_Y * 0.1f);

	private static int[] lodArray;

	private static int[] lodArrayNineNation = new int[9] { 0, 150, 250, 400, 600, 1200, 2200, 5000, 9000 };

	public Vector3[] cameraAnchor = new Vector3[4];

	public const int LeftBottom = 0;

	public const int LeftTop = 1;

	public const int RightTop = 2;

	public const int RightBottom = 3;

	private Dictionary<LookAtFocusState, FocusCurveStruct> _saveCurve;

	private bool waitToTrackTroop;

	private ITimer delayToTrackTimer;

	private const int DELAY_TO_TRACK_COUNT = 20;

	private int curDelayToTrackTroopCount;

	private RenderTexture m_FrameBuffer;

	private CommandBuffer m_CommandBuffer;

	private Resolution deviceResolution;

	public Camera __camera => camera;

	public long TrackMarchId => trackMarchId;

	public static int[] LodArray
	{
		get
		{
			SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
			if (curSkinMeta != null && curSkinMeta.IsNineNationMode())
			{
				return lodArrayNineNation;
			}
			if (lodArray == null)
			{
				LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetLodArray");
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

	public Vector2 ViewSize { get; private set; }

	public float InitZoom => touchCamera.CamZoomInit;

	public float ZoomMin
	{
		get
		{
			return touchCamera.CamZoomMin;
		}
		set
		{
			if (touchCamera != null)
			{
				touchCamera.CamZoomMin = value;
			}
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
			if (touchCamera != null)
			{
				touchCamera.CamZoomMax = value;
			}
		}
	}

	public Vector3 CurTarget => GetCameraTargetPos();

	public Vector2Int CurTilePos => world.WorldToTile(GetCameraTargetPos());

	public Vector2Int CurTilePosClamped => world.ClampTilePos(CurTilePos);

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

	public int CurrentLodLevel { get; private set; } = 1;

	public int frameBufferWidth
	{
		get
		{
			if (!(m_FrameBuffer == null))
			{
				return m_FrameBuffer.width;
			}
			return Screen.width;
		}
	}

	public int frameBufferHeight
	{
		get
		{
			if (!(m_FrameBuffer == null))
			{
				return m_FrameBuffer.height;
			}
			return Screen.height;
		}
	}

	private float resolutionScale => (float)frameBufferHeight / (float)Screen.height;

	public event Action AfterUpdate;

	public bool InCamera(float x, float z)
	{
		if (x >= cameraAnchor[0].x && z >= cameraAnchor[0].z && x <= cameraAnchor[2].x)
		{
			return z <= cameraAnchor[2].z;
		}
		return false;
	}

	public bool InCamera(float x, float z, float distance)
	{
		if (x >= cameraAnchor[0].x - distance && z >= cameraAnchor[0].z - distance && x <= cameraAnchor[2].x + distance)
		{
			return z <= cameraAnchor[2].z + distance;
		}
		return false;
	}

	public void SetRotRangeOverride(bool overrideRange, Vector2 range)
	{
		touchCamera?.SetRotRangeOverride(overrideRange, range);
	}

	public void SetLodRange(bool overrideRange, int minLod, int maxLod)
	{
		if (!overrideRange)
		{
			touchCamera?.SetZoomRangeOverride(overrideValue: false, Vector2.zero);
			return;
		}
		int[] array = LodArray;
		if (array != null && minLod >= 0 && minLod < array.Length && maxLod >= 0 && maxLod < array.Length && maxLod > minLod)
		{
			float x = ((minLod <= 0) ? array[0] : array[minLod - 1]);
			float y = array[maxLod] - 1;
			touchCamera?.SetZoomRangeOverride(overrideValue: true, new Vector2(x, y));
		}
		else
		{
			touchCamera?.SetZoomRangeOverride(overrideValue: false, Vector2.zero);
		}
	}

	public bool IsInMoveToState()
	{
		if (touchCamera != null)
		{
			return touchCamera.CurrentState == MobileTouchCamera.State.MoveTo;
		}
		return false;
	}

	public bool IsInFreeLookStateWithAtLeastSpeed(float speed)
	{
		if (touchCamera != null && touchCamera.CurrentState == MobileTouchCamera.State.FreeLook)
		{
			return ((CameraStateFreeLook)touchCamera.CurrentCameraState).sqrCameraSpeed >= speed * speed;
		}
		return false;
	}

	public WorldCamera(WorldScene scene)
		: base(scene)
	{
		camera = Camera.main;
		_saveCurve = new Dictionary<LookAtFocusState, FocusCurveStruct>();
	}

	public override void Init()
	{
		touchCamera = camera.GetComponent<MobileTouchCamera>();
		touchCamera.ResetCamera();
		touchInput = touchCamera.touchInput;
		SetTouchInputControllerEnable(able: false);
		touchCamera.AfterUpdate = OnAfterCameraUpdate;
		touchCamera.BeforeUpdate = OnBeforeCameraUpdate;
		touchCamera.CamZoomMax = touchCamera.CamZoomMaxWorld;
		touchCamera.CamZoom = touchCamera.CamZoomInit;
		touchCamera.CamZoomMin = touchCamera.CamZoomMinWorld;
		touchCamera.SetRotRangeOverride(overrideValue: false, Vector2.zero);
		touchCamera.SetZoomRangeOverride(overrideValue: false, Vector2.zero);
		CurrentLodLevel = GetLodLevel();
		touchCamera.LodLevel = CurrentLodLevel;
		touchCamera.use45XCamera = true;
		touchCamera.GetComponent<UniversalAdditionalCameraData>().renderShadows = false;
		SetFOV(10f);
		deviceResolution = Screen.currentResolution;
		LoadCurve();
		CameraUtil.CustomTransparencyAxis(camera);
	}

	public override void UnInit()
	{
		touchCamera.use45XCamera = false;
		touchCamera.AfterUpdate = null;
		touchCamera.BeforeUpdate = null;
		touchCamera.OnDestroy();
		this.AfterUpdate = null;
		DestroyCurve();
		ResetDelayToTrackTroop();
		base.UnInit();
	}

	public void InitCameraInfoOnSceneLoaded()
	{
		if (touchCamera != null)
		{
			touchCamera.use45XYCamera = false;
			touchCamera.SetRotation(Quaternion.Euler(45f, 0f, 0f));
		}
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

	public float GetLodDistance(int lod)
	{
		int[] array = LodArray;
		if (array == null || array.Length == 0 || lod < 0 || lod >= array.Length)
		{
			return InitZoom;
		}
		return array[lod];
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

	public MobileTouchCamera.State GetCurrentCameraState()
	{
		return touchCamera.CurrentState;
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
		touchCamera.StopMove();
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
		RefreshCameraAnchor();
	}

	public void RefreshCameraAnchor()
	{
		Ray ray = camera.ScreenPointToRay(new Vector3(0f, 0f, 0f));
		Ray ray2 = camera.ScreenPointToRay(new Vector3(camera.pixelWidth, 0f, 0f));
		Ray ray3 = camera.ScreenPointToRay(new Vector3(camera.pixelWidth, camera.pixelHeight, 0f));
		Ray ray4 = camera.ScreenPointToRay(new Vector3(0f, camera.pixelHeight, 0f));
		Plane refPlane = touchCamera.RefPlane;
		if (refPlane.Raycast(ray, out var enter))
		{
			cameraAnchor[0] = ray.GetPoint(enter);
		}
		if (refPlane.Raycast(ray2, out enter))
		{
			cameraAnchor[3] = ray2.GetPoint(enter);
		}
		if (refPlane.Raycast(ray3, out enter))
		{
			cameraAnchor[2] = ray3.GetPoint(enter);
		}
		if (refPlane.Raycast(ray4, out enter))
		{
			cameraAnchor[1] = ray4.GetPoint(enter);
		}
		cameraAnchor[0].x = cameraAnchor[1].x;
		cameraAnchor[3].x = cameraAnchor[2].x;
		ViewSize = new Vector2(cameraAnchor[3].x - cameraAnchor[0].x, cameraAnchor[1].z - cameraAnchor[0].z);
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
		return new Vector3(vector3.x / resolutionScale, vector3.y / resolutionScale, 0f);
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
		return camera.ScreenPointToRay(ConvertScreenPoint(pos));
	}

	public void TrackMarch(long marchId)
	{
		WorldMarch march = world.GetMarch(trackMarchId);
		if (march != null)
		{
			march.isCameraFollow = false;
		}
		ResetDelayToTrackTroop();
		trackMarchId = 0L;
		WorldMarch march2 = world.GetMarch(marchId);
		if (march2 != null)
		{
			trackMarchId = march2.uuid;
			march2.isCameraFollow = true;
		}
		if (marchId > 0)
		{
			WorldTroop troop = SceneManager.World.GetTroop(marchId);
			if (troop != null && (bool)troop.GetCameraFollowTransform())
			{
				touchCamera.Follow(troop.GetCameraFollowTransform().gameObject, 0.4f, troop.GetCameraFollowOffset());
			}
			else if (march2 != null)
			{
				touchCamera.AutoLookat(march2.position, -1f, 0.2f, null);
				DoDelayToTrackTroop();
			}
			else
			{
				touchCamera.Follow(null, 0f);
			}
		}
		else
		{
			touchCamera.Follow(null, 0f);
		}
	}

	public void TrackHSR(long marchId, GameObject go)
	{
		if (marchId > 0 && !(go == null))
		{
			WorldMarch march = world.GetMarch(trackMarchId);
			if (march != null)
			{
				march.isCameraFollow = false;
			}
			ResetDelayToTrackTroop();
			trackMarchId = marchId;
			touchCamera.Follow(go, 0.4f);
		}
	}

	private void ResetDelayToTrackTroop()
	{
		if (delayToTrackTimer != null)
		{
			GameEntry.Timer.CancelTimer(delayToTrackTimer);
			delayToTrackTimer = null;
		}
		waitToTrackTroop = false;
		curDelayToTrackTroopCount = 0;
	}

	private void DoDelayToTrackTroop()
	{
		if (delayToTrackTimer != null)
		{
			GameEntry.Timer.CancelTimer(delayToTrackTimer);
			delayToTrackTimer = null;
		}
		if (curDelayToTrackTroopCount >= 20)
		{
			waitToTrackTroop = false;
			return;
		}
		waitToTrackTroop = true;
		curDelayToTrackTroopCount++;
		delayToTrackTimer = GameEntry.Timer.RegisterTimer(0.1f, OnDelayToTrackTroop);
	}

	private void OnDelayToTrackTroop()
	{
		if (trackMarchId != 0L)
		{
			WorldTroop troop = SceneManager.World.GetTroop(trackMarchId);
			if (troop != null && troop.GetCameraFollowTransform() != null)
			{
				touchCamera.Follow(troop.GetCameraFollowTransform().gameObject, 0.4f);
				waitToTrackTroop = false;
				delayToTrackTimer = null;
			}
			else
			{
				DoDelayToTrackTroop();
			}
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
			int num = (CurrentLodLevel = GetLodLevel());
			touchCamera.LodLevel = num;
			GameEntry.Event.Fire(EventId.ChangeCameraLod, num);
		}
		prevLodDist = GetLodDistance();
	}

	public void ClampToEdge()
	{
		if (touchCamera.CurrentState != MobileTouchCamera.State.Focus && touchCamera.CurrentState != MobileTouchCamera.State.MoveTo)
		{
			int num = SceneManager.World.CurTileCountXMin;
			int num2 = SceneManager.World.CurTileCountXMax;
			int num3 = SceneManager.World.CurTileCountYMin;
			int num4 = SceneManager.World.CurTileCountYMax;
			SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
			if (curSkinMeta != null && curSkinMeta.IsNineNationMode() && !GameEntry.Data.Player.IsInBattleField())
			{
				num = 0;
				num2 = 3000;
				num3 = 0;
				num4 = 3000;
			}
			Vector2 vector = new Vector2(CurTarget.x / 2f, CurTarget.z / 2f);
			int num5 = (int)(camera.transform.position.y / MAX_POS_Y * (float)MAX_PADDING_Y);
			int num6 = (int)((float)num5 / (float)Screen.height * (float)Screen.width);
			if (num5 <= 6)
			{
				num5 = 0;
				num6 = 0;
			}
			else
			{
				num5 -= 6;
				num6 -= 6;
			}
			bool flag = false;
			if (vector.x < (float)(num + num6))
			{
				vector.x = num + num6;
				flag = true;
			}
			if (vector.x >= (float)(num2 - num6))
			{
				vector.x = (float)(num2 - num6) - 0.01f;
				flag = true;
			}
			if (vector.y < (float)(num3 + num5))
			{
				vector.y = num3 + num5;
				flag = true;
			}
			if (vector.y >= (float)(num4 - num5))
			{
				vector.y = (float)(num4 - num5) - 0.01f;
				flag = true;
			}
			if (flag)
			{
				touchCamera.LookAt(new Vector3(vector.x * 2f, 0f, vector.y * 2f));
			}
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

	public float GetMapLabelScale()
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
		Gizmos.DrawLine(cameraAnchor[0], cameraAnchor[1]);
		Gizmos.DrawLine(cameraAnchor[1], cameraAnchor[2]);
		Gizmos.DrawLine(cameraAnchor[2], cameraAnchor[3]);
		Gizmos.DrawLine(cameraAnchor[3], cameraAnchor[0]);
	}

	public void SetTouchInputControllerEnable(bool able)
	{
		if (touchInput != null)
		{
			touchInput.enabled = able;
		}
	}

	public bool GetTouchInputControllerEnable()
	{
		return touchInput.enabled;
	}

	private Vector3 ConvertScreenPoint(Vector3 screenPos)
	{
		return screenPos * resolutionScale;
	}

	private void AddCommand()
	{
		RemoveCommand();
		m_CommandBuffer = new CommandBuffer();
		m_CommandBuffer.name = "blit to Back buffer";
		m_CommandBuffer.SetRenderTarget(-1);
		m_CommandBuffer.Blit(m_FrameBuffer, BuiltinRenderTextureType.CurrentActive);
		camera.AddCommandBuffer(CameraEvent.AfterEverything, m_CommandBuffer);
	}

	private void RemoveCommand()
	{
		if (m_CommandBuffer != null)
		{
			camera.RemoveCommandBuffer(CameraEvent.AfterEverything, m_CommandBuffer);
			m_CommandBuffer = null;
		}
	}

	private void ClearFrameBuffer()
	{
		if (m_FrameBuffer != null)
		{
			m_FrameBuffer.Release();
			UnityEngine.Object.DestroyImmediate(m_FrameBuffer);
			m_FrameBuffer = null;
		}
	}

	private void UpdateFrameBuffer(int width, int height, int depth)
	{
		ClearFrameBuffer();
		RenderTextureFormat format = RenderTextureFormat.Default;
		m_FrameBuffer = RenderTexture.GetTemporary(width, height, depth, format);
		m_FrameBuffer.name = "cameraTargetBuffer";
		m_FrameBuffer.hideFlags = HideFlags.DontSave;
		m_FrameBuffer.useMipMap = false;
		m_FrameBuffer.Create();
	}

	public void SetResolution(int height)
	{
		float num = (float)deviceResolution.width / (float)deviceResolution.height;
		Screen.SetResolution(Mathf.RoundToInt((float)height * num), height, fullscreen: true);
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

	public void SetFOV(float fov)
	{
		camera.fieldOfView = fov;
		camera.transform.Find("HudCamera").GetComponent<Camera>().fieldOfView = fov;
	}
}
