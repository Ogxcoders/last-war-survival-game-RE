using System;
using System.Collections.Generic;
using System.Text;
using BitBenderGames;
using UnityEngine;

public class AutoAdjustScale : MonoBehaviourWrapped
{
	private static SceneInterface _world;

	private static Matrix4x4 worldToCameraMatrix = Matrix4x4.identity;

	private static Transform cameraTrans;

	private static HashSet<AutoAdjustScale> s_Nodes = new HashSet<AutoAdjustScale>();

	private static float _lastUpdateCameraHeight = float.MinValue;

	private static bool _dirtyEnabledNode = false;

	private bool standaloneScaleSpeed;

	private float minDepth;

	private float maxDepth;

	private float maxScaleValue;

	private Transform _transform;

	[NonSerialized]
	private bool isInitialized;

	private bool zScalable;

	private Transform Tran
	{
		get
		{
			if (_transform == null)
			{
				_transform = base.transform;
			}
			return _transform;
		}
	}

	private static void RefreshGlobalScale()
	{
		if ((!_dirtyEnabledNode && (double)Mathf.Abs(_lastUpdateCameraHeight - cameraTrans.position.y) < 0.0001) || SceneManager.World == null)
		{
			return;
		}
		_lastUpdateCameraHeight = cameraTrans.position.y;
		_dirtyEnabledNode = false;
		float lodCameraDistanceScale = SceneManager.World.LodCameraDistanceScale;
		Vector3 localScale = new Vector3(lodCameraDistanceScale, lodCameraDistanceScale, 1f);
		foreach (AutoAdjustScale s_Node in s_Nodes)
		{
			if (!(s_Node != null))
			{
				continue;
			}
			if (!s_Node.standaloneScaleSpeed)
			{
				if (s_Node.zScalable)
				{
					s_Node.Tran.localScale = Vector3.one * lodCameraDistanceScale;
				}
				else
				{
					s_Node.Tran.localScale = localScale;
				}
			}
			else
			{
				float num = Mathf.Lerp(1f, s_Node.maxScaleValue, (_lastUpdateCameraHeight - s_Node.minDepth) / (s_Node.maxDepth - s_Node.minDepth));
				s_Node.Tran.localScale = Vector3.one * num;
			}
		}
	}

	public void DoEnable()
	{
		if (isInitialized)
		{
			return;
		}
		isInitialized = true;
		if (s_Nodes.Count == 0 || _world == null)
		{
			_world = SceneManager.World;
			if (_world != null)
			{
				cameraTrans = Camera.main.transform;
				_world.AfterUpdate -= RefreshGlobalScale;
				_world.AfterUpdate += RefreshGlobalScale;
			}
			else
			{
				Debug.LogError("world is null when add AutoAdjustScale, go:" + base.gameObject.name);
			}
		}
		s_Nodes.Add(this);
		_dirtyEnabledNode = true;
	}

	private void OnEnable()
	{
		DoEnable();
	}

	public static string GetGameObjectPath(GameObject obj)
	{
		if (obj == null)
		{
			return string.Empty;
		}
		try
		{
			Transform parent = obj.transform.parent;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(parent.name);
			while (parent != null)
			{
				stringBuilder.Insert(0, parent.name + "/");
				parent = parent.parent;
			}
			return stringBuilder.ToString();
		}
		catch (Exception)
		{
			return obj.name;
		}
	}

	private void OnDisable()
	{
		DoDisable();
	}

	public void DoDisable()
	{
		if (!isInitialized)
		{
			return;
		}
		isInitialized = false;
		s_Nodes.Remove(this);
		if (s_Nodes.Count <= 0)
		{
			if (_world != null)
			{
				_world.AfterUpdate -= RefreshGlobalScale;
				_world = null;
			}
			else
			{
				Debug.LogError("world is null when remove AutoAdjustScale, go:" + GetGameObjectPath(base.gameObject));
			}
		}
	}

	public void SetStandaloneScaleSpeed(int minLod, int maxLod, float maxValue)
	{
		if (_world != null)
		{
			minDepth = _world.GetLodDistanceByLod(minLod);
			maxDepth = _world.GetLodDistanceByLod(maxLod);
			maxScaleValue = maxValue;
			standaloneScaleSpeed = true;
		}
	}

	public void SetZScalable(bool zscalable)
	{
		zScalable = zscalable;
	}
}
