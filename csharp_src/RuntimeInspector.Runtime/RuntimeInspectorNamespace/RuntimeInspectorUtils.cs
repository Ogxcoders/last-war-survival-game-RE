using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace;

public static class RuntimeInspectorUtils
{
	private static readonly Dictionary<Type, MemberInfo[]> typeToVariables = new Dictionary<Type, MemberInfo[]>(89) { 
	{
		typeof(object),
		null
	} };

	private static readonly Dictionary<Type, ExposedMethod[]> typeToExposedMethods = new Dictionary<Type, ExposedMethod[]>(89);

	private static readonly HashSet<Type> commonSerializableTypes = new HashSet<Type>
	{
		typeof(string),
		typeof(Vector4),
		typeof(Vector3),
		typeof(Vector2),
		typeof(Rect),
		typeof(Quaternion),
		typeof(Color),
		typeof(Color32),
		typeof(LayerMask),
		typeof(Bounds),
		typeof(Matrix4x4),
		typeof(AnimationCurve),
		typeof(Gradient),
		typeof(RectOffset),
		typeof(GUIStyle),
		typeof(bool[]),
		typeof(byte[]),
		typeof(sbyte[]),
		typeof(char[]),
		typeof(decimal[]),
		typeof(double[]),
		typeof(float[]),
		typeof(int[]),
		typeof(uint[]),
		typeof(long[]),
		typeof(ulong[]),
		typeof(short[]),
		typeof(ushort[]),
		typeof(string[]),
		typeof(Vector4[]),
		typeof(Vector3[]),
		typeof(Vector2[]),
		typeof(Rect[]),
		typeof(Quaternion[]),
		typeof(Color[]),
		typeof(Color32[]),
		typeof(LayerMask[]),
		typeof(Bounds[]),
		typeof(Matrix4x4[]),
		typeof(AnimationCurve[]),
		typeof(Gradient[]),
		typeof(RectOffset[]),
		typeof(GUIStyle[]),
		typeof(List<bool>),
		typeof(List<byte>),
		typeof(List<sbyte>),
		typeof(List<char>),
		typeof(List<decimal>),
		typeof(List<double>),
		typeof(List<float>),
		typeof(List<int>),
		typeof(List<uint>),
		typeof(List<long>),
		typeof(List<ulong>),
		typeof(List<short>),
		typeof(List<ushort>),
		typeof(List<string>),
		typeof(List<Vector4>),
		typeof(List<Vector3>),
		typeof(List<Vector2>),
		typeof(List<Rect>),
		typeof(List<Quaternion>),
		typeof(List<Color>),
		typeof(List<Color32>),
		typeof(List<LayerMask>),
		typeof(List<Bounds>),
		typeof(List<Matrix4x4>),
		typeof(List<AnimationCurve>),
		typeof(List<Gradient>),
		typeof(List<RectOffset>),
		typeof(List<GUIStyle>),
		typeof(Vector3Int),
		typeof(Vector2Int),
		typeof(RectInt),
		typeof(BoundsInt),
		typeof(Vector3Int[]),
		typeof(Vector2Int[]),
		typeof(RectInt[]),
		typeof(BoundsInt[]),
		typeof(List<Vector3Int>),
		typeof(List<Vector2Int>),
		typeof(List<RectInt>),
		typeof(List<BoundsInt>)
	};

	private static readonly List<MemberInfo> validVariablesList = new List<MemberInfo>(32);

	private static readonly List<Type> typesToSearchForVariablesList = new List<Type>(8);

	private static readonly List<string> propertyNamesInVariablesList = new List<string>(32);

	private static readonly List<ExposedMethod> exposedMethodsList = new List<ExposedMethod>(4);

	private static readonly List<ExposedExtensionMethodHolder> exposedExtensionMethods = new List<ExposedExtensionMethodHolder>();

	private static Dictionary<Type, Type> customEditors;

	private static readonly List<RuntimeInspectorCustomEditorAttribute> customEditorAttributes = new List<RuntimeInspectorCustomEditorAttribute>(4);

	public static readonly HashSet<Transform> IgnoredTransformsInHierarchy = new HashSet<Transform>();

	private static Canvas popupCanvas = null;

