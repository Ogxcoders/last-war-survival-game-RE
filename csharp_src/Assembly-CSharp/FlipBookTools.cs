using UnityEngine;

[ExecuteInEditMode]
public class FlipBookTools : MonoBehaviour
{
	public Material Material;

	public float _Angle;

	public float _Radius = 1.654562f;

	public float _Distance = 0.4433768f;

	public float _Backcolorintensity = 1f;

	private void Update()
	{
		Material.SetFloat("_Angle", _Angle);
		Material.SetFloat("_Radius", _Radius);
		Material.SetFloat("_Distance", _Distance);
		Material.SetFloat("_Backcolorintensity", _Backcolorintensity);
	}
}
