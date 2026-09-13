using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using XLua;

public class ModelManager : CityManagerBase
{
	public enum ModelObjectType
	{
		Build = 1,
		Road = 2,
		Garbage = 3,
		Monster = 4,
		MonsterReward = 5,
		GarbageReward = 6,
		MonsterLock = 8,
		Collect = 9,
		FreeGarbage = 20
	}

	public abstract class ModelObject
	{
		public InstanceRequest instance;

		public List<InstanceRequest> oldInstances;

		public int pointIndex;

		public ModelObjectType modelObjectType;

		public bool isVisible;

		public ModelManager parent;

		public ModelObject(ModelManager modelManager, int index, ModelObjectType modelType)
		{
			parent = modelManager;
			pointIndex = index;
			modelObjectType = modelType;
			oldInstances = new List<InstanceRequest>();
		}

		protected void AddOldObject()
		{
			if (instance != null)
			{
				oldInstances.Add(instance);
			}
		}

		protected void ClearOldObject()
		{
			if (oldInstances == null)
			{
				return;
			}
			foreach (InstanceRequest oldInstance in oldInstances)
			{
				oldInstance?.Destroy();
			}
			oldInstances.Clear();
		}

		public virtual void Destroy()
		{
			ClearOldObject();
			oldInstances = null;
			if (instance != null)
			{
				instance.Destroy();
				instance = null;
			}
		}

		public abstract void UpdateLod(int lod);

		public abstract void UpdateFog(int fogId);

		public abstract void CreateGameObject();

		public abstract void UpdateGameObject(object param = null);

		public virtual void OnUpdate(float deltaTime)
		{
		}

		public abstract void DoGuideStartAnim(int time);

		public abstract void SetIsVisible(bool visible);

		public virtual void SetLabelActive(bool visible)
		{
		}
	}

	public class BuildObject : ModelObject
	{
		public CityBuilding cityBuilding;

		public LuaBuildData luaBuild;

		public string curBuildModelName;

		private bool _usePveReturnOpt;

		private bool _useBuildArrayOpt;

		public BuildObject(ModelManager parent, int pointId, ModelObjectType modelObjectType, LuaBuildData build)
			: base(parent, pointId, modelObjectType)
		{
			luaBuild = build;
			_usePveReturnOpt = parent.UsePveReturnOpt;
			_useBuildArrayOpt = parent.UseBuildArrayOpt;
		}

		public override void Destroy()
		{
			if (cityBuilding != null)
			{
				cityBuilding.CSUninit();
			}
			curBuildModelName = "";
			GameEntry.Event.Fire(EventId.BUILD_OUT_VIEW, luaBuild.uuid);
			base.Destroy();
		}

		public override void CreateGameObject()
		{
			string cityBuildName = string.Empty;
			if (_useBuildArrayOpt)
			{
				if (luaBuild.buildId == 10100000 || !SceneLuaArrayFacade.TryGetCityBuildIdName(luaBuild.buildId, luaBuild.level, out cityBuildName))
				{
					cityBuildName = GameEntry.Lua.CallWithReturn<string, int, int>("CSharpCallLuaInterface.GetCityBuildingModelName", luaBuild.buildId, luaBuild.level);
				}
			}
			else
			{
				cityBuildName = GameEntry.Lua.CallWithReturn<string, int, int>("CSharpCallLuaInterface.GetCityBuildingModelName", luaBuild.buildId, luaBuild.level);
			}
			if (cityBuildName.IsNullOrEmpty())
			{
				return;
			}
			string prefabPath = $"Assets/Main/Prefabs/Building/{cityBuildName}.prefab";
			if (cityBuildName.StartsWith("Assets/"))
			{
				prefabPath = cityBuildName;
			}
			curBuildModelName = cityBuildName;
			AddOldObject();
			instance = GameEntry.Resource.InstantiateAsync(prefabPath);
			instance.completed += delegate
			{
				ClearOldObject();
				GameObject gameObject = instance.gameObject;
				if (gameObject != null)
				{
					gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
					if (cityBuilding != null)
					{
						cityBuilding.CSUninit();
					}
					cityBuilding = gameObject.GetComponent<CityBuilding>();
					if (cityBuilding != null)
					{
						UpdateBuildingView();
					}
				}
			};
		}

		public override void UpdateGameObject(object param = null)
		{
			if (luaBuild == null)
			{
				return;
			}
			long uuid = luaBuild.uuid;
			LuaBuildData buildingDataByUuid = GameEntry.Data.Building.GetBuildingDataByUuid(uuid);
			luaBuild = buildingDataByUuid;
			string cityBuildName = string.Empty;
			if (_useBuildArrayOpt)
			{
				if (luaBuild.buildId == 10100000 || !SceneLuaArrayFacade.TryGetCityBuildIdName(luaBuild.buildId, luaBuild.level, out cityBuildName))
				{
					cityBuildName = GameEntry.Lua.CallWithReturn<string, int, int>("CSharpCallLuaInterface.GetCityBuildingModelName", luaBuild.buildId, luaBuild.level);
				}
			}
			else
			{
				cityBuildName = GameEntry.Lua.CallWithReturn<string, int, int>("CSharpCallLuaInterface.GetCityBuildingModelName", luaBuild.buildId, luaBuild.level);
			}
			if (cityBuildName.IsNullOrEmpty())
			{
				return;
			}
			if (curBuildModelName == cityBuildName)
			{
				if (cityBuilding != null)
				{
					UpdateBuildingView();
				}
			}
			else
			{
				CreateGameObject();
			}
		}

		private void UpdateBuildingView()
		{
			CityBuilding.Param param = new CityBuilding.Param();
			param.buildUuid = luaBuild.uuid;
			param.buildSceneType = CityBuilding.BuildSceneType.City;
			param.visible = isVisible;
			param.noDoAnim = parent.IsNoDoBuildAnim(param.buildUuid);
			param.point = pointIndex;
			cityBuilding.CSInit(param);
			if (GameEntry.Lua.CallWithReturn<int, int>("CSharpCallLuaInterface.GetResourceTypeByBuildId", luaBuild.buildId) != -1)
			{
				SceneManager.World.CreateAnimalObject(param.buildUuid);
			}
			GameEntry.Event.Fire(EventId.BUILD_IN_VIEW, luaBuild.uuid);
		}

		public override void UpdateLod(int lod)
		{
		}

		public override void UpdateFog(int fogId)
		{
		}

		public override void OnUpdate(float deltaTime)
		{
			if (cityBuilding != null)
			{
				cityBuilding.CSUpdate(deltaTime);
			}
		}

		public override void DoGuideStartAnim(int time)
		{
		}

		public override void SetIsVisible(bool visible)
		{
			isVisible = visible;
			if (cityBuilding != null)
			{
				cityBuilding.SetVisible(visible);
			}
		}
	}

	public class RoadObject : ModelObject
	{
		public static float PrintRoadLeft = 0.245f;

		public static float PrintRoadRight = 0.2625f;

		private GameObject gameObject;

		private MeshRenderer[] renderers;

		private BoardState state;

		private string _prefabName;

		private string _lightPrefabName;

		private InstanceRequest _lightInstance;

