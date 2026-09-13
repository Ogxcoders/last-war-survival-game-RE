namespace UnityEngine.Rendering;

public struct SubMeshDescriptor
{
	public Bounds bounds { get; set; }

	public MeshTopology topology { get; set; }

	public int indexStart { get; set; }

	public int indexCount { get; set; }

	public int baseVertex { get; set; }

	public int firstVertex { get; set; }

	public int vertexCount { get; set; }

	public SubMeshDescriptor(int indexStart, int indexCount, MeshTopology topology = MeshTopology.Triangles)
	{
		this.indexStart = indexStart;
		this.indexCount = indexCount;
		this.topology = topology;
		bounds = default(Bounds);
		baseVertex = 0;
		firstVertex = 0;
		vertexCount = 0;
	}

	public override string ToString()
	{
		return $"(topo={topology} indices={indexStart},{indexCount} vertices={firstVertex},{vertexCount} basevtx={baseVertex} bounds={bounds})";
	}
}
