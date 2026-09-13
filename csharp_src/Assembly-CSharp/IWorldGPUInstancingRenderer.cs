using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public interface IWorldGPUInstancingRenderer
{
	string Name { get; }

	RenderPassEvent RenderPassEvent { get; }

	bool IsRenderable { get; }

	int SortingOrder { get; }

	void Draw(CommandBuffer cmdBuffer);

	void Draw(Camera camera);

	string Description();

	void OnGizmos();

	void OnGizmosSelected();
}
