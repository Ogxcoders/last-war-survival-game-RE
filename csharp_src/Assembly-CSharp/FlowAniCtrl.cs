using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class FlowAniCtrl : MonoBehaviour
{
	public float _Tick;

	public float _Power;

	private static int _TickId = Shader.PropertyToID("_Tick");

	private static int _PowerId = Shader.PropertyToID("_Power");

	private Material mat;

	private void Awake()
	{
		Image component = GetComponent<Image>();
		if (component != null)
		{
			mat = component.material;
		}
	}

	private void Update()
	{
		if (!(mat == null))
		{
			mat.SetFloat(_TickId, _Tick);
			mat.SetFloat(_PowerId, _Power);
		}
	}

	private void OnDestroy()
	{
	}
}
