using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MeshRenderHelper : MonoBehaviour
{
	[SerializeField]
	private MeshRenderer _meshRenderer;

	private Material _material;

	private MaterialPropertyBlock mbp;

	private int currentOrder = -1;

	public MeshRenderer MeshRenderer
	{
		get
		{
			_meshRenderer = _meshRenderer ?? GetComponent<MeshRenderer>();
			return _meshRenderer;
		}
	}

	public Material MainMaterial
	{
		get
		{
			if (_material == null)
			{
				_material = MeshRenderer.material;
			}
			return _material;
		}
	}

	public void SetMaterialFloat(int id, float value)
	{
		if (mbp == null)
		{
			mbp = new MaterialPropertyBlock();
		}
		mbp.SetFloat(id, value);
		MeshRenderer.SetPropertyBlock(mbp);
	}

	public void SetMaterialColor(int id, Color value)
	{
		if (mbp == null)
		{
			mbp = new MaterialPropertyBlock();
		}
		mbp.SetColor(id, value);
		MeshRenderer.SetPropertyBlock(mbp);
	}

	public void SetRenderOrder(int order)
	{
		if (currentOrder != order)
		{
			currentOrder = order;
			MeshRenderer.sortingOrder = currentOrder;
		}
	}
}
