using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tayx.Graphy.Audio;

public class G_AudioMonitor : MonoBehaviour
{
	private const float m_refValue = 1f;

	private GraphyManager m_graphyManager;

	private AudioListener m_audioListener;

	private GraphyManager.LookForAudioListener m_findAudioListenerInCameraIfNull = GraphyManager.LookForAudioListener.ON_SCENE_LOAD;

	private FFTWindow m_FFTWindow = FFTWindow.Blackman;

	private int m_spectrumSize = 512;

	private float[] m_spectrum;

	private float[] m_spectrumHighestValues;

	private float m_maxDB;

	public float[] Spectrum => m_spectrum;

	public float[] SpectrumHighestValues => m_spectrumHighestValues;

	public float MaxDB => m_maxDB;

	public bool SpectrumDataAvailable => m_audioListener != null;

	private void Awake()
	{
		Init();
	}

	private void Update()
	{
		if (m_audioListener != null)
		{
			AudioListener.GetOutputData(m_spectrum, 0);
			float num = 0f;
			for (int i = 0; i < m_spectrum.Length; i++)
			{
				num += m_spectrum[i] * m_spectrum[i];
			}
			float num2 = Mathf.Sqrt(num / (float)m_spectrum.Length);
			m_maxDB = 20f * Mathf.Log10(num2 / 1f);
			if (m_maxDB < -80f)
			{
				m_maxDB = -80f;
			}
			AudioListener.GetSpectrumData(m_spectrum, 0, m_FFTWindow);
			for (int j = 0; j < m_spectrum.Length; j++)
			{
				if (m_spectrum[j] > m_spectrumHighestValues[j])
				{
					m_spectrumHighestValues[j] = m_spectrum[j];
				}
				else
				{
					m_spectrumHighestValues[j] = Mathf.Clamp(m_spectrumHighestValues[j] - m_spectrumHighestValues[j] * Time.deltaTime * 2f, 0f, 1f);
				}
			}
		}
		else if (m_audioListener == null && m_findAudioListenerInCameraIfNull == GraphyManager.LookForAudioListener.ALWAYS)
		{
			m_audioListener = FindAudioListener();
		}
	}

	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	public void UpdateParameters()
	{
		m_findAudioListenerInCameraIfNull = m_graphyManager.FindAudioListenerInCameraIfNull;
		m_audioListener = m_graphyManager.AudioListener;
		m_FFTWindow = m_graphyManager.FftWindow;
		m_spectrumSize = m_graphyManager.SpectrumSize;
		if (m_audioListener == null && m_findAudioListenerInCameraIfNull != GraphyManager.LookForAudioListener.NEVER)
		{
			m_audioListener = FindAudioListener();
		}
		m_spectrum = new float[m_spectrumSize];
		m_spectrumHighestValues = new float[m_spectrumSize];
	}

	public float lin2dB(float linear)
	{
		return Mathf.Clamp(Mathf.Log10(linear) * 20f, -160f, 0f);
	}

	public float dBNormalized(float db)
	{
		return (db + 160f) / 160f;
	}

	private AudioListener FindAudioListener()
	{
		Camera main = Camera.main;
		if (main != null)
		{
			return main.GetComponent<AudioListener>();
		}
		return null;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
	{
		if (m_findAudioListenerInCameraIfNull == GraphyManager.LookForAudioListener.ON_SCENE_LOAD)
		{
			m_audioListener = FindAudioListener();
		}
	}

	private void Init()
	{
		m_graphyManager = base.transform.root.GetComponentInChildren<GraphyManager>();
		UpdateParameters();
		SceneManager.sceneLoaded += OnSceneLoaded;
	}
}
