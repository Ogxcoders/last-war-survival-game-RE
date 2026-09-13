using UnityEngine;

namespace BitBenderGames;

public class WrappedTouch
{
	public Vector3 Position { get; set; }

	public int FingerId { get; set; }

	public WrappedTouch()
	{
		FingerId = -1;
	}

	public static WrappedTouch FromTouch(Touch touch)
	{
		return new WrappedTouch
		{
			Position = touch.position,
			FingerId = touch.fingerId
		};
	}
}
