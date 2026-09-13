using UnityEngine;

public class RobotPathNode
{
	public enum NodeType
	{
		NORMAL,
		SMALL_CORNER,
		HUGE_CORNER,
		MID_CORNER
	}

	public Vector3 pos { get; }

	public NodeType type { get; }

	public RobotPathNode(Vector3 pos, NodeType type)
	{
		this.pos = pos;
		this.type = type;
	}

	public static bool IsSmallCorner(RobotPathNode now, RobotPathNode next)
	{
		if (now.type == NodeType.SMALL_CORNER)
		{
			return next.type == NodeType.SMALL_CORNER;
		}
		return false;
	}

	public static bool IsMidCorner(RobotPathNode now, RobotPathNode next)
	{
		if (now.type == NodeType.MID_CORNER)
		{
			return next.type == NodeType.MID_CORNER;
		}
		return false;
	}

	public static bool IsHugeCorner(RobotPathNode now, RobotPathNode next)
	{
		if (now.type == NodeType.HUGE_CORNER)
		{
			return next.type == NodeType.HUGE_CORNER;
		}
		return false;
	}
}
