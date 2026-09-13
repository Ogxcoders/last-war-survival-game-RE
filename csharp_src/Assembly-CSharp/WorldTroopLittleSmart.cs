using System;
using FibMatrix;
using UnityEngine;

public class WorldTroopLittleSmart : IDisposable, WorldGpuInstancingRenderer.IHandle
{
	public class RendererGroup : WorldGpuInstancingRenderer.RenderGroup
	{
		private float iconScale = 1f / 13f;

		private Vector4[] uvInfoArray = new Vector4[1023];

		private Matrix4x4 anchorMatrix;

		private Matrix4x4 rotationMatrix;

		public static RendererGroup Create()
		{
			return new RendererGroup
			{
				anchorMatrix = Matrix4x4.Translate(new Vector3(-0.5f, 0f, -0.24f)),
				rotationMatrix = Matrix4x4.Rotate(Quaternion.Euler(-41f, 0f, 0f))
			};
		}

		protected override void OnDataUpdate(int arrayIndex, object data)
		{
			WorldTroopLittleSmart worldTroopLittleSmart = data as WorldTroopLittleSmart;
			Matrix4x4 matrix4x = Matrix4x4.Translate(worldTroopLittleSmart.position);
			Matrix4x4 matrix4x2 = Matrix4x4.Scale(new Vector3(1f, 1f, 1.24f));
			Matrix4x4 matrix4X = matrix4x * rotationMatrix * matrix4x2 * anchorMatrix;
			uvInfoArray[arrayIndex] = new Vector4(iconScale, 1f, (float)worldTroopLittleSmart.marchType * iconScale, 0f);
			UpdateMatrixTRS(arrayIndex, matrix4X);
		}

		protected override void OnBeforeDraw()
		{
			base.MPB.SetVectorArray("_UVInfo", uvInfoArray);
		}
	}

	public const int MARCH_LOST = 0;

	public const int MARCH_TYPE_DEFAULT = 10;

	public const int MARCH_PLAYER_SELF = 7;

	public const int MARCH_ALLIANCE = 2;

	public const int MARCH_MONSTER = 12;

	private static ObjectPool<WorldTroopLittleSmart> pool;

	private WorldMarch marchInfo;

	private int dataIndex = -1;

	public long uuid;

	private Vector3 position;

	private Vector3 startPosition;

	private Vector3 targetPosition;

	private Vector3 moveDir;

	private float speed;

	private long startTime;

	private long endTime;

	private int marchType;

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

	static WorldTroopLittleSmart()
	{
		pool = new ObjectPool<WorldTroopLittleSmart>(1024, () => new WorldTroopLittleSmart());
	}

	public static WorldTroopLittleSmart Create(long uuid)
	{
		WorldTroopLittleSmart worldTroopLittleSmart = pool.Allocate();
		worldTroopLittleSmart.uuid = uuid;
		return worldTroopLittleSmart;
	}

	public static void Recycle(WorldTroopLittleSmart troop)
	{
		if (troop != null)
		{
			pool.Recycle(troop);
		}
	}

	public void InitByMarch(long serverTime, WorldMarch marchInfo)
	{
		this.marchInfo = marchInfo;
		position = marchInfo.position;
		startPosition = marchInfo.startWorldPos;
		targetPosition = marchInfo.targetWorldPos;
		moveDir = (targetPosition - startPosition).normalized;
		speed = marchInfo.speed * 2f;
		startTime = marchInfo.startTime;
		endTime = marchInfo.endTime;
		RefreshMarchType();
	}

	public void RefreshByMarch(long serverTime, WorldMarch marchInfo)
	{
		this.marchInfo = marchInfo;
		startPosition = marchInfo.startWorldPos;
		targetPosition = marchInfo.targetWorldPos;
		moveDir = (targetPosition - startPosition).normalized;
		speed = marchInfo.speed * 2f;
		startTime = marchInfo.startTime;
		endTime = marchInfo.endTime;
		RefreshMarchType();
	}

	private void RefreshMarchType()
	{
		if (marchInfo == null)
		{
			marchType = 10;
			return;
		}
		if (marchInfo.type == NewMarchType.MUMMY)
		{
			marchType = 12;
			return;
		}
		if (marchInfo.isBroken)
		{
			marchType = 0;
			return;
		}
		if (marchInfo.ownerUid == GameEntry.Data.Player.Uid)
		{
			marchType = 7;
			return;
		}
		string allianceId = GameEntry.Data.Player.GetAllianceId();
		if (!string.IsNullOrEmpty(allianceId) && allianceId == marchInfo.allianceUid)
		{
			marchType = 2;
		}
		else
		{
			marchType = 10;
		}
	}

	public void Dispose()
	{
		marchInfo = null;
	}

	public void Update(long currentTime, float deltaTime)
	{
		if (currentTime >= endTime)
		{
			position = targetPosition;
		}
		else
		{
			position += moveDir * speed * deltaTime;
		}
	}

	public void OnDrawGizmos()
	{
	}
}
