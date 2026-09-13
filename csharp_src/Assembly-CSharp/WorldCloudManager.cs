using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldCloudManager : WorldManagerBase
{
	private class WorldCloud
	{
		private InstanceRequest request;

		private Transform _transform;

		private string _path;

		private Vector3 _startPos;

		private float _scale;

		private float _duration;

		private Transform _parent;

		private bool _valid;

		private const float SPEED = 0.3f;

		private const float DURATION_MIN = 3f;

		private const float DURATION_MAX = 5f;

		public WorldCloud()
		{
		}

		public WorldCloud(string path, Vector3 pos, float scale)
		{
			_path = path;
			_startPos = pos;
			_scale = scale;
			_duration = UnityEngine.Random.Range(3f, 5f);
			_valid = false;
		}

		public void Init(string path, Vector3 pos, float scale)
		{
			_path = path;
			_startPos = pos;
			_scale = scale;
			_duration = 5f;
			_valid = false;
		}

		public void Load(Transform parent)
		{
			_parent = parent;
			request = GameEntry.Resource.InstantiateAsync(_path);
			request.completed += OnRequestCompleted;
		}

		public void Unload()
		{
			request?.Destroy();
			request = null;
			_transform = null;
			_parent = null;
			_valid = false;
		}

		private void OnRequestCompleted(InstanceRequest obj)
		{
			_valid = true;
			if (!(request.gameObject == null))
			{
				_transform = request.gameObject.transform;
				Transform transform = _transform;
				transform.SetParent(_parent, worldPositionStays: false);
				transform.localPosition = _startPos;
				transform.localRotation = Quaternion.identity;
				transform.localScale = Vector3.one * _scale;
			}
		}

		public bool Tick(float deltaTime)
		{
			if (!_valid)
			{
				return false;
			}
			Vector3 localPosition = _transform.localPosition;
			localPosition += Vector3.right * 0.3f * deltaTime;
			_transform.localPosition = localPosition;
			_duration -= deltaTime;
			return _duration <= 0f;
		}

		public void Show()
		{
			if (_transform != null)
			{
				_transform.gameObject.SetActive(value: true);
			}
		}

		public void Hide()
		{
			if (_transform != null)
			{
				_transform.gameObject.SetActive(value: false);
			}
		}
	}

	private static SimplePool<WorldCloud> _cloudPool = new SimplePool<WorldCloud>(null, null);

	private Transform _parentNode;

	private readonly List<WorldCloud> _clouds = new List<WorldCloud>(8);

	private string[] _cloudPaths;

	private int _cloudPathLength;

	private bool _lastVisible;

	private float _nextCreateTime;

	private float _lastLodDistance;

	private float _pinchTimer;

	private static float _createCloudLOD = 200f;

	private const float CREATE_TIME_MIN = 2f;

	private const float CREATE_TIME_MAX = 4f;

	private const float SCALE_RATIO = 0.005f;

	private const int CREATE_COUNT_MIN = 2;

	private const int CREATE_COUNT_MAX = 4;

	private const float POS_RATIO = 0.5f;

	private const float AUTO_CREATE_PINCH_TIME = 0.3f;

	private const float SELF_SCALE_MIN = 0.8f;

	private const float SELF_SCALE_MAX = 1.2f;

	public WorldCloudManager(WorldScene scene)
		: base(scene)
	{
	}

	public override void Init()
	{
		_parentNode = new GameObject("Cloud").transform;
		_parentNode.transform.SetParent(world.Transform, worldPositionStays: false);
		_cloudPaths = new string[4] { "Assets/Main/Prefabs/World/Eff_daditu_yun_01.prefab", "Assets/Main/Prefabs/World/Eff_daditu_yun_02.prefab", "Assets/Main/Prefabs/World/Eff_daditu_yun_03.prefab", "Assets/Main/Prefabs/World/Eff_daditu_yun_04.prefab" };
		_cloudPathLength = _cloudPaths.Length;
		_nextCreateTime = 0f;
		_lastLodDistance = world.GetLodDistance();
		_pinchTimer = 0f;
		int[] lodArray = WorldCamera.LodArray;
		if (lodArray.Length > 3)
		{
			_createCloudLOD = lodArray[2];
		}
		else
		{
			_createCloudLOD = 200f;
		}
	}

	public override void OnUpdate(float deltaTime)
	{
		float lodDistance = world.GetLodDistance();
		float num = lodDistance - _createCloudLOD;
		bool flag = num > 0f;
		bool num2 = Math.Abs(_lastLodDistance - lodDistance) > float.Epsilon;
		_lastLodDistance = lodDistance;
		if (num2)
		{
			_ = _pinchTimer;
			_pinchTimer += deltaTime;
		}
		else
		{
			_pinchTimer = 0f;
		}
		bool num3 = flag != _lastVisible;
		_lastVisible = flag;
		_nextCreateTime -= deltaTime;
		if (num3)
		{
			if (flag)
			{
				ShowCloud();
			}
			else
			{
				HideCloud();
			}
		}
		if (flag)
		{
			if (_nextCreateTime <= 0f || _pinchTimer >= 0.3f)
			{
				float num4 = 1f + num * 0.005f;
				num4 *= UnityEngine.Random.Range(0.8f, 1.2f);
				CreateCloud(num4);
			}
			TickCloud(deltaTime);
		}
	}

	private void TickCloud(float deltaTime)
	{
		for (int num = _clouds.Count - 1; num >= 0; num--)
		{
			WorldCloud worldCloud = _clouds[num];
			if (worldCloud.Tick(deltaTime))
			{
				worldCloud.Unload();
				_clouds.RemoveAt(num);
				_cloudPool.Release(worldCloud);
			}
		}
	}

	private void ShowCloud()
	{
		int i = 0;
		for (int count = _clouds.Count; i < count; i++)
		{
			_clouds[i].Show();
		}
	}

	private void HideCloud()
	{
		int i = 0;
		for (int count = _clouds.Count; i < count; i++)
		{
			_clouds[i].Hide();
		}
	}

	private void CreateCloud(float scale)
	{
		_nextCreateTime = UnityEngine.Random.Range(2f, 4f);
		_pinchTimer = float.Epsilon;
		WorldCamera camera = world.Camera;
		Vector3 position = camera.GetPosition();
		Vector3 vector = camera.cameraAnchor[0];
		Vector3 vector2 = camera.cameraAnchor[2];
		int num = UnityEngine.Random.Range(2, 4);
		for (int i = 0; i < num; i++)
		{
			float t = UnityEngine.Random.Range(0f, 1f);
			float t2 = UnityEngine.Random.Range(0f, 1f);
			float x = Mathf.Lerp(vector.x, vector2.x, t);
			float z = Mathf.Lerp(vector.z, vector2.z, t2);
			Vector3 b = new Vector3(x, vector.y, z);
			Vector3 pos = Vector3.Lerp(position, b, 0.5f);
			int num2 = UnityEngine.Random.Range(0, _cloudPathLength);
			string path = _cloudPaths[num2];
			WorldCloud worldCloud = _cloudPool.Get();
			worldCloud.Init(path, pos, scale);
			_clouds.Add(worldCloud);
			worldCloud.Load(_parentNode);
		}
	}

	public override void UnInit()
	{
		_parentNode = null;
		int i = 0;
		for (int count = _clouds.Count; i < count; i++)
		{
			WorldCloud worldCloud = _clouds[i];
			worldCloud.Unload();
			_cloudPool.Release(worldCloud);
		}
		_clouds.Clear();
	}
}
