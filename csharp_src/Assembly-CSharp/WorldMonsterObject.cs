using System;
using UnityEngine;

public class WorldMonsterObject : WorldPointObject, WorldCulling.ICullingObject
{
	private const float BoundingSphereRadius = 2f;

	private BoundingSphere boundingSphere = new BoundingSphere(Vector3.zero, 2f);

	private int monsterId;

	private GameObject model;

	private GameObject icon;

	private UIWorldLabel label;

	private GPUSkinningAnimator[] anims;

	private long battleDefUuid;

	public int CullingBoundsIndex { get; set; } = -1;

	public WorldMonsterObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
	}

	public override void CreateGameObject()
	{
		base.CreateGameObject();
	}

	public override void OnUpdate(float deltaTime)
	{
	}

	public override void OnUpdateIconScale(Quaternion rot, float scale)
	{
	}

	public override void UpdateGameObject()
	{
		base.UpdateGameObject();
	}

	public override void Destroy()
	{
		world.RemoveCullingBounds(this);
		base.Destroy();
	}

	public void ShowBattleHurt(int hurt)
	{
		world.ShowBattleBlood(new BattleDecBloodTip.Param
		{
			startPos = base.WorldPosition,
			num = hurt,
			path = "Assets/Main/Prefabs/UI/BattleWord/BattleNormalBloodTip.prefab"
		});
	}

	public BoundingSphere GetBoundingSphere()
	{
		return boundingSphere;
	}

	public void OnCullingStateVisible(bool visible)
	{
		GPUSkinningAnimator[] array = anims;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Visible = visible;
		}
	}

	public long GetMarchStartTime()
	{
		throw new NotImplementedException();
	}

	public int[] GetMovePath()
	{
		throw new NotImplementedException();
	}

	public float GetSpeed()
	{
		return 0.3f * world.TileSize;
	}

	public Vector3 GetPosition()
	{
		if (gameObject != null)
		{
			return gameObject.transform.position;
		}
		return Vector3.zero;
	}

	public void SetPosition(Vector3 position)
	{
		if (gameObject != null)
		{
			gameObject.transform.position = position;
		}
	}

	public Vector3 GetTargetPosition()
	{
		throw new NotImplementedException();
	}

	public Vector4 GetPatrolArea()
	{
		return Vector4.zero;
	}

	public Quaternion GetRotation()
	{
		if (model != null)
		{
			return model.transform.rotation;
		}
		return Quaternion.identity;
	}

	public void SetRotation(Quaternion rotation)
	{
		if (model != null)
		{
			model.transform.rotation = rotation;
		}
	}

	public void CreateMarchLine()
	{
		throw new NotImplementedException();
	}

	public void DestroyMarchLine()
	{
		throw new NotImplementedException();
	}

	public void UpdateMarchLine(WorldTroopPathSegment[] path, int currPath, Vector3 currPos, bool clear = false)
	{
		throw new NotImplementedException();
	}

	public void PlayAnim(string animName)
	{
		for (int i = 0; i < anims.Length; i++)
		{
			anims[i].Play(animName);
		}
	}

	public void OnMarchMoveEnd()
	{
		throw new NotImplementedException();
	}
}
