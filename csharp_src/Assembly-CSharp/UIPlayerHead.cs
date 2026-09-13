using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using UnityEngine;

[AddComponentMenu("UI/UIPlayerHead")]
public class UIPlayerHead : MonoBehaviour
{
	public class HeadRef
	{
		public string assetKey;

		public Sprite sprite;

		public int refCount;

		public HeadRef(string assetKey, Texture2D texture)
		{
			this.assetKey = assetKey;
			sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
		}
	}

	private const string UserHeadPath = "Assets/Main/Sprites/UI/UIHeadIcon/";

	public const string DefaultUserHead = "Assets/Main/Sprites/UI/LWCommon/Sprite/zyf_touxiang_da_hui";

	private const string CACHE_FOLDER = "LocalImages";

	private const string CDN_PATH = "https://lastwar-cdn.akamaized.net/img";

	private const string ONLINE_HEAD_COLLECTIONS_FILE = "onlinePlayerHeadCollections.txt";

	public static int CACHE_CAPACITY = 200;

	private static bool INITED = false;

	private static LinkedList<HeadRef> _linked = new LinkedList<HeadRef>();

	public static Dictionary<string, HeadRef> _cache = new Dictionary<string, HeadRef>();

	public static int _waitingCount = 0;

	public static Dictionary<string, int> _loadingCount = new Dictionary<string, int>();

	private static bool _collectOnlineHeadsSwitch = false;

	private static List<string> _onlineHeadCollections = new List<string>();

	public CircleImage circleImage;

	public SpriteRenderer spriteRenderer;

	private CircleMesh circleMesh;

	private string m_playerUid;

	private string m_playerPic;

	private int m_playerPicVer;

	private bool m_useBig;

	private string m_spResPath;

	private string m_usingAssetKey;

	private string m_loadingAssetKey;

	private bool m_isWaiting;

	private Action customLoadCallback;

	private SpriteDrawMode m_spriteRendererDrawMode;

	private Vector2 m_spriteRendererSize;

	public static bool CollectOnlineHeadsSwitch
	{
		get
		{
			return _collectOnlineHeadsSwitch;
		}
		set
		{
		}
	}

	public bool IsValidated
	{
		get
		{
			if (!(circleImage != null) && !(spriteRenderer != null))
			{
				return circleMesh != null;
			}
			return true;
		}
	}

	public bool IsEnabled
	{
		get
		{
			if (base.enabled)
			{
				return base.gameObject.activeInHierarchy;
			}
			return false;
		}
	}

	public bool IsSystemHead
	{
		get
		{
			if (string.IsNullOrEmpty(m_playerPic) && m_playerPicVer > 0)
			{
				return m_playerPicVer > 1000000;
			}
			return true;
		}
	}

	public static void OnStaticUpdate()
	{
		if (_waitingCount > 0)
		{
			ReleaseAllUnusedCache();
		}
	}

	public static void OnLowMemory()
	{
		CACHE_CAPACITY = Mathf.Max(CACHE_CAPACITY / 2, 20);
		ReleaseAllUnusedCache();
	}

	private static string GenAssetKey(string uid, int picVer, bool useBig = false)
	{
		string md5Hash = AESHelper.GetMd5Hash($"{uid}_{picVer}");
		string text = ((uid.Length > 6) ? uid.Substring(uid.Length - 6) : uid);
		string text2 = (useBig ? "_big" : "");
		if (string.IsNullOrEmpty(text))
		{
			try
			{
				string text3 = "";
				XLuaManager lua = GameEntry.Lua;
				if (lua != null && lua.Env != null)
				{
					text3 = lua.CallWithReturn<string>("debug.traceback");
				}
				Log.Error($"[UIPlayerHead] GenAssetKey with error assetKey {uid},{picVer},{useBig} lua: {text3}");
			}
			catch (Exception)
			{
				Log.Error($"[UIPlayerHead] GenAssetKey with error assetKey {uid},{picVer},{useBig}");
			}
		}
		return text + "/" + md5Hash + text2 + ".jpg";
	}

	private static void RefCache(UIPlayerHead host, string assetKey)
	{
		if (_cache.TryGetValue(assetKey, out var value))
		{
			_linked.Remove(value);
			_linked.AddFirst(value);
			value.refCount++;
		}
	}

	private static void UnRefCache(UIPlayerHead host, string assetKey)
	{
		if (_cache.TryGetValue(assetKey, out var value) && --value.refCount <= 0)
		{
			_linked.Remove(value);
			_linked.AddLast(value);
		}
	}

