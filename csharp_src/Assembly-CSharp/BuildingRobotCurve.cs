using UnityEngine;

[CreateAssetMenu(fileName = "BuildingRobotCurve", menuName = "ScriptableObjects/BuildingRobotCurve", order = 0)]
public class BuildingRobotCurve : ScriptableObject
{
	public AnimationCurve takeOffPosYCurve;

	public AnimationCurve landingPosYCurve;

	public AnimationCurve buildingMoveSpeedCurve;

	public AnimationCurve goTargetPosYCurve;

	public AnimationCurve goBackPosYCurve;

	public AnimationCurve approachTargetDisCurve;

	public AnimationCurve approachHomeDisCurve;
}
