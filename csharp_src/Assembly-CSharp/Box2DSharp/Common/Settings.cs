namespace Box2DSharp.Common;

public static class Settings
{
	public static FP MaxFloat = FP.MaxValue;

	public static FP MinFloat = FP.MinValue;

	public static FP Epsilon = FP.Epsilon;

	public static FP Pi = FP.Pi;

	public static FP LengthUnitsPerMeter = FP.One;

	public const int MaxManifoldPoints = 2;

	public const int MaxPolygonVertices = 8;

	public static FP AABBExtension = (FP)0.1f * LengthUnitsPerMeter;

	public const int AABBMultiplier = 4;

	public static FP LinearSlop = (FP)0.005f * LengthUnitsPerMeter;

	public static FP AngularSlop = FP.PiTimes2 / 180;

	public static FP PolygonRadius = (FP)2 * LinearSlop;

	public const int MaxSubSteps = 8;

	public const int MaxToiContacts = 32;

	public static FP MaxLinearCorrection = (FP)0.2f * LengthUnitsPerMeter;

	public static FP MaxAngularCorrection = (FP)8 * Pi / 180;

	public static FP MaxTranslation = (FP)2 * LengthUnitsPerMeter;

	public static FP MaxTranslationSquared = MaxTranslation * MaxTranslation;

	public static FP MaxRotation = (FP)0.5 * Pi;

	public static FP MaxRotationSquared = MaxRotation * MaxRotation;

	public static FP Baumgarte = 0.2;

	public static FP ToiBaumgarte = 0.75;

	public static FP TimeToSleep = 0.5;

	public static FP LinearSleepTolerance = (FP)0.01 * LengthUnitsPerMeter;

	public static FP AngularSleepTolerance = FP.PiTimes2 / 180;
}
