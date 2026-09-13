using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class WorldGpuInstancingRenderer : IWorldGPUInstancingRenderer
{
	public class RenderGroup
	{
		private WorldGpuInstancingRenderer renderer;

		private Matrix4x4[] trsMatrixArray = new Matrix4x4[1023];

		private bool[] validArray = new bool[1023];

		private MaterialPropertyBlock mpb;

		public int DataCount { get; private set; }

		protected MaterialPropertyBlock MPB
		{
			get
			{
				if (mpb == null)
				{
					mpb = new MaterialPropertyBlock();
				}
				return mpb;
			}
		}

		public bool Renderable
		{
			get
			{
				if (renderer != null && renderer.material != null && renderer.mesh != null)
				{
					return DataCount > 0;
				}
				return false;
			}
		}

		protected virtual Color GizmosColor => Color.green;

		public void Init(WorldGpuInstancingRenderer renderer)
		{
			this.renderer = renderer;
			OnInit();
		}

		public void SetFlag(int dataIdx, bool hasValue)
		{
			if (validArray[dataIdx] != hasValue)
			{
				if (hasValue)
				{
					DataCount++;
				}
				else
				{
					DataCount--;
				}
				validArray[dataIdx] = hasValue;
				if (hasValue)
				{
					trsMatrixArray[dataIdx] = Matrix4x4.identity;
				}
			}
		}

		public int GetLastValueIndex()
		{
			if (DataCount <= 0)
			{
				return -1;
			}
			for (int num = 1022; num >= 0; num--)
			{
				if (validArray[num])
				{
					return num;
				}
			}
			return -2;
		}

		public void BeforeDraw()
		{
			OnBeforeDraw();
		}

		public void Draw()
		{
			if (DataCount > 0 && !(renderer.material == null) && !(renderer.mesh == null))
			{
				Graphics.DrawMeshInstanced(renderer.mesh, 0, renderer.material, trsMatrixArray, DataCount, mpb);
			}
		}

		public void Draw(CommandBuffer cmd)
		{
			cmd.DrawMeshInstanced(renderer.mesh, 0, renderer.material, 0, trsMatrixArray, DataCount, mpb);
		}

		public void Draw(Camera camera)
		{
			Graphics.DrawMeshInstanced(renderer.mesh, 0, renderer.material, trsMatrixArray, DataCount, mpb, ShadowCastingMode.Off, receiveShadows: false, 0, camera);
		}

		protected void UpdatePosition(int arrayIndex, Vector3 position)
		{
			Matrix4x4 matrix4x = trsMatrixArray[arrayIndex];
			matrix4x.m03 = position.x;
			matrix4x.m13 = position.y;
			matrix4x.m23 = position.z;
			trsMatrixArray[arrayIndex] = matrix4x;
		}

		protected void UpdateScale(int arrayIndex, Vector3 scale)
		{
			Matrix4x4 matrix4x = trsMatrixArray[arrayIndex];
			matrix4x.m00 = scale.x;
			matrix4x.m11 = scale.y;
			matrix4x.m22 = scale.z;
			trsMatrixArray[arrayIndex] = matrix4x;
		}

		protected void UpdateMatrixTRS(int arrayIndex, Matrix4x4 matrix4X4)
		{
			trsMatrixArray[arrayIndex] = matrix4X4;
		}

		public void UpdateData(int arrayIndex, object data)
		{
			OnDataUpdate(arrayIndex, data);
		}

		public void Dispose()
		{
			DataCount = 0;
			trsMatrixArray = null;
			validArray = null;
			OnDispose();
			mpb = null;
		}

		protected virtual void OnInit()
		{
		}

		protected virtual void OnBeforeDraw()
		{
		}

		protected virtual void OnDispose()
		{
		}

		protected virtual void OnDataUpdate(int arrayIndex, object data)
		{
		}

		public string Description()
		{
			return $"DataCount:{DataCount}";
		}

		public virtual void EditorOnDrawGizmos()
		{
			if (DataCount > 0)
			{
				Gizmos.color = GizmosColor;
				for (int i = 0; i < DataCount; i++)
				{
					Gizmos.matrix = trsMatrixArray[i];
					Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
				}
			}
		}

		public virtual void EditorOnDrawGizmosSelected()
		{
		}
	}

	public interface IHandle
	{
		int DataIndex { get; set; }
	}

	private string rendererName;

	private Mesh mesh;

	public Material material;

	private int minLod;

	private int maxLod;

	public const int GROUP_MAX_ITEM_COUNT = 1023;

	private List<RenderGroup> groups;

	private Func<RenderGroup> groupCreator;

	private List<int> idleIndexList;

	private Stack<int> tempIdleIndexStack;

	private Dictionary<int, IHandle> allHandles;

	private RenderPassEvent renderPassEvent;

	public int SortingOrder { get; set; }

	public string Name => rendererName;

	public RenderPassEvent RenderPassEvent => renderPassEvent;

	public bool IsRenderable
	{
		get
		{
			if (material == null)
			{
				return false;
			}
			if (groups == null || groups.Count <= 0)
			{
				return false;
			}
			int num = (SceneManager.World as WorldScene)?.CurrentLodLevel ?? 0;
			if (num < minLod || num > maxLod)
			{
				return false;
			}
			return true;
		}
	}

	public static WorldGpuInstancingRenderer Create(string name, Mesh mesh, Material material, Func<RenderGroup> groupCreator, RenderPassEvent renderPassEvent, int sortingOrder, int minLod, int maxLod)
	{
		WorldGpuInstancingRenderer worldGpuInstancingRenderer = new WorldGpuInstancingRenderer();
		worldGpuInstancingRenderer.rendererName = "[WorldInstancing]" + name;
		worldGpuInstancingRenderer.mesh = mesh;
		worldGpuInstancingRenderer.material = material;
		worldGpuInstancingRenderer.groupCreator = groupCreator;
		worldGpuInstancingRenderer.renderPassEvent = renderPassEvent;
		worldGpuInstancingRenderer.SortingOrder = sortingOrder;
		worldGpuInstancingRenderer.minLod = minLod;
		worldGpuInstancingRenderer.maxLod = maxLod;
		worldGpuInstancingRenderer.InitRenderer();
		return worldGpuInstancingRenderer;
	}

	private WorldGpuInstancingRenderer()
	{
	}

	private void InitRenderer()
	{
		idleIndexList = new List<int>(1024);
		tempIdleIndexStack = new Stack<int>(1024);
		allHandles = new Dictionary<int, IHandle>(1024);
		WorldInstancingRenderers.AddRenderer(this);
	}

	public void DisposeRenderer()
	{
		WorldInstancingRenderers.RemoveRenderer(this);
		ClearRenderData();
		if (material != null)
		{
			UnityEngine.Object.Destroy(material);
			material = null;
		}
	}

	private void ClearRenderData()
	{
		if (allHandles != null)
		{
			foreach (KeyValuePair<int, IHandle> allHandle in allHandles)
			{
				allHandle.Value.DataIndex = -1;
			}
			allHandles.Clear();
		}
		if (groups != null)
		{
			int i = 0;
			for (int count = groups.Count; i < count; i++)
			{
				groups[i].Dispose();
			}
			groups.Clear();
		}
		idleIndexList?.Clear();
		tempIdleIndexStack?.Clear();
		groupCreator = null;
	}

	private void CheckGroup()
	{
		RenderGroup renderGroup = groupCreator();
		renderGroup.Init(this);
		groups = groups ?? new List<RenderGroup>();
		groups.Add(renderGroup);
		int num = (groups.Count - 1) * 1023;
		for (int num2 = 1022; num2 >= 0; num2--)
		{
			idleIndexList.Add(num + num2);
		}
	}

	public void CreateIndex(IHandle handle)
	{
		if (handle.DataIndex >= 0)
		{
			Debug.LogError($"Create data index exception, handle index {handle.DataIndex} already >= 0 !");
			return;
		}
		int num = -1;
		if (tempIdleIndexStack.Count > 0)
		{
			num = tempIdleIndexStack.Pop();
		}
		if (num < 0)
		{
			if (idleIndexList.Count <= 0)
			{
				CheckGroup();
			}
			int index = idleIndexList.Count - 1;
			num = idleIndexList[index];
			idleIndexList.RemoveAt(index);
		}
		int index2 = num / 1023;
		int dataIdx = num % 1023;
		groups[index2].SetFlag(dataIdx, hasValue: true);
		if (allHandles.ContainsKey(handle.DataIndex))
		{
			Debug.LogError($"Create data index exception, handle index {num} Exist!");
			return;
		}
		handle.DataIndex = num;
		allHandles.Add(num, handle);
		UpdateData(num, handle);
	}

	public void ReleaseIndex(IHandle handle)
	{
		int dataIndex = handle.DataIndex;
		if (dataIndex >= 0)
		{
			if (!allHandles.Remove(dataIndex))
			{
				Debug.LogError($"Remove data index {dataIndex} failed!");
				return;
			}
			int index = dataIndex / 1023;
			int dataIdx = dataIndex % 1023;
			groups[index].SetFlag(dataIdx, hasValue: false);
			tempIdleIndexStack.Push(dataIndex);
			handle.DataIndex = -1;
		}
	}

	public void UpdateData(int dataIndex, object data)
	{
		if (dataIndex >= 0)
		{
			int num = dataIndex / 1023;
			int arrayIndex = dataIndex % 1023;
			if (num >= groups.Count)
			{
				Debug.LogError($"Update data by index {dataIndex} failed! Missing group by idx {num}");
			}
			else
			{
				groups[num].UpdateData(arrayIndex, data);
			}
		}
	}

	public void UpdateData(IHandle handle, object data)
	{
		UpdateData(handle.DataIndex, data);
	}

	public void UpdateData(IHandle handle)
	{
		UpdateData(handle.DataIndex, handle);
	}

	public void Draw(CommandBuffer cmd)
	{
		OptimizeGroups();
		int i = 0;
		for (int count = groups.Count; i < count; i++)
		{
			RenderGroup renderGroup = groups[i];
			if (renderGroup != null && renderGroup.Renderable)
			{
				renderGroup.BeforeDraw();
				renderGroup.Draw(cmd);
			}
		}
	}

	public void Draw(Camera camera)
	{
		OptimizeGroups();
		int i = 0;
		for (int count = groups.Count; i < count; i++)
		{
			RenderGroup renderGroup = groups[i];
			if (renderGroup != null && renderGroup.Renderable)
			{
				renderGroup.BeforeDraw();
				renderGroup.Draw(camera);
			}
		}
	}

	private void OptimizeGroups()
	{
		bool flag = false;
		while (tempIdleIndexStack.Count > 0)
		{
			int num = tempIdleIndexStack.Pop();
			int num2 = -1;
			for (int num3 = groups.Count - 1; num3 >= 0; num3--)
			{
				RenderGroup renderGroup = groups[num3];
				if (renderGroup.DataCount > 0)
				{
					int lastValueIndex = renderGroup.GetLastValueIndex();
					if (lastValueIndex >= 0)
					{
						num2 = num3 * 1023 + lastValueIndex;
						if (num2 <= num)
						{
							idleIndexList.Add(num);
							flag = true;
							break;
						}
						int index = num / 1023;
						int dataIdx = num % 1023;
						groups[index].SetFlag(dataIdx, hasValue: true);
						renderGroup.SetFlag(lastValueIndex, hasValue: false);
						if (allHandles.TryGetValue(num2, out var value))
						{
							allHandles.Remove(num2);
							allHandles[num] = value;
							value.DataIndex = num;
							UpdateData(num, value);
						}
						idleIndexList.Add(num2);
						flag = true;
						break;
					}
				}
			}
			if (num2 < 0)
			{
				idleIndexList.Add(num);
				flag = true;
			}
		}
		if (flag)
		{
			idleIndexList.Sort(Compare);
		}
	}

	private int Compare(int x, int y)
	{
		return y - x;
	}

	public string Description()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(rendererName + "[WorldGpuInstancingRenderer]");
		stringBuilder.AppendLine("passEvent=" + renderPassEvent);
		stringBuilder.AppendLine($"拥有组的数量:{groups?.Count ?? 0}");
		stringBuilder.AppendLine($"可用下标数量:{idleIndexList?.Count ?? 0}");
		stringBuilder.AppendLine($"临时可用下标数量:{tempIdleIndexStack?.Count ?? 0}");
		stringBuilder.AppendLine("-----");
		if (groups != null && groups.Count > 0)
		{
			for (int i = 0; i < groups.Count; i++)
			{
				stringBuilder.AppendLine($"{i}:{groups[i].Description()}");
			}
		}
		return stringBuilder.ToString();
	}

	public void OnGizmos()
	{
		if (groups == null)
		{
			return;
		}
		foreach (RenderGroup group in groups)
		{
			group.EditorOnDrawGizmos();
		}
	}

	public void OnGizmosSelected()
	{
		if (groups == null)
		{
			return;
		}
		foreach (RenderGroup group in groups)
		{
			group.EditorOnDrawGizmosSelected();
		}
	}
}
