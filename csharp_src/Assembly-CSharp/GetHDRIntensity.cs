using UnityEngine;

[ExecuteAlways]
public class GetHDRIntensity : MonoBehaviour
{
	private int property = Shader.PropertyToID("_Color");

	private Material mat;

	[Range(0f, 10f)]
	public float intensity;

	[SerializeField]
	public Color color;

	public void Init(Material material)
	{
		mat = material;
		if (mat != null)
		{
			color = mat.GetColor(property);
		}
	}

	private void Update()
	{
		if (!(mat == null))
		{
			float num = Mathf.Pow(2f, intensity);
			Color value = new Color(color.r * num, color.g * num, color.b * num);
			mat.SetColor(property, value);
		}
	}

	private void OnDisable()
	{
		if (mat != null)
		{
			mat.SetColor(property, Color.white);
		}
	}
}
