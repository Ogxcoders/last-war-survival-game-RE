using System;
using System.Collections.Generic;
using BitBenderGames;
using GameFramework;
using GameKit.Base;
using UnityEngine;

public class AutoAdjustLod : MonoBehaviourWrapped, IFastUpdate
{
	public enum ActiveState
	{
		Unknown = -1,
		False,
		True
	}

	[Serializable]
	public class Setting
	{
		public GameObject[] targets;

		public string lodRange;

		public bool isMain;

		public bool noFading;

		public bool noOptimizeActivate;

		[HideInInspector]
		public List<int> showLods;

		public ActiveState activeCache = ActiveState.Unknown;

		private AutoAdjustLod adjuster;

		private float _fadeStartValue;

		private float _fadeEndValue;

		private float _t = -1f;

		private bool isActive;

		private bool _alreadyCollect;

		private List<MeshRenderer> _meshRenderers = new List<MeshRenderer>();

		private List<SkinnedMeshRenderer> _skinnedMeshRenderers = new List<SkinnedMeshRenderer>();

		private List<SpriteRenderer> _spriteRenderers = new List<SpriteRenderer>();

		private List<GameObject> dynamicVisibleList;

		public bool IsNeedUpdate
		{
			get
			{
				if (_spriteRenderers != null)
				{
					return _spriteRenderers.Count > 0;
				}
				return false;
			}
		}

		public void SetAdjuster(AutoAdjustLod adjuster)
		{
			this.adjuster = adjuster;
		}

		public void InitByConfig(GameObject[] targets, LodConfig lodConfig)
		{
			activeCache = ActiveState.Unknown;
			this.targets = targets;
			lodRange = $"{lodConfig.lodStart}-{lodConfig.lodEnd}";
			showLods = new List<int>();
			isMain = lodConfig.isMain;
			noFading = lodConfig.noFading;
			for (int i = lodConfig.lodStart; i <= lodConfig.lodEnd; i++)
			{
				showLods.Add(i);
			}
			if (!lodConfig.standaloneScaleSpeed || targets == null)
			{
				return;
			}
			int j = 0;
			for (int num = this.targets.Length; j < num; j++)
			{
				GameObject gameObject = this.targets[j];
				if (!(gameObject == null))
				{
					(gameObject.GetComponent<AutoAdjustScale>() ?? gameObject.AddComponent<AutoAdjustScale>())?.SetStandaloneScaleSpeed(lodConfig.minLod, lodConfig.maxLod, lodConfig.maxScale);
				}
			}
		}

		public void InitByPrefab()
		{
			activeCache = ActiveState.Unknown;
			showLods = new List<int>();
			string[] array = lodRange.Split(';', ',');
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(new char[1] { '-' });
				if (array2.Length == 1)
				{
					showLods.Add(array2[0].ToInt());
				}
				else if (array2.Length == 2)
				{
					for (int j = array2[0].ToInt(); j <= array2[1].ToInt(); j++)
					{
						showLods.Add(j);
					}
				}
			}
		}

		public void Append(GameObject go)
		{
			if (dynamicVisibleList == null)
			{
				dynamicVisibleList = new List<GameObject>();
			}
			dynamicVisibleList.Add(go);
			go.SetActive(isActive);
		}

		public void ClearAppend()
		{
			dynamicVisibleList?.Clear();
		}

		public void InitFadeComponent()
		{
			if (!noFading)
			{
				CollectComponent();
			}
		}