		public long uuid;

		private List<int> printId;

		private MaterialPropertyBlock _block;

		public RoadObject(ModelManager parent, int pointId, ModelObjectType modelObjectType)
			: base(parent, pointId, modelObjectType)
		{
			LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable, int>("CSharpCallLuaInterface.GetBoardDataByPointId", pointId);
			if (luaTable != null)
			{
				uuid = luaTable.Get<long>("uuid");
				state = (BoardState)luaTable.Get<int>("state");
			}
			_prefabName = "";
			_block = null;
			printId = new List<int>();
		}

		public override void Destroy()
		{
			DeleteMaterial();
			for (int i = 0; i < printId.Count; i++)
			{
				SceneManager.World.FinishPrintRoad(printId[i]);
			}
			if (_lightInstance != null)
			{
				_lightInstance.Destroy();
				_lightInstance = null;
			}
			base.Destroy();
			gameObject = null;
		}

		public override void CreateGameObject()
		{
			if (SceneManager.World == null)
			{
				return;
			}
			LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable, int>("CSharpCallLuaInterface.GetShowRoadDataByPointId", pointIndex);
			if (luaTable == null)
			{
				return;
			}
			string text = luaTable.Get<string>("prefabName");
			string text2 = luaTable.Get<string>("lightPrefabName");
			if (_prefabName != text)
			{
				_prefabName = text;
				if (!string.IsNullOrEmpty(text))
				{
					AddOldObject();
					instance = GameEntry.Resource.InstantiateAsync(text);
					instance.completed += delegate
					{
						ClearOldObject();
						gameObject = instance.gameObject;
						if (gameObject != null)
						{
							gameObject.SetActive(value: false);
							CheckLoadComplete();
						}
					};
				}
			}
			if (!(_lightPrefabName != text2))
			{
				return;
			}
			_lightPrefabName = text2;
			if (!string.IsNullOrEmpty(text2))
			{
				if (_lightInstance != null)
				{
					_lightInstance.Destroy();
					_lightInstance = null;
				}
				_lightInstance = GameEntry.Resource.InstantiateAsync(text2);
				_lightInstance.completed += delegate
				{
					_lightInstance.gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
					_lightInstance.gameObject.SetActive(value: true);
					_lightInstance.gameObject.transform.position = SceneManager.World.TileIndexToWorld(pointIndex);
				};
			}
			else if (_lightInstance != null)
			{
				_lightInstance.Destroy();
				_lightInstance = null;
			}
		}

		public override void UpdateGameObject(object param = null)
		{
			CreateGameObject();
		}

		public override void DoGuideStartAnim(int time)
		{
		}

		public override void SetIsVisible(bool visible)
		{
			isVisible = visible;
			if (gameObject != null)
			{
				gameObject.SetActive(visible);
			}
		}

		public override void UpdateLod(int lod)
		{
		}

		public override void UpdateFog(int fogId)
		{
		}

		public void StartPrint(int id)
		{
			printId.Add(id);
			state = BoardState.Updating;
			UpdateGameObject();
			UpdatePrintProgress(0f, new Vector4(1f, 0f, 1f, 0f));
		}

		public void UpdatePrintProgress(float progress, Vector4 dir)
		{
			if (_block == null)
			{
				_block = new MaterialPropertyBlock();
			}
			if (renderers != null)
			{
				float value = PrintRoadLeft + progress * (1f - PrintRoadLeft - PrintRoadRight);
				MeshRenderer[] array = renderers;
				foreach (MeshRenderer obj in array)
				{
					_block.SetFloat("_Progress", value);
					_block.SetVector("_Direction", dir);
					_block.SetVector("_WorldPivot", gameObject.transform.position);
					obj.SetPropertyBlock(_block);
				}
			}
		}

		public void FinishPrint()
		{
			state = BoardState.NORMAL;
			UpdateGameObject();
		}

		private void CheckLoadComplete()
		{
			if (gameObject != null)
			{
				gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
				gameObject.SetActive(value: true);
				gameObject.transform.position = SceneManager.World.TileIndexToWorld(pointIndex);
				renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
				if (state == BoardState.Updating)
				{
					UpdatePrintProgress(0f, new Vector4(1f, 0f, 1f, 0f));
				}
				gameObject.SetActive(isVisible);
			}
		}

		private void DeleteMaterial()
		{
			_block = null;
		}
	}

	public class LuaObject : ModelObject
	{
		private string luafpath = "CSharpCallLuaInterface.NewLuaObj";

		public LuaTable _luaTable;

		public LuaObject(ModelManager parent, int pointId, ModelObjectType modelObjectType)
			: base(parent, pointId, modelObjectType)
		{
			_luaTable = GameEntry.Lua.CallWithReturn<LuaTable, int, int>(luafpath, pointId, (int)modelObjectType);
		}

		public override void Destroy()
		{
			base.Destroy();
			CallLuaFunc("Destroy", _luaTable);
		}

		public override void UpdateLod(int lod)
		{
		}

		public override void UpdateFog(int fogId)
		{
		}

		public override void CreateGameObject()
		{
			CallLuaFunc("CreateGameObject", _luaTable);
		}

		public void OnCutOnce()
		{
			CallLuaFunc("OnCutOnce", _luaTable);
		}

		public void OnResetRes()
		{
			CallLuaFunc("OnResetRes", _luaTable);
		}

		public override void OnUpdate(float deltaTime)
		{
		}

		public void ShowFlyResAnim()
		{
			CallLuaFunc("ShowFlyResAnim", _luaTable);
		}

		public void ShowFlyBox()
		{
			CallLuaFunc("ShowFlyBox", _luaTable);
		}

		private void CallLuaFunc<T, T1, T2>(string funcname, T t, T1 t1, T2 t2)
		{
			_luaTable.Get<LuaFunction>(funcname).Call(t, t1, t2);
		}

		private void CallLuaFunc<T, T1>(string funcname, T t, T1 t1)
		{
			_luaTable.Get<LuaFunction>(funcname).Call(t, t1);
		}

		private void CallLuaFunc<T>(string funcname, T t)
		{
			_luaTable.Get<LuaFunction>(funcname).Call(t);
		}

		private void CallLuaFunc(string funcname)
		{
			_luaTable.Get<LuaFunction>(funcname).Call();
		}

		public override void UpdateGameObject(object param = null)
		{
		}

		public override void DoGuideStartAnim(int time)
		{
		}

		public override void SetIsVisible(bool visible)
		{
		}
	}

	public class GarbageObject : ModelObject
	{
		private GameObject _go;

		private bool isDisappearPlayEnd;

		public bool isDoingDisappear { get; private set; }

		public GarbageObject(ModelManager parent, int pointId, ModelObjectType modelObjectType)
			: base(parent, pointId, modelObjectType)
		{
			_go = null;
			isDisappearPlayEnd = false;
			isDoingDisappear = false;
		}

		public override void Destroy()
		{
			_go = null;
			base.Destroy();
		}

