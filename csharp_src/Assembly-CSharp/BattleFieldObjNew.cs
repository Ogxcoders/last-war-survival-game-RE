using UnityEngine;

public class BattleFieldObjNew : MonoBehaviour
{
	[SerializeField]
	private SimpleAnimation _sa;

	[SerializeField]
	private GameObject _firePoint;

	[SerializeField]
	private GameObject _upPoint;

	[SerializeField]
	private GameObject[] _blues;

	[SerializeField]
	private GameObject[] _reds;

	[SerializeField]
	private GameObject[] _yellows;

	public SimpleAnimation SimpleAnimation => _sa;

	public GameObject FirePoint => _firePoint;

	public GameObject UpPoint => _upPoint;

	public void SetState(int type)
	{
		if (_blues != null && _blues.Length != 0)
		{
			GameObject[] blues = _blues;
			for (int i = 0; i < blues.Length; i++)
			{
				blues[i].SetActive(type == 2);
			}
		}
		if (_reds != null && _reds.Length != 0)
		{
			GameObject[] blues = _reds;
			for (int i = 0; i < blues.Length; i++)
			{
				blues[i].SetActive(type == 1);
			}
		}
		if (_yellows != null && _yellows.Length != 0)
		{
			GameObject[] blues = _yellows;
			for (int i = 0; i < blues.Length; i++)
			{
				blues[i].SetActive(type == 0);
			}
		}
	}
}
