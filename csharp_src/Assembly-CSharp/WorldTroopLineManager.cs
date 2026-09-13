using System;
using System.Collections.Generic;
using System.Text;
using FibMatrix;
using GameFramework;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using VEngine;

public class WorldTroopLineManager : WorldManagerBase
{
	public class LittleSmart : LittleSmartUpdater<WorldTroopLineManager, LittleSmart>
	{
		private Dictionary<long, WorldTroopLineLittleSmart> troopLineDic;

		private WorldGpuInstancingRenderer renderer;

		private Asset materialRequester;

		private List<WorldMarch> temp = new List<WorldMarch>(1024);

		private const int MAX_CONVERT_PER_FRAME = 100;

		private bool converting;

		protected override string LuaCheckEnableFuncName => "CSharpCallLuaInterface.IsLittleSmartTroopLineEnable";

		protected override EventId ChangeTroopModeEventId => EventId.ChangeLittleSmartTroopLineMode;

		public int LitTroopLineCount => troopLineDic?.Count ?? 0;

		public int TroopLineCount => troopLineDic?.Count ?? 0;

		public override bool IsLittleSmartModeEnable => newTroopLineEnabled;

		protected override void OnInit()
		{
			troopLineDic = new Dictionary<long, WorldTroopLineLittleSmart>(1024);
			WorldTroopLineLittleSmart.Line.GlobalTroopLineScale = 1.1f;
			WorldTroopLineLittleSmart.Line.GlobalTroopLineAlpha = 1f;
			WorldTroopLineLittleSmart.Line.GlobalTroopCircleScale = 1f;
			renderer = WorldGpuInstancingRenderer.Create("WorldTroopLine", WorldGpuInstancingUtils.CreateNormalMeshVertical(), null, WorldTroopLineLittleSmart.RendererGroup.Create, RenderPassEvent.AfterRenderingTransparents, 10, 1, 5);
			materialRequester = GameEntry.Resource.LoadAssetAsync("Assets/Main/Material/TroopLineInstancing.mat", typeof(Material));
			Asset asset = materialRequester;
			asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
			{
				if (materialRequester != null && materialRequester.status == LoadableStatus.SuccessToLoad)
				{
					if (renderer == null)
					{
						materialRequester.Release();
						materialRequester = null;
					}
					else
					{
						Material material = null;
						material = new Material(materialRequester.asset as Material);
						renderer.material = material;
						OnRendererReady();
					}
				}
			});
			temp.Clear();
			converting = false;
		}

		protected override void OnDispose()
		{
			if (materialRequester != null)
			{
				materialRequester.Release();
				materialRequester = null;
			}
			Clear();
			renderer?.DisposeRenderer();
			converting = false;
		}

