using UnityEngine;
using UnityEngine.UI;

public class UIAnimationHelper : MonoBehaviour
{
	public Vector4 tillingAndOffset = new Vector4(1f, 1f, 0f, 0f);

	private Material mat;

	private int id = Shader.PropertyToID("_NoiseTex_ST");

	private void Awake()
	{
		if (GetComponent<Text>() == null)
		{
			mat = GetComponent<Image>().material;
		}
		else
		{
			mat = GetComponent<Text>().material;
		}
	}

	private void Update()
	{
		if (mat != null)
		{
			mat.SetVector("_ScaleAndOffset", tillingAndOffset);
		}
	}

	private void OnDestroy()
	{
	}
}
