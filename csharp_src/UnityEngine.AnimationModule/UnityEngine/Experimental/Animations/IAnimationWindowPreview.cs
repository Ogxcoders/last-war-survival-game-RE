using UnityEngine.Playables;

namespace UnityEngine.Experimental.Animations;

public interface IAnimationWindowPreview
{
	void StartPreview();

	void StopPreview();

	void UpdatePreviewGraph(PlayableGraph graph);

	Playable BuildPreviewGraph(PlayableGraph graph, Playable inputPlayable);
}
