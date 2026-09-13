using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class ShMapping : MonoBehaviour
{
	[SerializeField]
	public SphericalHarmonicsL2 ambientProbe;

	private int _Boyan_SHAr = Shader.PropertyToID("_Boyan_SHAr");

	private int _Boyan_SHAg = Shader.PropertyToID("_Boyan_SHAg");

	private int _Boyan_SHAb = Shader.PropertyToID("_Boyan_SHAb");

	private int _Boyan_SHBr = Shader.PropertyToID("_Boyan_SHBr");

	private int _Boyan_SHBg = Shader.PropertyToID("_Boyan_SHBg");

	private int _Boyan_SHBb = Shader.PropertyToID("_Boyan_SHBb");

	private int _Boyan_SHC = Shader.PropertyToID("_Boyan_SHC");

	private List<Vector4> CalculateSHVairentMimicUnity(SphericalHarmonicsL2 sh)
	{
		List<Vector4> list = new List<Vector4>();
		for (int i = 0; i < 3; i++)
		{
			list.Add(new Vector4
			{
				x = sh[i, 3],
				y = sh[i, 1],
				z = sh[i, 2],
				w = sh[i, 0] - sh[i, 6]
			});
		}
		for (int j = 0; j < 3; j++)
		{
			list.Add(new Vector4
			{
				x = sh[j, 4],
				y = sh[j, 5],
				z = sh[j, 6] * 3f,
				w = sh[j, 7]
			});
		}
		list.Add(new Vector4
		{
			x = sh[0, 8],
			y = sh[1, 8],
			z = sh[2, 8],
			w = 1f
		});
		return list;
	}

	private void OnEnable()
	{
		List<Vector4> list = CalculateSHVairentMimicUnity(ambientProbe);
		new Vector4(ambientProbe[0, 3], ambientProbe[0, 1], ambientProbe[0, 2], ambientProbe[0, 0]);
		new Vector4(ambientProbe[1, 3], ambientProbe[1, 1], ambientProbe[1, 2], ambientProbe[1, 0]);
		new Vector4(ambientProbe[2, 3], ambientProbe[2, 1], ambientProbe[2, 2], ambientProbe[2, 0]);
		new Vector4(ambientProbe[0, 4], ambientProbe[0, 5], ambientProbe[0, 6], ambientProbe[0, 7]);
		new Vector4(ambientProbe[1, 4], ambientProbe[1, 5], ambientProbe[1, 6], ambientProbe[1, 7]);
		new Vector4(ambientProbe[2, 4], ambientProbe[2, 5], ambientProbe[2, 6], ambientProbe[2, 7]);
		new Vector4(ambientProbe[0, 8], ambientProbe[1, 8], ambientProbe[2, 8], 1f);
		Shader.SetGlobalVector(_Boyan_SHAr, list[0]);
		Shader.SetGlobalVector(_Boyan_SHAg, list[1]);
		Shader.SetGlobalVector(_Boyan_SHAb, list[2]);
		Shader.SetGlobalVector(_Boyan_SHBr, list[3]);
		Shader.SetGlobalVector(_Boyan_SHBg, list[4]);
		Shader.SetGlobalVector(_Boyan_SHBb, list[5]);
		Shader.SetGlobalVector(_Boyan_SHC, list[6]);
	}
}
