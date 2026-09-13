namespace RuntimeInspectorNamespace;

public interface IRuntimeInspectorCustomEditor
{
	void GenerateElements(ObjectField parent);

	void Refresh();

	void Cleanup();
}
