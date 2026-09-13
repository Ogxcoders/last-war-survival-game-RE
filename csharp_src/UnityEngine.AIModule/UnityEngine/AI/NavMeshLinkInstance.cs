namespace UnityEngine.AI;

public struct NavMeshLinkInstance
{
	public bool valid => id != 0 && NavMesh.IsValidLinkHandle(id);

	internal int id { get; set; }

	public Object owner
	{
		get
		{
			return NavMesh.InternalGetLinkOwner(id);
		}
		set
		{
			int ownerID = ((value != null) ? value.GetInstanceID() : 0);
			if (!NavMesh.InternalSetLinkOwner(id, ownerID))
			{
				Debug.LogError("Cannot set 'owner' on an invalid NavMeshLinkInstance");
			}
		}
	}

	public void Remove()
	{
		NavMesh.RemoveLinkInternal(id);
	}
}
