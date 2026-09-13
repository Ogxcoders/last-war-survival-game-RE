using System;
using FibMatrix;
using UnityEngine;

public class WorldTroopLineLittleSmart : IDisposable
{
	public class RendererGroup : WorldGpuInstancingRenderer.RenderGroup
	{
		private Vector4[] tranVecArray = new Vector4[1023];

		private Vector4[] colorArray = new Vector4[1023];

		private float[] speedArray = new float[1023];

		public static float scaler = 1f;

		public static RendererGroup Create()
		{
			return new RendererGroup();
		}

		protected override void OnDataUpdate(int arrayIndex, object data)
		{
			Line line = data as Line;
			Matrix4x4.Translate(line.startPos);
			colorArray[arrayIndex] = line.troopLineColor;
			tranVecArray[arrayIndex] = line.tranVec;
			speedArray[arrayIndex] = line.speed;
			UpdateMatrixTRS(arrayIndex, Matrix4x4.identity);
		}

		protected override void OnBeforeDraw()
		{
			base.MPB.SetVectorArray("_TranVec", tranVecArray);
			base.MPB.SetVectorArray("_LineColor", colorArray);
			base.MPB.SetFloatArray("_Speed", speedArray);
		}
	}

	public class Line : WorldGpuInstancingRenderer.IHandle
	{
		private static float globalTroopLineScale;

		private static float globalTroopLineAlpha;

		private static float globalTroopCircleScale;

		public int dataIndex = -1;

		public Vector3 startPos;

		public Vector3 targetPos;

		public Vector4 troopLineColor;

		public Vector4 tranVec;

		public float speed = 1f;

		public static float GlobalTroopLineScale
		{
			get
			{
				return globalTroopLineScale;
			}
			set
			{
				if (!Mathf.Approximately(globalTroopLineScale, value))
				{
					globalTroopLineScale = value;
					Shader.SetGlobalFloat("_GlobalTroopLineScale", globalTroopLineScale);
				}
			}
		}

		public static float GlobalTroopCircleScale
		{
			get
			{
				return globalTroopCircleScale;
			}
			set
			{
				if (!Mathf.Approximately(globalTroopCircleScale, value))
				{
					globalTroopCircleScale = value;
					Shader.SetGlobalFloat("_GlobalTroopCircleScale", globalTroopCircleScale);
				}
			}
		}

		public static float GlobalTroopLineAlpha
		{
			get
			{
				return globalTroopLineAlpha;
			}
			set
			{
				if (!Mathf.Approximately(globalTroopLineAlpha, value))
				{
					globalTroopLineAlpha = value;
					Shader.SetGlobalFloat("_GlobalTroopLineAlpha", globalTroopLineAlpha);
				}
			}
		}

		public int DataIndex
		{
			get
			{
				return dataIndex;
			}
			set
			{
				dataIndex = value;
			}
		}

		public void Refresh()
		{
			Vector3 vector = targetPos - startPos;
			vector.y = 0f;
			float magnitude = vector.magnitude;
			float z = Mathf.Atan2(vector.x, vector.z);
			tranVec.x = startPos.x;
			tranVec.y = startPos.z;
			tranVec.z = z;
			tranVec.w = magnitude;
		}
	}

	private static ObjectPool<WorldTroopLineLittleSmart> pool;

	private WorldIconRendererFacade.HappyIcon troopLineStart;

	private WorldIconRendererFacade.HappyIcon troopLineMid;

	private WorldIconRendererFacade.HappyIcon troopLineEnd;

	public WorldMarch marchInfo;

	private WorldGpuInstancingRenderer renderer;

	public long uuid;

	private Line[] lines = new Line[2];

	private WorldScene world;

	private const float DEFAULT_SPEED = 0.775f;

	private const int MAX_SPEED = 5;

	public Line[] Lines => lines;

	static WorldTroopLineLittleSmart()
	{
		pool = new ObjectPool<WorldTroopLineLittleSmart>(1024, () => new WorldTroopLineLittleSmart());
	}

