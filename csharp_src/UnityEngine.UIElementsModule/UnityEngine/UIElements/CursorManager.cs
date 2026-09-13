namespace UnityEngine.UIElements;

internal class CursorManager : ICursorManager
{
	public void SetCursor(Cursor cursor)
	{
		if (cursor.texture != null)
		{
			UnityEngine.Cursor.SetCursor(cursor.texture, cursor.hotspot, CursorMode.Auto);
			return;
		}
		if (cursor.defaultCursorId != 0)
		{
			Debug.LogWarning("Runtime does not support setting a cursor without a texture. Use ResetCursor() to reset the cursor to the default cursor.");
		}
		ResetCursor();
	}

	public void ResetCursor()
	{
		UnityEngine.Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
	}
}
