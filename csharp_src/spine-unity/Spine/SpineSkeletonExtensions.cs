namespace Spine;

public static class SpineSkeletonExtensions
{
	public static bool IsWeighted(this VertexAttachment va)
	{
		if (va.Bones != null)
		{
			return va.Bones.Length != 0;
		}
		return false;
	}

	public static bool InheritsRotation(this TransformMode mode)
	{
		return ((ulong)mode & 1uL) == 0;
	}

	public static bool InheritsScale(this TransformMode mode)
	{
		return ((ulong)mode & 2uL) == 0;
	}
}
