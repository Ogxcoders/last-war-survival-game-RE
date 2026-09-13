using Cinemachine;
using UnityEngine;

public class BountyHunterCinemachineController : MonoBehaviour
{
	public CinemachineVirtualCamera normal;

	public CinemachineVirtualCamera attack;

	private CinemachineBasicMultiChannelPerlin _attackNoise;

	public CinemachineBasicMultiChannelPerlin AttackNoise
	{
		get
		{
			if (_attackNoise == null && attack != null)
			{
				_attackNoise = attack.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
			}
			return _attackNoise;
		}
	}

	public void SetAttackActive(bool active)
	{
		if (attack != null && attack.gameObject.activeSelf != active)
		{
			attack.gameObject.SetActive(active);
		}
	}

	public void SetAttackNoise(bool isOn)
	{
		if (!(AttackNoise == null))
		{
			AttackNoise.m_AmplitudeGain = (isOn ? 0.5f : 0f);
			AttackNoise.m_FrequencyGain = (isOn ? 0.8f : 0f);
		}
	}
}
