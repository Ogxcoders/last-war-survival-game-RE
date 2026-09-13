using System;
using System.Collections.Generic;
using BitBenderGames;
using GameFramework;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIChatSendPhoto : MonoBehaviour
{
	public static int CUR_CACHE_CAPACITY = 30;

	public static int MIN_CACHE_CAPACITY = 30;

	public static int MAX_CACHE_CAPACITY = 200;

	public static float SINGLE_FILE_SIZE = 0.0625f;

	public static Dictionary<string, LinkedListNode<CacheItem>> nodeMap = new Dictionary<string, LinkedListNode<CacheItem>>();

	public static LinkedList<CacheItem> nodeList = new LinkedList<CacheItem>();

	public static string chatPhotoFloderName = "ChatPhotos";

	public static string momentPhotoFloderName = "MomentPhotos";

	public static string newsCenterFloderName = "NewsCenterPhotos";

	public static string CACHE_FOLDER_NAME = "ChatPhotos";

	public static int _waitingCount = 0;

	private static string CDN_PATH = "https://lastwar-cdn.akamaized.net/img";

	public static HashSet<string> assetLoadingSet = new HashSet<string>();

	private string playerUid;

	private int playerPicVer;

	private bool isUseBig;

	private string curShowAssetKey = "";

	public int curPhotoFuncType = 2;

	public static int CUR_BIG_CACHE_CAPACITY = 5;

	public static Dictionary<string, LinkedListNode<CacheItem>> bigNodeMap = new Dictionary<string, LinkedListNode<CacheItem>>();

	public static LinkedList<CacheItem> bigNodeList = new LinkedList<CacheItem>();

	public static Dictionary<string, int> bigNodeUseNumMap = new Dictionary<string, int>();

	private PhotoViewer photoViewer;

	private string BtnReportName = "BtnReport";

	private string PhotoDefaultBgName = "PhotoDefaultBg";

	private string UIPhotoName = "UIPhoto";

	private Vector2 curInputPressPosition;

	private Vector2 curInputPosition;

	public static CacheItem GetCacheItem(string key)
	{
		if (!nodeMap.TryGetValue(key, out var value))
		{
			return null;
		}
		nodeList.Remove(value);
		nodeList.AddFirst(value);
		return value.Value;
	}

	public static void PutCacheItem(string key, Texture2D texture2D)
	{
		if (nodeMap.TryGetValue(key, out var value))
		{
			value.Value.textureAsset = texture2D;
			nodeList.Remove(value);
			nodeList.AddFirst(value);
			return;
		}
		if (nodeList.Count >= CUR_CACHE_CAPACITY)
		{
			LinkedListNode<CacheItem> last = nodeList.Last;
			if (last != null)
			{
				ClearCacheItem(last.Value, isBigPhoto: false);
			}
		}
		LinkedListNode<CacheItem> linkedListNode = new LinkedListNode<CacheItem>(new CacheItem(key, texture2D));
		nodeList.AddFirst(linkedListNode);
		nodeMap[key] = linkedListNode;
	}

	public static void ClearCacheItem(CacheItem cacheItem, bool isBigPhoto)
	{
		if (!isBigPhoto)
		{
			nodeMap.Remove(cacheItem.assetKey);
			nodeList.RemoveLast();
		}
		else
		{
			string assetKey = cacheItem.assetKey;
			if (bigNodeMap.TryGetValue(assetKey, out var value))
			{
				bigNodeMap.Remove(cacheItem.assetKey);
				bigNodeList.Remove(value);
			}
		}
		DynamicResourceManager.Instance.ReleaseAsset(cacheItem.assetKey);
		cacheItem.textureAsset = null;
		cacheItem.assetKey = null;
	}

	public static void OnLowMemory()
	{
		CUR_CACHE_CAPACITY = Mathf.Max(CUR_CACHE_CAPACITY / 2, MIN_CACHE_CAPACITY);
		while (nodeList.Count > CUR_CACHE_CAPACITY)
		{
			LinkedListNode<CacheItem> last = nodeList.Last;
			if (last != null)
			{
				ClearCacheItem(last.Value, isBigPhoto: false);
			}
		}
	}

	private void Awake()
	{
		if (SystemInfo.systemMemorySize > 0)
		{
			CUR_CACHE_CAPACITY = Mathf.Max(Mathf.Min((int)Math.Ceiling((float)SystemInfo.systemMemorySize / SINGLE_FILE_SIZE), MAX_CACHE_CAPACITY), MIN_CACHE_CAPACITY);
		}
		else
		{
			CUR_CACHE_CAPACITY = MIN_CACHE_CAPACITY;
		}
		DynamicResourceManager.Instance.OnLowMemory += OnLowMemory;
		photoViewer = GetComponent<PhotoViewer>();
	}

	private void OnDestroy()
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void Update()
	{
	}

	public void SetData(int photoFuncType, string uid, int picVer, string assetKey, bool useBig = false)
	{
		curPhotoFuncType = photoFuncType;
		if (curPhotoFuncType == 2)
		{
			CACHE_FOLDER_NAME = chatPhotoFloderName;
		}
		else if (curPhotoFuncType == 3 || curPhotoFuncType == 4)
		{
			CACHE_FOLDER_NAME = momentPhotoFloderName;
		}
		else if (curPhotoFuncType == 5)
		{
			CACHE_FOLDER_NAME = newsCenterFloderName;
		}
		playerUid = uid;
		playerPicVer = picVer;
		isUseBig = useBig;
		curShowAssetKey = assetKey;
	}

	public void StartUpdateSmallPhoto()
	{
		if (GetCacheItem(curShowAssetKey) == null)
		{
			if (!assetLoadingSet.Contains(curShowAssetKey))
			{
				assetLoadingSet.Add(curShowAssetKey);
				RequestTextureData(curShowAssetKey, isUseBig: false);
			}
		}
		else
		{
			GameEntry.Event.Fire(EventId.ChatSendPhotoSetSuccess, curShowAssetKey);
		}
	}

	private void OnLoadDone(string assetKey, UnityEngine.Object asset, object userdata, bool isLastCallback)
	{
		bool flag = (bool)userdata;
		assetLoadingSet.Remove(assetKey);
		if (asset == null || !(asset is Texture2D))
		{
			DynamicResourceManager.Instance.ReleaseAsset(assetKey);
			GameEntry.Event.Fire(EventId.ChatSendPhotoSetReload, assetKey);
			return;
		}
		if (!flag)
		{
			if (GetCacheItem(assetKey) == null)
			{
				PutCacheItem(assetKey, (Texture2D)asset);
			}
		}
		else if (GetCacheBigItem(assetKey) == null)
		{
			PutCacheBigItem(assetKey, (Texture2D)asset);
		}
		GameEntry.Event.Fire(EventId.ChatSendPhotoSetSuccess, assetKey);
	}

	public void RequestTextureData(string assetKey, bool isUseBig)
	{
		if (!DynamicResourceManager.Instance.IsTaskExist(assetKey))
		{
			DynamicResourceManager.Instance.CreateTexture2DTask(assetKey, OnLoadDone, isUseBig, CDN_PATH, CACHE_FOLDER_NAME, nonReadable: false);
		}
	}

	public CacheItem SetUILoadedSuccessShow(string assetKey)
	{
		CacheItem cacheItem = GetCacheItem(assetKey);
		if (cacheItem == null || cacheItem.textureAsset == null)
		{
			Log.Error("本地读取or网络下载完，LRU中还是没有缓存,异常！！");
			return null;
		}
		return cacheItem;
	}

	public static void ClearAssetLoadingSet()
	{
		assetLoadingSet.Clear();
	}

	public void AbortDownloadSmallCacheItem(string assetKey)
	{
		if (assetLoadingSet.Contains(assetKey))
		{
			assetLoadingSet.Remove(assetKey);
		}
		DynamicResourceManager.Instance.AbortTask(assetKey, OnLoadDone);
	}

	public void AbortUploadSmallCacheItem(string picVer, int upLoadImgType)
	{
		UploadImageManager.Instance.AbortUploadChatPhotoTask(picVer, upLoadImgType);
	}

	public static CacheItem GetCacheBigItem(string key)
	{
		if (!bigNodeMap.TryGetValue(key, out var value))
		{
			return null;
		}
		bigNodeList.Remove(value);
		bigNodeList.AddFirst(value);
		return value.Value;
	}

	public static void PutCacheBigItem(string key, Texture2D texture2D)
	{
		if (bigNodeMap.TryGetValue(key, out var value))
		{
			value.Value.textureAsset = texture2D;
			bigNodeList.Remove(value);
			bigNodeList.AddFirst(value);
			return;
		}
		if (bigNodeList.Count >= CUR_BIG_CACHE_CAPACITY)
		{
			LinkedListNode<CacheItem> last = bigNodeList.Last;
			if (last != null)
			{
				ClearCacheItem(last.Value, isBigPhoto: true);
			}
		}
		LinkedListNode<CacheItem> linkedListNode = new LinkedListNode<CacheItem>(new CacheItem(key, texture2D));
		bigNodeList.AddFirst(linkedListNode);
		bigNodeMap[key] = linkedListNode;
	}

	public void StartUpdateBigPhoto()
	{
		if (GetCacheBigItem(curShowAssetKey) == null)
		{
			if (!assetLoadingSet.Contains(curShowAssetKey))
			{
				assetLoadingSet.Add(curShowAssetKey);
				RequestTextureData(curShowAssetKey, isUseBig: true);
			}
		}
		else
		{
			GameEntry.Event.Fire(EventId.ChatSendPhotoSetSuccess, curShowAssetKey);
		}
	}

	public CacheItem GetSmallPhotoCacheItem(string smallAssetKey)
	{
		CacheItem cacheItem = GetCacheItem(smallAssetKey);
		if (cacheItem == null)
		{
			Log.Error("小图已存在LRU队列中，此时没有小图是异常的，检查原因！！！");
			return null;
		}
		return cacheItem;
	}

	public CacheItem GetBigPhotoCacheItem(string assetKey)
	{
		CacheItem cacheBigItem = GetCacheBigItem(assetKey);
		if (cacheBigItem == null)
		{
			return null;
		}
		return cacheBigItem;
	}

	public void CancelOrClearBigCacheItem()
	{
		if (string.IsNullOrEmpty(curShowAssetKey))
		{
			return;
		}
		if (bigNodeUseNumMap.ContainsKey(curShowAssetKey) && bigNodeUseNumMap[curShowAssetKey] > 1)
		{
			bigNodeUseNumMap[curShowAssetKey]--;
			return;
		}
		bigNodeUseNumMap.Remove(curShowAssetKey);
		CacheItem cacheBigItem = GetCacheBigItem(curShowAssetKey);
		if (cacheBigItem == null || cacheBigItem.assetKey == null)
		{
			if (assetLoadingSet.Contains(curShowAssetKey))
			{
				assetLoadingSet.Remove(curShowAssetKey);
			}
			DynamicResourceManager.Instance.CancelTask(curShowAssetKey, OnLoadDone);
		}
		else
		{
			ClearCacheItem(cacheBigItem, isBigPhoto: true);
		}
	}

	public void AddBigNodeUseNumMap()
	{
		if (!string.IsNullOrEmpty(curShowAssetKey))
		{
			if (bigNodeUseNumMap.ContainsKey(curShowAssetKey))
			{
				bigNodeUseNumMap[curShowAssetKey]++;
			}
			else
			{
				bigNodeUseNumMap.Add(curShowAssetKey, 1);
			}
		}
	}

	public void SetPhotoOriginalSize(float originalWidth, float originalHeight)
	{
		photoViewer.SetPhotoOriginalSize(originalWidth, originalHeight);
	}

	public void BindClickCloseBigPhotoView()
	{
		if (photoViewer == null)
		{
			Log.Error("当前大图界面获取不到 PhotoViewer 脚本,异常！！");
			return;
		}
		photoViewer.BindClick(delegate
		{
			EventSystem current = EventSystem.current;
			curInputPressPosition.Set(Input.mousePosition.x, Input.mousePosition.y);
			curInputPosition.Set(Input.mousePosition.x, Input.mousePosition.y);
			PointerEventData eventData = new PointerEventData(current)
			{
				pressPosition = curInputPressPosition,
				position = curInputPosition
			};
			List<RaycastResult> list = new List<RaycastResult>();
			current.RaycastAll(eventData, list);
			if (list.Count > 0)
			{
				if (list[0].gameObject.name == BtnReportName)
				{
					GameEntry.Event.Fire(EventId.ChatSendPhotoReport);
				}
				else if (list[0].gameObject.name == PhotoDefaultBgName || list[0].gameObject.name == UIPhotoName)
				{
					GameEntry.Event.Fire(EventId.ChatSendPhotoCloseBigPhotoView);
				}
			}
		});
	}

	public void SetInputControllerState(bool state)
	{
		photoViewer.SetInputControllerState(state);
	}
}