	private static Canvas popupReferenceCanvas = null;

	private static Tooltip tooltipPopup;

	private static readonly Stack<DraggedReferenceItem> draggedReferenceItemsPool = new Stack<DraggedReferenceItem>();

	internal static readonly NumberFormatInfo numberFormat = NumberFormatInfo.GetInstance(CultureInfo.InvariantCulture);

	internal static readonly StringBuilder stringBuilder = new StringBuilder(200);

	public static Type ExposedExtensionMethodsHolder
	{
		set
		{
			GetExposedExtensionMethods(value);
		}
	}

	public static bool IsNull(this object obj)
	{
		if (obj is UnityEngine.Object)
		{
			return obj?.Equals(null) ?? true;
		}
		return obj == null;
	}

	public static bool IsEmpty<T>(this IList<T> objects)
	{
		if (objects == null)
		{
			return true;
		}
		for (int num = objects.Count - 1; num >= 0; num--)
		{
			if (!objects[num].IsNull())
			{
				return false;
			}
		}
		return true;
	}

	public static string ToTitleCase(this string str)
	{
		if (str == null || str.Length == 0)
		{
			return string.Empty;
		}
		byte b = 1;
		int i = 0;
		if (str.Length > 1 && str[1] == '_')
		{
			i = 2;
		}
		stringBuilder.Length = 0;
		for (; i < str.Length; i++)
		{
			char c = str[i];
			if (char.IsUpper(c))
			{
				if ((b < 2 || (str.Length > i + 1 && char.IsLower(str[i + 1]))) && stringBuilder.Length > 0)
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append(c);
				b = 3;
				continue;
			}
			if (c == '_')
			{
				b = 1;
				continue;
			}
			if (char.IsNumber(c))
			{
				if (b != 2 && stringBuilder.Length > 0)
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append(c);
				b = 2;
				continue;
			}
			if (b == 1 || b == 2)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append(char.ToUpper(c));
			}
			else
			{
				stringBuilder.Append(c);
			}
			b = 0;
		}
		if (stringBuilder.Length == 0)
		{
			return str;
		}
		return stringBuilder.ToString();
	}

	public static string GetNameWithType(this object obj, Type defaultType = null)
	{
		if (obj.IsNull())
		{
			if (defaultType == null)
			{
				return "None";
			}
			return "None (" + defaultType.Name + ")";
		}
		if (!(obj is UnityEngine.Object))
		{
			return obj.GetType().Name;
		}
		return ((UnityEngine.Object)obj).name + " (" + obj.GetType().Name + ")";
	}

	public static Texture GetTexture(this UnityEngine.Object obj)
	{
		if ((bool)obj)
		{
			if (obj is Texture)
			{
				return (Texture)obj;
			}
			if (obj is Sprite)
			{
				return ((Sprite)obj).texture;
			}
		}
		return null;
	}

	public static Color Tint(this Color color, float tintAmount)
	{
		if (color.r + color.g + color.b > 1.5f)
		{
			color.r -= tintAmount;
			color.g -= tintAmount;
			color.b -= tintAmount;
		}
		else
		{
			color.r += tintAmount;
			color.g += tintAmount;
			color.b += tintAmount;
		}
		return color;
	}

	public static void ShowTooltip(string tooltip, PointerEventData pointer, UISkin skin = null, Canvas referenceCanvas = null)
	{
		bool flag = CreatePopupCanvas(referenceCanvas);
		if (!tooltipPopup)
		{
			tooltipPopup = UnityEngine.Object.Instantiate(Resources.Load<Tooltip>("RuntimeInspector/Tooltip"), popupCanvas.transform, worldPositionStays: false);
			flag = true;
		}
		else
		{
			tooltipPopup.gameObject.SetActive(value: true);
		}
		if (flag)
		{
			tooltipPopup.Initialize(popupCanvas);
		}
		if ((bool)skin)
		{
			tooltipPopup.Skin = skin;
		}
		tooltipPopup.SetContent(tooltip, pointer);
	}

	public static void HideTooltip()
	{
		if ((bool)tooltipPopup && tooltipPopup.gameObject.activeSelf)
		{
			tooltipPopup.gameObject.SetActive(value: false);
		}
	}

	public static DraggedReferenceItem CreateDraggedReferenceItem(UnityEngine.Object reference, PointerEventData draggingPointer, UISkin skin = null, Canvas referenceCanvas = null)
	{
		return CreateDraggedReferenceItem(new UnityEngine.Object[1] { reference }, draggingPointer, skin, referenceCanvas);
	}

	public static DraggedReferenceItem CreateDraggedReferenceItem(UnityEngine.Object[] references, PointerEventData draggingPointer, UISkin skin = null, Canvas referenceCanvas = null)
	{
		if (references.IsEmpty())
		{
			return null;
		}
		HideTooltip();
		bool flag = CreatePopupCanvas(referenceCanvas);
		DraggedReferenceItem draggedReferenceItem;
		if (draggedReferenceItemsPool.Count > 0)
		{
			draggedReferenceItem = draggedReferenceItemsPool.Pop();
			draggedReferenceItem.gameObject.SetActive(value: true);
		}
		else
		{
			draggedReferenceItem = UnityEngine.Object.Instantiate(Resources.Load<DraggedReferenceItem>("RuntimeInspector/DraggedReferenceItem"), popupCanvas.transform, worldPositionStays: false);
			flag = true;
		}
		if (flag)
		{
			draggedReferenceItem.Initialize(popupCanvas);
		}
		if ((bool)skin)
		{
			draggedReferenceItem.Skin = skin;
		}
		draggedReferenceItem.SetContent(references, draggingPointer);
		draggingPointer.dragging = true;
		draggingPointer.eligibleForClick = false;
		return draggedReferenceItem;
	}

	public static void PoolDraggedReferenceItem(DraggedReferenceItem item)
	{
		if (item.gameObject.activeSelf)
		{
			item.gameObject.SetActive(value: false);
			draggedReferenceItemsPool.Push(item);
		}
	}

	public static T GetAssignableObjectFromDraggedReferenceItem<T>(PointerEventData draggingPointer)
	{
		return (T)GetAssignableObjectsFromDraggedReferenceItemInternal(draggingPointer, typeof(T), returnFirstObject: true);
	}

	public static T[] GetAssignableObjectsFromDraggedReferenceItem<T>(PointerEventData draggingPointer)
	{
		return (T[])GetAssignableObjectsFromDraggedReferenceItemInternal(draggingPointer, typeof(T), returnFirstObject: false);
	}

	public static object GetAssignableObjectFromDraggedReferenceItem(PointerEventData draggingPointer, Type assignableType)
	{
		return GetAssignableObjectsFromDraggedReferenceItemInternal(draggingPointer, assignableType, returnFirstObject: true);
	}

	public static object[] GetAssignableObjectsFromDraggedReferenceItem(PointerEventData draggingPointer, Type assignableType)
	{
		return (object[])GetAssignableObjectsFromDraggedReferenceItemInternal(draggingPointer, assignableType, returnFirstObject: false);
	}

	private static object GetAssignableObjectsFromDraggedReferenceItemInternal(PointerEventData draggingPointer, Type assignableType, bool returnFirstObject)
	{
		if (!draggingPointer.pointerDrag)
		{
			return null;
		}
		DraggedReferenceItem component = draggingPointer.pointerDrag.GetComponent<DraggedReferenceItem>();
		if ((bool)component && component.References != null && component.References.Length != 0)
		{
			object[] references = component.References;
			bool flag = true;
			for (int i = 0; i < references.Length; i++)
			{
				if (references[i].IsNull() || !assignableType.IsAssignableFrom(references[i].GetType()))
				{
					flag = false;
					break;
				}
				if (returnFirstObject)
				{
					break;
				}
			}
			if (flag)
			{
				if (returnFirstObject)
				{
					return references[0];
				}
				return references;
			}
			Array array = (returnFirstObject ? null : Array.CreateInstance(assignableType, references.Length));
			int num = 0;
			foreach (object obj in references)
			{
				if (obj.IsNull())
				{
					continue;
				}
				object obj2 = null;
				if (assignableType.IsAssignableFrom(obj.GetType()))
				{
					obj2 = obj;
				}
				else if (typeof(Component).IsAssignableFrom(assignableType))
				{
					if (obj is Component)
					{
						obj2 = ((Component)obj).GetComponent(assignableType);
					}
					else if (obj is GameObject)
					{
						obj2 = ((GameObject)obj).GetComponent(assignableType);
					}
				}
				else if (typeof(GameObject).IsAssignableFrom(assignableType) && obj is Component)
				{
					obj2 = ((Component)obj).gameObject;
				}
				if (!obj2.IsNull())
				{
					if (returnFirstObject)
					{
						return obj2;
					}
					array.SetValue(obj2, num++);
				}
			}
			if (num > 0)
			{
				if (num != array.Length)
				{
					Array array2 = Array.CreateInstance(assignableType, num);
					Array.Copy(array, array2, num);
					return array2;
				}
				return array;
			}
		}
		return null;
	}

	public static void CopyValuesFrom(this Canvas canvas, Canvas referenceCanvas)
	{
		if (!canvas || !referenceCanvas)
		{
			return;
		}
		canvas.pixelPerfect = referenceCanvas.pixelPerfect;
		canvas.renderMode = referenceCanvas.renderMode;
		canvas.sortingLayerID = referenceCanvas.sortingLayerID;
		canvas.sortingOrder = referenceCanvas.sortingOrder;
		switch (referenceCanvas.renderMode)
		{
		case RenderMode.ScreenSpaceCamera:
			canvas.worldCamera = referenceCanvas.worldCamera;
			canvas.planeDistance = referenceCanvas.planeDistance * 0.75f;
			break;
		case RenderMode.WorldSpace:
		{
			canvas.worldCamera = referenceCanvas.worldCamera;
			RectTransform rectTransform = (RectTransform)referenceCanvas.transform;
			Vector3 position;
			if (rectTransform.pivot == new Vector2(0.5f, 0.5f))
			{
				position = rectTransform.position;
			}
			else
			{
				Rect rect = rectTransform.rect;
				Vector3 position2 = new Vector3((0.5f - rectTransform.pivot.x) * rect.width, (0.5f - rectTransform.pivot.y) * rect.height, 0f);
				position = rectTransform.TransformPoint(position2);
			}
			canvas.transform.SetPositionAndRotation(position, rectTransform.rotation);
			canvas.transform.localScale = rectTransform.localScale;
			break;
		}
		}
		CanvasScaler component = canvas.GetComponent<CanvasScaler>();
		CanvasScaler component2 = referenceCanvas.GetComponent<CanvasScaler>();
		if (!component || !component2)
		{
			return;
		}
		component.referencePixelsPerUnit = component2.referencePixelsPerUnit;
		if (referenceCanvas.renderMode == RenderMode.WorldSpace)
		{
			component.dynamicPixelsPerUnit = component2.dynamicPixelsPerUnit;
			return;
		}
		component.uiScaleMode = component2.uiScaleMode;
		switch (component2.uiScaleMode)
		{
		case CanvasScaler.ScaleMode.ConstantPixelSize:
			component.scaleFactor = component2.scaleFactor;
			break;
		case CanvasScaler.ScaleMode.ScaleWithScreenSize:
			component.referenceResolution = component2.referenceResolution;
			component.screenMatchMode = component2.screenMatchMode;
			component.matchWidthOrHeight = component2.matchWidthOrHeight;
			break;
		case CanvasScaler.ScaleMode.ConstantPhysicalSize:
			component.physicalUnit = component2.physicalUnit;
			component.fallbackScreenDPI = component2.fallbackScreenDPI;
			component.defaultSpriteDPI = component2.defaultSpriteDPI;
			break;
		}
	}

	private static bool CreatePopupCanvas(Canvas referenceCanvas)
	{
		bool flag = !popupCanvas;
		if (!popupCanvas)
		{
			popupCanvas = new GameObject("PopupCanvas").AddComponent<Canvas>();
			popupCanvas.gameObject.AddComponent<CanvasScaler>();
			if (!referenceCanvas)
			{
				popupCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
				popupCanvas.sortingOrder = 987654;
			}
			SceneManager.sceneLoaded -= OnSceneLoaded;
			SceneManager.sceneLoaded += OnSceneLoaded;
			UnityEngine.Object.DontDestroyOnLoad(popupCanvas.gameObject);
			IgnoredTransformsInHierarchy.Add(popupCanvas.transform);
		}
		if ((bool)referenceCanvas && referenceCanvas != popupReferenceCanvas)
		{
			popupReferenceCanvas = referenceCanvas;
			popupCanvas.CopyValuesFrom(referenceCanvas);
			popupCanvas.sortingOrder = Mathf.Max(987654, referenceCanvas.sortingOrder + 100);
			flag = true;
		}
		if (flag)
		{
			popupCanvas.gameObject.SetActive(value: false);
			popupCanvas.gameObject.SetActive(value: true);
		}
		return flag;
	}

	private static void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		if ((bool)popupCanvas)
		{
			Transform transform = popupCanvas.transform;
			for (int num = transform.childCount - 1; num >= 0; num--)
			{
				UnityEngine.Object.Destroy(transform.GetChild(num).gameObject);
			}
		}
	}

	public static bool IsPointerValid(this PointerEventData eventData)
	{
		for (int num = Input.touchCount - 1; num >= 0; num--)
		{
			if (Input.GetTouch(num).fingerId == eventData.pointerId)
			{
				return true;
			}
		}
		return Input.GetMouseButton((int)eventData.button);
	}

	public static MemberInfo[] GetAllVariables(this Type type)
	{
		if (typeToVariables.TryGetValue(type, out var value))
		{
			return value;
		}
		validVariablesList.Clear();
		typesToSearchForVariablesList.Clear();
		Type type2 = type;
		while (type2 != typeof(object))
		{
			if (typeToVariables.TryGetValue(type2, out value))
			{
				if (value != null)
				{
					validVariablesList.AddRange(value);
				}
				break;
			}
			typesToSearchForVariablesList.Add(type2);
			type2 = type2.BaseType;
		}
		for (int num = typesToSearchForVariablesList.Count - 1; num >= 0; num--)
		{
			type2 = typesToSearchForVariablesList[num];
			propertyNamesInVariablesList.Clear();
			PropertyInfo[] properties = type2.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (PropertyInfo propertyInfo in properties)
			{
				MethodInfo getMethod = propertyInfo.GetGetMethod(nonPublic: true);
				if (!(getMethod == null) && !(propertyInfo.GetSetMethod(nonPublic: true) == null) && propertyInfo.GetIndexParameters().Length == 0 && propertyInfo.PropertyType.IsSerializable() && !propertyInfo.HasAttribute<ObsoleteAttribute>() && !propertyInfo.HasAttribute<NonSerializedAttribute>() && !propertyInfo.HasAttribute<HideInInspector>() && !(getMethod.GetBaseDefinition().DeclaringType != getMethod.DeclaringType))
				{
					propertyNamesInVariablesList.Add(propertyInfo.Name);
					validVariablesList.Add(propertyInfo);
				}
			}
			FieldInfo[] fields = type2.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (FieldInfo fieldInfo in fields)
			{
				if (fieldInfo.IsLiteral || fieldInfo.IsInitOnly || !fieldInfo.FieldType.IsSerializable() || fieldInfo.HasAttribute<ObsoleteAttribute>() || fieldInfo.HasAttribute<NonSerializedAttribute>() || fieldInfo.HasAttribute<HideInInspector>())
				{
					continue;
				}
				string name = fieldInfo.Name;
				if (name.Contains("_BackingField"))
				{
					continue;
				}
				int num2 = 0;
				if (name.Length > 1)
				{
					if (name.Length > 2 && name[1] == '_')
					{
						num2 = 2;
					}
					else if (name[0] == '_')
					{
						num2 = 1;
					}
				}
				bool flag = false;
				for (int num3 = propertyNamesInVariablesList.Count - 1; num3 >= 0; num3--)
				{
					string text = propertyNamesInVariablesList[num3];
					if (name.Length - num2 == text.Length)
					{
						int num4 = name[num2];
						int num5 = text[0];
						if (num4 == num5 || num4 + 32 == num5 || num4 - 32 == num5)
						{
							int k;
							for (k = 1; k < text.Length && name[num2 + k] == text[k]; k++)
							{
							}
							if (k == text.Length)
							{
								flag = true;
								break;
							}
						}
					}
				}
				if (!flag)
				{
					validVariablesList.Add(fieldInfo);
				}
			}
			value = ((validVariablesList.Count > 0) ? validVariablesList.ToArray() : null);
			typeToVariables[type2] = value;
		}
		return value;
	}

	public static ExposedMethod[] GetExposedMethods(this Type type)
	{
		if (!typeToExposedMethods.TryGetValue(type, out var value))
		{
			MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			exposedMethodsList.Clear();
			for (int i = 0; i < methods.Length; i++)
			{
				if (methods[i].HasAttribute<RuntimeInspectorButtonAttribute>() && methods[i].GetParameters().Length == 0)
				{
					RuntimeInspectorButtonAttribute attribute = methods[i].GetAttribute<RuntimeInspectorButtonAttribute>();
					if (!attribute.IsInitializer || type.IsAssignableFrom(methods[i].ReturnType))
					{
						exposedMethodsList.Add(new ExposedMethod(methods[i], attribute, isExtensionMethod: false));
					}
				}
			}
			for (int j = 0; j < exposedExtensionMethods.Count; j++)
			{
				ExposedExtensionMethodHolder exposedExtensionMethodHolder = exposedExtensionMethods[j];
				if (exposedExtensionMethodHolder.extendedType.IsAssignableFrom(type))
				{
					exposedMethodsList.Add(new ExposedMethod(exposedExtensionMethodHolder.method, exposedExtensionMethodHolder.properties, isExtensionMethod: true));
				}
			}
			value = ((exposedMethodsList.Count <= 0) ? null : exposedMethodsList.ToArray());
			typeToExposedMethods[type] = value;
		}
		return value;
	}

	private static bool IsSerializable(this Type type)
	{
		if (type.IsPrimitive || commonSerializableTypes.Contains(type) || type.IsEnum)
		{
			return true;
		}
		if (typeof(UnityEngine.Object).IsAssignableFrom(type))
		{
			return true;
		}
		if (type.IsArray)
		{
			if (type.GetArrayRank() != 1)
			{
				return false;
			}
			return type.GetElementType().IsSerializable();
		}
		if (type.IsGenericType)
		{
			if (type.GetGenericTypeDefinition() != typeof(List<>))
			{
				return false;
			}
			return type.GetGenericArguments()[0].IsSerializable();
		}
		if (Attribute.IsDefined(type, typeof(SerializableAttribute), inherit: false))
		{
			return true;
		}
		return false;
	}

	public static bool HasAttribute<T>(this MemberInfo variable) where T : Attribute
	{
		return Attribute.IsDefined(variable, typeof(T), inherit: true);
	}

	public static T GetAttribute<T>(this MemberInfo variable) where T : Attribute
	{
		return (T)Attribute.GetCustomAttribute(variable, typeof(T), inherit: true);
	}

	public static T[] GetAttributes<T>(this MemberInfo variable) where T : Attribute
	{
		return (T[])Attribute.GetCustomAttributes(variable, typeof(T), inherit: true);
	}

	public static object Instantiate(this Type type)
	{
		try
		{
			if (typeof(ScriptableObject).IsAssignableFrom(type))
			{
				return ScriptableObject.CreateInstance(type);
			}
			if (type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null) != null)
			{
				return Activator.CreateInstance(type, nonPublic: true);
			}
			return FormatterServices.GetUninitializedObject(type);
		}
		catch
		{
			return null;
		}
	}

	public static Type GetType(string typeName)
	{
		try
		{
			Type type = Type.GetType(typeName);
			if (type != null)
			{
				return type;
			}
			type = typeof(Transform).Assembly.GetType("UnityEngine." + typeName);
			if (type != null)
			{
				return type;
			}
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				Type[] types = assemblies[i].GetTypes();
				foreach (Type type2 in types)
				{
					if (type2.Name == typeName || type2.FullName == typeName)
					{
						return type2;
					}
				}
			}
		}
		catch
		{
		}
		return null;
	}

	private static void GetExposedExtensionMethods(Type type)
	{
		exposedExtensionMethods.Clear();
		typeToExposedMethods.Clear();
		MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		for (int i = 0; i < methods.Length; i++)
		{
			if (!methods[i].HasAttribute<RuntimeInspectorButtonAttribute>())
			{
				continue;
			}
			ParameterInfo[] parameters = methods[i].GetParameters();
			if (parameters.Length == 1)
			{
				RuntimeInspectorButtonAttribute attribute = methods[i].GetAttribute<RuntimeInspectorButtonAttribute>();
				Type parameterType = parameters[0].ParameterType;
				if (!attribute.IsInitializer || parameterType.IsAssignableFrom(methods[i].ReturnType))
				{
					exposedExtensionMethods.Add(new ExposedExtensionMethodHolder(parameterType, methods[i], attribute));
				}
			}
		}
	}

	public static void AddCustomEditor(Type customEditorType)
	{
		AddCustomEditorInternal(customEditorType, showWarnings: true);
	}

	private static void AddCustomEditorInternal(Type customEditorType, bool showWarnings)
	{
		if (customEditors == null)
		{
			GetCustomEditor(typeof(object));
		}
		if (!typeof(IRuntimeInspectorCustomEditor).IsAssignableFrom(customEditorType))
		{
			if (showWarnings)
			{
				Debug.LogWarning("Type doesn't implement IRuntimeInspectorCustomEditor interface: " + customEditorType);
			}
			return;
		}
		RuntimeInspectorCustomEditorAttribute[] array = (RuntimeInspectorCustomEditorAttribute[])Attribute.GetCustomAttributes(customEditorType, typeof(RuntimeInspectorCustomEditorAttribute), inherit: false);
		if (array == null || array.Length == 0)
		{
			if (showWarnings)
			{
				Debug.LogWarning("Type doesn't have RuntimeInspectorCustomEditor attribute: " + customEditorType);
			}
			return;
		}
		for (int i = 0; i < array.Length; i++)
		{
			customEditors[array[i].InspectedType] = customEditorType;
			if (!customEditorAttributes.Contains(array[i]))
			{
				int num = customEditorAttributes.BinarySearch(array[i]);
				if (num < 0)
				{
					num = ~num;
				}
				customEditorAttributes.Insert(num, array[i]);
			}
		}
	}

	public static IRuntimeInspectorCustomEditor GetCustomEditor(Type type)
	{
		if (customEditors == null)
		{
			customEditors = new Dictionary<Type, Type>(89);
			string[] array = new string[12]
			{
				"Unity", "System", "Mono.", "mscorlib", "netstandard", "TextMeshPro", "Microsoft.GeneratedCode", "I18N", "Boo.", "UnityScript.",
				"ICSharpCode.", "ExCSS.Unity"
			};
			CompareInfo compareInfo = new CultureInfo("en-US").CompareInfo;
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				if (assembly.IsDynamic)
				{
					continue;
				}
				string name = assembly.GetName().Name;
				bool flag = false;
				for (int j = 0; j < array.Length; j++)
				{
					if (compareInfo.IsPrefix(name, array[j], CompareOptions.IgnoreCase))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					continue;
				}
				try
				{
					Type[] exportedTypes = assembly.GetExportedTypes();
					for (int k = 0; k < exportedTypes.Length; k++)
					{
						AddCustomEditorInternal(exportedTypes[k], showWarnings: false);
					}
				}
				catch (NotSupportedException)
				{
				}
				catch (FileNotFoundException)
				{
				}
				catch (Exception ex3)
				{
					Debug.LogError("Couldn't search assembly for RuntimeInspectorCustomEditor attributes: " + assembly.GetName().Name + "\n" + ex3.ToString());
				}
			}
		}
		if (!customEditors.TryGetValue(type, out var value))
		{
			for (int l = 0; l < customEditorAttributes.Count; l++)
			{
				if (customEditorAttributes[l].EditorForChildClasses && customEditorAttributes[l].InspectedType.IsAssignableFrom(type))
				{
					value = customEditors[customEditorAttributes[l].InspectedType];
					break;
				}
			}
			customEditors[type] = value;
		}
		if (value != null)
		{
			try
			{
				return (IRuntimeInspectorCustomEditor)Activator.CreateInstance(value, nonPublic: true);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				customEditors[type] = null;
			}
		}
		return null;
	}
}