		public override void CreateGameObject()
		{
			LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable, int>("CSharpCallLuaInterface.GetCityPointDataByPointId", pointIndex);
			if (luaTable == null)
			{
				return;
			}
			string str = luaTable.Get<string>("itemId");
			string templateData = GameEntry.ConfigCache.GetTemplateData("aps_singlemap_junk", str.ToInt(), "Show");
			if (templateData == null)
			{
				return;
			}
			AddOldObject();
			instance = GameEntry.Resource.InstantiateAsync($"Assets/Main/Prefabs/Garbage/{templateData}.prefab");
			instance.completed += delegate
			{
				ClearOldObject();
				GameObject gameObject = instance.gameObject;
				if (gameObject != null)
				{
					_go = gameObject;
					gameObject.name = "Garbage_" + pointIndex;
					gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
					gameObject.transform.position = SceneManager.World.TileIndexToWorld(pointIndex);
					gameObject.transform.localScale = Vector3.one;
					gameObject.SetActive(isVisible);
				}
			};
		}

		public bool DoDisappear()
		{
			if (isDoingDisappear)
			{
				return true;
			}
			isDoingDisappear = true;
			if (_go != null)
			{
				Sequence sequence = DOTween.Sequence();
				sequence.Append(_go.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f));
				sequence.Append(_go.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.3f));
				sequence.onComplete = delegate
				{
					isDisappearPlayEnd = true;
					isDoingDisappear = false;
					GameEntry.Event.Fire(EventId.UpdateCityPoint, pointIndex);
				};
				return true;
			}
			return false;
		}

		public bool NeedDestroyWhenDisappearEnd()
		{
			return isDisappearPlayEnd;
		}

		public override void UpdateGameObject(object param = null)
		{
		}

		public override void UpdateLod(int lod)
		{
		}

		public override void UpdateFog(int fogId)
		{
		}

		public override void OnUpdate(float deltaTime)
		{
		}

		public override void DoGuideStartAnim(int time)
		{
		}

		public override void SetIsVisible(bool visible)
		{
			isVisible = visible;
			if (_go != null)
			{
				_go.SetActive(visible);
			}
		}

		public GameObject GetObject()
		{
			return _go;
		}
	}

	public class GarbageRewardObject : ModelObject
	{
		private GameObject _go;

		private bool isDisappearPlayEnd;

		public bool isDoingDisappear { get; private set; }

		public GarbageRewardObject(ModelManager parent, int pointId, ModelObjectType modelObjectType)
			: base(parent, pointId, modelObjectType)
		{
			_go = null;
			isDisappearPlayEnd = false;
			isDoingDisappear = false;
		}

		public override void Destroy()
		{
			_go = null;
			base.Destroy();
		}

		public override void CreateGameObject()
		{
			LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable, int>("CSharpCallLuaInterface.GetCityPointDataByPointId", pointIndex);
			if (luaTable == null)
			{
				return;
			}
			string str = luaTable.Get<string>("itemId");
			string templateData = GameEntry.ConfigCache.GetTemplateData("aps_singlemap_junk", str.ToInt(), "Show");
			if (templateData == null)
			{
				return;
			}
			AddOldObject();
			instance = GameEntry.Resource.InstantiateAsync($"Assets/Main/Prefabs/Garbage/{templateData}.prefab");
			instance.completed += delegate
			{
				ClearOldObject();
				GameObject gameObject = instance.gameObject;
				if (gameObject != null)
				{
					_go = gameObject;
					gameObject.name = "GarbageReward_" + pointIndex;
					gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
					gameObject.transform.position = SceneManager.World.TileIndexToWorld(pointIndex);
					gameObject.transform.localScale = Vector3.one;
					gameObject.SetActive(isVisible);
				}
			};
		}

		public bool DoDisappear()
		{
			if (isDoingDisappear)
			{
				return true;
			}
			isDoingDisappear = true;
			if (_go != null)
			{
				Sequence sequence = DOTween.Sequence();
				sequence.Append(_go.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f));
				sequence.Append(_go.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.3f));
				sequence.onComplete = delegate
				{
					isDisappearPlayEnd = true;
					isDoingDisappear = false;
					GameEntry.Event.Fire(EventId.UpdateCityPoint, pointIndex);
				};
				return true;
			}
			return false;
		}

		public bool NeedDestroyWhenDisappearEnd()
		{
			return isDisappearPlayEnd;
		}

		public override void UpdateGameObject(object param = null)
		{
		}

		public override void UpdateLod(int lod)
		{
		}

		public override void UpdateFog(int fogId)
		{
		}

		public override void OnUpdate(float deltaTime)
		{
		}

		public override void DoGuideStartAnim(int time)
		{
		}

		public override void SetIsVisible(bool visible)
		{
			isVisible = visible;
			if (_go != null)
			{
				_go.SetActive(visible);
			}
		}

		public GameObject GetObject()
		{
			return _go;
		}
	}

	public class MonsterRewardObject : ModelObject
	{
		private GameObject _go;

		public MonsterRewardObject(ModelManager parent, int pointId, ModelObjectType modelObjectType)
			: base(parent, pointId, modelObjectType)
		{
			_go = null;
		}

		public override void Destroy()
		{
			_go = null;
			base.Destroy();
		}

		public override void CreateGameObject()
		{
			if (GameEntry.Lua.CallWithReturn<LuaTable, int>("CSharpCallLuaInterface.GetCityPointDataByPointId", pointIndex) == null)
			{
				return;
			}
			AddOldObject();
			instance = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/World/MonsterReward.prefab");
			instance.completed += delegate
			{
				ClearOldObject();
				GameObject gameObject = instance.gameObject;
				if (gameObject != null)
				{
					_go = gameObject;
					gameObject.name = "Garbage_" + pointIndex;
					gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
					gameObject.transform.position = SceneManager.World.TileIndexToWorld(pointIndex);
					gameObject.transform.localScale = Vector3.one;
					gameObject.SetActive(isVisible);
				}
			};
		}

		public override void UpdateGameObject(object param = null)
		{
		}

		public override void UpdateLod(int lod)
		{
		}

		public override void UpdateFog(int fogId)
		{
		}

		public override void OnUpdate(float deltaTime)
		{
		}

		public override void DoGuideStartAnim(int time)
		{
		}

		public override void SetIsVisible(bool visible)
		{
			isVisible = visible;
			if (_go != null)
			{
				_go.SetActive(visible);
			}
		}

		public GameObject GetObject()
		{
			return _go;
		}
	}

	public class MonsterObject : ModelObject
	{
		private GameObject _go;

		private UIWorldLabel _label;

		public MonsterObject(ModelManager parent, int pointId, ModelObjectType modelObjectType)
			: base(parent, pointId, modelObjectType)
		{
			_go = null;
			_label = null;
		}

		public override void Destroy()
		{
			_go = null;
			_label = null;
			base.Destroy();
		}

		public override void CreateGameObject()
		{
			LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable, int>("CSharpCallLuaInterface.GetCityPointDataByPointId", pointIndex);
			if (luaTable == null)
			{
				return;
			}
			string itemId = luaTable.Get<string>("itemId");
			string templateData = GameEntry.ConfigCache.GetTemplateData("lw_world_monster", itemId.ToInt(), "model_name");
			if (templateData == null)
			{
				return;
			}
			AddOldObject();
			instance = GameEntry.Resource.InstantiateAsync($"Assets/Main/Prefabs/Monsters/{templateData}.prefab");
			instance.completed += delegate
			{
				ClearOldObject();
				GameObject gameObject = instance.gameObject;
				if (gameObject != null)
				{
					_go = gameObject;
					gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
					gameObject.transform.position = SceneManager.World.TileIndexToWorld(pointIndex);
					gameObject.transform.localScale = Vector3.one;
					gameObject.SetActive(isVisible);
					_label = _go.GetComponentInChildren<UIWorldLabel>();
					if (_label != null)
					{
						string templateData2 = GameEntry.ConfigCache.GetTemplateData("lw_world_monster", itemId.ToInt(), "level");
						_label.SetLevel(templateData2.ToInt());
						SetLabelActive(visible: true);
					}
				}
			};
		}

		public override void UpdateGameObject(object param = null)
		{
		}

		public override void UpdateLod(int lod)
		{
		}

		public override void UpdateFog(int fogId)
		{
			DCFog fog = GameEntry.Data.Fog;
			bool flag = fog != null && !fog.IsUnlock(pointIndex);
			_label.gameObject.SetActive(!flag);
		}

		public override void OnUpdate(float deltaTime)
		{
		}

		public override void DoGuideStartAnim(int time)
		{
		}

		public override void SetIsVisible(bool visible)
		{
			isVisible = visible;
			if (_go != null)
			{
				_go.SetActive(visible);
			}
			if (visible)
			{
				SetLabelActive(visible: true);
			}
		}

		public GameObject GetObject()
		{
			return _go;
		}

		public override void SetLabelActive(bool visible)
		{
			DCFog fog = GameEntry.Data.Fog;
			if (fog != null && !fog.IsUnlock(pointIndex))
			{
				visible = false;
			}
			base.SetLabelActive(visible);
			if (_label != null)
			{
				_label.gameObject.SetActive(visible);
			}
		}
	}

	public class MonsterLockObject : ModelObject
	{
		private int monsterId;

		private ITimer timer;

		public MonsterLockObject(ModelManager parent, int index, ModelObjectType modelType, int monsterId)
			: base(parent, index, modelType)
		{
			this.monsterId = monsterId;
		}

		public override void UpdateLod(int lod)
		{
		}

		public override void UpdateFog(int fogId)
		{
		}

		public override void CreateGameObject()
		{
			string templateData = GameEntry.ConfigCache.GetTemplateData("aps_monsterlock", monsterId, "model");
			instance = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/Monsters/" + templateData + ".prefab");
			instance.completed += delegate
			{
				if (!(instance.gameObject == null))
				{
					GameObject gameObject = instance.gameObject;
					Transform transform = gameObject.transform;
					gameObject.SetActive(value: true);
					transform.SetParent(SceneManager.World.DynamicObjNode);
					Transform transform2 = gameObject.transform.Find("ModelLabel");
					if (transform2 != null)
					{
						transform2.gameObject.SetActive(value: false);
					}
					int index = pointIndex;
					Vector2Int vector2Int = SceneManager.World.IndexToTilePos(index);
					Vector3 vector = SceneManager.World.TileFloatToWorld(vector2Int);
					transform.localPosition = vector + new Vector3(0f, 0f, 0f);
					transform.localScale = Vector3.one;
					transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
					TouchObjectEventTrigger componentInChildren = gameObject.GetComponentInChildren<TouchObjectEventTrigger>();
					if (componentInChildren != null)
					{
						componentInChildren.onPointerClick = delegate
						{
							GameEntry.Lua.Call("CSharpCallLuaInterface.ClickMonsterLockById", monsterId);
						};
					}
					GameEntry.Event.Fire(EventId.MonsterLockInView, monsterId);
				}
			};
		}

		public override void UpdateGameObject(object param = null)
		{
		}

		public override void Destroy()
		{
			GameEntry.Event.Fire(EventId.MonsterLockOutView, monsterId);
			base.Destroy();
		}

		public void FadeOut(Action callback = null)
		{
			GameEntry.Event.Fire(EventId.MonsterLockOutView, monsterId);
			instance.gameObject.SetActive(value: false);
			InstanceRequest req = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/World/LandLockFadeOut.prefab");
			req.completed += delegate
			{
				req.gameObject.transform.position = instance.gameObject.transform.position;
				timer = GameEntry.Timer.RegisterTimer(2f, delegate
				{
					req.Destroy();
					GameEntry.Timer.CancelTimer(timer);
					callback?.Invoke();
				});
			};
		}

		public override void DoGuideStartAnim(int time)
		{
		}

		public override void SetIsVisible(bool visible)
		{
			isVisible = visible;
			if (instance != null && instance.gameObject != null)
			{
				instance.gameObject.SetActive(visible);
			}
		}
	}

	public class CollectObject : ModelObject
	{
		private GameObject _go;

		private LuaTable _param;

		public CollectObject(ModelManager parent, int pointId, ModelObjectType modelObjectType, LuaTable param)
			: base(parent, pointId, modelObjectType)
		{
			_go = null;
			_param = param;
		}

		public override void Destroy()
		{
			_go = null;
			base.Destroy();
		}

		public override void CreateGameObject()
		{
			AddOldObject();
			string prefabPath = _param.Get<string>("modelName");
			instance = GameEntry.Resource.InstantiateAsync(prefabPath);
			instance.completed += delegate
			{
				ClearOldObject();
				GameObject gameObject = instance.gameObject;
				if (gameObject != null)
				{
					_go = gameObject;
					gameObject.name = "Collect_" + pointIndex;
					gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
					gameObject.transform.position = SceneManager.World.TileIndexToWorld(pointIndex);
					gameObject.transform.localScale = Vector3.one;
					gameObject.SetActive(isVisible);
				}
			};
		}

		public override void UpdateGameObject(object param = null)
		{
		}

		public override void UpdateLod(int lod)
		{
		}

		public override void UpdateFog(int fogId)
		{
		}

		public override void OnUpdate(float deltaTime)
		{
		}

		public override void DoGuideStartAnim(int time)
		{
		}

		public override void SetIsVisible(bool visible)
		{
			isVisible = visible;
			if (_go != null)
			{
				_go.SetActive(visible);
			}
		}

		public GameObject GetObject()
		{
			return _go;
		}
	}

	private static readonly Vector2Int ObjSize = new Vector2Int(2, 2);

	private bool _usePveReturnOpt;

	private bool _useBuildArrayOpt;

	private Dictionary<int, ModelObject> _modelObjects;

	private InstanceRequest _cityTroopInstance;

	private CityTroop _cityTroop;

	private long formationUuid;

	private CitySpaceMan _citySpaceMan;

	private HashSet<long> _noDoAnimBuild;

	public bool UsePveReturnOpt => _usePveReturnOpt;

	public bool UseBuildArrayOpt => _useBuildArrayOpt;

	public ModelManager(CityScene scene)
		: base(scene)
	{
		_modelObjects = new Dictionary<int, ModelObject>();
		_noDoAnimBuild = new HashSet<long>();
	}

	public override void Init()
	{
		base.Init();
		GameEntry.Event.Subscribe(EventId.CreateFormationUuid, CreateFormationUuidSignal);
		GameEntry.Event.Subscribe(EventId.UPDATE_BUILD_DATA, UpdateBuildDataSignal);
		GameEntry.Event.Subscribe(EventId.UserCitySkinUpdate, UpdateBuildDataSignal);
		GameEntry.Event.Subscribe(EventId.ShowAllGuideObject, ShowAllGuideObjectSignal);
		GameEntry.Event.Subscribe(EventId.FarmGuideFakePlantShowState, FarmGuideFakePlantShowStateSignal);
		GameEntry.Event.Subscribe(EventId.OpenFogSuccess, OpenFogSuccessSignal);
		GameEntry.Event.Subscribe(EventId.UpdateCityPoint, UpdateCityPointSignal);
		GameEntry.Event.Subscribe(EventId.MonsterLockStateUpdate, MonsterLockStateUpdateSignal);
		GameEntry.Event.Subscribe(EventId.RefreshCityRoadArr, RefreshCityRoadArrSignal);
		GameEntry.Event.Subscribe(EventId.DeleteCityRoadArr, DeleteCityRoadArrSignal);
		GameEntry.Event.Subscribe(EventId.SetBuildCanDoAnim, SetBuildCanDoAnimSignal);
		GameEntry.Event.Subscribe(EventId.SetBuildNoDoAnim, SetBuildNoDoAnimSignal);
		GameEntry.Event.Subscribe(EventId.UpdateActivityAlarmClockBuildingTimeShow, OnUpdateActivityAlarmClockBuildingTimeShow);
		InitLoadModel();
	}

	public override void UnInit()
	{
		base.UnInit();
		GameEntry.Event.Unsubscribe(EventId.CreateFormationUuid, CreateFormationUuidSignal);
		GameEntry.Event.Unsubscribe(EventId.UPDATE_BUILD_DATA, UpdateBuildDataSignal);
		GameEntry.Event.Unsubscribe(EventId.UserCitySkinUpdate, UpdateBuildDataSignal);
		GameEntry.Event.Unsubscribe(EventId.ShowAllGuideObject, ShowAllGuideObjectSignal);
		GameEntry.Event.Unsubscribe(EventId.FarmGuideFakePlantShowState, FarmGuideFakePlantShowStateSignal);
		GameEntry.Event.Unsubscribe(EventId.OpenFogSuccess, OpenFogSuccessSignal);
		GameEntry.Event.Unsubscribe(EventId.UpdateCityPoint, UpdateCityPointSignal);
		GameEntry.Event.Unsubscribe(EventId.MonsterLockStateUpdate, MonsterLockStateUpdateSignal);
		GameEntry.Event.Unsubscribe(EventId.RefreshCityRoadArr, RefreshCityRoadArrSignal);
		GameEntry.Event.Unsubscribe(EventId.DeleteCityRoadArr, DeleteCityRoadArrSignal);
		GameEntry.Event.Unsubscribe(EventId.SetBuildCanDoAnim, SetBuildCanDoAnimSignal);
		GameEntry.Event.Unsubscribe(EventId.SetBuildNoDoAnim, SetBuildNoDoAnimSignal);
		GameEntry.Event.Unsubscribe(EventId.UpdateActivityAlarmClockBuildingTimeShow, OnUpdateActivityAlarmClockBuildingTimeShow);
		ClearReInitObject();
		if (_cityTroop != null)
		{
			_cityTroop.UnInit();
			_cityTroop = null;
		}
		if (_cityTroopInstance != null)
		{
			_cityTroopInstance.Destroy();
			_cityTroopInstance = null;
		}
		_noDoAnimBuild.Clear();
		DestroyCitySpaceMan();
	}

	public override void OnUpdate(float deltaTime)
	{
		_citySpaceMan?.OnUpdate();
		foreach (KeyValuePair<int, ModelObject> modelObject in _modelObjects)
		{
			modelObject.Value.OnUpdate(deltaTime);
		}
	}

	public void InitLoadModel()
	{
		_usePveReturnOpt = GameEntry.Data?.Player?.CheckImmediateSwitch(6, defaultVal: false) ?? false;
		_useBuildArrayOpt = GameEntry.Data?.Player?.CheckImmediateSwitch(8, defaultVal: false) ?? false;
		ReInitObject();
		int initCityTroopPosition = GetInitCityTroopPosition();
		if (initCityTroopPosition >= 0)
		{
			LoadCityTroop(initCityTroopPosition);
		}
	}

	private void CreateFormationUuidSignal(object userData)
	{
		long num = (long)userData;
		formationUuid = num;
	}

	public long GetFormationUuid()
	{
		return formationUuid;
	}

	public ModelObject LoadOneObject(int index, int modelObjectType, object param = null)
	{
		if (_modelObjects.ContainsKey(index))
		{
			if (_modelObjects[index].modelObjectType == (ModelObjectType)modelObjectType)
			{
				_modelObjects[index].UpdateGameObject(param);
				return _modelObjects[index];
			}
			RemoveOneObjectByPointType(index, (int)_modelObjects[index].modelObjectType);
		}
		ModelObject modelObject = null;
		switch (modelObjectType)
		{
		case 1:
		{
			LuaBuildData luaBuildData = (LuaBuildData)param;
			if (luaBuildData != null)
			{
				int buildId = luaBuildData.buildId;
				if (buildId != 792000 && buildId != 735000)
				{
					modelObject = new BuildObject(this, index, ModelObjectType.Build, luaBuildData);
				}
			}
			break;
		}
		case 2:
		{
			Vector2Int mainPos = GameEntry.Data.Building.GetMainPos();
			Vector2Int vector2Int = SceneManager.World.IndexToTilePos(index);
			if (vector2Int.x != mainPos.x && vector2Int.y != mainPos.y)
			{
				modelObject = new RoadObject(this, index, ModelObjectType.Road);
			}
			break;
		}
		case 3:
			modelObject = new GarbageObject(this, index, ModelObjectType.Garbage);
			break;
		case 20:
			modelObject = new LuaObject(this, index, ModelObjectType.FreeGarbage);
			break;
		case 4:
			modelObject = new MonsterObject(this, index, ModelObjectType.Monster);
			break;
		case 8:
			modelObject = new MonsterLockObject(this, index, ModelObjectType.MonsterLock, (int)param);
			break;
		case 5:
			modelObject = new MonsterRewardObject(this, index, ModelObjectType.MonsterReward);
			break;
		case 6:
			if (!GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.IsBeforePrologue"))
			{
				modelObject = new GarbageRewardObject(this, index, ModelObjectType.GarbageReward);
			}
			break;
		case 9:
			modelObject = new CollectObject(this, index, ModelObjectType.Collect, (LuaTable)param);
			break;
		}
		if (modelObject != null)
		{
			_modelObjects.Add(index, modelObject);
			modelObject.isVisible = IsCanShowBuild();
			modelObject.CreateGameObject();
			scene.AddOccupyPoints(scene.IndexToTilePos(index), ObjSize);
		}
		return modelObject;
	}

	public void RemoveOneObject(int index)
	{
		if (_modelObjects.ContainsKey(index))
		{
			_modelObjects[index].Destroy();
			_modelObjects.Remove(index);
			scene.RemoveOccupyPoints(scene.IndexToTilePos(index), ObjSize);
		}
	}

	public void RemoveOneObjectByPointType(int index, int pointType)
	{
		if (_modelObjects.ContainsKey(index))
		{
			if (_modelObjects[index].modelObjectType == (ModelObjectType)pointType)
			{
				_modelObjects[index].Destroy();
				_modelObjects.Remove(index);
			}
			scene.RemoveOccupyPoints(scene.IndexToTilePos(index), ObjSize);
		}
	}

	private void UpdateBuildDataSignal(object userData)
	{
		long num = (long)userData;
		int num2 = 0;
		LuaBuildData buildingDataByUuid = GameEntry.Data.Building.GetBuildingDataByUuid(num);
		if (buildingDataByUuid != null && buildingDataByUuid.buildId > 0 && SceneManager.IsInCity() && GameEntry.Lua.CallWithReturn<int, string>("CSharpCallLuaInterface.GetInt", "CheckSeasonBuild," + buildingDataByUuid.buildId) == 1)
		{
			return;
		}
		foreach (KeyValuePair<int, ModelObject> modelObject in _modelObjects)
		{
			if (modelObject.Value.modelObjectType == ModelObjectType.Build && modelObject.Value is BuildObject buildObject && buildObject.luaBuild.uuid == num)
			{
				num2 = buildObject.pointIndex;
				break;
			}
		}
		if (buildingDataByUuid != null && buildingDataByUuid.state != 2)
		{
			if (num2 == buildingDataByUuid.pointId || num2 == 0)
			{
				LoadOneObject(buildingDataByUuid.pointId, 1, buildingDataByUuid);
			}
			else
			{
				ChangePointId(num2, buildingDataByUuid.pointId);
			}
		}
		else if (num2 != 0)
		{
			RemoveOneObject(num2);
		}
	}

	public ModelObject GetObjectByPointId(int index)
	{
		if (_modelObjects.ContainsKey(index))
		{
			return _modelObjects[index];
		}
		return null;
	}

	private void RefreshCityRoadArrSignal(object userData)
	{
		if (_usePveReturnOpt)
		{
			return;
		}
		LuaTable luaTable = (LuaTable)userData;
		if (luaTable == null)
		{
			return;
		}
		int num = 0;
		for (int i = 1; i <= luaTable.Length; i++)
		{
			LuaTable luaTable2 = (LuaTable)luaTable[i];
			if (luaTable2 != null && luaTable2.ContainsKey("pointId"))
			{
				num = luaTable2.Get<int>("pointId");
				ModelObject objectByPointId = GetObjectByPointId(num);
				if (objectByPointId != null)
				{
					objectByPointId.UpdateGameObject();
				}
				else
				{
					LoadOneObject(num, 2);
				}
			}
		}
	}

	private void DeleteCityRoadArrSignal(object userData)
	{
		LuaTable luaTable = (LuaTable)userData;
		if (luaTable != null)
		{
			for (int i = 1; i <= luaTable.Length; i++)
			{
				RemoveOneObject(Convert.ToInt32(luaTable[i]));
			}
		}
	}

	private void ChangePointId(int oldId, int newId)
	{
		if (_modelObjects.ContainsKey(oldId))
		{
			ModelObject modelObject = _modelObjects[oldId];
			modelObject.pointIndex = newId;
			_modelObjects.Remove(oldId);
			scene.RemoveOccupyPoints(scene.IndexToTilePos(oldId), ObjSize);
			if (!_modelObjects.ContainsKey(newId))
			{
				_modelObjects.Add(newId, modelObject);
				scene.AddOccupyPoints(scene.IndexToTilePos(newId), ObjSize);
			}
			modelObject.UpdateGameObject();
		}
	}

	public void ClearReInitObject()
	{
		GameEntry.Event.Fire(EventId.HideCityDome);
		GameEntry.Event.Fire(EventId.HideCityZone);
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, ModelObject> modelObject in _modelObjects)
		{
			if (modelObject.Value.modelObjectType != ModelObjectType.FreeGarbage)
			{
				modelObject.Value.Destroy();
				list.Add(modelObject.Key);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			_modelObjects.Remove(list[i]);
		}
		list = null;
	}

	public void ReInitObject()
	{
		GameEntry.Event.Fire(EventId.ShowCityDome);
		GameEntry.Event.Fire(EventId.ShowCityZone);
		List<LuaBuildData> param = new List<LuaBuildData>();
		if (_useBuildArrayOpt)
		{
			if (!GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.TryGetAllLuaBuildWithoutFoldUp"))
			{
				List<LuaBuildData> list = GameEntry.Lua.CallWithReturn<List<LuaBuildData>, List<LuaBuildData>>("CSharpCallLuaInterface.GetAllLuaBuildWithoutFoldUp", param);
				if (list != null)
				{
					foreach (LuaBuildData item in list)
					{
						LoadOneObject(item.pointId, 1, item);
					}
				}
			}
			else
			{
				long longLuaArrayValue = SceneLuaArrayFacade.GetLongLuaArrayValue(1);
				if (longLuaArrayValue > 0)
				{
					for (int i = 0; i < longLuaArrayValue; i++)
					{
						LuaBuildData luaBuildData = new LuaBuildData();
						luaBuildData.uuid = SceneLuaArrayFacade.GetLongLuaArrayValue(1 + i * 6 + 1);
						luaBuildData.buildUpdateTime = SceneLuaArrayFacade.GetLongLuaArrayValue(1 + i * 6 + 2);
						luaBuildData.pointId = (int)SceneLuaArrayFacade.GetLongLuaArrayValue(1 + i * 6 + 3);
						luaBuildData.state = (int)SceneLuaArrayFacade.GetLongLuaArrayValue(1 + i * 6 + 4);
						luaBuildData.buildId = (int)SceneLuaArrayFacade.GetLongLuaArrayValue(1 + i * 6 + 5);
						luaBuildData.level = (int)SceneLuaArrayFacade.GetLongLuaArrayValue(1 + i * 6 + 6);
						LoadOneObject(luaBuildData.pointId, 1, luaBuildData);
					}
				}
			}
		}
		else
		{
			List<LuaBuildData> list2 = GameEntry.Lua.CallWithReturn<List<LuaBuildData>, List<LuaBuildData>>("CSharpCallLuaInterface.GetAllLuaBuildWithoutFoldUp", param);
			if (list2 != null)
			{
				foreach (LuaBuildData item2 in list2)
				{
					LoadOneObject(item2.pointId, 1, item2);
				}
			}
		}
		if (!_usePveReturnOpt)
		{
			LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetAllShowRoadData");
			if (luaTable != null)
			{
				for (int j = 1; j <= luaTable.Length; j++)
				{
					LuaTable luaTable2 = (LuaTable)luaTable[j];
					if (luaTable2 != null && luaTable2.ContainsKey("pointId"))
					{
						int index = luaTable2.Get<int>("pointId");
						LoadOneObject(index, 2);
					}
				}
			}
		}
		GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetAllCityPointData")?.ForEach(delegate(long uuid, LuaTable cityPointData)
		{
			int index3 = cityPointData.Get<int>("pointId");
			switch (cityPointData.Get<int>("type"))
			{
			case 4:
				LoadOneObject(index3, 6);
				break;
			case 1:
				LoadOneObject(index3, 3);
				break;
			case 2:
				LoadOneObject(index3, 4);
				break;
			case 3:
				LoadOneObject(index3, 5);
				break;
			}
		});
		SceneManager.World.InitFogOfWar(GameEntry.Data.Fog.GetAllFogData());
		GameEntry.Lua.Call("DataCenter.CanUnlockFogManager:ShowAllEffect");
		MonsterLockStateUpdateSignal(null);
		LuaTable luaTable3 = GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetShowObjectModelParam");
		if (luaTable3 == null)
		{
			return;
		}
		for (int num = 1; num <= luaTable3.Length; num++)
		{
			LuaTable luaTable4 = (LuaTable)luaTable3[num];
			if (luaTable4 != null && luaTable4.ContainsKey("pointId"))
			{
				int index2 = luaTable4.Get<int>("pointId");
				LoadOneObject(index2, 9, luaTable4);
			}
		}
	}

	public void ClearReInitObjectByFilter()
	{
		GameEntry.Event.Fire(EventId.HideCityDome);
		GameEntry.Event.Fire(EventId.RefreshCityZone);
		List<int> list = new List<int>();
		List<LuaBuildData> list2 = null;
		if (_useBuildArrayOpt)
		{
			if (!GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.TryGetAllLuaBuildWithoutFoldUp"))
			{
				List<LuaBuildData> param = new List<LuaBuildData>();
				list2 = GameEntry.Lua.CallWithReturn<List<LuaBuildData>, List<LuaBuildData>>("CSharpCallLuaInterface.GetAllLuaBuildWithoutFoldUp", param);
			}
			else
			{
				long longLuaArrayValue = SceneLuaArrayFacade.GetLongLuaArrayValue(1);
				if (longLuaArrayValue > 0)
				{
					list2 = new List<LuaBuildData>();
					for (int i = 0; i < longLuaArrayValue; i++)
					{
						LuaBuildData luaBuildData = new LuaBuildData();
						luaBuildData.uuid = SceneLuaArrayFacade.GetLongLuaArrayValue(1 + i * 6 + 1);
						luaBuildData.buildUpdateTime = SceneLuaArrayFacade.GetLongLuaArrayValue(1 + i * 6 + 2);
						luaBuildData.pointId = (int)SceneLuaArrayFacade.GetLongLuaArrayValue(1 + i * 6 + 3);
						luaBuildData.state = (int)SceneLuaArrayFacade.GetLongLuaArrayValue(1 + i * 6 + 4);
						luaBuildData.buildId = (int)SceneLuaArrayFacade.GetLongLuaArrayValue(1 + i * 6 + 5);
						luaBuildData.level = (int)SceneLuaArrayFacade.GetLongLuaArrayValue(1 + i * 6 + 6);
						list2.Add(luaBuildData);
					}
				}
			}
		}
		else
		{
			List<LuaBuildData> param2 = new List<LuaBuildData>();
			list2 = GameEntry.Lua.CallWithReturn<List<LuaBuildData>, List<LuaBuildData>>("CSharpCallLuaInterface.GetAllLuaBuildWithoutFoldUp", param2);
		}
		LuaTable luaTable = null;
		if (!_usePveReturnOpt)
		{
			luaTable = GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetAllShowRoadData");
		}
		LuaTable luaTable2 = GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetAllCityPointData");
		List<int> cityPointList = new List<int>();
		luaTable2?.ForEach(delegate(long uuid, LuaTable cityPointData)
		{
			int item = cityPointData.Get<int>("pointId");
			cityPointList.Add(item);
		});
		LuaTable luaTable3 = GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetShowObjectModelParam");
		LuaTable luaTable4 = GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetMonsterLockDataList");
		List<int> monsterLockPointList = new List<int>();
		luaTable4?.ForEach(delegate(int _, LuaTable data)
		{
			int item = data.Get<int>("pointId");
			monsterLockPointList.Add(item);
		});
		foreach (KeyValuePair<int, ModelObject> modelObject in _modelObjects)
		{
			if (modelObject.Value.modelObjectType == ModelObjectType.FreeGarbage)
			{
				continue;
			}
			bool flag = false;
			if (modelObject.Value.modelObjectType == ModelObjectType.Build)
			{
				if (modelObject.Value is BuildObject buildObject && list2 != null && list2.Count > 0)
				{
					foreach (LuaBuildData item2 in list2)
					{
						if (item2.uuid == buildObject.luaBuild.uuid)
						{
							flag = true;
							break;
						}
					}
				}
			}
			else if (!_usePveReturnOpt && modelObject.Value.modelObjectType == ModelObjectType.Road)
			{
				if (luaTable != null && luaTable.Length > 0)
				{
					for (int num = 1; num <= luaTable.Length; num++)
					{
						LuaTable luaTable5 = (LuaTable)luaTable[num];
						if (luaTable5 != null && luaTable5.ContainsKey("pointId"))
						{
							int num2 = luaTable5.Get<int>("pointId");
							if (modelObject.Value.pointIndex == num2)
							{
								flag = true;
								break;
							}
						}
					}
				}
			}
			else if (modelObject.Value.modelObjectType == ModelObjectType.Collect)
			{
				if (luaTable3 != null && luaTable3.Length > 0)
				{
					for (int num3 = 1; num3 <= luaTable3.Length; num3++)
					{
						LuaTable luaTable6 = (LuaTable)luaTable3[num3];
						if (luaTable6 != null && luaTable6.ContainsKey("pointId") && luaTable6.Get<int>("pointId") == modelObject.Value.pointIndex)
						{
							flag = true;
							break;
						}
					}
				}
			}
			else if (modelObject.Value.modelObjectType == ModelObjectType.MonsterLock)
			{
				if (monsterLockPointList.Contains(modelObject.Value.pointIndex))
				{
					flag = true;
				}
			}
			else if ((modelObject.Value.modelObjectType == ModelObjectType.Garbage || modelObject.Value.modelObjectType == ModelObjectType.GarbageReward || modelObject.Value.modelObjectType == ModelObjectType.Monster || modelObject.Value.modelObjectType == ModelObjectType.MonsterReward) && cityPointList.Contains(modelObject.Value.pointIndex))
			{
				flag = true;
			}
			if (!flag)
			{
				list.Add(modelObject.Value.pointIndex);
			}
		}
		for (int num4 = 0; num4 < list.Count; num4++)
		{
			RemoveOneObject(list[num4]);
		}
	}

	public bool IsCanShowBuild()
	{
		if (_useBuildArrayOpt)
		{
			return true;
		}
		return GameEntry.Lua.CallWithReturn<bool>("DataCenter.GuideManager:IsStartCanShowBuild");
	}

	private void ShowAllGuideObjectSignal(object userData)
	{
		foreach (ModelObject value in _modelObjects.Values)
		{
			value.SetIsVisible(visible: true);
			if (value.modelObjectType == ModelObjectType.Build && value is BuildObject buildObject)
			{
				GameEntry.Event.Fire(EventId.BUILD_IN_VIEW, buildObject.luaBuild.uuid);
			}
		}
		if (_cityTroop != null)
		{
			_cityTroop.gameObject.SetActive(value: true);
		}
	}

	private void FarmGuideFakePlantShowStateSignal(object userData)
	{
		bool labelActive = (bool)userData;
		foreach (ModelObject value in _modelObjects.Values)
		{
			value.SetLabelActive(labelActive);
		}
	}

	private void OpenFogSuccessSignal(object fogIdObj)
	{
		long num = (long)fogIdObj;
		foreach (ModelObject value in _modelObjects.Values)
		{
			value.UpdateFog((int)num);
		}
	}

	private void UpdateCityPointSignal(object userData)
	{
		int num = Convert.ToInt32(userData);
		LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable, int>("CSharpCallLuaInterface.GetCityPointDataByPointId", num);
		if (luaTable != null)
		{
			switch (luaTable.Get<int>("type"))
			{
			case 1:
				LoadOneObject(num, 3);
				break;
			case 2:
				LoadOneObject(num, 4);
				break;
			case 3:
				LoadOneObject(num, 5);
				break;
			case 4:
				LoadOneObject(num, 6);
				break;
			}
		}
		else
		{
			if (num == 0)
			{
				return;
			}
			ModelObject objectByPointId = GetObjectByPointId(num);
			if (objectByPointId != null && objectByPointId is GarbageRewardObject garbageRewardObject && !garbageRewardObject.NeedDestroyWhenDisappearEnd())
			{
				if (!garbageRewardObject.isDoingDisappear)
				{
					garbageRewardObject.DoDisappear();
				}
			}
			else
			{
				RemoveOneObject(num);
			}
		}
	}

	private void MonsterLockStateUpdateSignal(object userData)
	{
		LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetMonsterLockDataList");
		Dictionary<int, int> monsters = new Dictionary<int, int>();
		luaTable.ForEach(delegate(int _, LuaTable data)
		{
			int num = data.Get<int>("monsterId");
			data.Get<int>("state");
			int num2 = data.Get<int>("pointId");
			if (GetObjectByPointId(num2) == null)
			{
				LoadOneObject(num2, 8, num);
				GameEntry.Event.Fire(EventId.MonsterLockInView, num);
			}
			monsters[num2] = 1;
		});
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, ModelObject> modelObject in _modelObjects)
		{
			if (!monsters.ContainsKey(modelObject.Key) && modelObject.Value.modelObjectType == ModelObjectType.MonsterLock)
			{
				list.Add(modelObject.Key);
			}
		}
		foreach (int item in list)
		{
			RemoveOneObjectByPointType(item, 8);
		}
	}

	private int GetInitCityTroopPosition()
	{
		return GameEntry.Setting.GetPrivateInt("CITY_TROOP_POSITION", -1);
	}

	public void LoadCityTroop(int createPos, int targetPos = 0)
	{
		_cityTroopInstance = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/March/CityTroop.prefab");
		_cityTroopInstance.completed += delegate
		{
			GameObject gameObject = _cityTroopInstance.gameObject;
			if (gameObject != null)
			{
				gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
				gameObject.transform.position = SceneManager.World.TileIndexToWorld(createPos);
				_cityTroop = gameObject.GetComponent<CityTroop>();
				_cityTroop.Init();
				if (targetPos > 0)
				{
					_cityTroop.MoveAfterCreate(targetPos);
				}
				gameObject.SetActive(IsCanShowBuild());
			}
		};
	}

	public void DestroyCityTroop()
	{
		if (_cityTroop != null)
		{
			_cityTroop.UnInit();
			_cityTroop = null;
		}
		if (_cityTroopInstance != null)
		{
			_cityTroopInstance.Destroy();
			_cityTroopInstance = null;
		}
		GameEntry.Setting.SetPrivateInt("CITY_TROOP_POSITION", -1);
	}

	public CitySpaceMan CreateCitySpaceMan()
	{
		if (_citySpaceMan != null)
		{
			return _citySpaceMan;
		}
		_citySpaceMan = new CitySpaceMan();
		return _citySpaceMan;
	}

	public void DestroyCitySpaceMan()
	{
		if (_citySpaceMan != null)
		{
			_citySpaceMan.Destroy();
			_citySpaceMan = null;
		}
	}

	public CityTroop GetCityTroop()
	{
		return _cityTroop;
	}

	public void SetVisibleByPointType(int pointType, bool isVisible)
	{
		foreach (ModelObject value in _modelObjects.Values)
		{
			if (value.modelObjectType == (ModelObjectType)pointType)
			{
				value.SetIsVisible(isVisible);
			}
		}
	}

	private void SetBuildCanDoAnimSignal(object userData)
	{
		long num = (long)userData;
		RemoveOneNoDoAnimBuild(num);
		foreach (KeyValuePair<int, ModelObject> modelObject in _modelObjects)
		{
			if (modelObject.Value.modelObjectType == ModelObjectType.Build && modelObject.Value is BuildObject buildObject && buildObject.luaBuild.uuid == num)
			{
				buildObject.cityBuilding.SetCanDoAnim(canDoAnim: true);
				break;
			}
		}
	}

	private void SetBuildNoDoAnimSignal(object userData)
	{
		long num = (long)userData;
		AddOneNoDoAnimBuild(num);
		foreach (KeyValuePair<int, ModelObject> modelObject in _modelObjects)
		{
			if (modelObject.Value.modelObjectType == ModelObjectType.Build && modelObject.Value is BuildObject buildObject && buildObject.luaBuild.uuid == num)
			{
				buildObject.cityBuilding.SetCanDoAnim(canDoAnim: false);
				break;
			}
		}
	}

	private void AddOneNoDoAnimBuild(long uuid)
	{
		if (!_noDoAnimBuild.Contains(uuid))
		{
			_noDoAnimBuild.Add(uuid);
		}
	}

	private void RemoveOneNoDoAnimBuild(long uuid)
	{
		if (_noDoAnimBuild.Contains(uuid))
		{
			_noDoAnimBuild.Remove(uuid);
		}
	}

	public bool IsNoDoBuildAnim(long uuid)
	{
		return _noDoAnimBuild.Contains(uuid);
	}

	private void OnUpdateActivityAlarmClockBuildingTimeShow(object obj)
	{
		bool isShowServerTime = (bool)obj;
		LuaBuildData buildingDataByBuildId = GameEntry.Data.Building.GetBuildingDataByBuildId(10224000);
		if (buildingDataByBuildId == null || buildingDataByBuildId.buildId <= 0 || !SceneManager.IsInCity())
		{
			return;
		}
		foreach (KeyValuePair<int, ModelObject> modelObject in _modelObjects)
		{
			if (modelObject.Value.modelObjectType == ModelObjectType.Build && modelObject.Value is BuildObject buildObject && buildObject.luaBuild.uuid == buildingDataByBuildId.uuid && buildObject.cityBuilding != null)
			{
				buildObject.cityBuilding.UpdateActivityAlarmClockTimeShow(isShowServerTime);
				break;
			}
		}
	}
}
