using System.Runtime.CompilerServices;

namespace Box2DSharp.Collision;

public struct TreeNode
{
	public AABB AABB;

	public int Child1;

	public int Child2;

	public int Height;

	public object UserData;

	public bool Moved;

	public int Parent { get; set; }

	public int Next
	{
		get
		{
			return Parent;
		}
		set
		{
			Parent = value;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool IsLeaf()
	{
		return Child1 == -1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Reset()
	{
		AABB = default(AABB);
		Child1 = 0;
		Child2 = 0;
		Height = 0;
		UserData = null;
		Parent = 0;
	}
}
