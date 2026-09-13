namespace UnityEngine.UIElements;

public static class INotifyValueChangedExtensions
{
	public static bool RegisterValueChangedCallback<T>(this INotifyValueChanged<T> control, EventCallback<ChangeEvent<T>> callback)
	{
		if (control is CallbackEventHandler callbackEventHandler)
		{
			callbackEventHandler.RegisterCallback(callback);
			return true;
		}
		return false;
	}

	public static bool UnregisterValueChangedCallback<T>(this INotifyValueChanged<T> control, EventCallback<ChangeEvent<T>> callback)
	{
		if (control is CallbackEventHandler callbackEventHandler)
		{
			callbackEventHandler.UnregisterCallback(callback);
			return true;
		}
		return false;
	}
}
