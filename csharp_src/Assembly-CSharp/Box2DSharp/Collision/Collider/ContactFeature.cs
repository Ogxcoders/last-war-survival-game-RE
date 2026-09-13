namespace Box2DSharp.Collision.Collider;

public struct ContactFeature
{
	public enum FeatureType : byte
	{
		Vertex,
		Face
	}

	public byte IndexA;

	public byte IndexB;

	public byte TypeA;

	public byte TypeB;
}