	public static WorldTroopLineLittleSmart Create(WorldGpuInstancingRenderer renderer, long uuid)
	{
		WorldTroopLineLittleSmart worldTroopLineLittleSmart = pool.Allocate();
		worldTroopLineLittleSmart.uuid = uuid;
		worldTroopLineLittleSmart.world = SceneManager.World as WorldScene;
		worldTroopLineLittleSmart.renderer = renderer;
		return worldTroopLineLittleSmart;
	}

	public static void Recycle(WorldTroopLineLittleSmart troop)
	{
		if (troop != null)
		{
			pool.Recycle(troop);
		}
	}

	public void RefreshByMarch(WorldMarch marchInfo)
	{
		this.marchInfo = marchInfo;
		if (renderer != null)
		{
			RefreshLines();
		}
	}

	private void RefreshLines()
	{
		int num = 1;
		if (marchInfo.homePos > 0 && marchInfo.targetPos != marchInfo.homePos && marchInfo.startPos != marchInfo.homePos)
		{
			num = 2;
		}
		Vector4 lineColorVec = WorldTroopLineManager.GetLineColorVec4(marchInfo);
		Line line = lines[0];
		if (line == null)
		{
			line = new Line();
			renderer.CreateIndex(line);
			lines[0] = line;
		}
		line.startPos = marchInfo.startWorldPos;
		line.targetPos = marchInfo.targetWorldPos;
		line.troopLineColor = lineColorVec;
		line.speed = UpdateSpeed(marchInfo.speed);
		line.Refresh();
		renderer.UpdateData(line);
		if (troopLineStart == null)
		{
			troopLineStart = world?.IconRendererFacade?.CreateIcon("HappyTroopLinePoint");
			troopLineStart?.SetColorNoRefresh(lineColorVec);
		}
		troopLineStart?.Refresh(marchInfo.startWorldPos, 0);
		if (troopLineEnd == null)
		{
			troopLineEnd = world?.IconRendererFacade?.CreateIcon("HappyTroopLinePoint");
			troopLineEnd?.SetColorNoRefresh(lineColorVec);
		}
		troopLineEnd?.Refresh(marchInfo.targetWorldPos, 0);
		Line line2 = lines[1];
		if (num == 2)
		{
			if (line2 == null)
			{
				line2 = new Line();
				renderer.CreateIndex(line2);
				lines[1] = line2;
			}
			line2.startPos = marchInfo.homeWorldPos;
			line2.targetPos = marchInfo.startWorldPos;
			line2.troopLineColor = lineColorVec;
			line2.speed = UpdateSpeed(marchInfo.speed * 2f);
			line2.Refresh();
			renderer.UpdateData(line2);
			if (troopLineMid == null)
			{
				troopLineMid = world?.IconRendererFacade?.CreateIcon("HappyTroopLinePoint");
				troopLineMid?.SetColorNoRefresh(lineColorVec);
			}
			troopLineMid?.Refresh(marchInfo.homeWorldPos, 0);
		}
		else if (line2 != null && line2.dataIndex >= 0)
		{
			renderer.ReleaseIndex(line2);
			troopLineMid?.Destroy();
			troopLineMid = null;
		}
	}

	public float UpdateSpeed(float speed)
	{
		if (speed <= 0f)
		{
			speed = 0.775f;
		}
		return Mathf.Clamp(Mathf.RoundToInt(Mathf.Clamp(speed / 0.775f / 2f, 1f, 10f)), 0, 5);
	}

	public void Dispose()
	{
		int i = 0;
		for (int num = lines.Length; i < num; i++)
		{
			Line line = lines[i];
			if (line != null && line.dataIndex >= 0)
			{
				renderer?.ReleaseIndex(line);
			}
		}
		Array.Clear(lines, 0, lines.Length);
		troopLineStart?.Destroy();
		troopLineStart = null;
		troopLineMid?.Destroy();
		troopLineMid = null;
		troopLineEnd?.Destroy();
		troopLineEnd = null;
		marchInfo = null;
		world = null;
		renderer = null;
	}

	public void OnDrawGizmos()
	{
	}
}
