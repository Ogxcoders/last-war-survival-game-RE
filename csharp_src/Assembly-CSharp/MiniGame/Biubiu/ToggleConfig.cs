using System;

namespace MiniGame.Biubiu;

[Serializable]
public class ToggleConfig : IConfig
{
	public enum ToggleType
	{
		Button = 1,
		RemoteSensing
	}

	public float Angle;

	public ToggleType Type;
}
