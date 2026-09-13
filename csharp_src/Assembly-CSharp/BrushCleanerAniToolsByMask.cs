using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class BrushCleanerAniToolsByMask : MonoBehaviour
{
	public float AlphaMove;

	public float AlphaDetailRange = 10f;

	public bool IfTrun = true;

	public float _Angle;

	public float _Radius = 1.654562f;

	public float _Distance = 0.4433768f;

	public float _Backcolorintensity = 1f;

	public Graphic _Image;

	private void Update()
	{
		if (!(_Image == null))
		{
			Material materialForRendering = _Image.materialForRendering;
			materialForRendering.SetFloat("_Cutoff", AlphaMove);
			materialForRendering.SetFloat("_Cutoff1", AlphaDetailRange);
			materialForRendering.SetFloat("_Angle", _Angle);
			materialForRendering.SetFloat("_Radius", _Radius);
			materialForRendering.SetFloat("_Distance", _Distance);
			materialForRendering.SetFloat("_Backcolorintensity", _Backcolorintensity);
			if (IfTrun)
			{
				materialForRendering.SetFloat("_IfT", 1f);
			}
			else
			{
				materialForRendering.SetFloat("_IfT", 0f);
			}
		}
	}
}
