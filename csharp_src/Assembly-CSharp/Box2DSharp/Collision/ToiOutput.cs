using Box2DSharp.Common;

namespace Box2DSharp.Collision;

public struct ToiOutput
{
	public enum ToiState
	{
		Unknown,
		Failed,
		Overlapped,
		Touching,
		Separated
	}

	public ToiState State;

	public FP Time;
}
