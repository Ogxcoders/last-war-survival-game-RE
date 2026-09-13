using Google.Protobuf.Collections;
using Protobuf;

public class WorldAreaGreenInfo
{
	public enum GreenType
	{
		Default,
		Green,
		Desert
	}

	private RepeatedField<int> points;

	private GreenType type;

	public GreenType Type
	{
		get
		{
			return type;
		}
		set
		{
			type = value;
		}
	}

	public RepeatedField<int> Points
	{
		get
		{
			return points;
		}
		set
		{
			points = value;
		}
	}

	public WorldAreaGreenInfo(GreenPoints di)
	{
		points = di.Points;
		type = (GreenType)di.Type;
	}
}
