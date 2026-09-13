using System.Collections.Generic;
using UnityEngine;

public class AutoFaceToCamera : MonoBehaviour
{
	private Quaternion _quaternion;

	private int _refreshIndex = -1;

	private static List<AutoFaceToCamera> _needRefreshInstList = new List<AutoFaceToCamera>(100);

	private static SceneInterface _curWorld = null;

	private bool ignoreCacheRotation;

	public bool IgnoreCacheRotation
	{
		set
		{
			ignoreCacheRotation = value;
		}
	}

	private void Awake()
	{
	}

	private void RefreshRotate()
	{
		Quaternion rotation = SceneManager.World.GetRotation();
		DoRefreshRotate(ref rotation);
	}

	private void DoRefreshRotate(ref Quaternion rotation)
	{
		if (ignoreCacheRotation)
		{
			base.transform.rotation = rotation;
		}
		else if (rotation != _quaternion)
		{
			_quaternion = rotation;
			base.transform.rotation = _quaternion;
		}
	}

	private static void BatchRefreshRotate()
	{
		int count = _needRefreshInstList.Count;
		if (count == 0)
		{
			SceneInterface world = SceneManager.World;
			if (world != null)
			{
				world.AfterUpdate -= BatchRefreshRotate;
			}
		}
		else
		{
			Quaternion rotation = SceneManager.World.GetRotation();
			for (int i = 0; i < count; i++)
			{
				_needRefreshInstList[i].DoRefreshRotate(ref rotation);
			}
		}
	}

	public void DoEnable()
	{
		_quaternion = Quaternion.identity;
		SceneInterface world = SceneManager.World;
		if (world == null)
		{
			return;
		}
		if (SceneManager.DISABLE_UNIFORM_EVENT_DISPATCH)
		{
			world.AfterUpdate += RefreshRotate;
			return;
		}
		if (_needRefreshInstList.Count == 0 || world != _curWorld)
		{
			if (world != _curWorld)
			{
				_needRefreshInstList.Clear();
			}
			world.AfterUpdate += BatchRefreshRotate;
			_curWorld = world;
		}
		_refreshIndex = _needRefreshInstList.Count;
		_needRefreshInstList.Add(this);
	}

	public void DoDisable()
	{
		SceneInterface world = SceneManager.World;
		if (world == null)
		{
			return;
		}
		if (SceneManager.DISABLE_UNIFORM_EVENT_DISPATCH)
		{
			world.AfterUpdate -= RefreshRotate;
		}
		else if (_refreshIndex >= 0)
		{
			int num = _needRefreshInstList.Count - 1;
			if (num == _refreshIndex)
			{
				_needRefreshInstList.RemoveAt(num);
			}
			else if (num > _refreshIndex)
			{
				AutoFaceToCamera autoFaceToCamera = _needRefreshInstList[num];
				_needRefreshInstList[num] = this;
				_needRefreshInstList[_refreshIndex] = autoFaceToCamera;
				_needRefreshInstList.RemoveAt(num);
				autoFaceToCamera._refreshIndex = _refreshIndex;
			}
			_refreshIndex = -1;
			if (_needRefreshInstList.Count == 0)
			{
				world.AfterUpdate -= BatchRefreshRotate;
				_curWorld = null;
			}
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

	private void OnDestroy()
	{
		DoDisable();
	}
}
