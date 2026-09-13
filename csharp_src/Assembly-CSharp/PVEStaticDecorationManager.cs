using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using GameFramework;
using Unity.Mathematics;
using UnityEngine;
using VEngine;

public class PVEStaticDecorationManager : PVEDecorationManagerBase
{
	public sealed class MatrixIndexedList<T> where T : class
	{
		private class LinearListNode
		{
			public int index;

			public T obj;
		}

		private List<LinearListNode> list;

		private LinearListNode head;

		private int count;

		public T this[int i]
		{
			get
			{
				if (i > 0 && i < count)
				{
					return list[i].obj;
				}
				return null;
			}
		}

		public int Count => count;

		public MatrixIndexedList(int capacity = 15)
		{
			list = new List<LinearListNode>(capacity);
			head = new LinearListNode
			{
				index = 0,
				obj = null
			};
			list.Add(head);
			list.Add(new LinearListNode
			{
				index = 1,
				obj = null
			});
			count = list.Count;
		}

		public void Clear()
		{
			list.Clear();
			head = new LinearListNode
			{
				index = 0,
				obj = null
			};
			list.Add(head);
			list.Add(new LinearListNode
			{
				index = 1,
				obj = null
			});
			count = list.Count;
		}

		public int Add(T obj)
		{
			int num = -1;
			if (head.index != 0)
			{
				num = head.index;
				list[num].obj = obj;
				head.index = list[num].index;
			}
			else
			{
				num = list.Count;
				list.Add(new LinearListNode
				{
					index = num,
					obj = obj
				});
				count = num + 1;
			}
			return num;
		}

		public T TryGetValue(int index)
		{
			if (index > 0 && index < count)
			{
				return list[index].obj;
			}
			return null;
		}

		public T Remove(int pos)
		{
			if (pos > 0 && pos < count)
			{
				T obj = list[pos].obj;
				if (obj == null)
				{
					return obj;
				}
				list[pos].obj = null;
				list[pos].index = head.index;
				head.index = pos;
				return obj;
			}
			return null;
		}

		public T Replace(int pos, T obj)
		{
			if (pos > 0 && pos < count)
			{
				T obj2 = list[pos].obj;
				list[pos].obj = obj;
				return obj2;
			}
			return null;
		}
	}

	public const string PVEDecorationNewBytePath = "Assets/Main/Prefabs/PVELevel/{0}/decorationNew.bytes";

	public const string PVEDecorationAssetPath = "Assets/Main/Prefabs/PVELevel/{0}/DecorationAsset/PVEDecoration.asset";

	public const int MAX_INST_PER_BATCH = 1023;

	private Dictionary<string, List<DecorationRenderMesh>> _renderAssetMap = new Dictionary<string, List<DecorationRenderMesh>>();

	private Dictionary<DecorationRenderMesh, Material> _renderMeshMaterialMap = new Dictionary<DecorationRenderMesh, Material>();

	private Dictionary<Vector2Int, List<WorldSceneDesc.ObjectDesc>> _chunks = new Dictionary<Vector2Int, List<WorldSceneDesc.ObjectDesc>>();

	private Dictionary<string, List<WorldSceneDesc.ObjectDesc>> _showObjMap = new Dictionary<string, List<WorldSceneDesc.ObjectDesc>>();

	private Matrix4x4[] _mInstanceTransform = new Matrix4x4[1023];

	private List<Vector2Int> _chunksToRemove = new List<Vector2Int>();

	private List<WorldSceneDesc.ObjectDesc> _tmpObjectDescList;

	private Dictionary<string, Asset> _tmpDecorationRenderDatAssetMap;

	private bool _enableInstancing;

	private PVEChunkGizmoDrawer _chunkGizmoDrawer;

	private float4x4 _vp;

	private bool _forceCalc;

	private Stack<List<Matrix4x4>> _matrixListStack = new Stack<List<Matrix4x4>>(64);

	private MatrixIndexedList<List<Matrix4x4>> _cacheMatrixIndexedList = new MatrixIndexedList<List<Matrix4x4>>(256);

	public override void Init(string descPath, int tileCountPerChunk, int createCountPerFrame)
	{
		InitLW(tileCountPerChunk, createCountPerFrame);
		if (!string.IsNullOrEmpty(descPath))
		{
			Append(descPath, 0f);
		}
	}

