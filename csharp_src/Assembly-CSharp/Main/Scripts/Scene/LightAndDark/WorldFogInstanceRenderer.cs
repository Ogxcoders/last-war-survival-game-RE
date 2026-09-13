using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Main.Scripts.Scene.LightAndDark;

public class WorldFogInstanceRenderer
{
	public class DrawMeshInstGraphic
	{
		public int layer;

		public Mesh mesh;

		public Material material;
	}

	public class DrawMeshInstancedBufferData
	{
		public DrawMeshInstancedBatch[] mainRenderData;

		public DrawMeshInstancedBufferData(List<DrawMeshInstGraphic> graphicInfos)
		{
			int count = graphicInfos.Count;
			mainRenderData = new DrawMeshInstancedBatch[count];
			for (int i = 0; i < count; i++)
			{
				DrawMeshInstGraphic graphicsInfo = graphicInfos[i];
				mainRenderData[i] = new DrawMeshInstancedBatch(graphicsInfo);
			}
		}

		public void ClearMainThreadRenderData()
		{
			DrawMeshInstancedBatch[] array = mainRenderData;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].ClearData();
			}
		}
	}

	public class DrawMeshInstancedBatch
	{
		public MaterialPropertyBlock propertyBlock;

		public DrawMeshInstGraphic graphicsInfo;

		public int bufferCount;

		public List<Matrix4x4[]> matrices { get; private set; }

		public List<Vector4[]>[] buffers { get; private set; }

		public int[] bufferIds { get; private set; }

		public int batchesIndex { get; private set; }

		public int numberInLastBatchIndex { get; private set; }

		public DrawMeshInstancedBatch(DrawMeshInstGraphic graphicsInfo)
		{
			this.graphicsInfo = graphicsInfo;
			propertyBlock = new MaterialPropertyBlock();
			matrices = new List<Matrix4x4[]>();
			batchesIndex = -1;
		}

		private void CheckCount()
		{
			if (batchesIndex == -1)
			{
				if (matrices == null)
				{
					matrices = new List<Matrix4x4[]>();
				}
				matrices.Add(DrawMeshInstancedBatchPool.GetMatrices());
				batchesIndex = 0;
			}
			int num = numberInLastBatchIndex + 1;
			numberInLastBatchIndex = num;
			if (numberInLastBatchIndex >= 511)
			{
				matrices.Add(DrawMeshInstancedBatchPool.GetMatrices());
				numberInLastBatchIndex = 0;
				num = batchesIndex + 1;
				batchesIndex = num;
			}
		}

		public void AddBuffer(Matrix4x4 matrix)
		{
			CheckCount();
			matrices[batchesIndex][numberInLastBatchIndex] = matrix;
		}

		public void AddBuffer(Matrix4x4 matrix, Vector4 buffer01)
		{
			CheckCount();
			matrices[batchesIndex][numberInLastBatchIndex] = matrix;
			buffers[0][batchesIndex][numberInLastBatchIndex] = buffer01;
		}

		public void SetPropertyBuffer(int index)
		{
			propertyBlock.Clear();
			for (int i = 0; i < bufferIds.Length; i++)
			{
				propertyBlock.SetVectorArray(bufferIds[i], buffers[i][index]);
			}
		}

		public void ClearData()
		{
			numberInLastBatchIndex = -1;
			batchesIndex = -1;
			if (matrices == null || matrices.Count <= 0)
			{
				return;
			}
			foreach (Matrix4x4[] matrix in matrices)
			{
				DrawMeshInstancedBatchPool.ReleaseMatrices(matrix);
			}
			matrices.Clear();
		}
	}

	public static class DrawMeshInstancedBatchPool
	{
		public static int buffersCount = 0;

		public static int matricesCount = 0;

		private static List<Vector4[]> bufferPool = new List<Vector4[]>();

		private static List<Matrix4x4[]> matricesPool = new List<Matrix4x4[]>();

		public static Vector4[] GetBufferArray()
		{
			if (bufferPool.Count == 0)
			{
				buffersCount += 511;
				return new Vector4[511];
			}
			Vector4[] result = bufferPool[0];
			bufferPool.RemoveAt(0);
			return result;
		}

		public static void ReleaseBufferArray(Vector4[] array)
		{
			bufferPool.Add(array);
		}

		public static Matrix4x4[] GetMatrices()
		{
			if (matricesPool.Count == 0)
			{
				matricesCount += 511;
				return new Matrix4x4[511];
			}
			Matrix4x4[] result = matricesPool[0];
			matricesPool.RemoveAt(0);
			return result;
		}

		public static void ReleaseMatrices(Matrix4x4[] array)
		{
			matricesPool.Add(array);
		}
	}

	protected const int MASK = 65535;

	public const int MatrixMaxCount = 511;

	public Material mBuildingBrushMaterial;

	public Material mMarchBrushMaterial;

	public Material mWorldFogMaterial;

	private int _maxX;

	private int _maxY;

	protected int _quadSize;

	private bool _hasData;

	private bool __rebuild;

	private int mBuildingBrushGraphicIndex;

	private int mDiscoBrushGraphicIndex = 1;

	private int mMarchBrushGraphicIndex = 2;

	public Mesh mFogBrushQuad;

	public DrawMeshInstGraphic mBrushGraphicData;

	public DrawMeshInstancedBufferData mBrushInstanceData;

	public Mesh mMarchBrushQuad;

	public DrawMeshInstGraphic mMarchBrushGraphicData;

	public string passId { get; protected set; }

	public virtual int blockSize { get; protected set; }

	public virtual int minCellSize { get; protected set; }

	public float scaler { get; protected set; }

	public float invScaler { get; protected set; }

	public void Setup()
	{
		mFogBrushQuad = CreateQuadBase(2);
		mBrushGraphicData = new DrawMeshInstGraphic();
		mBrushGraphicData.mesh = mFogBrushQuad;
		mBrushGraphicData.material = mBuildingBrushMaterial;
		mMarchBrushQuad = CreateQuadBase(2);
		mMarchBrushGraphicData = new DrawMeshInstGraphic();
		mMarchBrushGraphicData.mesh = mMarchBrushQuad;
		mMarchBrushGraphicData.material = mMarchBrushMaterial;
		List<DrawMeshInstGraphic> list = new List<DrawMeshInstGraphic>();
		list.Add(mBrushGraphicData);
		list.Add(mBrushGraphicData);
		list.Add(mMarchBrushGraphicData);
		mBuildingBrushGraphicIndex = 0;
		mDiscoBrushGraphicIndex = 1;
		mMarchBrushGraphicIndex = 2;
		mBrushInstanceData = new DrawMeshInstancedBufferData(list);
	}

	public void SetupBuildingBrushMaterial(Material material)
	{
		mBuildingBrushMaterial = material;
		mBrushInstanceData.mainRenderData[mBuildingBrushGraphicIndex].graphicsInfo.material = material;
		mBrushInstanceData.mainRenderData[mDiscoBrushGraphicIndex].graphicsInfo.material = material;
	}

	public void SetupMarchBrushMaterial(Material material)
	{
		mMarchBrushMaterial = material;
		mBrushInstanceData.mainRenderData[mMarchBrushGraphicIndex].graphicsInfo.material = material;
	}

	public void SetupFogMaterial(Material material)
	{
		mWorldFogMaterial = material;
	}

	public bool Ready()
	{
		if (mBrushInstanceData.mainRenderData.Length < 2)
		{
			return false;
		}
		if (mMarchBrushMaterial == null || mBuildingBrushMaterial == null || mWorldFogMaterial == null)
		{
			return false;
		}
		return true;
	}

	public bool TryCalMarchFogBrushMatrix()
	{
		Dictionary<long, LightDataManager.LightSourceData> mLightMarchDict = LightDataManager.GetInstance().mLightMarchDict;
		bool mMarchLightInstanceDirty = LightDataManager.GetInstance().mMarchLightInstanceDirty;
		bool mShowMarchLight = LightDataManager.GetInstance().mShowMarchLight;
		if ((mLightMarchDict == null || mLightMarchDict.Count == 0) && !mMarchLightInstanceDirty)
		{
			return false;
		}
		if (!mShowMarchLight && !mMarchLightInstanceDirty)
		{
			return false;
		}
		DrawMeshInstancedBatch drawMeshInstancedBatch = mBrushInstanceData.mainRenderData[mMarchBrushGraphicIndex];
		drawMeshInstancedBatch.ClearData();
		if (mShowMarchLight)
		{
			WorldMarchDataManager marchDataMgr = SceneManager.MarchDataMgr;
			foreach (KeyValuePair<long, LightDataManager.LightSourceData> item in mLightMarchDict)
			{
				LightDataManager.LightSourceData value = item.Value;
				Vector3 s = new Vector3(value.brushSize, 1f, value.brushSize);
				WorldMarch march = marchDataMgr.GetMarch(value.pointId);
				if (march != null)
				{
					Matrix4x4 matrix = Matrix4x4.TRS(march.position, value.rotation, s);
					drawMeshInstancedBatch.AddBuffer(matrix);
				}
			}
		}
		LightDataManager.GetInstance().mMarchLightInstanceDirty = false;
		return true;
	}

	public bool TryCalDiscoFogBrushMatrix()
	{
		Dictionary<long, LightDataManager.LightSourceData> mDiscoLightDataCache = LightDataManager.GetInstance().mDiscoLightDataCache;
		bool mDiscoLightDataDirty = LightDataManager.GetInstance().mDiscoLightDataDirty;
		if ((mDiscoLightDataCache == null || mDiscoLightDataCache.Count == 0) && !mDiscoLightDataDirty)
		{
			return false;
		}
		if (mDiscoLightDataDirty)
		{
			DrawMeshInstancedBatch drawMeshInstancedBatch = mBrushInstanceData.mainRenderData[mDiscoBrushGraphicIndex];
			drawMeshInstancedBatch.ClearData();
			foreach (KeyValuePair<long, LightDataManager.LightSourceData> item in mDiscoLightDataCache)
			{
				LightDataManager.LightSourceData value = item.Value;
				Matrix4x4 matrix = Matrix4x4.TRS(s: new Vector3(value.brushSize, 1f, value.brushSize), pos: value.worldPosVector3, q: Quaternion.identity);
				drawMeshInstancedBatch.AddBuffer(matrix);
			}
			LightDataManager.GetInstance().mDiscoLightDataDirty = false;
		}
		return true;
	}

	public bool TryCalFogBrushMatrix()
	{
		Dictionary<long, LightDataManager.LightSourceData> mLightSourceDataCache = LightDataManager.GetInstance().mLightSourceDataCache;
		bool mLightInstanceDirty = LightDataManager.GetInstance().mLightInstanceDirty;
		if ((mLightSourceDataCache == null || mLightSourceDataCache.Count == 0) && !mLightInstanceDirty)
		{
			return false;
		}
		if (mLightInstanceDirty)
		{
			DrawMeshInstancedBatch drawMeshInstancedBatch = mBrushInstanceData.mainRenderData[mBuildingBrushGraphicIndex];
			drawMeshInstancedBatch.ClearData();
			foreach (KeyValuePair<long, LightDataManager.LightSourceData> item in mLightSourceDataCache)
			{
				LightDataManager.LightSourceData value = item.Value;
				Matrix4x4 matrix = Matrix4x4.TRS(s: new Vector3(value.brushSize, 1f, value.brushSize), pos: value.worldPosVector3, q: Quaternion.identity);
				drawMeshInstancedBatch.AddBuffer(matrix);
			}
			LightDataManager.GetInstance().mLightInstanceDirty = false;
		}
		return true;
	}

	public void DrawFogBrushRT(CommandBuffer cmd)
	{
		DrawMeshInstancedBatch[] mainRenderData = mBrushInstanceData.mainRenderData;
		foreach (DrawMeshInstancedBatch drawMeshInstancedBatch in mainRenderData)
		{
			int num = drawMeshInstancedBatch.batchesIndex + 1;
			if (num == 0)
			{
				continue;
			}
			for (int j = 0; j < num; j++)
			{
				bool flag = false;
				if (j == drawMeshInstancedBatch.batchesIndex)
				{
					flag = true;
				}
				int num2 = 0;
				cmd.DrawMeshInstanced(count: (!flag) ? 511 : (drawMeshInstancedBatch.numberInLastBatchIndex + 1), mesh: drawMeshInstancedBatch.graphicsInfo.mesh, submeshIndex: 0, material: drawMeshInstancedBatch.graphicsInfo.material, shaderPass: 0, matrices: drawMeshInstancedBatch.matrices[j]);
			}
		}
	}

	private void AddQuad(List<Vector3> vertices, List<Vector2> uvs, List<int> triangles, Vector2 posMin, Vector2 posMax, Vector2 uvMin, Vector2 uvMax)
	{
		int count = vertices.Count;
		vertices.Add(new Vector3(posMin.x, 0f, posMin.y));
		vertices.Add(new Vector3(posMax.x, 0f, posMin.y));
		vertices.Add(new Vector3(posMin.x, 0f, posMax.y));
		vertices.Add(new Vector3(posMax.x, 0f, posMax.y));
		uvs.Add(new Vector2(uvMin.x, uvMin.y));
		uvs.Add(new Vector2(uvMax.x, uvMin.y));
		uvs.Add(new Vector2(uvMin.x, uvMax.y));
		uvs.Add(new Vector2(uvMax.x, uvMax.y));
		triangles.Add(count);
		triangles.Add(count + 3);
		triangles.Add(count + 1);
		triangles.Add(count + 3);
		triangles.Add(count);
		triangles.Add(count + 2);
	}

	private Mesh CreateQuadBase(int width)
	{
		Mesh mesh = new Mesh();
		Vector2 posMin = new Vector2((float)(-width) * 0.5f, (float)(-width) * 0.5f);
		Vector2 posMax = new Vector2((float)width * 0.5f, (float)width * 0.5f);
		List<Vector3> vertices = new List<Vector3>(4);
		List<Color> list = new List<Color>(4);
		List<Vector2> uvs = new List<Vector2>(4);
		List<int> list2 = new List<int>(4);
		AddQuad(vertices, uvs, list2, posMin, posMax, new Vector2(0f, 0f), new Vector2(1f, 1f));
		list.Add(new Color(1f, 0f, 0f, 0f));
		list.Add(new Color(0f, 1f, 0f, 0f));
		list.Add(new Color(0f, 0f, 1f, 0f));
		list.Add(new Color(0f, 0f, 0f, 1f));
		mesh.SetVertices(vertices);
		mesh.SetColors(list);
		mesh.SetUVs(0, uvs);
		mesh.SetIndices(list2, MeshTopology.Triangles, 0);
		return mesh;
	}

	private void AddQuadColor(List<Color> colors, Color color)
	{
		colors.Add(color);
		colors.Add(color);
		colors.Add(color);
		colors.Add(color);
	}

	public void DrawGizmos()
	{
	}
}