	private static void ReleaseAllUnusedCache()
	{
		LinkedListNode<HeadRef> last = _linked.Last;
		while (last != null && last.Value.refCount <= 0)
		{
			last.Value.sprite = null;
			DynamicResourceManager.Instance.ReleaseAsset(last.Value.assetKey);
			_cache.Remove(last.Value.assetKey);
			_linked.RemoveLast();
			last = _linked.Last;
		}
	}

	public static string DumpCache()
	{
		if (CommonUtils.IsDebug())
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine($"Capacity: {CACHE_CAPACITY}");
			stringBuilder.AppendLine($"Linked: {_linked.Count} ({_cache.Count})");
			stringBuilder.AppendLine($"Loading: {_loadingCount.Count}");
			stringBuilder.AppendLine($"Queue: {_waitingCount}");
			stringBuilder.AppendLine($"Asset: {DynamicResourceManager.Instance.GetCacheCount()}");
			stringBuilder.AppendLine("Cache Keys: ");
			foreach (HeadRef item in _linked)
			{
				stringBuilder.Append(item.refCount.ToString()).Append(" -> ").AppendLine(item.assetKey);
			}
			return stringBuilder.ToString();
		}
		return "Not Supported In Release";
	}

	public static string DumpDynamicAssets()
	{
		return DynamicResourceManager.Instance.DumpCache();
	}

	public static string DumpCollections()
	{
		return "Not Supported Outside Editor";
	}

	private static void ReadCollectionsFile()
	{
	}

	private static void WriteCollectionsFile()
	{
	}

	private void Awake()
	{
		if (!INITED)
		{
			CACHE_CAPACITY = ((SystemInfo.systemMemorySize > 0) ? (SystemInfo.systemMemorySize / 24) : 200);
			DynamicResourceManager.Instance.OnUpdate += OnStaticUpdate;
			DynamicResourceManager.Instance.OnLowMemory += OnLowMemory;
			INITED = true;
		}
		if (!IsValidated)
		{
			circleImage = GetComponent<CircleImage>();
			spriteRenderer = GetComponent<SpriteRenderer>();
			if (circleImage == null && spriteRenderer == null)
			{
				circleMesh = GetComponent<CircleMesh>();
			}
		}
		if (spriteRenderer != null)
		{
			m_spriteRendererDrawMode = spriteRenderer.drawMode;
			m_spriteRendererSize = spriteRenderer.size;
		}
	}

	private void OnDestroy()
	{
		circleImage = null;
		spriteRenderer = null;
		customLoadCallback = null;
	}

	private void OnEnable()
	{
		if (IsValidated)
		{
			UpdateHead();
		}
	}

	private void OnDisable()
	{
		CancelLoading();
		ReleaseUsing();
		if (m_isWaiting)
		{
			m_isWaiting = false;
			_waitingCount--;
		}
	}

	private void Update()
	{
		if (m_isWaiting && _linked.Count + _loadingCount.Count < CACHE_CAPACITY)
		{
			m_isWaiting = false;
			_waitingCount--;
			UpdateHead();
		}
	}

	public void SetData(string uid, string pic, int picVer, bool useBig = false)
	{
		m_playerUid = uid;
		m_playerPic = pic;
		m_playerPicVer = picVer;
		m_useBig = false;
		m_spResPath = null;
		UpdateHead();
	}

	public void SetBigData(string uid, string pic, int picVer, bool useBig = false)
	{
		m_playerUid = uid;
		m_playerPic = pic;
		m_playerPicVer = picVer;
		m_useBig = true;
		m_spResPath = null;
		UpdateHead();
	}

	public void SetFakeData()
	{
	}

	public void SetCustomLoadCallback(Action action)
	{
		customLoadCallback = action;
	}

	public void UseSystemHead()
	{
		CancelLoading();
		ReleaseUsing();
		m_playerUid = null;
		m_playerPic = null;
		m_playerPicVer = 0;
		m_useBig = false;
		m_spResPath = null;
		UpdateHead();
	}

	public void UseSpecifiedRes(string spResPath)
	{
		CancelLoading();
		ReleaseUsing();
		m_playerUid = null;
		m_playerPic = null;
		m_playerPicVer = 0;
		m_useBig = false;
		m_spResPath = spResPath;
		UpdateHead();
	}

	private void ReleaseUsing()
	{
		if (circleImage != null)
		{
			circleImage.sprite = null;
		}
		if (spriteRenderer != null)
		{
			spriteRenderer.sprite = null;
		}
		circleMesh?.Release();
		if (!string.IsNullOrEmpty(m_usingAssetKey))
		{
			UnRefCache(this, m_usingAssetKey);
			m_usingAssetKey = null;
		}
	}

	private void CancelLoading()
	{
		if (!string.IsNullOrEmpty(m_loadingAssetKey))
		{
			DynamicResourceManager.Instance.CancelTask(m_loadingAssetKey, OnLoadDone);
			int num = _loadingCount[m_loadingAssetKey] - 1;
			if (num <= 0)
			{
				_loadingCount.Remove(m_loadingAssetKey);
			}
			else
			{
				_loadingCount[m_loadingAssetKey] = num;
			}
			m_loadingAssetKey = null;
		}
	}

	private void UpdateHead()
	{
		if (!IsValidated || !IsEnabled)
		{
			return;
		}
		if (IsSystemHead)
		{
			customLoadCallback?.Invoke();
			SetSpriteBySystemHead();
			return;
		}
		string assetKey = GenAssetKey(m_playerUid, m_playerPicVer, m_useBig);
		if (!TrySetSprite(assetKey))
		{
			SetSpriteBySystemHead();
			if (!m_isWaiting)
			{
				LoadHead(assetKey);
			}
		}
	}

	private void LoadHead(string assetKey)
	{
		if ((_linked.Count + _loadingCount.Count < CACHE_CAPACITY || _loadingCount.ContainsKey(assetKey)) && string.IsNullOrEmpty(m_loadingAssetKey))
		{
			if (_loadingCount.ContainsKey(assetKey))
			{
				_loadingCount[assetKey]++;
			}
			else
			{
				_loadingCount.Add(assetKey, 1);
			}
			m_loadingAssetKey = assetKey;
			DynamicResourceManager.Instance.CreateTexture2DTask(assetKey, OnLoadDone, m_playerUid, "https://lastwar-cdn.akamaized.net/img", "LocalImages", nonReadable: false);
		}
		else
		{
			m_isWaiting = true;
			_waitingCount++;
		}
	}

	private void OnLoadDone(string assetKey, UnityEngine.Object asset, object userdata, bool isLastCallback)
	{
		bool flag = string.IsNullOrEmpty(m_loadingAssetKey) || m_loadingAssetKey != assetKey;
		CancelLoading();
		bool num = asset == null || !(asset is Texture2D);
		bool flag2 = !IsValidated;
		bool flag3 = m_playerUid != (string)userdata && (string)userdata != "FAKE";
		bool flag4 = num || flag2 || flag3 || flag;
		if (!_cache.TryGetValue(assetKey, out var value))
		{
			if (flag4)
			{
				if (isLastCallback)
				{
					DynamicResourceManager.Instance.ReleaseAsset(assetKey);
				}
			}
			else
			{
				value = new HeadRef(assetKey, (Texture2D)asset);
				_cache.Add(assetKey, value);
			}
		}
		if (!flag4)
		{
			TrySetSprite(assetKey);
			customLoadCallback?.Invoke();
		}
	}

	private bool TrySetSprite(string assetKey)
	{
		if (!string.IsNullOrEmpty(m_loadingAssetKey) && m_loadingAssetKey != assetKey)
		{
			CancelLoading();
		}
		if (!_cache.TryGetValue(assetKey, out var value))
		{
			return false;
		}
		ReleaseUsing();
		if (circleImage != null)
		{
			circleImage.sprite = value.sprite;
		}
		if (spriteRenderer != null)
		{
			spriteRenderer.sprite = value.sprite;
			spriteRenderer.drawMode = m_spriteRendererDrawMode;
			spriteRenderer.size = m_spriteRendererSize;
		}
		if (circleMesh != null)
		{
			circleMesh.SetupSprite(value.sprite);
		}
		RefCache(this, assetKey);
		m_usingAssetKey = assetKey;
		customLoadCallback?.Invoke();
		return true;
	}

	private void SetSpriteBySystemHead()
	{
		string spritePath = ((!string.IsNullOrEmpty(m_spResPath)) ? m_spResPath : (string.IsNullOrEmpty(m_playerPic) ? "Assets/Main/Sprites/UI/LWCommon/Sprite/zyf_touxiang_da_hui" : ("Assets/Main/Sprites/UI/UIHeadIcon/" + m_playerPic)));
		if (circleImage != null)
		{
			circleImage.LoadSprite(spritePath, "Assets/Main/Sprites/UI/LWCommon/Sprite/zyf_touxiang_da_hui");
		}
		else if (spriteRenderer != null)
		{
			spriteRenderer.LoadSprite(spritePath, "Assets/Main/Sprites/UI/LWCommon/Sprite/zyf_touxiang_da_hui");
			spriteRenderer.drawMode = m_spriteRendererDrawMode;
			spriteRenderer.size = m_spriteRendererSize;
		}
		else if (circleMesh != null)
		{
			circleMesh.LoadSprite(spritePath, "Assets/Main/Sprites/UI/LWCommon/Sprite/zyf_touxiang_da_hui");
		}
	}

	private long GetTextureRuntimeMemorySize(Texture2D texture)
	{
		return 0L;
	}
}
