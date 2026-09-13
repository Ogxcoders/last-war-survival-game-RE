using UnityEngine;

public class ShowFPS : MonoBehaviour
{
	private float updateInterval = 0.5f;

	private double lastInterval;

	private int frames;

	private int fps;

	private void Start()
	{
		lastInterval = Time.realtimeSinceStartup;
		frames = 0;
	}

	private void OnGUI()
	{
		GUILayout.Label("FPS:" + fps);
	}

	private void Update()
	{
		frames++;
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if ((double)realtimeSinceStartup > lastInterval + (double)updateInterval)
		{
			fps = (int)((double)frames / ((double)realtimeSinceStartup - lastInterval));
			frames = 0;
			lastInterval = realtimeSinceStartup;
		}
	}
}
