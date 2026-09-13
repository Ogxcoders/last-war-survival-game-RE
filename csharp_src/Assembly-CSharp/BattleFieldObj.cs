using UnityEngine;

public class BattleFieldObj : MonoBehaviour
{
	[SerializeField]
	private bool _isSMR;

	[SerializeField]
	private SimpleAnimation _sa;

	[SerializeField]
	private SkinnedMeshRenderer _smrBlue;

	[SerializeField]
	private SkinnedMeshRenderer _smrRed;

	[SerializeField]
	private SkinnedMeshRenderer _smrYellow;

	[SerializeField]
	private MeshRenderer _mrBlue;

	[SerializeField]
	private MeshRenderer _mrRed;

	[SerializeField]
	private MeshRenderer _mrYellow;

	[SerializeField]
	private GameObject _firePoint;

	[SerializeField]
	private GameObject _upPoint;

	public SimpleAnimation SimpleAnimation => _sa;

	public GameObject FirePoint => _firePoint;

	public GameObject UpPoint => _upPoint;

	public void SetState(int type)
	{
		if (_isSMR)
		{
			if (_smrBlue != null)
			{
				_smrBlue.gameObject.SetActive(type == 2);
			}
			if (_smrRed != null)
			{
				_smrRed.gameObject.SetActive(type == 1);
			}
			if (_smrYellow != null)
			{
				_smrYellow.gameObject.SetActive(type == 0);
			}
		}
		else
		{
			if (_mrBlue != null)
			{
				_mrBlue.gameObject.SetActive(type == 2);
			}
			if (_mrRed != null)
			{
				_mrRed.gameObject.SetActive(type == 1);
			}
			if (_mrYellow != null)
			{
				_mrYellow.gameObject.SetActive(type == 0);
			}
		}
	}
}
