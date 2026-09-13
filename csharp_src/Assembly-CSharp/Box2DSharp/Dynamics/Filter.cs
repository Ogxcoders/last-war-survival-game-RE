namespace Box2DSharp.Dynamics;

public struct Filter
{
	public short GroupIndex;

	private ushort? _categoryBits;

	private ushort? _maskBits;

	public ushort CategoryBits
	{
		get
		{
			return _categoryBits ?? 1;
		}
		set
		{
			_categoryBits = value;
		}
	}

	public ushort MaskBits
	{
		get
		{
			return _maskBits ?? ushort.MaxValue;
		}
		set
		{
			_maskBits = value;
		}
	}
}
