using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneViewerGFXPanel : BaseGFXPanel
{
	private Transform currTrm;

	private string currSceneName = "";

	private string tempSearchStr = "";

	private string targetSearchStr = "";

	private Transform detailTrm;

	private List<GameObject> tempDebugGameObjectList = new List<GameObject>();

	public SceneViewerGFXPanel()
		: base("场景查看")
	{
	}

	public override void DrawGUI()
	{
		if (currTrm == null)
		{
			DrawSceneList();
		}
		else
		{
			DrawTransformList();
		}
	}

	private void DrawSceneList()
	{
		Scene[] allScenes = UnityEngine.SceneManagement.SceneManager.GetAllScenes();
		for (int i = 0; i < allScenes.Length; i++)
		{
			Scene scene = allScenes[i];
			GUILayout.Label("Scene:" + scene.name);
			GameObject[] rootGameObjects = scene.GetRootGameObjects();
			foreach (GameObject gameObject in rootGameObjects)
			{
				if (GUILayout.Button(gameObject.name ?? ""))
				{
					currTrm = gameObject.transform;
					currSceneName = scene.name;
				}
			}
			GUILayout.Space(20f);
		}
	}

	private void DrawTransformList()
	{
		GUILayout.Label("Scene:" + currSceneName);
		GUILayout.Label("Path:" + GetTrmPath(currTrm));
		GUILayout.Space(20f);
		GUILayout.BeginHorizontal();
		GUILayout.Label("搜索:");
		tempSearchStr = GUILayout.TextField(tempSearchStr);
		if (GUILayout.Button("确定"))
		{
			targetSearchStr = tempSearchStr;
		}
		if (GUILayout.Button("清空"))
		{
			tempSearchStr = "";
			targetSearchStr = "";
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(20f);
		if (GUILayout.Button("上一级"))
		{
			if (currTrm.parent != null)
			{
				currTrm = currTrm.parent;
			}
			else
			{
				currTrm = null;
				currSceneName = "";
			}
			detailTrm = null;
		}
		if (currTrm.childCount == 0)
		{
			GUILayout.Label(currTrm.name);
			return;
		}
		foreach (Transform item in currTrm)
		{
			if (item.name.Contains(targetSearchStr))
			{
				DrawTransform(item);
			}
		}
	}

	private void DrawTransform(Transform trm)
	{
		GUILayout.BeginHorizontal();
		if (GUILayout.Button(trm.name))
		{
			currTrm = trm;
			detailTrm = null;
			tempSearchStr = "";
			targetSearchStr = "";
		}
		if (GUILayout.Button($"显隐:{trm.gameObject.activeSelf}"))
		{
			trm.gameObject.SetActive(!trm.gameObject.activeSelf);
		}
		if (trm.GetComponent<MeshRenderer>() != null)
		{
			if (GUILayout.Button("M.."))
			{
				detailTrm = trm;
			}
		}
		else if (trm.GetComponent<SkinnedMeshRenderer>() != null)
		{
			if (GUILayout.Button("SM.."))
			{
				detailTrm = trm;
			}
		}
		else if (GUILayout.Button("..."))
		{
			detailTrm = trm;
		}
		GUILayout.EndHorizontal();
		if (detailTrm == trm)
		{
			DrawDetail();
		}
	}

	private void DrawDetail()
	{
		Renderer component = detailTrm.GetComponent<MeshRenderer>();
		if (component == null)
		{
			component = detailTrm.GetComponent<SkinnedMeshRenderer>();
		}
		if (component != null)
		{
			Material[] sharedMaterials = component.sharedMaterials;
			foreach (Material material in sharedMaterials)
			{
				GUILayout.Label(" Mat:" + material.name);
				if (material.shader != null)
				{
					GUILayout.Label(" Shader:" + material.shader.name);
				}
			}
		}
		GUILayout.BeginHorizontal();
		GUILayout.Space(20f);
		if (GUILayout.Button("升高 1"))
		{
			detailTrm.Translate(Vector3.up);
		}
		if (GUILayout.Button("降低1"))
		{
			detailTrm.Translate(Vector3.down);
		}
		if (GUILayout.Button("旋转45"))
		{
			detailTrm.Rotate(Vector3.up, 45f);
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Space(20f);
		if (GUILayout.Button("升高0.1"))
		{
			detailTrm.Translate(Vector3.up * 0.1f);
		}
		if (GUILayout.Button("降低0.1"))
		{
			detailTrm.Translate(Vector3.down * 0.1f);
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Space(20f);
		if (GUILayout.Button("打印贴图(仅Tex2D)"))
		{
			DumpOneMaterialAllTextures(component, detailTrm);
		}
		if (GUILayout.Button("清理GO"))
		{
			foreach (GameObject tempDebugGameObject in tempDebugGameObjectList)
			{
				Object.Destroy(tempDebugGameObject);
			}
			tempDebugGameObjectList.Clear();
		}
		GUILayout.EndHorizontal();
	}

	private void DumpOneMaterialAllTextures(Renderer renderer, Transform trm)
	{
		Material[] sharedMaterials = renderer.sharedMaterials;
		foreach (Material mat in sharedMaterials)
		{
			List<Texture> allTexturesOfMaterial = GetAllTexturesOfMaterial(mat);
			if (allTexturesOfMaterial != null)
			{
				int num = 0;
				foreach (Texture item in allTexturesOfMaterial)
				{
					GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
					gameObject.transform.position = trm.position + Vector3.right * (1 + num) * 3f + Vector3.up * 4f;
					gameObject.transform.localScale = Vector3.one * 2f;
					gameObject.GetComponent<MeshRenderer>().material.mainTexture = item;
					num++;
					tempDebugGameObjectList.Add(gameObject);
				}
			}
			else
			{
				Debug.Log("贴图为空");
			}
		}
	}

	private List<Texture> GetAllTexturesOfMaterial(Material mat)
	{
		List<Texture> list = new List<Texture>();
		if (mat.shader == null)
		{
			return null;
		}
		string[] texturePropertyNames = mat.GetTexturePropertyNames();
		foreach (string text in texturePropertyNames)
		{
			Texture texture = mat.GetTexture(text);
			if (texture is Texture2D)
			{
				list.Add(texture);
			}
		}
		return list;
	}

	private string GetTrmPath(Transform trm)
	{
		List<string> list = new List<string>();
		list.Append(trm.name);
		while (trm.parent != null)
		{
			trm = trm.parent;
			list.Add(trm.name);
		}
		list.Reverse();
		return string.Join("/", list.ToArray());
	}
}
