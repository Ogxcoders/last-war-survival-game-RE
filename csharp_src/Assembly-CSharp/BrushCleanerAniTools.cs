using UnityEngine;

[ExecuteInEditMode]
public class BrushCleanerAniTools : MonoBehaviour
{
	public Material Material;

	public float AlphaMove;

	public float AlphaDetailRange = 10f;

	public bool IfTrun = true;

	public float _Angle;

	public float _Radius = 1.654562f;

	public float _Distance = 0.4433768f;

	public float _Backcolorintensity = 1f;

	private void Update()
	{
		Material.SetFloat("_Cutoff", AlphaMove);
		Material.SetFloat("_Cutoff1", AlphaDetailRange);
		Material.SetFloat("_Angle", _Angle);
		Material.SetFloat("_Radius", _Radius);
		Material.SetFloat("_Distance", _Distance);
		Material.SetFloat("_Backcolorintensity", _Backcolorintensity);
		if (IfTrun)
		{
			Material.SetFloat("_IfT", 1f);
		}
		else
		{
			Material.SetFloat("_IfT", 0f);
		}
	}
}
