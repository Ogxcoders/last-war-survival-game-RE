using UnityEngine;

public class CountryFlag : MonoBehaviour
{
	[SerializeField]
	private MeshRenderer _meshRenderer;

	private Material _mat;

	private string countryCode = string.Empty;

	private void Awake()
	{
		_mat = _meshRenderer.material;
		_meshRenderer.sortingOrder = 1;
	}

	private void OnDestroy()
	{
	}

	public void SetCountry(string countryCode)
	{
		if (!countryCode.IsNullOrEmpty())
		{
			this.countryCode = countryCode;
		}
	}
}
