using System;
using System.Collections.Generic;
using UnityEngine;

namespace SuperScrollView;

public class DataSourceMgr : MonoBehaviour
{
	private List<ItemData> mItemDataList = new List<ItemData>();

	private Action mOnRefreshFinished;

	private Action mOnLoadMoreFinished;

	private int mLoadMoreCount = 20;

	private float mDataLoadLeftTime;

	private bool mIsWaittingRefreshData;

	private bool mIsWaitLoadingMoreData;

	public int mTotalDataCount = 10000;

	private static DataSourceMgr instance;

	public static DataSourceMgr Get
	{
		get
		{
			if (instance == null)
			{
				instance = UnityEngine.Object.FindObjectOfType<DataSourceMgr>();
			}
			return instance;
		}
	}

	public int TotalItemCount => mItemDataList.Count;

	private void Awake()
	{
		Init();
	}

	public void Init()
	{
		DoRefreshDataSource();
	}

	public ItemData GetItemDataByIndex(int index)
	{
		if (index < 0 || index >= mItemDataList.Count)
		{
			return null;
		}
		return mItemDataList[index];
	}

	public ItemData GetItemDataById(int itemId)
	{
		int count = mItemDataList.Count;
		for (int i = 0; i < count; i++)
		{
			if (mItemDataList[i].mId == itemId)
			{
				return mItemDataList[i];
			}
		}
		return null;
	}

	public void RequestRefreshDataList(Action onReflushFinished)
	{
		mDataLoadLeftTime = 1f;
		mOnRefreshFinished = onReflushFinished;
		mIsWaittingRefreshData = true;
	}

	public void RequestLoadMoreDataList(int loadCount, Action onLoadMoreFinished)
	{
		mLoadMoreCount = loadCount;
		mDataLoadLeftTime = 1f;
		mOnLoadMoreFinished = onLoadMoreFinished;
		mIsWaitLoadingMoreData = true;
	}

	public void Update()
	{
		if (mIsWaittingRefreshData)
		{
			mDataLoadLeftTime -= Time.deltaTime;
			if (mDataLoadLeftTime <= 0f)
			{
				mIsWaittingRefreshData = false;
				DoRefreshDataSource();
				if (mOnRefreshFinished != null)
				{
					mOnRefreshFinished();
				}
			}
		}
		else
		{
			if (!mIsWaitLoadingMoreData)
			{
				return;
			}
			mDataLoadLeftTime -= Time.deltaTime;
			if (mDataLoadLeftTime <= 0f)
			{
				mIsWaitLoadingMoreData = false;
				DoLoadMoreDataSource();
				if (mOnLoadMoreFinished != null)
				{
					mOnLoadMoreFinished();
				}
			}
		}
	}

	public void SetDataTotalCount(int count)
	{
		mTotalDataCount = count;
		DoRefreshDataSource();
	}

	private void DoRefreshDataSource()
	{
		mItemDataList.Clear();
		for (int i = 0; i < mTotalDataCount; i++)
		{
			ItemData itemData = new ItemData();
			itemData.mId = i;
			itemData.mName = "Item" + i;
			itemData.mDesc = "Item Desc For Item " + i;
			itemData.mIcon = ResManager.Get.GetSpriteNameByIndex(UnityEngine.Random.Range(0, 24));
			itemData.mStarCount = UnityEngine.Random.Range(0, 6);
			itemData.mFileSize = UnityEngine.Random.Range(20, 999);
			itemData.mChecked = false;
			itemData.mIsExpand = false;
			mItemDataList.Add(itemData);
		}
	}

	private void DoLoadMoreDataSource()
	{
		int count = mItemDataList.Count;
		for (int i = 0; i < mLoadMoreCount; i++)
		{
			int num = i + count;
			ItemData itemData = new ItemData();
			itemData.mId = num;
			itemData.mName = "Item" + num;
			itemData.mDesc = "Item Desc For Item " + num;
			itemData.mIcon = ResManager.Get.GetSpriteNameByIndex(UnityEngine.Random.Range(0, 24));
			itemData.mStarCount = UnityEngine.Random.Range(0, 6);
			itemData.mFileSize = UnityEngine.Random.Range(20, 999);
			itemData.mChecked = false;
			itemData.mIsExpand = false;
			mItemDataList.Add(itemData);
		}
	}

	public void CheckAllItem()
	{
		int count = mItemDataList.Count;
		for (int i = 0; i < count; i++)
		{
			mItemDataList[i].mChecked = true;
		}
	}

	public void UnCheckAllItem()
	{
		int count = mItemDataList.Count;
		for (int i = 0; i < count; i++)
		{
			mItemDataList[i].mChecked = false;
		}
	}

	public bool DeleteAllCheckedItem()
	{
		int count = mItemDataList.Count;
		mItemDataList.RemoveAll((ItemData it) => it.mChecked);
		return count != mItemDataList.Count;
	}
}