		public override string EditorDescription()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("---World Troop Line Manager---");
			stringBuilder.AppendLine("新行军线是否开启:" + (IsLittleSmartModeEnable ? "开启" : "关闭"));
			stringBuilder.AppendLine($"当前传统行军线数量:{host._troopLines.Count}");
			int num = 0;
			int num2 = 0;
			if (host._troopLines.Count > 0)
			{
				stringBuilder.AppendLine("各类型的数量为:");
				Dictionary<NewMarchType, int> dictionary = new Dictionary<NewMarchType, int>();
				foreach (KeyValuePair<long, TroopLine> troopLine in host._troopLines)
				{
					NewMarchType type = troopLine.Value.march.type;
					if (dictionary.TryGetValue(type, out var value))
					{
						dictionary[type] = value + 1;
					}
					else
					{
						dictionary[type] = 1;
					}
					num += (troopLine.Value.march.SupportLittleSmartTroopLineMode() ? 1 : 0);
					num2 += (troopLine.Value.IsLoaded() ? 1 : 0);
				}
				foreach (KeyValuePair<NewMarchType, int> item in dictionary)
				{
					stringBuilder.AppendLine($"{item.Key.ToString()}:{item.Value}");
				}
			}
			stringBuilder.AppendLine($"可转换为小聪明行军线的数量为:{num}");
			stringBuilder.AppendLine($"当前行军已实例化数量:{num2}");
			stringBuilder.AppendLine($"当前行军(小聪明)数量:{troopLineDic.Count}");
			return stringBuilder.ToString();
		}

		protected override void OnLittleSmartModeChanged()
		{
			if (IsLittleSmartModeEnable)
			{
				if (host._troopLines.Count <= 0)
				{
					return;
				}
				converting = true;
				temp.Clear();
				foreach (KeyValuePair<long, TroopLine> troopLine in host._troopLines)
				{
					TroopLine value = troopLine.Value;
					WorldMarch march = value.march;
					if (march != null && march.SupportLittleSmartTroopLineMode() && !troopLine.Value.IsLoaded())
					{
						temp.Add(value.march);
					}
				}
				if (temp.Count > 0)
				{
					int i = 0;
					for (int count = temp.Count; i < count; i++)
					{
						WorldMarch worldMarch = temp[i];
						host.DestroyLegacyTroopLine(worldMarch.uuid);
					}
					temp.Clear();
				}
			}
			else
			{
				converting = false;
			}
		}

		private void Clear()
		{
			if (troopLineDic == null || troopLineDic.Count <= 0)
			{
				return;
			}
			foreach (KeyValuePair<long, WorldTroopLineLittleSmart> item in troopLineDic)
			{
				WorldTroopLineLittleSmart.Recycle(item.Value);
			}
			troopLineDic.Clear();
		}

		public void RefreshOrCreateTroop(WorldMarch march)
		{
			if (!troopLineDic.TryGetValue(march.uuid, out var value))
			{
				value = WorldTroopLineLittleSmart.Create(renderer, march.uuid);
				troopLineDic.Add(march.uuid, value);
			}
			value.RefreshByMarch(march);
		}

		protected override void OnLittleSmartUpdate(long currentServerTime, float delteTime)
		{
			if (!converting)
			{
				return;
			}
			temp.Clear();
			int num = 100;
			foreach (KeyValuePair<long, TroopLine> troopLine in host._troopLines)
			{
				WorldMarch march = troopLine.Value.march;
				if (march != null && march.SupportLittleSmartTroopLineMode())
				{
					temp.Add(troopLine.Value.march);
					if (--num <= 0)
					{
						break;
					}
				}
			}
			if (temp.Count > 0)
			{
				int i = 0;
				for (int count = temp.Count; i < count; i++)
				{
					WorldMarch worldMarch = temp[i];
					RefreshOrCreateTroop(worldMarch);
					host.DestroyLegacyTroopLine(worldMarch.uuid);
				}
				temp.Clear();
			}
			else
			{
				converting = false;
			}
		}

		public void DestroyTroopLine(long marchUuid)
		{
			if (troopLineDic == null || !troopLineDic.TryGetValue(marchUuid, out var value))
			{
				return;
			}
			WorldTroopLineLittleSmart.Line[] lines = value.Lines;
			int i = 0;
			for (int num = lines.Length; i < num; i++)
			{
				WorldTroopLineLittleSmart.Line line = lines[i];
				if (line != null && line.DataIndex >= 0)
				{
					renderer?.ReleaseIndex(line);
				}
			}
			troopLineDic.Remove(marchUuid);
			WorldTroopLineLittleSmart.Recycle(value);
		}

		protected override void OnDrawGizmos()
		{
			if (troopLineDic == null || troopLineDic.Count <= 0)
			{
				return;
			}
			foreach (KeyValuePair<long, WorldTroopLineLittleSmart> item in troopLineDic)
			{
				item.Value.OnDrawGizmos();
			}
		}

		public bool IsTroopLineCreate(long marchUuid)
		{
			return troopLineDic.ContainsKey(marchUuid);
		}

		private void OnRendererReady()
		{
		}

		public bool HasTroopLine(long uuid)
		{
			return troopLineDic.ContainsKey(uuid);
		}

		public bool IsTroopLineMarchOutOfData(WorldMarch march)
		{
			if (troopLineDic != null && troopLineDic.TryGetValue(march.uuid, out var value))
			{
				return value.marchInfo != march;
			}
			return false;
		}
	}

	private class TroopLine : IDisposable
	{
		public long uuid;

		public InstanceRequest lineInst;

		public WorldTroopLine line;

		public WorldCamp camp = WorldCamp.Neutral;

		public WorldMarch march;

		public WorldTroopTimer lineTimer;

		private WorldTroopLineManager troopLineManager;

		private float lineWidthScale = 1f;

		private int currentDisplayLevel;

		private Color GetDarkColorByCamp()
		{
			if (hasNewTroopLineColor)
			{
				return camp switch
				{
					WorldCamp.Self => SeasonMyTroopLineColor, 
					WorldCamp.Ally => SeasonAllianceTroopLineColor, 
					WorldCamp.Enemy => SeasonEnemyTroopLineColor, 
					WorldCamp.WsTeammate => WsTeammateTroopLineColor, 
					WorldCamp.WsEnemy => WsEnemyTroopLineColor, 
					_ => SeasonOtherTroopLineColor, 
				};
			}
			return camp switch
			{
				WorldCamp.Self => MyTroopLineColor, 
				WorldCamp.Ally => AllianceTroopLineColor, 
				WorldCamp.Enemy => EnemyTroopLineColor, 
				WorldCamp.WsTeammate => WsTeammateTroopLineColor, 
				WorldCamp.WsEnemy => WsEnemyTroopLineColor, 
				_ => OtherTroopLineColor, 
			};
		}

		private Color GetLightColorByCamp()
		{
			return GetLineColor(march);
		}

		public void SetData(WorldTroopLineManager mgr, WorldMarch march)
		{
			this.march = march;
			troopLineManager = mgr;
			lineWidthScale = mgr.TroopLineWidthScale;
			currentDisplayLevel = mgr.CurrentDisplayLevel;
			camp = march.GetCamp();
			if (lineInst == null)
			{
				lineInst = troopLineManager.world.InstantiateAsyncDynamicObj("Assets/Main/Prefabs/March/TroopLine.prefab");
				lineInst.completed += OnInstantiateComplete;
			}
			else
			{
				RefreshView();
			}
		}

		private void OnInstantiateComplete(InstanceRequest req)
		{
			lineInst.gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
			line = lineInst.gameObject.GetComponent<WorldTroopLine>();
			if (march.SupportLittleSmartTroopLineMode())
			{
				if (troopLineManager.littleSmart.IsLittleSmartModeEnable)
				{
					Log.Error("[WorldMarch] World troop line OnInstantiateComplete exception!");
				}
				else
				{
					troopLineManager.littleSmart?.DestroyTroopLine(march.uuid);
				}
			}
			RefreshView();
		}

		public void RefreshView()
		{
			if (line != null)
			{
				line.Clear();
				SetColor();
				SetWidthScale();
				SetScale();
				SetRotation();
				SetStart();
				SetEnd();
				SetDelayShow();
				UpdateDisplayMode();
				CheckPathHide();
			}
		}

		public void CheckPathHide()
		{
			if (march.IsBloodyQueenMonster() && (march.pathList == null || march.pathList.Length == 0) && march.IsBloodyQueenMonster())
			{
				line.SetLineVisible(visible: false);
				line.SetMidSpriteVisible(visible: false);
			}
		}

		public void UpdateDisplayMode()
		{
			if (line == null)
			{
				return;
			}
			int num = 7;
			bool midSpriteVisible = true;
			float a = 1f;
			if (troopLineManager != null)
			{
				num = troopLineManager.ShowTroopLineMask;
				midSpriteVisible = troopLineManager.ShowTroopMidSprite;
				a = troopLineManager.TroopLineWidthScale;
				currentDisplayLevel = troopLineManager.CurrentDisplayLevel;
			}
			int num2 = 7;
			if (march != null)
			{
				if (march.ownerUid == GameEntry.Data.Player.Uid)
				{
					num2 = 1;
				}
				else if (!string.IsNullOrEmpty(march.allianceUid))
				{
					string allianceId = GameEntry.Data.Player.GetAllianceId();
					if (!string.IsNullOrEmpty(allianceId) && allianceId == march.allianceUid)
					{
						num2 = 2;
					}
				}
				else
				{
					num2 = 4;
				}
			}
			bool lineVisible = (num & num2) != 0;
			line.SetLineVisible(lineVisible);
			line.SetMidSpriteVisible(midSpriteVisible);
			if (!Mathf.Approximately(a, lineWidthScale))
			{
				lineWidthScale = a;
				line.SetWidthScale(lineWidthScale);
			}
			int num3 = -currentDisplayLevel;
			Vector3 one = Vector3.one;
			one = ((num3 >= 0 && num3 < midScaleArray.Length) ? midScaleArray[num3] : midScaleArray[0]);
			line.SetMidScale(one);
		}

		public void Dispose()
		{
			if (line != null)
			{
				line.OnRecycle();
				line = null;
			}
			if (lineInst != null)
			{
				lineInst.Destroy();
				lineInst = null;
			}
			march = null;
			troopLineManager = null;
		}

		private void SetColor()
		{
			if (line != null)
			{
				line.SetColor(GetDarkColorByCamp(), GetLightColorByCamp());
			}
		}

		private void SetWidthScale()
		{
			if (line != null)
			{
				float num = 1f;
				if (march.type == NewMarchType.BEHEMOTH_BOSS)
				{
					num = 2f;
				}
				line.SetWidthScale(num * lineWidthScale);
			}
		}

		private void SetStart()
		{
			line.InitStart(march.startWorldPos);
			if (march.homePos > 0 && march.targetPos != march.homePos && march.startPos != march.homePos)
			{
				line.InitHomePath(march.homeWorldPos, march.startWorldPos);
			}
		}

		private void SetEnd()
		{
			if (march.pathList != null && march.pathList.Length != 0)
			{
				Vector3 pos = march.pathList[march.pathList.Length - 1].pos;
				line.InitEnd(pos);
			}
			else
			{
				line.InitEnd(march.targetWorldPos);
			}
		}

		private void SetDelayShow()
		{
			if (march.type != NewMarchType.ALL_OUT || !(march.allianceUid == GameEntry.Data.Player.GetAllianceId()) || _allOutMarchUuids.Contains(march.uuid))
			{
				return;
			}
			allOutWaitTime = 2f;
			_allOutLines.Enqueue(this);
			line.EnableFrontLine(enable: false);
			_allOutMarchUuids.Add(march.uuid);
			if (timer != null)
			{
				return;
			}
			timer = GameEntry.Timer.RegisterTimerRepeat(1f, 0.1f, delegate
			{
				if (_allOutLines.Count > 0)
				{
					TroopLine troopLine = _allOutLines.Dequeue();
					if (troopLine.line != null && troopLine.march != null)
					{
						troopLine.line.EnableFrontLine(enable: true);
						if (_allOutLines.Count == 0)
						{
							_lastAllOutLine = troopLine;
							GameEntry.Lua.Call("CSharpCallLuaInterface.ShowAllOutHead", troopLine.march, 4);
						}
						else
						{
							GameEntry.Lua.Call("CSharpCallLuaInterface.ShowAllOutHead", troopLine.march, 1);
						}
					}
				}
				else
				{
					allOutWaitTime -= 0.1f;
					if (allOutWaitTime <= 0f)
					{
						if (_lastAllOutLine != null && _lastAllOutLine.line != null && _lastAllOutLine.march != null)
						{
							GameEntry.Lua.Call("CSharpCallLuaInterface.ShowAllOutBubble", _lastAllOutLine.march, 2);
						}
						GameEntry.Timer.CancelTimer(timer);
						timer = null;
					}
				}
			});
		}

		private void SetScale()
		{
			if (line != null)
			{
				if (march.type == NewMarchType.ZOMBIE_RETREAT)
				{
					line.SetScale(1f, 0f, 1f);
				}
				else if (march.type == NewMarchType.ZOMBIE_RUSH && march.status == MarchStatus.ZOMBIE_RUSH_WAITING)
				{
					line.SetScale(1f, 0f, 1f);
				}
				else if (march.type == NewMarchType.ACT_BERSERK_BOSS && march.status == MarchStatus.BERSERK_BOSS_WAITING)
				{
					line.SetScale(1f, 0f, 1f);
				}
				else
				{
					line.SetScale(1f, 1f, 1f);
				}
			}
		}

		private void SetRotation()
		{
			if (line != null && march.pathList != null && march.pathList.Length != 0)
			{
				line.SetRotation(march.pathList[0].dir);
			}
		}

		public void SetCurPos(Vector3 curPos)
		{
			if (line != null)
			{
				line.UpdatePath(curPos);
			}
		}

		public void SetSpeed(float speed)
		{
			if (line != null)
			{
				line.UpdateSpeed(speed);
			}
		}

		public bool IsLoaded()
		{
			return line != null;
		}
	}

	private LittleSmart littleSmart;

	private static readonly Color MyTroopLineColor = new Color(0.46f, 0.93f, 0.18f, 1f);

	private static readonly Color AllianceTroopLineColor = new Color(0.2f, 0.78f, 0.81f, 1f);

	private static readonly Color EnemyTroopLineColor = new Color(0.89f, 0.23f, 0.2f, 1f);

	private static readonly Color OtherTroopLineColor = new Color(0.89f, 0.89f, 0.89f, 1f);

	private static readonly Color WsTeammateTroopLineColor = new Color(0.2f, 0.78f, 0.81f, 1f);

	private static readonly Color WsEnemyTroopLineColor = new Color(0.89f, 0.23f, 0.2f, 1f);

	private static readonly Color MyLightLineColor = new Color(0.46f, 0.93f, 0.18f, 0.5f);

	private static readonly Color AllianceLightLineColor = new Color(0.2f, 0.78f, 0.81f, 0.5f);

	private static readonly Color EnemyLightLineColor = new Color(0.89f, 0.23f, 0.2f, 0.5f);

	private static readonly Color OtherLightLineColor = new Color(0.89f, 0.89f, 0.89f, 0.5f);

	private static readonly Color WsTeammateLightLineColor = new Color(0.2f, 0.78f, 0.81f, 0.5f);

	private static readonly Color WsEnemyLightLineColor = new Color(0.89f, 0.23f, 0.2f, 0.5f);

	private static Queue<TroopLine> _allOutLines = new Queue<TroopLine>();

	private static TroopLine _lastAllOutLine;

	private const float AllOutInterval = 0.1f;

	private const float AllOutMaxWaitTime = 2f;

	private static float allOutWaitTime = 2f;

	private static readonly HashSet<long> _allOutMarchUuids = new HashSet<long>();

	private static ITimer timer;

	private static bool hasNewTroopLineColor = false;

	private static Color SeasonMyTroopLineColor = new Color(0.46f, 0.93f, 0.18f, 1f);

	private static Color SeasonAllianceTroopLineColor = new Color(0.2f, 0.78f, 0.81f, 1f);

	private static Color SeasonEnemyTroopLineColor = new Color(0.89f, 0.23f, 0.2f, 1f);

	private static Color SeasonOtherTroopLineColor = new Color(0.89f, 0.89f, 0.89f, 1f);

	private static Color SeasonMyLightLineColor = new Color(0.46f, 0.93f, 0.18f, 0.5f);

	private static Color SeasonAllianceLightLineColor = new Color(0.2f, 0.78f, 0.81f, 0.5f);

	private static Color SeasonEnemyLightLineColor = new Color(0.89f, 0.23f, 0.2f, 0.5f);

	private static Color SeasonOtherLightLineColor = new Color(0.89f, 0.89f, 0.89f, 0.5f);

	private static readonly Vector3[] midScaleArray = new Vector3[4]
	{
		new Vector3(2.65f, 2.65f, 2.65f),
		new Vector3(1.55f, 1.55f, 1.55f),
		new Vector3(1f, 1f, 1f),
		new Vector3(1f, 1f, 1f)
	};

	private string troopLineColorOverride = string.Empty;

	private string troopLineColorSeason = string.Empty;

	private static bool newTroopLineEnabled = false;

	private Dictionary<long, TroopLine> _troopLines = new Dictionary<long, TroopLine>();

	private ObjectPool<TroopLine> pool;

	public static WorldTroopLineManager EditorInstance => null;

	public int ShowTroopLineMask { get; private set; } = 7;

	public bool ShowTroopMidSprite { get; private set; } = true;

	public float TroopLineWidthScale { get; private set; } = 1f;

	public float TroopCircleScale { get; private set; } = 1f;

	public int CurrentDisplayLevel { get; private set; }

	public int LegacyTroopLineCount => _troopLines.Count;

	public int NewTroopLineCount => littleSmart?.TroopLineCount ?? 0;

	public string EditorDescription()
	{
		return littleSmart?.EditorDescription();
	}

	public void UpdateTroopLineColor(string troopLineColorArgs)
	{
		troopLineColorOverride = troopLineColorArgs;
		RefreshTroopLineColor();
	}

	public void OnSkinChange()
	{
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta != null && !string.IsNullOrEmpty(curSkinMeta.troop_line_color))
		{
			troopLineColorSeason = curSkinMeta.troop_line_color;
		}
		RefreshTroopLineColor();
	}

	private void RefreshTroopLineColor()
	{
		string empty = string.Empty;
		if (!string.IsNullOrEmpty(troopLineColorOverride))
		{
			empty = troopLineColorOverride;
		}
		else if (!string.IsNullOrEmpty(troopLineColorSeason))
		{
			empty = troopLineColorSeason;
		}
		if (!string.IsNullOrEmpty(empty))
		{
			string[] array = empty.Split(new char[1] { '|' });
			if (array.Length == 4)
			{
				hasNewTroopLineColor = true;
				SeasonMyTroopLineColor = MyTroopLineColor;
				SeasonAllianceTroopLineColor = AllianceTroopLineColor;
				SeasonEnemyTroopLineColor = EnemyTroopLineColor;
				SeasonOtherTroopLineColor = OtherTroopLineColor;
				SeasonMyLightLineColor = MyLightLineColor;
				SeasonAllianceLightLineColor = AllianceLightLineColor;
				SeasonEnemyLightLineColor = EnemyLightLineColor;
				SeasonOtherLightLineColor = OtherLightLineColor;
				string[] array2 = array[0].Split(new char[1] { ';' });
				if (array2.Length == 3)
				{
					SeasonMyTroopLineColor = WorldCityColor.HexToColor(array2[1]);
					SeasonMyLightLineColor = WorldCityColor.HexToColor(array2[2]);
				}
				array2 = array[1].Split(new char[1] { ';' });
				if (array2.Length == 3)
				{
					SeasonAllianceTroopLineColor = WorldCityColor.HexToColor(array2[1]);
					SeasonAllianceLightLineColor = WorldCityColor.HexToColor(array2[2]);
				}
				array2 = array[2].Split(new char[1] { ';' });
				if (array2.Length == 3)
				{
					SeasonEnemyTroopLineColor = WorldCityColor.HexToColor(array2[1]);
					SeasonEnemyLightLineColor = WorldCityColor.HexToColor(array2[2]);
				}
				array2 = array[3].Split(new char[1] { ';' });
				if (array2.Length == 3)
				{
					SeasonOtherTroopLineColor = WorldCityColor.HexToColor(array2[1]);
					SeasonOtherLightLineColor = WorldCityColor.HexToColor(array2[2]);
				}
			}
			else
			{
				hasNewTroopLineColor = false;
			}
		}
		else
		{
			hasNewTroopLineColor = false;
		}
	}

	public static Color GetLineColor(WorldMarch march)
	{
		Color white = Color.white;
		WorldCamp camp = march.GetCamp();
		if (hasNewTroopLineColor)
		{
			return camp switch
			{
				WorldCamp.Self => SeasonMyLightLineColor, 
				WorldCamp.Ally => SeasonAllianceLightLineColor, 
				WorldCamp.Enemy => SeasonEnemyLightLineColor, 
				WorldCamp.WsTeammate => WsTeammateLightLineColor, 
				WorldCamp.WsEnemy => WsEnemyLightLineColor, 
				_ => SeasonOtherLightLineColor, 
			};
		}
		return camp switch
		{
			WorldCamp.Self => MyLightLineColor, 
			WorldCamp.Ally => AllianceLightLineColor, 
			WorldCamp.Enemy => EnemyLightLineColor, 
			WorldCamp.WsTeammate => WsTeammateLightLineColor, 
			WorldCamp.WsEnemy => WsEnemyLightLineColor, 
			_ => OtherLightLineColor, 
		};
	}

	public static Vector4 GetLineColorVec4(WorldMarch march)
	{
		Color lineColor = GetLineColor(march);
		return new Vector4(lineColor.r, lineColor.g, lineColor.b, lineColor.a);
	}

	public WorldTroopLineManager(WorldScene scene)
		: base(scene)
	{
	}

	public override void Init()
	{
		base.Init();
		pool = new ObjectPool<TroopLine>(16);
		OnSkinChange();
		GameEntry.Event.Subscribe(EventId.WorldMarchUpdateDisplayMode, OnWorldMarchUpdateDisplayModeChanged);
		littleSmart?.Dispose();
		littleSmart = LittleSmartUpdater<WorldTroopLineManager, LittleSmart>.Create(this);
		newTroopLineEnabled = GameEntry.Lua?.CallWithReturn<bool>("WorldBattleUtil.EnableNewTroopLine") ?? false;
	}

	public override void UnInit()
	{
		base.UnInit();
		pool.RecycleNoClear(_troopLines.Values);
		pool.Dispose();
		_troopLines.Clear();
		if (CommonUtils.IsDebug())
		{
			_allOutMarchUuids.Clear();
		}
		GameEntry.Event.Unsubscribe(EventId.WorldMarchUpdateDisplayMode, OnWorldMarchUpdateDisplayModeChanged);
		littleSmart?.Dispose();
		littleSmart = null;
	}

	public override void OnUpdate(float deltaTime)
	{
		littleSmart?.UpdateLittleSmart(deltaTime);
	}

	public void CreateTroopLine(WorldMarch march)
	{
		if (!march.NeedCreateTroopLine())
		{
			return;
		}
		if (littleSmart != null && littleSmart.IsLittleSmartModeEnable && march.SupportLittleSmartTroopLineMode())
		{
			littleSmart.RefreshOrCreateTroop(march);
			return;
		}
		if (_troopLines.TryGetValue(march.uuid, out var value))
		{
			value.SetData(this, march);
			return;
		}
		value = pool?.Allocate();
		if (value != null)
		{
			value.SetData(this, march);
			_troopLines.Add(march.uuid, value);
		}
	}

	private void DestroyLegacyTroopLine(long marchUuid)
	{
		if (_troopLines.Count > 0 && _troopLines.TryGetValue(marchUuid, out var value))
		{
			pool.Recycle(value);
			_troopLines.Remove(marchUuid);
		}
	}

	public void DestroyTroopLine(long marchUuid)
	{
		littleSmart?.DestroyTroopLine(marchUuid);
		DestroyLegacyTroopLine(marchUuid);
	}

	public bool IsTroopLineCreate(long marchUuid)
	{
		if (littleSmart == null || !littleSmart.IsLittleSmartModeEnable)
		{
			return _troopLines.ContainsKey(marchUuid);
		}
		if (!littleSmart.IsTroopLineCreate(marchUuid))
		{
			return _troopLines.ContainsKey(marchUuid);
		}
		return true;
	}

	public bool IsTroopLineMarchOutOfData(WorldMarch march)
	{
		if (littleSmart != null && littleSmart.IsLittleSmartModeEnable)
		{
			return littleSmart.IsTroopLineMarchOutOfData(march);
		}
		if (_troopLines.TryGetValue(march.uuid, out var value))
		{
			return value.march != march;
		}
		return false;
	}

	[Obsolete]
	public void UpdateTroopLine(WorldMarch march, WorldTroopPathSegment[] path, int currPath, Vector3 currPos, int realTargetPos = 0, bool needRefresh = false, bool clear = false)
	{
		if (path != null && path.Length != 0)
		{
			_troopLines.TryGetValue(march.uuid, out var _);
		}
	}

	public void UpdateTroopLineNew(WorldMarch march, Vector3 currPos)
	{
		if (march.pathList != null && march.pathList.Length != 0 && _troopLines.TryGetValue(march.uuid, out var value))
		{
			value.SetCurPos(currPos);
			value.SetSpeed(march.speed);
		}
	}

	public void HideDestination(long uuid)
	{
	}

	private void OnWorldMarchUpdateDisplayModeChanged(object obj)
	{
		ShowTroopLineMask = 7;
		ShowTroopMidSprite = true;
		if (GameEntry.Lua != null)
		{
			ShowTroopLineMask = GameEntry.Lua.CallWithReturn<int>("CSharpCallLuaInterface.GetShowTroopLineMask");
			ShowTroopMidSprite = GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.GetShowTroopLineMidSprite");
			TroopLineWidthScale = GameEntry.Lua.CallWithReturn<float>("CSharpCallLuaInterface.GetTroopLineWidthScale");
			CurrentDisplayLevel = GameEntry.Lua.CallWithReturn<int>("CSharpCallLuaInterface.GetCurrentDisplayLevel");
			TroopCircleScale = GameEntry.Lua.CallWithReturn<float>("CSharpCallLuaInterface.GetTroopCircleScale");
			WorldTroopLineLittleSmart.Line.GlobalTroopLineScale = TroopLineWidthScale * 1.1f;
			WorldTroopLineLittleSmart.Line.GlobalTroopCircleScale = TroopCircleScale;
		}
		if (_troopLines == null)
		{
			return;
		}
		foreach (KeyValuePair<long, TroopLine> troopLine in _troopLines)
		{
			troopLine.Value.UpdateDisplayMode();
		}
	}
}
