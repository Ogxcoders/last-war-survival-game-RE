using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MeteoriteWorldEffectPlayer : MonoBehaviour
{
	public class AbandonData
	{
		public int serverId;

		private int fromPoint;

		private int toPoint;

		public float flyTimeSec;

		public float postEffectTimeSec;

		private float totalTime;

		private WorldScene worldScene;

		private InstanceRequest instanceRequest;

		private MeteoriteWorldAbandonUpdater updater;

		private bool loadFailed;

		private float timer;

		private bool inited;

		private Vector3 fromPosition;

		private Vector3 toPosition;

		public static AbandonData Create(WorldScene scene, int serverId, int fromPoint, int toPoint, float elapsedSec, float flyTimeSec, float postEffectTimeSec)
		{
			return new AbandonData
			{
				worldScene = scene,
				serverId = serverId,
				fromPoint = fromPoint,
				toPoint = toPoint,
				fromPosition = scene.TileIndexToWorld(fromPoint, serverId),
				toPosition = scene.TileIndexToWorld(toPoint, serverId),
				flyTimeSec = flyTimeSec,
				postEffectTimeSec = postEffectTimeSec,
				timer = elapsedSec
			};
		}

		private void Init()
		{
			if (!inited)
			{
				loadFailed = false;
				inited = true;
				totalTime = flyTimeSec + postEffectTimeSec;
			}
		}

		private void Load()
		{
			loadFailed = false;
			string prefabPath = "Assets/Main/Prefabs/Effect/World/Meteorite/WorldMeteoriteAbandonEffect.prefab";
			instanceRequest = GameEntry.Resource.InstantiateAsync(prefabPath);
			instanceRequest.completed += delegate
			{
				if (instanceRequest.gameObject == null)
				{
					loadFailed = true;
				}
				else
				{
					updater = instanceRequest.gameObject.GetComponent<MeteoriteWorldAbandonUpdater>();
					updater.transform.SetParent(SceneManager.World?.DynamicObjNode);
					if (updater == null)
					{
						loadFailed = true;
					}
					else
					{
						updater.Init(fromPosition, toPosition, flyTimeSec, 10f, postEffectTimeSec, timer);
						updater.gameObject.SetActive(value: true);
					}
				}
			};
		}

		public bool Update(int lod, float deltaTime)
		{
			if (loadFailed)
			{
				return true;
			}
			if (!inited)
			{
				Init();
			}
			timer += deltaTime;
			if (timer < totalTime)
			{
				bool flag = true;
				flag = lod > 0 && lod <= 4;
				if (flag && worldScene != null)
				{
					flag = !worldScene.IsOutOfLWAoi(toPoint, serverId);
				}
				if (flag && instanceRequest == null)
				{
					Load();
				}
				else if (!flag && instanceRequest != null)
				{
					DisposePlayer();
				}
				if (updater != null)
				{
					updater.DeltaUpdate(deltaTime);
				}
				return false;
			}
			return true;
		}

		private void DisposePlayer()
		{
			instanceRequest?.Destroy();
			instanceRequest = null;
			updater = null;
		}

		public void Dispose()
		{
			DisposePlayer();
		}
	}

	public class FragmentData
	{
		public enum FragmentType
		{
			Small,
			Middle,
			Daddy
		}

		public FragmentType fragmentType;

		public Vector3 targetPosition;

		public int serverId;

		public int pointIndex;

		public Vector2Int tilePosition;

		public Vector3 scale;

		public float height;

		public float degree;

		public float dropTime;

		public float exploreTime;

		public float crackTime;

		private float totalTime;

		public long startTime;

		public float rotateSpeed;

		public float delayTimeSec;

		public WorldScene worldScene;

		private InstanceRequest instanceRequest;

		private MeteoriteWorldFragmentPlayer player;

		private bool loadFailed;

		private float timer;

		public int maxLod = 4;

		private bool inited;

		public bool IsVisible(int lod)
		{
			bool flag = false;
			if (fragmentType == FragmentType.Daddy)
			{
				flag = true;
			}
			else
			{
				flag = lod > 0 && lod <= maxLod;
				if (flag && worldScene != null)
				{
					flag = !worldScene.IsOutOfLWAoi(pointIndex, serverId);
				}
			}
			return flag;
		}

		private void Init()
		{
			if (!inited)
			{
				loadFailed = false;
				inited = true;
				totalTime = dropTime + Mathf.Max(exploreTime, crackTime);
			}
		}

		private void Load()
		{
			loadFailed = false;
			string empty = string.Empty;
			empty = fragmentType switch
			{
				FragmentType.Daddy => "Assets/Main/Prefabs/World/Meteorite/EffPrefabs/MeteoriteWorldDropDaddy.prefab", 
				FragmentType.Small => "Assets/Main/Prefabs/World/Meteorite/EffPrefabs/MeteoriteWorldFragmentSmall.prefab", 
				FragmentType.Middle => "Assets/Main/Prefabs/World/Meteorite/EffPrefabs/MeteoriteWorldFragmentMiddle.prefab", 
				_ => "Assets/Main/Prefabs/World/Meteorite/EffPrefabs/MeteoriteWorldFragmentSmall.prefab", 
			};
			instanceRequest = GameEntry.Resource.InstantiateAsync(empty);
			instanceRequest.completed += delegate
			{
				if (instanceRequest.gameObject == null)
				{
					loadFailed = true;
				}
				else
				{
					player = instanceRequest.gameObject.GetComponent<MeteoriteWorldFragmentPlayer>();
					if (player == null)
					{
						loadFailed = true;
					}
					else
					{
						player.transform.SetParent(SceneManager.World?.DynamicObjNode);
						player.transform.localPosition = targetPosition;
						player.gameObject.SetActive(value: true);
						player.Init(new MeteoriteWorldFragmentPlayer.DisplaySetting
						{
							target = targetPosition,
							degree = degree,
							height = height,
							dropTimeSec = dropTime,
							exploreTimeSec = exploreTime,
							crackTimeSec = crackTime,
							rotateSpeed = rotateSpeed,
							scale = scale
						}, (float)(GameEntry.Timer.GetServerTime() - startTime) * 0.001f);
					}
				}
			};
		}

		public bool Update(int lod, float deltaTime)
		{
			if (loadFailed)
			{
				return true;
			}
			if (!inited)
			{
				Init();
			}
			delayTimeSec -= deltaTime;
			if (delayTimeSec > 0f)
			{
				return false;
			}
			timer += deltaTime;
			if (timer <= totalTime)
			{
				bool flag = IsVisible(lod);
				if (flag && instanceRequest == null)
				{
					Load();
				}
				else if (!flag && instanceRequest != null)
				{
					DisposePlayer();
				}
				if (player != null)
				{
					player.DeltaUpdate(deltaTime);
				}
				return false;
			}
			return true;
		}

		private void DisposePlayer()
		{
			instanceRequest?.Destroy();
			instanceRequest = null;
			player = null;
		}

		public void Dispose()
		{
			DisposePlayer();
		}
	}

	public class LastKillData
	{
		public Vector3 position;

		public string iconPic;

		public string iconPic2;

		public string uuid;

		public string headPic;

		public int picVer;

		public int score;

		public int score2;

		public WorldScene worldScene;

		private InstanceRequest instanceRequest;

		private MeteoriteWorldLastKillNotice notice;

		private bool loadFailed;

		private void Load()
		{
			loadFailed = false;
			string prefabPath = "Assets/Main/Prefabs/MeteoriteResource/MeteoriteResourceLastKillNotice.prefab";
			instanceRequest = GameEntry.Resource.InstantiateAsync(prefabPath);
			instanceRequest.completed += delegate
			{
				if (instanceRequest.gameObject == null)
				{
					loadFailed = true;
				}
				else
				{
					notice = instanceRequest.gameObject.GetComponent<MeteoriteWorldLastKillNotice>();
					if (notice == null)
					{
						loadFailed = true;
					}
					else
					{
						notice.transform.SetParent(SceneManager.World?.DynamicObjNode);
						notice.transform.localPosition = position;
						notice.gameObject.SetActive(value: true);
						notice.Init(uuid, headPic, picVer, iconPic, score, iconPic2, score2);
					}
				}
			};
		}

		public bool Update(int lod, float deltaTime)
		{
			if (loadFailed)
			{
				return true;
			}
			if (notice == null)
			{
				if (instanceRequest == null)
				{
					Load();
				}
				return false;
			}
			return notice.DeltaUpdate(lod, deltaTime);
		}

		public void Dispose()
		{
			notice?.Dispose();
			notice = null;
			instanceRequest?.Destroy();
			instanceRequest = null;
			uuid = string.Empty;
			headPic = string.Empty;
			picVer = 0;
		}
	}

	private struct MeteoriteBattleInfo
	{
		public int serverId;

		public int pointIndex;

		public Vector3 worldPosition;

		public Vector2Int tilePosition;

		public int highDamageSize;

		public int lowDamageSize;

		public int hightRadius;

		public int lowRadius;

		public int startTime;

		public int endTime;

		public float maxRadius;

		public void Setup(int serverId, int pointIndex, int highDamageRadius, int lowDamageRadius, int startTime, int endTime)
		{
			this.serverId = serverId;
			this.pointIndex = pointIndex;
			hightRadius = highDamageRadius;
			lowRadius = lowDamageRadius;
			highDamageSize = highDamageRadius * 2 + 1;
			lowDamageSize = (lowDamageRadius + highDamageRadius) * 2 + 1;
			this.startTime = startTime;
			this.endTime = endTime;
			maxRadius = (float)lowDamageSize * 2f * 0.5f * 1.414f;
			worldPosition = SceneManager.World.TileIndexToWorld(pointIndex, serverId);
			tilePosition = SceneManager.World.IndexToTilePos(pointIndex);
		}
	}

	private enum State
	{
		None,
		Happy,
		WarningLv_0,
		WarningLv_1,
		WarningLv_2,
		WarningLv_3,
		InBattle,
		BattleEnd
	}

	public abstract class BattleState
	{
		protected MeteoriteWorldEffectPlayer player;

		public void EnterState(MeteoriteWorldEffectPlayer player)
		{
			this.player = player;
			OnEnterState();
		}

		public void LeaveState()
		{
			OnLeaveState();
			player = null;
		}

		protected abstract void OnEnterState();

		public abstract void UpdateState(float deltaTime);

		protected abstract void OnLeaveState();
	}

	public class BattleStateBattleEnd : BattleState
	{
		protected override void OnEnterState()
		{
			UpdateState(0f);
		}

		public override void UpdateState(float deltaTime)
		{
			float num = 3000f;
			long num2 = player.serverTimeMs - (long)player.battleInfo.endTime * 1000L;
			if (num2 > 0)
			{
				float num3 = Mathf.Clamp01((float)num2 / num);
				player.SetDissolve(num3);
				if (num3 >= 1f - Mathf.Epsilon)
				{
					ClearAllEffs();
					player.OnBattleStateChanged(State.BattleEnd, State.None);
				}
			}
		}

		private void ClearAllEffs()
		{
			player.mrHigh.gameObject.TryActive(active: false);
			player.mrLow.gameObject.TryActive(active: false);
			player.mrMapHigh.gameObject.TryActive(active: false);
			player.mrMapLow.gameObject.TryActive(active: false);
			player.mrLow.gameObject.TryActive(active: false);
			player.mrRedRing.gameObject.TryActive(active: false);
			player.mrShadow.gameObject.TryActive(active: false);
			player.highRectangle.gameObject.TryActive(active: false);
			player.lowRectangle.gameObject.TryActive(active: false);
			player.showHudCountdown = false;
			player.UpdateTerrainDiffuseDistance(0f);
			player.SetFragmentDensity(0);
			player.worldScene?.StaticManager?.SetDecorationInvisibleRect(WorldStaticManager.DecorationInvisibleRectType.None, Vector3.zero, 0f);
			player.SetFragmentDensity(0);
			player.highRectangle.SetMaterialFloat(player.KEY_Alpha, 0f);
			player.lowRectangle.SetMaterialFloat(player.KEY_Alpha, 0f);
		}

		protected override void OnLeaveState()
		{
		}
	}

	public class BattleStateInBattle : BattleState
	{
		private WorldStaticManager _worldStaticManager;

		private bool inBattleSettled;

		private bool countdownShowed;

		private float rectAlpha;

		protected override void OnEnterState()
		{
			player.mrHigh.gameObject.TryActive(active: true);
			player.mrLow.gameObject.TryActive(active: true);
			player.mrMapHigh.gameObject.TryActive(active: true);
			player.mrMapLow.gameObject.TryActive(active: true);
			player.mrRedRing.gameObject.TryActive(active: true);
			player.mrShadow.gameObject.TryActive(active: false);
			player.highRectangle.gameObject.TryActive(active: false);
			player.lowRectangle.gameObject.TryActive(active: false);
			player.showHudCountdown = false;
			inBattleSettled = false;
			player.SetFragmentDensity(20);
			_worldStaticManager = player.worldScene?.StaticManager;
			player.highRectangle.SetMaterialFloat(player.KEY_Alpha, 0f);
			player.lowRectangle.SetMaterialFloat(player.KEY_Alpha, 0f);
			player.SetDissolve(0f);
			countdownShowed = false;
		}

		public override void UpdateState(float deltaTime)
		{
			long num = player.serverTimeMs - (long)player.battleInfo.startTime * 1000L;
			if (num <= (long)(player.exploreTimeDaddy * 1000f))
			{
				player.TryRetrieveDaddy();
			}
			if (num <= 10000)
			{
				player.UpdateTerrainDiffuseDistance(player.waveDiffuseRadius);
				_worldStaticManager?.SetDecorationInvisibleRect(WorldStaticManager.DecorationInvisibleRectType.Circle, player.battleInfo.worldPosition, player.waveDiffuseRadius * player.destroyDecorateSpeed);
				player.TryLoadCameraEffects();
				long num2 = 8000L;
				if (num >= num2)
				{
					rectAlpha = Mathf.Lerp(0f, 1f, (float)(num - num2) / (10000f - (float)num2));
					player.highRectangle.SetMaterialFloat(player.KEY_Alpha, rectAlpha);
					player.lowRectangle.SetMaterialFloat(player.KEY_Alpha, rectAlpha);
					player.highRectangle.gameObject.TryActive(active: true);
					player.lowRectangle.gameObject.TryActive(active: true);
				}
			}
			else if (!inBattleSettled)
			{
				player.UpdateTerrainDiffuseDistance(1024f);
				player.mrRedRing.gameObject.TryActive(active: false);
				player.mrShadow.gameObject.TryActive(active: false);
				_worldStaticManager?.SetDecorationInvisibleRect(WorldStaticManager.DecorationInvisibleRectType.Circle, player.battleInfo.worldPosition, (float)player.battleInfo.lowDamageSize * 1.414f);
				inBattleSettled = true;
				player.ClearCameraEffects();
				rectAlpha = 1f;
				player.highRectangle.SetMaterialFloat(player.KEY_Alpha, 1f);
				player.lowRectangle.SetMaterialFloat(player.KEY_Alpha, 1f);
				player.highRectangle.gameObject.TryActive(active: true);
				player.lowRectangle.gameObject.TryActive(active: true);
			}
			if (!countdownShowed)
			{
				long num3 = (long)player.battleInfo.endTime * 1000L;
				num = num3 - player.serverTimeMs;
				if (num <= 30000 && num >= 5000)
				{
					countdownShowed = true;
					GameEntry.Lua.Call("CSharpCallLuaInterface.ShowCountdownTimeUI", num3 + 1000, "", "winter_battlefield_interface_tips1008");
				}
			}
		}

		protected override void OnLeaveState()
		{
			player.UpdateTerrainDiffuseDistance(1024f);
			player.ClearCameraEffects();
			player.highRectangle.SetMaterialFloat(player.KEY_Alpha, 1f);
			player.lowRectangle.SetMaterialFloat(player.KEY_Alpha, 1f);
		}
	}

	public class BattleStateWarning0 : BattleState
	{
		protected override void OnEnterState()
		{
			player.mrHigh.gameObject.TryActive(active: false);
			player.mrLow.gameObject.TryActive(active: false);
			player.mrMapHigh.gameObject.TryActive(active: false);
			player.mrMapLow.gameObject.TryActive(active: false);
			player.mrRedRing.gameObject.TryActive(active: true);
			player.mrShadow.gameObject.TryActive(active: true);
			player.highRectangle.gameObject.TryActive(active: false);
			player.lowRectangle.gameObject.TryActive(active: false);
			player.showHudCountdown = true;
			player.UpdateTerrainDiffuseDistance(0f);
			player.SetDissolve(0f);
			player.SetShadowSize(1f);
		}

		public override void UpdateState(float deltaTime)
		{
		}

		protected override void OnLeaveState()
		{
		}
	}

	public class BattleStateWarning1 : BattleState
	{
		protected override void OnEnterState()
		{
			player.mrHigh.gameObject.TryActive(active: false);
			player.mrLow.gameObject.TryActive(active: false);
			player.mrMapHigh.gameObject.TryActive(active: false);
			player.mrMapLow.gameObject.TryActive(active: false);
			player.mrRedRing.gameObject.TryActive(active: true);
			player.mrShadow.gameObject.TryActive(active: true);
			player.highRectangle.gameObject.TryActive(active: false);
			player.lowRectangle.gameObject.TryActive(active: false);
			player.showHudCountdown = true;
			player.SetFragmentDensity(20);
			player.SetRedRingFadeInOut(2f);
			player.UpdateTerrainDiffuseDistance(0f);
			player.SetDissolve(0f);
			player.SetShadowSize(1f);
		}

		public override void UpdateState(float deltaTime)
		{
		}

		protected override void OnLeaveState()
		{
		}
	}

	public class BattleStateWarning2 : BattleState
	{
		protected override void OnEnterState()
		{
			player.mrHigh.gameObject.TryActive(active: false);
			player.mrLow.gameObject.TryActive(active: false);
			player.mrMapHigh.gameObject.TryActive(active: false);
			player.mrMapLow.gameObject.TryActive(active: false);
			player.mrRedRing.gameObject.TryActive(active: true);
			player.mrShadow.gameObject.TryActive(active: true);
			player.highRectangle.gameObject.TryActive(active: false);
			player.lowRectangle.gameObject.TryActive(active: false);
			player.showHudCountdown = true;
			player.SetFragmentDensity(30);
			player.SetRedRingFadeInOut(1f);
			player.UpdateTerrainDiffuseDistance(0f);
			player.SetDissolve(0f);
			player.SetShadowSize(1f);
		}

		public override void UpdateState(float deltaTime)
		{
		}

		protected override void OnLeaveState()
		{
		}
	}

	public class BattleStateWarning3 : BattleState
	{
		private float shakeTimer;

		private float maxHight = 100f;

		private bool countdownShowed;

		private float shadowMinScale = 0.05f;

		protected override void OnEnterState()
		{
			player.mrHigh.gameObject.TryActive(active: false);
			player.mrLow.gameObject.TryActive(active: false);
			player.mrMapHigh.gameObject.TryActive(active: false);
			player.mrMapLow.gameObject.TryActive(active: false);
			player.mrRedRing.gameObject.TryActive(active: true);
			player.mrShadow.gameObject.TryActive(active: true);
			player.highRectangle.gameObject.TryActive(active: false);
			player.lowRectangle.gameObject.TryActive(active: false);
			player.UpdateTerrainDiffuseDistance(0f);
			countdownShowed = false;
			player.showHudCountdown = true;
			player.SetRedRingFadeInOut(0.5f);
			shakeTimer = 0f;
			player.SetFragmentDensity(30);
			player.SetDissolve(0f);
			player.SetShadowSize(1f);
		}

		public override void UpdateState(float deltaTime)
		{
			shakeTimer -= deltaTime;
			int msToStart = player.MsToStart;
			msToStart = ((msToStart >= 0) ? msToStart : 0);
			if (msToStart > 10000)
			{
				player.SetFragmentDensity(30);
			}
			else
			{
				if (!countdownShowed && msToStart <= 10000)
				{
					countdownShowed = true;
					GameEntry.Lua.Call<long, string, string>("CSharpCallLuaInterface.ShowCountdownTimeUI", player.StartTimeMs, null, "yuntieBattle_tips_1029");
				}
				Mathf.Lerp(shadowMinScale, 1f, (float)msToStart / 10000f);
				player.TryRetrieveDaddy();
				UpdateMeteoriteState(msToStart);
				player.SetFragmentDensity(50);
				player.mrHigh.gameObject.TryActive(active: true);
				player.mrLow.gameObject.TryActive(active: true);
			}
			if (msToStart < 100 && shakeTimer <= 0f)
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.DoCameraShake", 2f, 5f, 25);
				GameEntry.Lua.Call("CSharpCallLuaInterface.DoMobileShake", 1f, 0.2f, 0.5f);
				shakeTimer = float.MaxValue;
			}
		}

		private void UpdateMeteoriteState(float ms)
		{
		}

		protected override void OnLeaveState()
		{
		}
	}

	private List<AbandonData> allDrops = new List<AbandonData>();

	private Dictionary<long, int> dropKeys = new Dictionary<long, int>();

	private List<long> tmpList = new List<long>();

	private List<FragmentData> allFragments = new List<FragmentData>();

	private FragmentData daddyFragment;

	private List<long> preparePlayers = new List<long>();

	private int fragmentMaxLod7;

	private int fragmentMaxLod6;

	private int fragmentMaxLod5;

	private float fragmentGenerateSpeed;

	private float nextFrameGenerateCount;

	private System.Random rand;

	private List<LastKillData> allKillNotice;

	private InstanceRequest cameraEffectRequest;

	private float sceneWhiteClip;

	private float maxWhiteClipDuration;

	private Tonemapping whiteClipTonemapping;

	private long fakeMeteoriteCreateTime;

	private GameObject cameraEffect;

	private float waveDiffuseRadius;

	private float redRingFadeSpd;

	private float redRingAlpha;

	private readonly int KEY_DISSOLVE_PROGRESS = Shader.PropertyToID("_DissolveProgress");

	private readonly int KEY_AlphaBlendVal = Shader.PropertyToID("_AlphaBlendVal");

	private MeteoriteBattleInfo battleInfo;

	private State battleState;

	private BattleState currentState;

	private long serverTimeMs;

	private int serverTimeSec;

	private float refreshTimer;

	private int currentLod = -1;

	private int serverId;

	private Vector3 currentCameraPosition;

	private WorldScene worldScene;

	private WorldCamera worldCamera;

	private StringBuilder sbTimeFormat;

	private bool showHudCountdown;

	private bool isLookAtCenter;

	private const float REFRESH_INTERVAL = 0.5f;

	private readonly int KEY_Thickness = Shader.PropertyToID("_Thickness");

	private readonly int KEY_Alpha = Shader.PropertyToID("_Alpha");

	private readonly int KEY_MaxDistance = Shader.PropertyToID("_MaxDistance");

	private readonly int KEY_CenterX = Shader.PropertyToID("_CenterX");

	private readonly int KEY_CenterY = Shader.PropertyToID("_CenterY");

	private const float MaxDiffuseDistance = 1024f;

	[Range(0f, 1000f)]
	private float editorDiffuseRange = 1f;

	private bool showGizmos;

	[SerializeField]
	private GameObject mapNode;

	[SerializeField]
	private GameObject terrainNode;

	[SerializeField]
	private MeshRenderHelper mrMapHigh;

	[SerializeField]
	private MeshRenderHelper mrMapLow;

	[SerializeField]
	private MeshRenderHelper mrHigh;

	[SerializeField]
	private MeshRenderHelper mrLow;

	[SerializeField]
	private RingMesh mrRedRing;

	[SerializeField]
	private CircleMesh mrShadow;

	[SerializeField]
	private RectangleMesh highRectangle;

	[SerializeField]
	private RectangleMesh lowRectangle;

	[SerializeField]
	private MeteoriteWorldCountdown hudCountdown;

	[SerializeField]
	private AnimationCurve whiteClipCurve;

	[SerializeField]
	private Volume volume;

	[SerializeField]
	private Vector2 thicknessRange;

	[SerializeField]
	private float rectangleLineWidth = 1f;

	[SerializeField]
	private Vector2 rectangleLineWidthRange;

	[SerializeField]
	private Vector2 rectangleLineDistanceRange;

	[SerializeField]
	private float explodeSpeed = 15f;

	[SerializeField]
	private float destroyDecorateSpeed = 1f;

	[SerializeField]
	private float heightDaddy;

	[SerializeField]
	private float degreeDaddy;

	[SerializeField]
	private float dropTimeDaddy;

	[SerializeField]
	private float exploreTimeDaddy;

	[SerializeField]
	private float rorateSpeedDaddy;

	[SerializeField]
	private float scaleDaddy = 1f;

	[SerializeField]
	private Vector2 middleFragmentDegreeRange;

	[SerializeField]
	private Vector2 middleFragmentDropSpeedRange;

	[SerializeField]
	private Vector2 middleFragmentDropScaleRange;

	[SerializeField]
	private float middleFragmentExploreTimeSec;

	[SerializeField]
	private float middleFragmentCrackTimeSec;

	[SerializeField]
	private Vector2 smallFragmentHeightRange;

	[SerializeField]
	private Vector2 smallFragmentDegreeRange;

	[SerializeField]
	private Vector2 smallFragmentDropSpeedRange;

	[SerializeField]
	private Vector2 smallFragmentDropPosOffsetRange;

	[SerializeField]
	private float smallFragmentExploreTimeSec;

	[SerializeField]
	private float smallFragmentCrackTimeSec;

	[SerializeField]
	private float abandonPostEffectSec;

	private static int TIME_WARNING_3 = 60;

	private static int TIME_WARNING_2 = 3600;

	private static int TIME_WARNING_1 = 86400;

	private static int TIME_WARNING_0 = 432000;

	private System.Random Rand
	{
		get
		{
			if (rand == null)
			{
				rand = new System.Random();
			}
			return rand;
		}
	}

	public static MeteoriteWorldEffectPlayer Instance { get; private set; }

	protected bool Inited { get; private set; }

	private int MsToStart
	{
		get
		{
			if (!Inited)
			{
				return int.MaxValue;
			}
			return (int)(battleInfo.startTime * 1000 - serverTimeMs);
		}
	}

	private long StartTimeMs
	{
		get
		{
			if (!Inited)
			{
				return -1L;
			}
			return (long)battleInfo.startTime * 1000L;
		}
	}

	private int DeviceLevel { get; set; } = 1;

	public int CurrentState => (int)battleState;

	public void CreateMeteoriteDropPlayer(int fromPoint, int toPoint, int infoOpenTime)
	{
		if (SceneManager.World != null && Inited && fromPoint > 0 && toPoint > 0)
		{
			long key = ((long)fromPoint << 32) | (uint)toPoint;
			if (!dropKeys.ContainsKey(key))
			{
				dropKeys.Add(key, infoOpenTime);
			}
		}
	}

	private void UpdateMeteoriteDropPlayer(float deltaTime)
	{
		WorldScene worldScene = SceneManager.World as WorldScene;
		if (worldScene == null)
		{
			return;
		}
		if (dropKeys.Count > 0)
		{
			foreach (KeyValuePair<long, int> dropKey in dropKeys)
			{
				long key = dropKey.Key;
				int fromPoint = (int)(key >> 32);
				int toPoint = (int)(key & 0xFFFFFFFFu);
				float num = (float)((long)dropKey.Value * 1000L - serverTimeMs) * 0.001f;
				if (num <= 0f)
				{
					tmpList.Add(key);
					continue;
				}
				float num2 = 1f;
				if (!(num > num2))
				{
					allDrops.Add(AbandonData.Create(worldScene, serverId, fromPoint, toPoint, num2 - num, num2, abandonPostEffectSec));
					tmpList.Add(key);
				}
			}
			if (tmpList.Count > 0)
			{
				int i = 0;
				for (int count = tmpList.Count; i < count; i++)
				{
					dropKeys.Remove(tmpList[i]);
				}
				tmpList.Clear();
			}
		}
		if (allDrops.Count <= 0)
		{
			return;
		}
		int lod = SceneManager.World?.GetLodLevel() ?? (-1);
		for (int num3 = allDrops.Count - 1; num3 >= 0; num3--)
		{
			AbandonData abandonData = allDrops[num3];
			if (abandonData == null)
			{
				allDrops.RemoveAt(num3);
			}
			else if (abandonData.Update(lod, deltaTime))
			{
				abandonData.Dispose();
				allDrops.RemoveAt(num3);
			}
		}
	}

	private void ClearDrops()
	{
		if (allDrops.Count > 0)
		{
			foreach (AbandonData allDrop in allDrops)
			{
				allDrop.Dispose();
			}
			allDrops.Clear();
		}
		dropKeys.Clear();
		tmpList.Clear();
	}

	private void ClearFragments()
	{
		for (int num = allFragments.Count - 1; num >= 0; num--)
		{
			allFragments[num]?.Dispose();
		}
		allFragments.Clear();
		if (daddyFragment != null)
		{
			daddyFragment.Dispose();
			daddyFragment = null;
		}
		fragmentGenerateSpeed = 0f;
		nextFrameGenerateCount = 0f;
		fragmentMaxLod7 = 0;
		fragmentMaxLod6 = 0;
		fragmentMaxLod5 = 0;
		preparePlayers.Clear();
	}

	public void RandomCreateMeteoriteSonPlayer(int pointIndex)
	{
		RandomCreateMeteoriteSonPlayer(pointIndex, 0L);
	}

	private float GetRandomFloat(float min, float max)
	{
		int minValue = (int)(min * 100f);
		int maxValue = (int)(max * 100f);
		return (float)Rand.Next(minValue, maxValue) / 100f;
	}

	private int GetRandomInt(int min, int max)
	{
		return Rand.Next(min, max);
	}

	private void RandomCreateMeteoriteSonPlayer(int pointIndex, long delayMs)
	{
		SceneInterface world = SceneManager.World;
		if (world != null)
		{
			Vector3 position = world.TileIndexToWorld(pointIndex);
			position.x += GetRandomFloat(smallFragmentDropPosOffsetRange.x, smallFragmentDropPosOffsetRange.y);
			position.z += GetRandomFloat(smallFragmentDropPosOffsetRange.x, smallFragmentDropPosOffsetRange.y);
			float randomFloat = GetRandomFloat(smallFragmentHeightRange.x, smallFragmentHeightRange.y);
			float randomFloat2 = GetRandomFloat(smallFragmentDegreeRange.x, smallFragmentDegreeRange.y);
			float randomFloat3 = GetRandomFloat(smallFragmentDropSpeedRange.x, smallFragmentDropSpeedRange.y);
			CreateSmallFragmentPlayer(position, randomFloat, randomFloat2, randomFloat / randomFloat3, Vector3.one, 0f, delayMs);
		}
	}

	private void UpdateFragments(float deltaTime)
	{
		UpdateMiddleMeteoritePreparePlayer();
		UpdateAutoGenerate(deltaTime);
		if (allFragments != null)
		{
			int lod = SceneManager.World?.GetLodLevel() ?? (-1);
			for (int num = allFragments.Count - 1; num >= 0; num--)
			{
				FragmentData fragmentData = allFragments[num];
				if (fragmentData == null)
				{
					allFragments.RemoveAt(num);
				}
				else if (fragmentData.Update(lod, deltaTime))
				{
					switch (fragmentData.maxLod)
					{
					case 7:
						fragmentMaxLod7--;
						fragmentMaxLod7 = ((fragmentMaxLod7 >= 0) ? fragmentMaxLod7 : 0);
						break;
					case 6:
						fragmentMaxLod6--;
						fragmentMaxLod6 = ((fragmentMaxLod6 >= 0) ? fragmentMaxLod6 : 0);
						break;
					case 5:
						fragmentMaxLod5--;
						fragmentMaxLod5 = ((fragmentMaxLod5 >= 0) ? fragmentMaxLod5 : 0);
						break;
					}
					fragmentData.Dispose();
					allFragments.RemoveAt(num);
				}
			}
		}
		if (daddyFragment != null && daddyFragment.Update(currentLod, deltaTime))
		{
			daddyFragment.Dispose();
			daddyFragment = null;
		}
	}

	private void UpdateAutoGenerate(float deltaTime)
	{
		if (fragmentGenerateSpeed <= 0f)
		{
			return;
		}
		nextFrameGenerateCount += fragmentGenerateSpeed * deltaTime;
		if (nextFrameGenerateCount > 1f)
		{
			int num = Mathf.FloorToInt(nextFrameGenerateCount);
			for (int i = 0; i < num; i++)
			{
				Vector2Int tilePosition = battleInfo.tilePosition;
				int num2 = Mathf.RoundToInt(GetRandomFloat((float)(-battleInfo.highDamageSize) * 0.5f, (float)battleInfo.highDamageSize * 0.5f));
				tilePosition.x += num2;
				int num3 = Mathf.RoundToInt(GetRandomFloat((float)(-battleInfo.highDamageSize) * 0.5f, (float)battleInfo.highDamageSize * 0.5f));
				tilePosition.y += num3;
				int pointIndex = SceneManager.World.TilePosToIndex(tilePosition);
				RandomCreateMeteoriteSonPlayer(pointIndex);
			}
			nextFrameGenerateCount = 0f;
		}
	}

	private void UpdateMiddleMeteoritePreparePlayer()
	{
		if (preparePlayers.Count <= 0)
		{
			return;
		}
		SceneInterface world = SceneManager.World;
		if (world == null)
		{
			return;
		}
		for (int num = preparePlayers.Count - 1; num >= 0; num--)
		{
			long num2 = preparePlayers[num];
			int num3 = (int)(num2 >> 32);
			int num4 = (int)(num2 & 0xFFFFFFFFu) - serverTimeSec;
			if (num4 <= 0)
			{
				preparePlayers.RemoveAt(num);
			}
			else if (num4 <= 2)
			{
				preparePlayers.RemoveAt(num);
				float height = GetRandomFloat(middleFragmentDropSpeedRange.x, middleFragmentDropSpeedRange.y) * (float)num4;
				Vector3 targetPosition = world.TileIndexToWorld(num3, serverId);
				float randomFloat = GetRandomFloat(middleFragmentDegreeRange.x, middleFragmentDegreeRange.y);
				Vector3 scale = new Vector3(GetRandomFloat(middleFragmentDropScaleRange.x, middleFragmentDropScaleRange.y), GetRandomFloat(middleFragmentDropScaleRange.x, middleFragmentDropScaleRange.y), GetRandomFloat(middleFragmentDropScaleRange.x, middleFragmentDropScaleRange.y));
				float rotateSpeed = 360f;
				FragmentData item = new FragmentData
				{
					fragmentType = FragmentData.FragmentType.Middle,
					serverId = serverId,
					pointIndex = num3,
					targetPosition = targetPosition,
					tilePosition = world.IndexToTilePos(num3),
					height = height,
					degree = randomFloat,
					dropTime = (float)num4 - MathF.PI / 10f,
					exploreTime = middleFragmentExploreTimeSec,
					crackTime = middleFragmentCrackTimeSec,
					rotateSpeed = rotateSpeed,
					worldScene = worldScene,
					scale = scale,
					startTime = serverTimeMs,
					maxLod = GetFragmentMaxLod()
				};
				allFragments.Add(item);
			}
		}
	}

	public void SetFragmentDensity(int density = 5)
	{
		if (density <= 0)
		{
			fragmentGenerateSpeed = -1f;
			return;
		}
		fragmentGenerateSpeed = density;
		switch (DeviceLevel)
		{
		case 1:
			fragmentGenerateSpeed /= 5f;
			break;
		case 2:
			fragmentGenerateSpeed /= 2f;
			break;
		case 3:
			fragmentGenerateSpeed /= 1f;
			break;
		}
	}

	private int GetFragmentMaxLod()
	{
		int num = 25;
		int num2 = 20 + num;
		int num3 = 15 + num2;
		if (DeviceLevel <= 1)
		{
			num = 5;
			num2 = 5 + num;
			num3 = 5 + num2;
		}
		int randomInt = GetRandomInt(1, 101);
		if (randomInt <= num)
		{
			fragmentMaxLod7++;
			return 7;
		}
		if (randomInt <= num2)
		{
			fragmentMaxLod6++;
			return 6;
		}
		if (randomInt <= num3)
		{
			fragmentMaxLod5++;
			return 5;
		}
		return 4;
	}

	public void CreateSmallFragmentPlayer(Vector3 position, float height, float degree, float dropTimeSec, Vector3 scale, float rotateSpeed)
	{
		CreateSmallFragmentPlayer(position, height, degree, dropTimeSec, scale, rotateSpeed, 0L);
	}

	public void CreateMiddleFragmentPlayer(int pointIndex, int openTime)
	{
		if (SceneManager.World != null && Inited)
		{
			long item = ((long)pointIndex << 32) | (uint)openTime;
			if (!preparePlayers.Contains(item))
			{
				preparePlayers.Add(item);
			}
		}
	}

	private void TryRetrieveDaddy()
	{
		if (daddyFragment == null)
		{
			int msToStart = MsToStart;
			bool flag = false;
			if (msToStart >= 0 && msToStart < (long)(dropTimeDaddy * 1000f))
			{
				flag = true;
			}
			else if (msToStart < 0 && -msToStart < (long)(exploreTimeDaddy * 1000f))
			{
				flag = true;
			}
			if (flag)
			{
				long startTime = 1000L * (long)battleInfo.startTime - (long)(dropTimeDaddy * 1000f);
				CreateMeteoriteDaddyPlayer(battleInfo.worldPosition, heightDaddy, degreeDaddy, dropTimeDaddy, exploreTimeDaddy, scaleDaddy, rorateSpeedDaddy, startTime);
			}
		}
	}

	private void CreateSmallFragmentPlayer(Vector3 position, float height, float degree, float dropTimeSec, Vector3 scale, float rotateSpeed, long delayMs)
	{
		if (Inited)
		{
			SceneInterface world = SceneManager.World;
			if (world != null)
			{
				int num = world.WorldToTileIndex(position);
				FragmentData item = new FragmentData
				{
					fragmentType = FragmentData.FragmentType.Small,
					serverId = serverId,
					pointIndex = num,
					targetPosition = position,
					tilePosition = world.IndexToTilePos(num),
					height = height,
					degree = degree,
					dropTime = dropTimeSec,
					exploreTime = smallFragmentExploreTimeSec,
					crackTime = smallFragmentCrackTimeSec,
					rotateSpeed = rotateSpeed,
					worldScene = worldScene,
					maxLod = GetFragmentMaxLod(),
					scale = scale,
					startTime = GameEntry.Timer.GetServerTime() + delayMs,
					delayTimeSec = (float)delayMs * 0.001f
				};
				allFragments.Add(item);
			}
		}
	}

	public void CreateMeteoriteDaddyPlayer(Vector3 position, float height, float degree, float dropTimeSec, float exploreTimeSec, float scale, float rotateSpeed, long startTime)
	{
		if (!Inited)
		{
			return;
		}
		SceneInterface world = SceneManager.World;
		if (world != null)
		{
			if (daddyFragment != null)
			{
				daddyFragment.Dispose();
				daddyFragment = null;
			}
			int num = world.WorldToTileIndex(position);
			daddyFragment = new FragmentData
			{
				fragmentType = FragmentData.FragmentType.Daddy,
				serverId = serverId,
				pointIndex = num,
				targetPosition = position,
				tilePosition = world.IndexToTilePos(num),
				height = height,
				degree = degree,
				dropTime = dropTimeSec,
				exploreTime = exploreTimeSec,
				crackTime = 0f,
				rotateSpeed = rotateSpeed,
				worldScene = worldScene,
				scale = Vector3.one * scale,
				startTime = startTime
			};
		}
	}

	public void PlayLastKillNotice(int pointIndex, string uuid, string headPic, int picVer, string iconPic, int scoreAdd, string iconPic2, int scoreAdd2)
	{
		if (Inited)
		{
			if (allKillNotice == null)
			{
				allKillNotice = new List<LastKillData>();
			}
			SceneInterface world = SceneManager.World;
			if (world != null)
			{
				Vector3 position = world.TileIndexToWorld(pointIndex);
				LastKillData item = new LastKillData
				{
					position = position,
					iconPic = iconPic,
					iconPic2 = iconPic2,
					uuid = uuid,
					headPic = headPic,
					picVer = picVer,
					score = scoreAdd,
					score2 = scoreAdd2
				};
				allKillNotice.Add(item);
			}
		}
	}

	private void UpdateKillNotice(float deltaTime)
	{
		if (allKillNotice == null)
		{
			return;
		}
		int lod = SceneManager.World?.GetLodLevel() ?? (-1);
		for (int num = allKillNotice.Count - 1; num >= 0; num--)
		{
			LastKillData lastKillData = allKillNotice[num];
			if (lastKillData == null)
			{
				allKillNotice.RemoveAt(num);
			}
			else if (lastKillData.Update(lod, deltaTime))
			{
				lastKillData.Dispose();
				allKillNotice.RemoveAt(num);
			}
		}
	}

	private void ClearKillNotice()
	{
		if (allKillNotice != null)
		{
			for (int num = allKillNotice.Count - 1; num >= 0; num--)
			{
				allKillNotice[num]?.Dispose();
			}
			allKillNotice.Clear();
		}
	}

	private void InitOtherEffects()
	{
		redRingFadeSpd = 0f;
		redRingAlpha = 1f;
		waveDiffuseRadius = 0f;
		mrRedRing?.SetMaterialFloat(KEY_Alpha, redRingAlpha);
		ResetVolumeOverride();
	}

	private void ResetVolumeOverride()
	{
		if (this.volume == null)
		{
			return;
		}
		this.volume.enabled = false;
		Volume volume = worldScene?.SceneInstanceGameObject?.GetComponentInChildren<Volume>();
		sceneWhiteClip = -1f;
		maxWhiteClipDuration = -1f;
		whiteClipTonemapping = null;
		if (volume == null)
		{
			return;
		}
		if (volume.profile.TryGet<Tonemapping>(out var component))
		{
			sceneWhiteClip = component.neutralWhiteClip.value;
			this.volume.priority = volume.priority + 1f;
		}
		if (sceneWhiteClip > 0f)
		{
			if (this.volume.profile.TryGet<Tonemapping>(out whiteClipTonemapping))
			{
				whiteClipTonemapping.neutralWhiteClip.value = sceneWhiteClip;
				whiteClipTonemapping.neutralWhiteClip.overrideState = true;
			}
			maxWhiteClipDuration = whiteClipCurve[whiteClipCurve.length - 1].time;
		}
	}

	private void ClearOtherEffects()
	{
		ClearCameraEffects();
		if (volume != null)
		{
			volume.enabled = false;
		}
	}

	private void TryLoadCameraEffects()
	{
	}

	private void ClearCameraEffects()
	{
		if (cameraEffectRequest != null)
		{
			cameraEffectRequest.Destroy();
			cameraEffectRequest = null;
		}
		cameraEffect = null;
	}

	private void UpdateOtherEffects(float deltaTime)
	{
		if (cameraEffect != null)
		{
			if (currentLod < 6 && battleState == State.InBattle)
			{
				Vector3 vector = currentCameraPosition - battleInfo.worldPosition;
				vector.y = 0f;
				cameraEffect.TryActive(vector.sqrMagnitude <= waveDiffuseRadius * waveDiffuseRadius);
			}
			else
			{
				cameraEffect.TryActive(active: false);
			}
		}
		if (!Mathf.Approximately(redRingFadeSpd, 0f))
		{
			redRingAlpha += redRingFadeSpd * deltaTime;
			if (redRingFadeSpd >= 0f && redRingAlpha >= 1f)
			{
				redRingAlpha = 1f;
				redRingFadeSpd *= -1f;
			}
			else if (redRingFadeSpd < 0f && redRingAlpha <= 0f)
			{
				redRingAlpha = 0f;
				redRingFadeSpd *= -1f;
			}
			mrRedRing.SetMaterialFloat(KEY_Alpha, redRingAlpha);
		}
		long num = serverTimeMs - (long)battleInfo.startTime * 1000L;
		long num2 = 10000L;
		if (num <= 0)
		{
			waveDiffuseRadius = 0f;
		}
		else if (num <= num2)
		{
			waveDiffuseRadius = (float)num * 0.001f * explodeSpeed;
		}
		else
		{
			waveDiffuseRadius = explodeSpeed * 10f;
		}
		if (!(whiteClipTonemapping != null) || !(sceneWhiteClip > 0f) || num < 0)
		{
			return;
		}
		if ((float)num <= 1000f * maxWhiteClipDuration)
		{
			float num3 = 0f;
			if (true)
			{
				float time = (float)num / 1000f;
				if (!volume.enabled)
				{
					volume.enabled = true;
				}
				num3 = whiteClipCurve.Evaluate(time);
			}
			whiteClipTonemapping.neutralWhiteClip.value = sceneWhiteClip + num3;
		}
		else
		{
			whiteClipTonemapping = null;
			volume.enabled = false;
		}
	}

	private void SetRedRingFadeInOut(float speed)
	{
		redRingFadeSpd = ((redRingFadeSpd > 0f) ? speed : (0f - speed));
	}

	private void CreateFakeMeteoriteFragment(object pId)
	{
		if (int.TryParse(pId.ToString(), out var result) && (fakeMeteoriteCreateTime <= 0 || serverTimeMs - fakeMeteoriteCreateTime >= 5000))
		{
			RandomCreateMeteoriteSonPlayer(result, 500L);
			RandomCreateMeteoriteSonPlayer(result, 5000L);
			fakeMeteoriteCreateTime = serverTimeMs;
		}
	}

	private void SetDissolve(float progress)
	{
		mrHigh?.SetMaterialFloat(KEY_DISSOLVE_PROGRESS, progress);
		mrHigh?.SetMaterialFloat(KEY_AlphaBlendVal, 1f);
		mrLow?.SetMaterialFloat(KEY_DISSOLVE_PROGRESS, 0f);
		mrLow?.SetMaterialFloat(KEY_AlphaBlendVal, 1f - progress);
		mrMapHigh?.SetMaterialFloat(KEY_DISSOLVE_PROGRESS, progress);
		mrMapHigh?.SetMaterialFloat(KEY_AlphaBlendVal, 1f);
		mrMapLow?.SetMaterialFloat(KEY_DISSOLVE_PROGRESS, 0f);
		mrMapLow?.SetMaterialFloat(KEY_AlphaBlendVal, 1f - progress);
	}

	private void SetShadowSize(float size)
	{
		size = Mathf.Clamp01(size);
		if (mrShadow != null)
		{
			mrShadow.transform.localScale = new Vector3(size, 1f, size);
		}
	}

	public void UpdateInfo(int serverId, int pointIndex, int hRadius, int lRadius, int startTime, int endTime)
	{
		if (!Inited)
		{
			Init();
		}
		Instance = this;
		this.serverId = serverId;
		battleInfo.Setup(serverId, pointIndex, hRadius, lRadius, startTime, endTime);
		Vector3 localPosition = SceneManager.World.TileIndexToWorld(pointIndex, serverId);
		base.transform.localPosition = localPosition;
		mrHigh.SetMaterialFloat(KEY_CenterX, localPosition.x);
		mrHigh.SetMaterialFloat(KEY_CenterY, localPosition.z);
		mrLow.SetMaterialFloat(KEY_CenterX, localPosition.x);
		mrLow.SetMaterialFloat(KEY_CenterY, localPosition.z);
		mrMapHigh.SetMaterialFloat(KEY_CenterX, localPosition.x);
		mrMapHigh.SetMaterialFloat(KEY_CenterY, localPosition.z);
		mrMapLow.SetMaterialFloat(KEY_CenterX, localPosition.x);
		mrMapLow.SetMaterialFloat(KEY_CenterY, localPosition.z);
		float num = 0.2f;
		num *= 1.1f;
		Vector3 localScale = new Vector3((float)battleInfo.lowDamageSize * num, 1f, (float)battleInfo.lowDamageSize * num);
		Vector3 localScale2 = new Vector3((float)battleInfo.highDamageSize * num, 1f, (float)battleInfo.highDamageSize * num);
		mrHigh.transform.localScale = localScale2;
		mrMapHigh.transform.localScale = localScale2;
		mrLow.transform.localScale = localScale;
		mrMapLow.transform.localScale = localScale;
		mrRedRing.RebuildMesh(battleInfo.maxRadius, battleInfo.maxRadius + 3f, 512);
		mrShadow.RebuildMesh(battleInfo.maxRadius, 360);
		float num2 = (float)(hRadius * 2) * 2f + 1f;
		float num3 = (float)((hRadius + lRadius) * 2) * 2f + 1f;
		highRectangle.RebuildMesh(num2, num2, rectangleLineWidth);
		lowRectangle.RebuildMesh(num3, num3, rectangleLineWidth);
		RefreshServerTime();
		RefreshState();
		RefreshLod(SceneManager.World.GetLodLevel());
		OnCameraChangePoint(null);
	}

	private void Init()
	{
		Inited = true;
		base.enabled = true;
		base.transform.SetParent(SceneManager.World.DynamicObjNode);
		DeviceLevel = GameEntry.Lua.CallWithReturn<int>("CSharpCallLuaInterface.GetDeviceLevel");
		hudCountdown.gameObject.TryActive(active: false);
		hudCountdown.Notice = GameEntry.Localization.GetString("yuntieBattle_interface_1005");
		worldScene = SceneManager.World as WorldScene;
		if (worldCamera == null)
		{
			worldCamera = worldScene.Camera;
			if (worldCamera != null)
			{
				worldCamera.AfterUpdate += AfterCameraUpdate;
			}
		}
		refreshTimer = 0f;
		currentLod = -1;
		battleState = State.None;
		mrHigh.gameObject.TryActive(active: false);
		mrLow.gameObject.TryActive(active: false);
		mrRedRing.gameObject.TryActive(active: false);
		mrShadow.gameObject.TryActive(active: false);
		InitOtherEffects();
		GameEntry.Event.Subscribe(EventId.ChangeCameraLod, OnLodChanged);
		GameEntry.Event.Subscribe(EventId.WORLD_CAMERA_CHANGE_POINT, OnCameraChangePoint);
		GameEntry.Event.Subscribe(EventId.MeteoriteFakeFragment, CreateFakeMeteoriteFragment);
	}

	public void Dispose()
	{
		if (Inited)
		{
			Inited = false;
			if ((object)Instance == this)
			{
				Instance = null;
			}
			GameEntry.Event.Unsubscribe(EventId.ChangeCameraLod, OnLodChanged);
			GameEntry.Event.Unsubscribe(EventId.WORLD_CAMERA_CHANGE_POINT, OnCameraChangePoint);
			GameEntry.Event.Unsubscribe(EventId.MeteoriteFakeFragment, CreateFakeMeteoriteFragment);
			currentState?.LeaveState();
			currentState = null;
			battleState = State.None;
			worldScene?.StaticManager?.SetDecorationInvisibleRect(WorldStaticManager.DecorationInvisibleRectType.None, Vector3.zero, 0f);
			ClearFragments();
			ClearKillNotice();
			ClearDrops();
			ClearOtherEffects();
			if (worldCamera != null)
			{
				worldCamera.AfterUpdate -= AfterCameraUpdate;
				worldCamera = null;
			}
		}
	}

	private void Update()
	{
		if (Inited)
		{
			RefreshServerTime();
			float unscaledDeltaTime = Time.unscaledDeltaTime;
			refreshTimer -= unscaledDeltaTime;
			if (refreshTimer <= 0f)
			{
				RefreshState();
				RefreshHudTimeCountdown();
				refreshTimer = 0.5f;
			}
			currentState?.UpdateState(unscaledDeltaTime);
			UpdateFragments(unscaledDeltaTime);
			UpdateKillNotice(unscaledDeltaTime);
			UpdateMeteoriteDropPlayer(unscaledDeltaTime);
			UpdateOtherEffects(unscaledDeltaTime);
		}
	}

	private void RefreshServerTime()
	{
		serverTimeMs = GameEntry.Timer?.GetServerTime() ?? 0;
		serverTimeSec = GameEntry.Timer?.GetServerTimeSeconds() ?? 0;
	}

	private void OnLodChanged(object obj)
	{
		if (int.TryParse(obj.ToString(), out var result))
		{
			RefreshLod(result);
		}
	}

	private void OnCameraChangePoint(object obj)
	{
		if (!(worldScene == null))
		{
			Vector2Int curTilePos = worldScene.CurTilePos;
			Vector2Int tilePosition = battleInfo.tilePosition;
			int num = battleInfo.lowRadius + battleInfo.hightRadius;
			currentCameraPosition = worldCamera?.CurTarget ?? Vector3.zero;
			isLookAtCenter = curTilePos.x > tilePosition.x - num && curTilePos.x < tilePosition.x + num && curTilePos.y > tilePosition.y - num && curTilePos.y < tilePosition.y + num;
		}
	}

	private void AfterCameraUpdate()
	{
		if (worldCamera != null)
		{
			float lodDistance = worldCamera.GetLodDistance();
			float value = Mathf.Lerp(t: (lodDistance - 50f) / 1000f, a: thicknessRange.x, b: thicknessRange.y);
			mrRedRing.SetMaterialFloat(KEY_Thickness, value);
			float t = (lodDistance - rectangleLineDistanceRange.x) / (rectangleLineDistanceRange.y - rectangleLineDistanceRange.x);
			value = Mathf.Lerp(rectangleLineWidthRange.x, rectangleLineWidthRange.y, t);
			highRectangle.SetMaterialFloat(KEY_Thickness, value);
			lowRectangle.SetMaterialFloat(KEY_Thickness, value);
		}
	}

	private void RefreshLod(int lod)
	{
		if (currentLod != lod)
		{
			currentLod = lod;
			RefreshHudTimeCountdown();
			mapNode.TryActive(lod > 5);
			terrainNode.TryActive(lod <= 5);
			highRectangle.SetRenderOrder((lod > 5) ? 1 : 0);
			lowRectangle.SetRenderOrder((lod > 5) ? 1 : 0);
		}
	}

	private void RefreshHudTimeCountdown()
	{
		if (!Inited || hudCountdown == null)
		{
			return;
		}
		if (!showHudCountdown || currentLod < 4 || daddyFragment != null)
		{
			hudCountdown.gameObject.TryActive(active: false);
			return;
		}
		hudCountdown.gameObject.TryActive(active: true);
		int num = (int)(((long)battleInfo.startTime * 1000L - serverTimeMs) / 1000);
		num = ((num > 0) ? num : 0);
		int num2 = num / 3600 / 24;
		int num3 = (num - num2 * 3600 * 24) / 3600;
		int num4 = (num - num2 * 3600 * 24 - num3 * 3600) / 60;
		int num5 = num - num2 * 3600 * 24 - num3 * 3600 - num4 * 60;
		sbTimeFormat = sbTimeFormat ?? new StringBuilder();
		sbTimeFormat.Length = 0;
		if (num2 > 0)
		{
			sbTimeFormat.AppendFormat("{0}d {1:D2}:{2:D2}:{3:D2}", num2, num3, num4, num5);
		}
		else
		{
			sbTimeFormat.AppendFormat("{0:D2}:{1:D2}:{2:D2}", num3, num4, num5);
		}
		hudCountdown.Countdown = sbTimeFormat.ToString();
	}

	private string FormatTime(int time)
	{
		return DateTimeOffset.FromUnixTimeSeconds(time).LocalDateTime.ToString("yyyy-M-d hh:mm:ss");
	}

	private void UpdateTerrainDiffuseDistance(float distance)
	{
		mrMapLow.SetMaterialFloat(KEY_MaxDistance, distance);
		mrMapHigh.SetMaterialFloat(KEY_MaxDistance, distance);
		mrLow.SetMaterialFloat(KEY_MaxDistance, distance);
		mrHigh.SetMaterialFloat(KEY_MaxDistance, distance);
	}

	public string Description()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(string.Format("当前状态:{0}(状态机类:{1})", battleState, (currentState == null) ? "NULL" : currentState.GetType().Name));
		stringBuilder.AppendLine($"是否初始化:{Inited}");
		stringBuilder.AppendLine($"中心点位置:{battleInfo.pointIndex}");
		stringBuilder.AppendLine($"世界坐标:{battleInfo.worldPosition}");
		stringBuilder.AppendLine($"地图坐标:{battleInfo.tilePosition}");
		stringBuilder.AppendLine($"尺寸(直径):{battleInfo.highDamageSize}[黑土地], {battleInfo.lowDamageSize}[黄土地]");
		stringBuilder.AppendLine("陨石落下的时间:" + FormatTime(battleInfo.startTime));
		stringBuilder.AppendLine("争夺战结束时间:" + FormatTime(battleInfo.endTime));
		stringBuilder.AppendLine("当前时间:" + FormatTime(serverTimeSec));
		stringBuilder.AppendLine($"当前Lod级别:{currentLod}");
		stringBuilder.AppendLine($"当前设备级别:{DeviceLevel}");
		int num = 0;
		foreach (FragmentData allFragment in allFragments)
		{
			num += (allFragment.IsVisible(currentLod) ? 1 : 0);
		}
		stringBuilder.AppendLine($"碎片演员数量:{allFragments?.Count}[可见:{num}]");
		stringBuilder.AppendLine($"碎片显示层级数量[7]:{fragmentMaxLod7}");
		stringBuilder.AppendLine($"碎片显示层级数量[6]:{fragmentMaxLod6}");
		stringBuilder.AppendLine($"碎片显示层级数量[5]:{fragmentMaxLod5}");
		stringBuilder.AppendLine($"特效(掉落):{allDrops.Count}");
		stringBuilder.AppendLine($"特效(击杀):{allKillNotice?.Count}");
		stringBuilder.AppendLine($"自动生成碎片陨石速度(个/秒):{fragmentGenerateSpeed}");
		stringBuilder.AppendLine($"准备阶段待创建的陨石数量:{preparePlayers.Count}");
		return stringBuilder.ToString();
	}

	private void OnDestroy()
	{
		Dispose();
	}

	private void OnEditorDiffuseRangeChanged()
	{
		UpdateTerrainDiffuseDistance(editorDiffuseRange);
	}

	private void OnDrawGizmos()
	{
		if (showGizmos)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawCube(currentCameraPosition, Vector3.one);
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(battleInfo.worldPosition, waveDiffuseRadius);
		}
	}

	private void RefreshState()
	{
		if (!Inited)
		{
			battleState = State.None;
			return;
		}
		State state = State.None;
		int num = serverTimeSec;
		if (num >= battleInfo.endTime + 3)
		{
			state = State.None;
		}
		else if (num >= battleInfo.endTime - 1)
		{
			state = State.BattleEnd;
		}
		else if (num >= battleInfo.startTime)
		{
			state = State.InBattle;
		}
		else
		{
			int num2 = battleInfo.startTime - num;
			state = ((num2 <= TIME_WARNING_3) ? State.WarningLv_3 : ((num2 <= TIME_WARNING_2) ? State.WarningLv_2 : ((num2 <= TIME_WARNING_1) ? State.WarningLv_1 : ((num2 > TIME_WARNING_0) ? State.Happy : State.WarningLv_0))));
		}
		if (state != battleState)
		{
			OnBattleStateChanged(battleState, state);
		}
	}

	private void OnBattleStateChanged(State lastState, State newState)
	{
		currentState?.LeaveState();
		currentState = null;
		if (CommonUtils.IsDebug())
		{
			Debug.Log($"[meteorite]战斗状态切换:{battleState} -> {newState}");
		}
		battleState = newState;
		switch (battleState)
		{
		case State.WarningLv_0:
			currentState = new BattleStateWarning0();
			currentState.EnterState(this);
			break;
		case State.WarningLv_1:
			currentState = new BattleStateWarning1();
			currentState.EnterState(this);
			break;
		case State.WarningLv_2:
			currentState = new BattleStateWarning2();
			currentState.EnterState(this);
			break;
		case State.WarningLv_3:
			currentState = new BattleStateWarning3();
			currentState.EnterState(this);
			break;
		case State.InBattle:
			currentState = new BattleStateInBattle();
			currentState.EnterState(this);
			break;
		case State.BattleEnd:
			currentState = new BattleStateBattleEnd();
			currentState.EnterState(this);
			break;
		}
		GameEntry.Event.Fire(EventId.MeteoriteBattlePlayerStateChanged, (int)battleState);
	}
}
