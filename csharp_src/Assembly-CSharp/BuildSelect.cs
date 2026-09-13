using FibMatrix.Rendering;
using UnityEngine;

public class BuildSelect : MonoBehaviour
{
	[SerializeField]
	private MeshRenderer[] _diMeshRenderer;

	[SerializeField]
	private MeshRenderer[] _arrowMeshRenderer;

	[SerializeField]
	private GameObject _MeshRendererGroup;

	private bool _isOk;

	private GameObject _baseObject;

	private MeshRenderer[] _cloneMeshRenderer;

	private MaterialPropertyBlock _diBlock;

	private static int _ColorSwitch = Shader.PropertyToID("_ColorSwitch");

	private void Awake()
	{
		_diBlock = new MaterialPropertyBlock();
		_isOk = true;
		SetColor();
	}

	private void OnDestroy()
	{
		CleanCloneRange();
	}

	public void ChangeColor(bool isOk)
	{
		if (_isOk != isOk)
		{
			_isOk = isOk;
			SetColor();
		}
	}

	private void SetColor()
	{
		if (_isOk)
		{
			_diBlock.SetFloat(_ColorSwitch, 0f);
		}
		else
		{
			_diBlock.SetFloat(_ColorSwitch, 1f);
		}
		for (int i = 0; i < _diMeshRenderer.Length; i++)
		{
			_diMeshRenderer[i].SetPropertyBlock(_diBlock);
		}
		for (int j = 0; j < _arrowMeshRenderer.Length; j++)
		{
			_arrowMeshRenderer[j].SetPropertyBlock(_diBlock);
		}
		if (_cloneMeshRenderer == null || _cloneMeshRenderer.Length == 0)
		{
			return;
		}
		int num = _cloneMeshRenderer.Length;
		for (int k = 0; k < num; k++)
		{
			if (_cloneMeshRenderer[k] != null)
			{
				_cloneMeshRenderer[k].SetPropertyBlock(_diBlock);
			}
		}
	}

	public void CleanCloneRange()
	{
		if (_cloneMeshRenderer != null && _cloneMeshRenderer.Length != 0)
		{
			int num = _cloneMeshRenderer.Length;
			for (int i = 0; i < num; i++)
			{
				if (_cloneMeshRenderer[i] != null)
				{
					_cloneMeshRenderer[i].transform.parent = null;
					Object.Destroy(_cloneMeshRenderer[i].gameObject);
				}
			}
		}
		if (_MeshRendererGroup != null)
		{
			_MeshRendererGroup.RemoveAllComponents<MeshRenderer>();
		}
		if (_baseObject != null)
		{
			_baseObject.GameObjectRecycleAll();
		}
	}

	public void SetCloneRange(int range)
	{
		if (_MeshRendererGroup == null)
		{
			return;
		}
		if (_baseObject == null)
		{
			_baseObject = _diMeshRenderer[0].gameObject;
			_baseObject.SetActive(value: false);
			_baseObject.GameObjectCreatePool();
		}
		if (_cloneMeshRenderer == null)
		{
			_cloneMeshRenderer = new MeshRenderer[range * range];
		}
		Transform parent = _MeshRendererGroup.transform;
		int num = 0;
		int num2 = -2;
		for (int i = 0; i < range; i++)
		{
			for (int j = 0; j < range; j++)
			{
				GameObject gameObject = _baseObject.GameObjectSpawn(parent);
				gameObject.SetActive(value: true);
				gameObject.transform.SetLocalPositionX(i * num2);
				gameObject.transform.SetLocalPositionZ(j * num2);
				_cloneMeshRenderer[num] = gameObject.GetComponent<MeshRenderer>();
				num++;
			}
		}
	}
}
