namespace RuntimeInspectorNamespace;

public interface INumberHandler
{
	float MinValue { get; }

	float MaxValue { get; }

	bool TryParse(string input, out object value);

	bool ValuesAreEqual(object value1, object value2);

	object ConvertFromFloat(float value);

	float ConvertToFloat(object value);

	string ToString(object value);
}
