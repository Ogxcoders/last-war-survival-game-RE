using UnityEngine;

[CreateAssetMenu(fileName = "BuildingGrowAnimationCurve", menuName = "ScriptableObjects/BuildingGrowAnimationCurve", order = 1)]
public class BuildingGrowAnimationCurve : ScriptableObject
{
	[Header("生长进度")]
	public AnimationCurve progressCurve;

	public AnimationCurve progressCurve1;

	[Header("扫光进度")]
	public AnimationCurve scanCurve;

	public AnimationCurve scanCurve1;

	[Header("格子进度")]
	public AnimationCurve gridCurve;

	public AnimationCurve gridCurve1;

	[Header("大玻璃罩进度")]
	public AnimationCurve glassCoverCurve;

	public AnimationCurve glassCoverCurve1;
}
