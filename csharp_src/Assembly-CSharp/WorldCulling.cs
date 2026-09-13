using System.Collections.Generic;
using UnityEngine;

public class WorldCulling : WorldManagerBase
{
	public interface ICullingObject
	{
		int CullingBoundsIndex { get; set; }

		BoundingSphere GetBoundingSphere();

		void OnCullingStateVisible(bool visible);
	}

	private CullingGroup cullingGroup;

	private GPUSkinningBetterList<BoundingSphere> cullingBounds = new GPUSkinningBetterList<BoundingSphere>(1000);

	private Dictionary<int, ICullingObject> objs = new Dictionary<int, ICullingObject>();

	public WorldCulling(WorldScene scene)
		: base(scene)
	{
	}

	public override void Init()
	{
		cullingGroup = new CullingGroup();
		cullingGroup.targetCamera = Camera.main;
		cullingGroup.SetBoundingDistances(new float[1] { float.PositiveInfinity });
		cullingGroup.SetDistanceReferencePoint(Camera.main.transform);
		cullingGroup.onStateChanged = OnCullingGroupStateChanged;
	}

	public override void UnInit()
	{
		cullingBounds.Release();
		objs.Clear();
		if (cullingGroup != null)
		{
			cullingGroup.Dispose();
			cullingGroup = null;
		}
	}

	public override void OnUpdate(float deltaTime)
	{
		base.OnUpdate(deltaTime);
		UpdateCullingBounds();
	}

	public void AddCullingBounds(ICullingObject cullingObject)
	{
		int key = (cullingObject.CullingBoundsIndex = cullingBounds.size);
		objs.Add(key, cullingObject);
		cullingBounds.Add(default(BoundingSphere));
		cullingGroup.SetBoundingSpheres(cullingBounds.buffer);
		cullingGroup.SetBoundingSphereCount(cullingBounds.size);
	}

	public void RemoveCullingBounds(ICullingObject cullingObject)
	{
		if (cullingObject != null && cullingObject.CullingBoundsIndex >= 0)
		{
			int key = cullingBounds.size - 1;
			ICullingObject cullingObject2 = objs[key];
			cullingObject2.CullingBoundsIndex = cullingObject.CullingBoundsIndex;
			objs[cullingObject2.CullingBoundsIndex] = cullingObject2;
			objs.Remove(key);
			cullingBounds.RemoveAt(cullingBounds.size - 1);
			cullingGroup.SetBoundingSphereCount(cullingBounds.size);
			cullingObject.CullingBoundsIndex = -1;
		}
	}

	private void UpdateCullingBounds()
	{
		foreach (ICullingObject value in objs.Values)
		{
			cullingBounds[value.CullingBoundsIndex] = value.GetBoundingSphere();
		}
	}

	private void OnCullingGroupStateChanged(CullingGroupEvent evt)
	{
		objs[evt.index].OnCullingStateVisible(evt.isVisible);
	}
}