	public override void InitLW(int tileCountPerChunk, int createCountPerFrame)
	{
		parentNode = new GameObject("PVEStatic").transform;
		lastViewChunk = new Vector2Int(int.MinValue, int.MinValue);
		base.tileCountPerChunk = tileCountPerChunk;
		loadCount = 0;
		finishCount = 0;
		_enableInstancing = SystemInfo.supportsInstancing;
		_tmpDecorationRenderDatAssetMap = new Dictionary<string, Asset>(8);
		_chunks.Clear();
		_renderAssetMap.Clear();
	}

	public override void UnInit()
	{
		if (_tmpDecorationRenderDatAssetMap != null)
		{
			foreach (Asset value in _tmpDecorationRenderDatAssetMap.Values)
			{
				value.Release();
			}
			_tmpDecorationRenderDatAssetMap.Clear();
		}
		enableChunkUnload = false;
		ClearShowObjMap();
		_renderAssetMap.Clear();
		_renderMeshMaterialMap.Clear();
		_chunks.Clear();
		_chunksToRemove.Clear();
		UnityEngine.Object.Destroy(parentNode.gameObject);
		parentNode = null;
		_matrixListStack.Clear();
		_cacheMatrixIndexedList.Clear();
		base.UnInit();
	}

	public override void SetVisibleChunk(int range)
	{
		base.SetVisibleChunk(range);
	}

	public override void SetRenderOffsetZ(float renderOffsetZ)
	{
		base.SetRenderOffsetZ(renderOffsetZ);
		_forceCalc = true;
	}

	public override void Append(string sceneName, float offset)
	{
		LoadDecorationAsset(sceneName, offset);
	}

