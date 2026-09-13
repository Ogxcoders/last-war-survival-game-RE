using Box2DSharp.Common;

namespace Box2DSharp.Dynamics;

public struct BodyDef
{
	private bool? _enabled;

	private bool? _allowSleep;

	public FP Angle;

	public FP AngularDamping;

	public FP AngularVelocity;

	private bool? _awake;

	public BodyType BodyType;

	public bool Bullet;

	public bool FixedRotation;

	private FP? _gravityScale;

	public FP LinearDamping;

	public FVector2 LinearVelocity;

	public FVector2 Position;

	public object UserData;

	public bool Enabled
	{
		get
		{
			return _enabled ?? true;
		}
		set
		{
			_enabled = value;
		}
	}

	public bool AllowSleep
	{
		get
		{
			return _allowSleep ?? true;
		}
		set
		{
			_allowSleep = value;
		}
	}

	public bool Awake
	{
		get
		{
			return _awake ?? true;
		}
		set
		{
			_awake = value;
		}
	}

	public FP GravityScale
	{
		get
		{
			return _gravityScale ?? ((FP)1);
		}
		set
		{
			_gravityScale = value;
		}
	}
}