		private void CollectComponent()
		{
			if (_alreadyCollect)
			{
				return;
			}
			_alreadyCollect = true;
			GameObject[] array = targets;
			foreach (GameObject gameObject in array)
			{
				if (!(gameObject == null))
				{
					MeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<MeshRenderer>(includeInactive: true);
					for (int j = 0; j < componentsInChildren.Length; j++)
					{
						_meshRenderers.Add(componentsInChildren[j]);
					}
					SkinnedMeshRenderer[] componentsInChildren2 = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true);
					for (int k = 0; k < componentsInChildren2.Length; k++)
					{
						_skinnedMeshRenderers.Add(componentsInChildren2[k]);
					}
					SpriteRenderer[] componentsInChildren3 = gameObject.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
					for (int l = 0; l < componentsInChildren3.Length; l++)
					{
						_spriteRenderers.Add(componentsInChildren3[l]);
					}
				}
			}
		}

		public void FadeIn()
		{
			if (noFading)
			{
				SetTargetsActive(active: true);
				return;
			}
			if (CommonUtils.IsDebug() && !_alreadyCollect)
			{
				Log.Error("not already Collect!");
			}
			CollectComponent();
			DoFideIn();
		}

		private void DoFideIn()
		{
			SetTargetsActive(active: true);
			foreach (MeshRenderer meshRenderer in _meshRenderers)
			{
				if (meshRenderer != null)
				{
					meshRenderer.enabled = true;
				}
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in _skinnedMeshRenderers)
			{
				if (skinnedMeshRenderer != null)
				{
					skinnedMeshRenderer.enabled = true;
				}
			}
			if (_spriteRenderers.Count > 0)
			{
				_t = 0f;
				_fadeEndValue = 1f;
				if (_spriteRenderers[0] != null)
				{
					_fadeStartValue = _spriteRenderers[0].color.a;
				}
			}
		}

		public void FadeOut()
		{
			if (noFading)
			{
				SetTargetsActive(active: false);
				return;
			}
			if (CommonUtils.IsDebug() && !_alreadyCollect)
			{
				Log.Error("not already Collect!");
			}
			CollectComponent();
			DoFideOut();
		}

		private void DoFideOut()
		{
			SetTargetsActive(active: false);
			foreach (MeshRenderer meshRenderer in _meshRenderers)
			{
				if (meshRenderer != null)
				{
					meshRenderer.enabled = false;
				}
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in _skinnedMeshRenderers)
			{
				if (skinnedMeshRenderer != null)
				{
					skinnedMeshRenderer.enabled = false;
				}
			}
			if (_spriteRenderers.Count > 0)
			{
				SpriteRenderer spriteRenderer = _spriteRenderers[0];
				if (spriteRenderer != null)
				{
					_t = 0f;
					_fadeEndValue = 0f;
					_fadeStartValue = spriteRenderer.color.a;
				}
			}
		}

		public void Update()
		{
			if (_fadeEndValue < 0f || _spriteRenderers.Count == 0)
			{
				return;
			}
			if (_t >= FADE_DURATION)
			{
				foreach (SpriteRenderer spriteRenderer in _spriteRenderers)
				{
					if (spriteRenderer != null)
					{
						spriteRenderer.Set_color_a(_fadeEndValue);
					}
				}
				_fadeEndValue = (_fadeStartValue = -1f);
				_t = 0f;
				return;
			}
			if ((double)Mathf.Abs(_fadeStartValue - _fadeEndValue) < 1E-05)
			{
				_fadeEndValue = (_fadeStartValue = -1f);
				_t = 0f;
				return;
			}
			float value = Mathf.Lerp(_fadeStartValue, _fadeEndValue, _t / FADE_DURATION);
			value = Mathf.Clamp01(value);
			_t += Time.deltaTime;
			foreach (SpriteRenderer spriteRenderer2 in _spriteRenderers)
			{
				if (spriteRenderer2 != null)
				{
					spriteRenderer2.Set_color_a(value);
				}
			}
		}

		public void SetTargetsActive(bool active)
		{
			isActive = active;
			if (noOptimizeActivate)
			{
				GameObject[] array = targets;
				foreach (GameObject gameObject in array)
				{
					if (gameObject != null)
					{
						if (targetOriginPos.ContainsKey(gameObject))
						{
							gameObject.transform.localPosition = targetOriginPos[gameObject];
							targetOriginPos.Remove(gameObject);
						}
						if (gameObject.activeSelf != active)
						{
							gameObject.SetActive(active);
						}
					}
				}
			}
			else
			{
				GameObject[] array = targets;
				foreach (GameObject gameObject2 in array)
				{
					if (!(gameObject2 != null))
					{
						continue;
					}
					if (active)
					{
						if (!gameObject2.activeSelf)
						{
							gameObject2.SetActive(value: true);
						}
						if (targetOriginPos.ContainsKey(gameObject2))
						{
							gameObject2.transform.localPosition = targetOriginPos[gameObject2];
							targetOriginPos.Remove(gameObject2);
						}
					}
					else if (!IsOutPos(gameObject2.transform.localPosition))
					{
						targetOriginPos[gameObject2] = gameObject2.transform.localPosition;
						gameObject2.transform.localPosition = OUT_POS;
					}
				}
			}
			if (dynamicVisibleList != null && dynamicVisibleList.Count > 0)
			{
				int j = 0;
				for (int count = dynamicVisibleList.Count; j < count; j++)
				{
					dynamicVisibleList[j].SetActive(active);
				}
			}
		}

		private bool IsOutPos(Vector3 pos)
		{
			if (pos.x < OUT_POS.x + 1f)
			{
				return pos.z < OUT_POS.z + 1f;
			}
			return false;
		}
	}

	public static readonly float FADE_DURATION = 0.3f;

	public static readonly Vector3 OUT_POS = new Vector3(-10000f, 0f, -10000f);

	public static readonly Dictionary<GameObject, Vector3> targetOriginPos = new Dictionary<GameObject, Vector3>();

	private bool settingSucc;

	[SerializeField]
	private LodType lodType;

	[SerializeField]
	private Setting[] settings;

	private float _fadeEndValue = -1f;

	private Action<int> lodUpdateCallback;

	private Action<bool, bool> beforeLodFadeAction;

	[NonSerialized]
	private bool isInitialized;

	private List<GameObject> dynamicAppends;

	public bool LowLodLevelHasShowed { get; private set; }

	public static void SetTargetOriginPos(GameObject key, Vector3 value)
	{
		targetOriginPos[key] = value;
	}

	public Setting[] GetSettings()
	{
		return settings;
	}

	public LodType getLodType()
	{
		return lodType;
	}

	public void SetLodType(LodType t)
	{
		lodType = t;
		InitConfig();
		RegisterLodAdjuster();
	}

	public void UpdateLod(int lod)
	{
		if (base.gameObject == null)
		{
			Log.Info("AutoAdjustLod: Game object has been destroyed! GameObject: " + base.name);
		}
		else
		{
			if (settings == null)
			{
				return;
			}
			if (lod <= 2)
			{
				LowLodLevelHasShowed = true;
			}
			Setting[] array = settings;
			foreach (Setting setting in array)
			{
				List<int> showLods = setting.showLods;
				ActiveState activeState = ((showLods != null && showLods.Contains(lod)) ? ActiveState.True : ActiveState.False);
				if (setting.activeCache == activeState)
				{
					continue;
				}
				setting.activeCache = activeState;
				if (activeState == ActiveState.True)
				{
					if (beforeLodFadeAction != null)
					{
						beforeLodFadeAction(arg1: true, setting.isMain);
					}
					setting.FadeIn();
				}
				else
				{
					if (beforeLodFadeAction != null)
					{
						beforeLodFadeAction(arg1: false, setting.isMain);
					}
					setting.FadeOut();
				}
			}
			if (lodUpdateCallback != null)
			{
				lodUpdateCallback(lod);
			}
		}
	}

	public void SetBeforeLodFadeCallback(Action<bool, bool> callback)
	{
		beforeLodFadeAction = callback;
	}

	public void SetLodUpdateCallback(Action<int> callback)
	{
		lodUpdateCallback = callback;
	}

	private void Awake()
	{
		if (IsSceneUseLod())
		{
			InitConfig();
		}
	}

	public void DoUpdate()
	{
		if (settings != null)
		{
			Setting[] array = settings;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Update();
			}
		}
	}

	private void OnDestroy()
	{
		lodUpdateCallback = null;
		beforeLodFadeAction = null;
	}

	private void InitConfig()
	{
		settingSucc = InitSettingsByConfig() || InitSettingsByPrefab();
		lodUpdateCallback = null;
		beforeLodFadeAction = null;
	}

	private void RegisterLodAdjuster()
	{
		if (IsSceneUseLod() && settingSucc)
		{
			InitLod();
			LowLodLevelHasShowed = false;
			SceneManager.World?.AddLodAdjuster(this);
		}
	}

	private void OnEnable()
	{
		DoEnable();
	}

	private void OnDisable()
	{
		DoDisable();
	}

	public void DoEnable()
	{
		if (!isInitialized)
		{
			isInitialized = true;
			RegisterLodAdjuster();
			SingletonBehaviour<FastUpdater>.Instance?.Add(this);
		}
	}

	public void DoDisable()
	{
		if (!isInitialized)
		{
			return;
		}
		isInitialized = false;
		if (IsSceneUseLod() && settingSucc)
		{
			SceneManager.World?.RemoveLodAdjuster(this);
			if (SceneManager.IsInWorld())
			{
				WorldScene.RecordLodState(lodType, LowLodLevelHasShowed);
			}
		}
		SingletonBehaviour<FastUpdater>.Instance?.Remove(this);
	}

	private void InitLod()
	{
		int lodLevel = SceneManager.World.GetLodLevel();
		if (base.gameObject == null)
		{
			Log.Info("AutoAdjustLod: Game object has been destroyed! GameObject: " + base.name);
		}
		else
		{
			if (settings == null)
			{
				return;
			}
			Setting[] array = settings;
			foreach (Setting setting in array)
			{
				List<int> showLods = setting.showLods;
				ActiveState activeState = ((showLods != null && showLods.Contains(lodLevel)) ? ActiveState.True : ActiveState.False);
				if (setting.activeCache == activeState)
				{
					continue;
				}
				setting.activeCache = activeState;
				setting.InitFadeComponent();
				if (activeState == ActiveState.True)
				{
					try
					{
						setting.FadeIn();
					}
					catch (Exception ex)
					{
						Log.Error("AutoAdjustLod::FadeIn error:" + base.gameObject.transform.GetFullPath() + " " + ex.Message + " " + ex.StackTrace);
					}
				}
				else
				{
					setting.SetTargetsActive(active: false);
				}
			}
		}
	}

	public bool IsMainShow()
	{
		if (settings == null)
		{
			return false;
		}
		Setting[] array = settings;
		foreach (Setting setting in array)
		{
			if (setting.isMain && setting.activeCache == ActiveState.True)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsShow()
	{
		if (settings == null)
		{
			return false;
		}
		Setting[] array = settings;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].activeCache == ActiveState.True)
			{
				return true;
			}
		}
		return false;
	}

	private bool InitSettingsByConfig()
	{
		if (lodType == LodType.None)
		{
			return false;
		}
		Dictionary<string, LodConfig> dictionary = SceneManager.World?.GetLodConfigs((int)lodType);
		if (dictionary == null)
		{
			return false;
		}
		List<Setting> list = new List<Setting>();
		foreach (KeyValuePair<string, LodConfig> item in dictionary)
		{
			string key = item.Key;
			Transform transform = base.transform.Find(key);
			if (transform != null)
			{
				GameObject[] targets = new GameObject[1] { transform.gameObject };
				Setting setting = new Setting();
				setting.SetAdjuster(this);
				setting.InitByConfig(targets, item.Value);
				setting.SetTargetsActive(active: false);
				list.Add(setting);
				setting.noFading = lodType == LodType.Ground || lodType == LodType.Zone;
			}
		}
		settings = list.ToArray();
		return true;
	}

	private bool InitSettingsByPrefab()
	{
		if (settings == null)
		{
			return false;
		}
		Setting[] array = settings;
		foreach (Setting obj in array)
		{
			obj.SetAdjuster(this);
			obj.InitByPrefab();
			obj.SetTargetsActive(active: false);
		}
		return true;
	}

	private bool IsSceneUseLod()
	{
		if (!SceneManager.IsInWorld())
		{
			return SceneManager.IsInCity();
		}
		return true;
	}

	public void SetNoOptimizeActivate(bool noOptimizeActivate)
	{
		if (settings != null)
		{
			Setting[] array = settings;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].noOptimizeActivate = noOptimizeActivate;
			}
		}
	}

	public void AppendLod(GameObject target, string lodRange)
	{
		if (target == null || settings == null)
		{
			return;
		}
		int i = 0;
		for (int num = settings.Length; i < num; i++)
		{
			Setting setting = settings[i];
			if (setting != null && !(setting.lodRange != lodRange))
			{
				setting.Append(target);
			}
		}
	}

	public void ClearAllAppend()
	{
		if (settings != null)
		{
			int i = 0;
			for (int num = settings.Length; i < num; i++)
			{
				settings[i]?.ClearAppend();
			}
		}
	}
}
