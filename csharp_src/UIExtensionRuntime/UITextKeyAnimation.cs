using UnityEngine;

[ExecuteAlways]
public class UITextKeyAnimation : MonoBehaviour
{
	public static int key = Shader.PropertyToID("_Alpha");

	public float alpha = 1f;

	private Material mat;

	private SuperTextMesh textMesh;

	private void Awake()
	{
		textMesh = GetComponent<SuperTextMesh>();
	}

	private void Update()
	{
		if (textMesh != null)
		{
			alpha = Mathf.Clamp(alpha, 0f, 1f);
			textMesh.OnUpdate(alpha);
		}
	}
}
