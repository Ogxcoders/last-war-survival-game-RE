using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class CustomSpherical : MonoBehaviour
{
	[SerializeField]
	public SphericalHarmonicsL2 l2;

	private static readonly string[] UnitySHParamNames = new string[7] { "_Boyan_SHAr", "_Boyan_SHAg", "_Boyan_SHAb", "_Boyan_SHBr", "_Boyan_SHBg", "_Boyan_SHBb", "_Boyan_SHC" };

	private void Awake()
	{
		DumpSphericalHarmonicsL2(l2);
	}

	public void Bake()
	{
		l2 = RenderSettings.ambientProbe;
	}

	private static void Print(string key, float a, float b, float c, float d)
	{
		Debug.LogError($"{key}=Vector4:New({a},{b},{c},{d})");
	}

	private static void DumpSphericalHarmonicsL2(SphericalHarmonicsL2 sh)
	{
		for (int i = 0; i < 3; i++)
		{
			switch (i)
			{
			case 0:
				Shader.SetGlobalVector(UnitySHParamNames[0], new Vector4(sh[i, 3], sh[i, 1], sh[i, 2], sh[i, 0] - sh[i, 6]));
				Print(UnitySHParamNames[0], sh[i, 3], sh[i, 1], sh[i, 2], sh[i, 0] - sh[i, 6]);
				break;
			case 1:
				Shader.SetGlobalVector(UnitySHParamNames[1], new Vector4(sh[i, 3], sh[i, 1], sh[i, 2], sh[i, 0] - sh[i, 6]));
				Print(UnitySHParamNames[1], sh[i, 3], sh[i, 1], sh[i, 2], sh[i, 0] - sh[i, 6]);
				break;
			case 2:
				Shader.SetGlobalVector(UnitySHParamNames[2], new Vector4(sh[i, 3], sh[i, 1], sh[i, 2], sh[i, 0] - sh[i, 6]));
				Print(UnitySHParamNames[2], sh[i, 3], sh[i, 1], sh[i, 2], sh[i, 0] - sh[i, 6]);
				break;
			}
		}
		for (int j = 0; j < 3; j++)
		{
			switch (j)
			{
			case 0:
				Shader.SetGlobalVector(UnitySHParamNames[3], new Vector4(sh[j, 4], sh[j, 5], sh[j, 6] * 3f, sh[j, 7]));
				Print(UnitySHParamNames[3], sh[j, 4], sh[j, 5], sh[j, 6] * 3f, sh[j, 7]);
				break;
			case 1:
				Shader.SetGlobalVector(UnitySHParamNames[4], new Vector4(sh[j, 4], sh[j, 5], sh[j, 6] * 3f, sh[j, 7]));
				Print(UnitySHParamNames[4], sh[j, 4], sh[j, 5], sh[j, 6] * 3f, sh[j, 7]);
				break;
			case 2:
				Shader.SetGlobalVector(UnitySHParamNames[5], new Vector4(sh[j, 4], sh[j, 5], sh[j, 6] * 3f, sh[j, 7]));
				Print(UnitySHParamNames[5], sh[j, 4], sh[j, 5], sh[j, 6] * 3f, sh[j, 7]);
				break;
			}
		}
		Shader.SetGlobalVector(UnitySHParamNames[6], new Vector4(sh[0, 8], sh[1, 8], sh[2, 8], 1f));
		Print(UnitySHParamNames[6], sh[0, 8], sh[1, 8], sh[2, 8], 1f);
	}
}
