using UnityEngine.Playables;

namespace UnityEngine.Audio;

public static class AudioPlayableBinding
{
	public static PlayableBinding Create(string name, Object key)
	{
		return PlayableBinding.CreateInternal(name, key, typeof(AudioSource), CreateAudioOutput);
	}

	private static PlayableOutput CreateAudioOutput(PlayableGraph graph, string name)
	{
		return AudioPlayableOutput.Create(graph, name, null);
	}
}
