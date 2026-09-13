namespace UnityEngine.XR.WSA;

internal static class HolographicEmulationHelpers
{
	public const float k_DefaultBodyHeight = 1.776f;

	public const float k_DefaultHeadDiameter = 0.2319999f;

	public const float k_ForwardOffset = 0.0985f;

	public static Vector3 CalcExpectedCameraPosition(SimulatedHead head, SimulatedBody body)
	{
		Vector3 position = body.position;
		position.y += body.height - 1.776f;
		position.y -= head.diameter / 2f;
		position.y += 0.11599995f;
		Vector3 eulerAngles = head.eulerAngles;
		eulerAngles.y += body.rotation;
		Quaternion quaternion = Quaternion.Euler(eulerAngles);
		return position + quaternion * (0.0985f * Vector3.forward);
	}
}
