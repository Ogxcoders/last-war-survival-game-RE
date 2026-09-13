using System.Collections.Generic;
using UnityEngine;

public class PVEDecorationManagerBase
{
	protected int tileCountPerChunk = 20;

	protected WorldSceneDesc sceneDesc;

	protected int dataViewX;

	protected int dataViewY;

	protected Transform parentNode;

	protected int loadCount;

	protected int finishCount;

	protected int visibleChunkRange = 1;

	protected bool enableChunkUnload;

	protected Vector2Int lastViewChunk;

	protected int lastVisibleChunkRange = 1;

	protected float renderOffsetZ;

	protected int currentGrading = 2;

	protected float checkInViewOffset;

	protected int _lwObjId;

	protected readonly Stack<WorldSceneDesc.ObjectDesc> _stack = new Stack<WorldSceneDesc.ObjectDesc>(64);

	public virtual void Init(string sceneName, int tileCountPerChunk, int createCountPerFrame)
	{
	}

	public virtual void InitLW(int tileCountPerChunk, int createCountPerFrame)
	{
	}

	public virtual void UnInit()
	{
		_stack.Clear();
	}

	public virtual void Append(string sceneName, float offset)
	{
	}

	public virtual void OnUpdate(int viewX, int viewY)
	{
	}

	public virtual void OnUpdateData(int viewX, int viewY, int dataViewX, int dataViewY)
	{
	}

	public virtual void SetVisibleChunk(int range)
	{
		visibleChunkRange = range;
	}

	public virtual void SetChunkUnloadEnable(bool enable)
	{
		enableChunkUnload = enable;
	}

	public virtual void SetRenderOffsetZ(float renderOffsetZ)
	{
		this.renderOffsetZ = renderOffsetZ;
	}

	public virtual void SetGrading(int grading, float offset = 0f)
	{
		currentGrading = grading;
		checkInViewOffset = offset;
	}

	protected Vector2Int TilePosToChunkCoord(Vector2Int tilePos)
	{
		return new Vector2Int(tilePos.x / tileCountPerChunk, tilePos.y / tileCountPerChunk);
	}

	protected void ClearChunkToMove(List<WorldSceneDesc.ObjectDesc> value)
	{
		foreach (WorldSceneDesc.ObjectDesc item in value)
		{
			item.Clear();
			_stack.Push(item);
		}
		value.Clear();
	}
}