	private void LoadDecorationAsset(string sceneName, float offset)
	{
		string text = $"Assets/Main/Prefabs/PVELevel/{sceneName}/DecorationAsset/PVEDecoration.asset";
		if (!GameEntry.Resource.HasAsset(text))
		{
			Log.Error(text + "不存在");
			return;
		}
		if (_tmpDecorationRenderDatAssetMap.TryGetValue(text, out var value))
		{
			if (value.isDone && value.asset != null)
			{
				DecorationRenderData decorationRenderData = value.asset as DecorationRenderData;
				if (decorationRenderData != null)
				{
					LoadDecorationBytes(sceneName, offset, decorationRenderData);
				}
			}
			return;
		}
		loadCount++;
		Asset decorationRenderDatAsset = GameEntry.Resource.LoadAssetAsync(text, typeof(DecorationRenderData));
		_tmpDecorationRenderDatAssetMap[text] = decorationRenderDatAsset;
		Asset asset = decorationRenderDatAsset;
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate(Asset asset2)
		{
			if (asset2.asset == null || parentNode == null)
			{
				decorationRenderDatAsset.Release();
			}
			else
			{
				DecorationRenderData decorationRenderData2 = asset2.asset as DecorationRenderData;
				if (decorationRenderData2 == null)
				{
					decorationRenderDatAsset.Release();
				}
				else
				{
					DecorationRenderAsset[] renderAssets = decorationRenderData2.renderAssets;
					int i = 0;
					for (int num = renderAssets.Length; i < num; i++)
					{
						DecorationRenderAsset decorationRenderAsset = renderAssets[i];
						if (decorationRenderAsset == null)
						{
							Debug.LogError("装饰物引用资源丢失！！！meta文件异常");
						}
						else if (!_renderAssetMap.ContainsKey(decorationRenderAsset.assetPath))
						{
							List<DecorationRenderMesh> list = new List<DecorationRenderMesh>(4);
							_renderAssetMap[decorationRenderAsset.assetPath] = list;
							DecorationLodMesh[] lodMeshes = decorationRenderAsset.lodMeshes;
							int j = 0;
							for (int num2 = lodMeshes.Length; j < num2; j++)
							{
								DecorationLodMesh decorationLodMesh = lodMeshes[j];
								int[] lodRanges = decorationLodMesh.lodRanges;
								bool flag = false;
								if (lodRanges != null)
								{
									int num3 = lodRanges.Length;
									if (num3 == 0)
									{
										flag = true;
									}
									else
									{
										int k = 0;
										for (int num4 = num3; k < num4; k++)
										{
											if (lodRanges[k] == currentGrading)
											{
												flag = true;
											}
										}
									}
								}
								else
								{
									flag = true;
								}
								if (flag)
								{
									list.AddRange(decorationLodMesh.renderMeshes);
								}
							}
						}
					}
					LoadDecorationBytes(sceneName, offset, decorationRenderData2);
				}
			}
		});
	}

	private void LoadDecorationBytes(string sceneName, float offset, DecorationRenderData decorationRenderData)
	{
		string path = $"Assets/Main/Prefabs/PVELevel/{sceneName}/decorationNew.bytes";
		Asset req = GameEntry.Resource.LoadAssetAsync(path, typeof(TextAsset));
		Asset asset = req;
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
		{
			TextAsset textAsset = req.asset as TextAsset;
			if (textAsset != null)
			{
				finishCount++;
				_tmpObjectDescList = _tmpObjectDescList ?? new List<WorldSceneDesc.ObjectDesc>();
				_tmpObjectDescList.Clear();
				using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(textAsset.bytes)))
				{
					int num = binaryReader.ReadInt32();
					for (int i = 0; i < num; i++)
					{
						WorldSceneDesc.ObjectDesc objectDesc = null;
						objectDesc = ((_stack == null || _stack.Count <= 0) ? new WorldSceneDesc.ObjectDesc() : _stack.Pop());
						objectDesc.LoadPVEDecorationOptimize(binaryReader);
						objectDesc.id = ++_lwObjId;
						objectDesc.type = -1;
						_tmpObjectDescList.Add(objectDesc);
					}
				}
				for (int j = 0; j < _tmpObjectDescList.Count; j++)
				{
					WorldSceneDesc.ObjectDesc objectDesc2 = _tmpObjectDescList[j];
					Vector3 localPos = objectDesc2.localPos;
					objectDesc2.localPos = localPos + new Vector3(0f, 0f, offset);
					if (decorationRenderData != null)
					{
						for (int k = 0; k < decorationRenderData.renderAssets.Length; k++)
						{
							DecorationRenderAsset decorationRenderAsset = decorationRenderData.renderAssets[k];
							if (decorationRenderAsset != null && decorationRenderAsset.guid == objectDesc2.assetGuid)
							{
								objectDesc2.assetPath = decorationRenderAsset.assetPath;
								break;
							}
						}
					}
					objectDesc2.worldDecorationTilePos = TileCoord.WorldToTile(objectDesc2.localPos);
					Vector2Int key = TilePosToChunkCoord(objectDesc2.worldDecorationTilePos);
					if (_chunks.TryGetValue(key, out var value))
					{
						value.Add(objectDesc2);
					}
					else
					{
						value = new List<WorldSceneDesc.ObjectDesc> { objectDesc2 };
						_chunks.Add(key, value);
					}
				}
			}
			req.Release();
		});
	}

	public override void OnUpdate(int viewX, int viewY)
	{
		dataViewX = viewX;
		dataViewY = viewY;
		UpdateView(viewX, viewY);
		bool forceCalc = _forceCalc;
		_forceCalc = false;
		UpdateDrawBatch(forceCalc);
	}

	public override void OnUpdateData(int viewX, int viewY, int dataViewX, int dataViewY)
	{
		base.dataViewX = dataViewX;
		base.dataViewY = dataViewY;
		UpdateView(viewX, viewY);
		bool forceCalc = _forceCalc;
		_forceCalc = false;
		UpdateDrawBatch(forceCalc);
	}

	private void UpdateView(int viewX, int viewY)
	{
		if (loadCount > finishCount)
		{
			return;
		}
		Camera main = Camera.main;
		if (main == null)
		{
			return;
		}
		Matrix4x4 matrix4x = PerspectiveMatrix(main.fieldOfView + 15f, main.aspect, main.nearClipPlane, main.farClipPlane);
		_vp = matrix4x * main.worldToCameraMatrix;
		Vector2Int tilePos = new Vector2Int(dataViewX, dataViewY);
		Vector2Int vector2Int = TilePosToChunkCoord(tilePos);
		if (!(lastViewChunk != vector2Int) && lastVisibleChunkRange == visibleChunkRange)
		{
			return;
		}
		lastViewChunk = vector2Int;
		lastVisibleChunkRange = visibleChunkRange;
		int num = vector2Int.y - ((!enableChunkUnload) ? visibleChunkRange : 0);
		if (enableChunkUnload)
		{
			foreach (Vector2Int key2 in _chunks.Keys)
			{
				if (key2.y < num)
				{
					_chunksToRemove.Add(key2);
				}
			}
			if (_chunksToRemove.Count > 0)
			{
				for (int i = 0; i < _chunksToRemove.Count; i++)
				{
					Vector2Int key = _chunksToRemove[i];
					if (_chunks.TryGetValue(key, out var value))
					{
						foreach (WorldSceneDesc.ObjectDesc item in value)
						{
							int type = item.type;
							if (type >= 0)
							{
								List<Matrix4x4> list = _cacheMatrixIndexedList.Remove(type);
								if (list != null)
								{
									list.Clear();
									_matrixListStack.Push(list);
								}
							}
						}
						ClearChunkToMove(value);
					}
					_chunks.Remove(key);
				}
				_chunksToRemove.Clear();
			}
		}
		ClearShowObjMap();
		for (int j = ((!enableChunkUnload) ? (-visibleChunkRange) : 0); j <= visibleChunkRange; j++)
		{
			for (int k = -visibleChunkRange; k <= visibleChunkRange; k++)
			{
				Vector2Int chunkCoord = vector2Int + new Vector2Int(k, j);
				List<WorldSceneDesc.ObjectDesc> chunkObjList = GetChunkObjList(chunkCoord);
				if (chunkObjList != null)
				{
					for (int l = 0; l < chunkObjList.Count; l++)
					{
						WorldSceneDesc.ObjectDesc objDesc = chunkObjList[l];
						AddShowObjMap(objDesc);
					}
				}
			}
		}
	}

	private List<WorldSceneDesc.ObjectDesc> GetChunkObjList(Vector2Int chunkCoord)
	{
		if (_chunks.TryGetValue(chunkCoord, out var value))
		{
			return value;
		}
		return null;
	}

	private void AddShowObjMap(WorldSceneDesc.ObjectDesc objDesc)
	{
		if (!string.IsNullOrEmpty(objDesc.assetPath))
		{
			if (!_showObjMap.TryGetValue(objDesc.assetPath, out var value))
			{
				value = new List<WorldSceneDesc.ObjectDesc>();
				_showObjMap.Add(objDesc.assetPath, value);
			}
			value.Add(objDesc);
		}
	}

	private void ClearShowObjMap()
	{
		foreach (List<WorldSceneDesc.ObjectDesc> value in _showObjMap.Values)
		{
			value?.Clear();
		}
	}

	public void UpdateDrawBatch(bool force = false)
	{
		if (_showObjMap.Count == 0)
		{
			return;
		}
		foreach (KeyValuePair<string, List<WorldSceneDesc.ObjectDesc>> item in _showObjMap)
		{
			string key = item.Key;
			List<WorldSceneDesc.ObjectDesc> value = item.Value;
			if (_renderAssetMap.TryGetValue(key, out var value2))
			{
				DrawImp(value2, value, force);
			}
		}
	}

	private void DrawImp(List<DecorationRenderMesh> renderMeshes, List<WorldSceneDesc.ObjectDesc> showList, bool forceCalc)
	{
		if (renderMeshes == null)
		{
			return;
		}
		int count = showList.Count;
		int num = count / 1023;
		if (count % 1023 > 0)
		{
			num++;
		}
		int i = 0;
		for (int count2 = renderMeshes.Count; i < count2; i++)
		{
			int num2 = i;
			DecorationRenderMesh decorationRenderMesh = renderMeshes[num2];
			DecorationTransformInfo localTransformInfo = decorationRenderMesh.localTransformInfo;
			Vector3 pos = localTransformInfo.pos;
			Vector3 rotation = localTransformInfo.rotation;
			Vector3 scale = localTransformInfo.scale;
			Material renderMeshMaterial = GetRenderMeshMaterial(decorationRenderMesh);
			if (renderMeshMaterial == null)
			{
				continue;
			}
			for (int j = 0; j < num; j++)
			{
				int num3 = 1023 * j;
				int num4 = Mathf.Min(1023, count - num3);
				int num5 = 0;
				for (int k = 0; k < num4; k++)
				{
					int index = num3 + k;
					WorldSceneDesc.ObjectDesc objectDesc = showList[index];
					Vector3 localPos = objectDesc.localPos;
					float x = localPos.x;
					float y = localPos.y;
					float num6 = localPos.z + renderOffsetZ;
					if (!IsInViewport(x, y, num6 + checkInViewOffset, _vp))
					{
						continue;
					}
					List<Matrix4x4> list = null;
					int type = objectDesc.type;
					if (type < 0)
					{
						list = ((_matrixListStack.Count > 0) ? _matrixListStack.Pop() : new List<Matrix4x4>(4));
						objectDesc.type = _cacheMatrixIndexedList.Add(list);
					}
					else
					{
						list = _cacheMatrixIndexedList.TryGetValue(type);
						if (list == null)
						{
							continue;
						}
					}
					if (num2 < list.Count)
					{
						_mInstanceTransform[num5] = list[num2];
						num5++;
						continue;
					}
					Vector3 vector = new Vector3(x, y, num6);
					_ = Vector3.zero;
					Quaternion identity = Quaternion.identity;
					Vector3 one = Vector3.one;
					if (objectDesc.rotation != Vector3.zero || objectDesc.scale != Vector3.one)
					{
						Matrix4x4 matrix4x = Matrix4x4.TRS(vector, Quaternion.Euler(objectDesc.rotation), objectDesc.scale);
						Matrix4x4 matrix4x2 = Matrix4x4.TRS(pos, Quaternion.Euler(rotation), scale);
						Matrix4x4 matrix4x3 = matrix4x * matrix4x2;
						_mInstanceTransform[num5] = matrix4x3;
						list.Add(matrix4x3);
					}
					else
					{
						Vector3 pos2 = vector + pos;
						identity = Quaternion.Euler(objectDesc.rotation + rotation);
						Vector3 scale2 = objectDesc.scale;
						one = new Vector3(scale2.x * scale.x, scale2.y * scale.y, scale2.z * scale.z);
						Matrix4x4 matrix4x4 = Matrix4x4.TRS(pos2, identity, one);
						_mInstanceTransform[num5] = matrix4x4;
						list.Add(matrix4x4);
					}
					num5++;
				}
				if (num4 != 0 && num5 != 0)
				{
					ShowMesh(decorationRenderMesh, renderMeshMaterial, num5, _enableInstancing && renderMeshMaterial.enableInstancing);
				}
			}
		}
	}

	private void ShowMesh(DecorationRenderMesh renderMesh, Material material, int instancedCount, bool instanced)
	{
		Mesh mesh = renderMesh.mesh;
		if (instanced)
		{
			Graphics.DrawMeshInstanced(mesh, 0, material, _mInstanceTransform, instancedCount, null, renderMesh.shadowCastingMode, renderMesh.receiveShadows, renderMesh.layer);
			return;
		}
		for (int i = 0; i < instancedCount; i++)
		{
			Graphics.DrawMesh(mesh, _mInstanceTransform[i], material, renderMesh.layer, null, 0, null, renderMesh.shadowCastingMode, renderMesh.receiveShadows);
		}
	}

	private Material GetRenderMeshMaterial(DecorationRenderMesh decorationRenderMesh)
	{
		if (!_renderMeshMaterialMap.TryGetValue(decorationRenderMesh, out var value))
		{
			if (decorationRenderMesh.material == null)
			{
				_renderMeshMaterialMap.Add(decorationRenderMesh, null);
				return null;
			}
			Material material = UnityEngine.Object.Instantiate(decorationRenderMesh.material);
			_renderMeshMaterialMap.Add(decorationRenderMesh, material);
			return material;
		}
		return value;
	}

	public static Matrix4x4 PerspectiveMatrix(float fov, float aspect, float near, float far)
	{
		float num = fov * (MathF.PI / 180f);
		float num2 = 1f / Mathf.Tan(num / 2f);
		return new Matrix4x4
		{
			[0, 0] = num2 / aspect,
			[0, 1] = 0f,
			[0, 2] = 0f,
			[0, 3] = 0f,
			[1, 0] = 0f,
			[1, 1] = num2,
			[1, 2] = 0f,
			[1, 3] = 0f,
			[2, 0] = 0f,
			[2, 1] = 0f,
			[2, 2] = (far + near) / (near - far),
			[2, 3] = 2f * far * near / (near - far),
			[3, 0] = 0f,
			[3, 1] = 0f,
			[3, 2] = -1f,
			[3, 3] = 0f
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsInViewport(float pX, float pY, float pZ, float4x4 vp)
	{
		float num = vp.c0.w * pX + vp.c1.w * pY + vp.c2.w * pZ + vp.c3.w;
		if (num <= 0f)
		{
			return false;
		}
		float num2 = (vp.c0.x * pX + vp.c1.x * pY + vp.c2.x * pZ + vp.c3.x) / num;
		if (num2 < -1f || num2 > 1f)
		{
			return false;
		}
		float num3 = (vp.c0.y * pX + vp.c1.y * pY + vp.c2.y * pZ + vp.c3.y) / num;
		if (num3 < -1f || num3 > 1f)
		{
			return false;
		}
		float num4 = (vp.c0.z * pX + vp.c1.z * pY + vp.c2.z * pZ + vp.c3.z) / num;
		if (num4 > 0f)
		{
			return num4 < 1f;
		}
		return false;
	}
}
