using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.Rendering;

public class DebugUI
{
	public class Container : Widget, IContainer
	{
		public ObservableList<Widget> children { get; private set; }

		public override Panel panel
		{
			get
			{
				return m_Panel;
			}
			internal set
			{
				m_Panel = value;
				foreach (Widget child in children)
				{
					child.panel = value;
				}
			}
		}

		public Container()
		{
			base.displayName = "";
			children = new ObservableList<Widget>();
			children.ItemAdded += OnItemAdded;
			children.ItemRemoved += OnItemRemoved;
		}

		public Container(string displayName, ObservableList<Widget> children)
		{
			base.displayName = displayName;
			this.children = children;
			children.ItemAdded += OnItemAdded;
			children.ItemRemoved += OnItemRemoved;
		}

		internal override void GenerateQueryPath()
		{
			base.GenerateQueryPath();
			foreach (Widget child in children)
			{
				child.GenerateQueryPath();
			}
		}

		protected virtual void OnItemAdded(ObservableList<Widget> sender, ListChangedEventArgs<Widget> e)
		{
			if (e.item != null)
			{
				e.item.panel = m_Panel;
				e.item.parent = this;
			}
			if (m_Panel != null)
			{
				m_Panel.SetDirty();
			}
		}

		protected virtual void OnItemRemoved(ObservableList<Widget> sender, ListChangedEventArgs<Widget> e)
		{
			if (e.item != null)
			{
				e.item.panel = null;
				e.item.parent = null;
			}
			if (m_Panel != null)
			{
				m_Panel.SetDirty();
			}
		}

		public override int GetHashCode()
		{
			int num = 17;
			num = num * 23 + base.queryPath.GetHashCode();
			foreach (Widget child in children)
			{
				num = num * 23 + child.GetHashCode();
			}
			return num;
		}
	}

	public class Foldout : Container, IValueField
	{
		public bool opened;

		public bool isReadOnly => false;

		public string[] columnLabels { get; set; }

		public Foldout()
		{
		}

		public Foldout(string displayName, ObservableList<Widget> children, string[] columnLabels = null)
			: base(displayName, children)
		{
			this.columnLabels = columnLabels;
		}

		public bool GetValue()
		{
			return opened;
		}

		object IValueField.GetValue()
		{
			return GetValue();
		}

		public void SetValue(object value)
		{
			SetValue((bool)value);
		}

		public object ValidateValue(object value)
		{
			return value;
		}

		public void SetValue(bool value)
		{
			opened = value;
		}
	}

	public class HBox : Container
	{
		public HBox()
		{
			base.displayName = "HBox";
		}
	}

	public class VBox : Container
	{
		public VBox()
		{
			base.displayName = "VBox";
		}
	}

	public abstract class Field<T> : Widget, IValueField
	{
		public Action<Field<T>, T> onValueChanged;

		public Func<T> getter { get; set; }

		public Action<T> setter { get; set; }

		object IValueField.ValidateValue(object value)
		{
			return ValidateValue((T)value);
		}

		public virtual T ValidateValue(T value)
		{
			return value;
		}

		object IValueField.GetValue()
		{
			return GetValue();
		}

		public T GetValue()
		{
			return getter();
		}

		public void SetValue(object value)
		{
			SetValue((T)value);
		}

		public void SetValue(T value)
		{
			T val = ValidateValue(value);
			if (!val.Equals(getter()))
			{
				setter(val);
				if (onValueChanged != null)
				{
					onValueChanged(this, val);
				}
			}
		}
	}

	public class BoolField : Field<bool>
	{
	}

	public class HistoryBoolField : BoolField
	{
		public Func<bool>[] historyGetter { get; set; }

		public int historyDepth
		{
			get
			{
				Func<bool>[] array = historyGetter;
				if (array == null)
				{
					return 0;
				}
				return array.Length;
			}
		}

		public bool GetHistoryValue(int historyIndex)
		{
			return historyGetter[historyIndex]();
		}
	}

	public class IntField : Field<int>
	{
		public Func<int> min;

		public Func<int> max;

		public int incStep = 1;

		public int intStepMult = 10;

		public override int ValidateValue(int value)
		{
			if (min != null)
			{
				value = Mathf.Max(value, min());
			}
			if (max != null)
			{
				value = Mathf.Min(value, max());
			}
			return value;
		}
	}

	public class UIntField : Field<uint>
	{
		public Func<uint> min;

		public Func<uint> max;

		public uint incStep = 1u;

		public uint intStepMult = 10u;

		public override uint ValidateValue(uint value)
		{
			if (min != null)
			{
				value = (uint)Mathf.Max((int)value, (int)min());
			}
			if (max != null)
			{
				value = (uint)Mathf.Min((int)value, (int)max());
			}
			return value;
		}
	}

	public class FloatField : Field<float>
	{
		public Func<float> min;

		public Func<float> max;

		public float incStep = 0.1f;

		public float incStepMult = 10f;

		public int decimals = 3;

		public override float ValidateValue(float value)
		{
			if (min != null)
			{
				value = Mathf.Max(value, min());
			}
			if (max != null)
			{
				value = Mathf.Min(value, max());
			}
			return value;
		}
	}

	public class EnumField : Field<int>
	{
		public GUIContent[] enumNames;

		public int[] enumValues;

		internal int[] quickSeparators;

		internal int[] indexes;

		public Func<int> getIndex { get; set; }

		public Action<int> setIndex { get; set; }

		public int currentIndex
		{
			get
			{
				return getIndex();
			}
			set
			{
				setIndex(value);
			}
		}

		public Type autoEnum
		{
			set
			{
				enumNames = (from x in Enum.GetNames(value)
					select new GUIContent(x)).ToArray();
				Array values = Enum.GetValues(value);
				enumValues = new int[values.Length];
				for (int num = 0; num < values.Length; num++)
				{
					enumValues[num] = (int)values.GetValue(num);
				}
				InitIndexes();
				InitQuickSeparators();
			}
		}

		internal void InitQuickSeparators()
		{
			IEnumerable<string> source = enumNames.Select(delegate(GUIContent x)
			{
				string[] array = x.text.Split(new char[1] { '/' });
				return (array.Length == 1) ? "" : array[0];
			});
			quickSeparators = new int[source.Distinct().Count()];
			string text = null;
			int num = 0;
			int num2 = 0;
			for (; num < quickSeparators.Length; num++)
			{
				string text2 = source.ElementAt(num2);
				while (text == text2)
				{
					text2 = source.ElementAt(++num2);
				}
				text = text2;
				quickSeparators[num] = num2++;
			}
		}

		internal void InitIndexes()
		{
			indexes = new int[enumNames.Length];
			for (int i = 0; i < enumNames.Length; i++)
			{
				indexes[i] = i;
			}
		}
	}

	public class HistoryEnumField : EnumField
	{
		public Func<int>[] historyIndexGetter { get; set; }

		public int historyDepth
		{
			get
			{
				Func<int>[] array = historyIndexGetter;
				if (array == null)
				{
					return 0;
				}
				return array.Length;
			}
		}

		public int GetHistoryValue(int historyIndex)
		{
			return historyIndexGetter[historyIndex]();
		}
	}

	public class BitField : Field<Enum>
	{
		internal Type m_EnumType;

		public GUIContent[] enumNames { get; private set; }

		public int[] enumValues { get; private set; }

		public Type enumType
		{
			get
			{
				return m_EnumType;
			}
			set
			{
				enumNames = (from x in Enum.GetNames(value)
					select new GUIContent(x)).ToArray();
				Array values = Enum.GetValues(value);
				enumValues = new int[values.Length];
				for (int num = 0; num < values.Length; num++)
				{
					enumValues[num] = (int)values.GetValue(num);
				}
				m_EnumType = value;
			}
		}
	}

	public class ColorField : Field<Color>
	{
		public bool hdr;

		public bool showAlpha = true;

		public bool showPicker = true;

		public float incStep = 0.025f;

		public float incStepMult = 5f;

		public int decimals = 3;

		public override Color ValidateValue(Color value)
		{
			if (!hdr)
			{
				value.r = Mathf.Clamp01(value.r);
				value.g = Mathf.Clamp01(value.g);
				value.b = Mathf.Clamp01(value.b);
				value.a = Mathf.Clamp01(value.a);
			}
			return value;
		}
	}

	public class Vector2Field : Field<Vector2>
	{
		public float incStep = 0.025f;

		public float incStepMult = 10f;

		public int decimals = 3;
	}

	public class Vector3Field : Field<Vector3>
	{
		public float incStep = 0.025f;

		public float incStepMult = 10f;

		public int decimals = 3;
	}

	public class Vector4Field : Field<Vector4>
	{
		public float incStep = 0.025f;

		public float incStepMult = 10f;

		public int decimals = 3;
	}

	public class Panel : IContainer, IComparable<Panel>
	{
		public Flags flags { get; set; }

		public string displayName { get; set; }

		public int groupIndex { get; set; }

		public string queryPath => displayName;

		public bool isEditorOnly => (flags & Flags.EditorOnly) != 0;

		public bool isRuntimeOnly => (flags & Flags.RuntimeOnly) != 0;

		public bool isInactiveInEditor
		{
			get
			{
				if (isRuntimeOnly)
				{
					return !Application.isPlaying;
				}
				return false;
			}
		}

		public bool editorForceUpdate => (flags & Flags.EditorForceUpdate) != 0;

		public ObservableList<Widget> children { get; private set; }

		public event Action<Panel> onSetDirty = delegate
		{
		};

		public Panel()
		{
			children = new ObservableList<Widget>();
			children.ItemAdded += OnItemAdded;
			children.ItemRemoved += OnItemRemoved;
		}

		protected virtual void OnItemAdded(ObservableList<Widget> sender, ListChangedEventArgs<Widget> e)
		{
			if (e.item != null)
			{
				e.item.panel = this;
				e.item.parent = this;
			}
			SetDirty();
		}

		protected virtual void OnItemRemoved(ObservableList<Widget> sender, ListChangedEventArgs<Widget> e)
		{
			if (e.item != null)
			{
				e.item.panel = null;
				e.item.parent = null;
			}
			SetDirty();
		}

		public void SetDirty()
		{
			foreach (Widget child in children)
			{
				child.GenerateQueryPath();
			}
			this.onSetDirty(this);
		}

		public override int GetHashCode()
		{
			int num = 17;
			num = num * 23 + displayName.GetHashCode();
			foreach (Widget child in children)
			{
				num = num * 23 + child.GetHashCode();
			}
			return num;
		}

		int IComparable<Panel>.CompareTo(Panel other)
		{
			if (other != null)
			{
				return groupIndex.CompareTo(other.groupIndex);
			}
			return 1;
		}
	}

	[Flags]
	public enum Flags
	{
		None = 0,
		EditorOnly = 2,
		RuntimeOnly = 4,
		EditorForceUpdate = 8
	}

	public abstract class Widget
	{
		protected Panel m_Panel;

		protected IContainer m_Parent;

		public virtual Panel panel
		{
			get
			{
				return m_Panel;
			}
			internal set
			{
				m_Panel = value;
			}
		}

		public virtual IContainer parent
		{
			get
			{
				return m_Parent;
			}
			internal set
			{
				m_Parent = value;
			}
		}

		public Flags flags { get; set; }

		public string displayName { get; set; }

		public string queryPath { get; private set; }

		public bool isEditorOnly => (flags & Flags.EditorOnly) != 0;

		public bool isRuntimeOnly => (flags & Flags.RuntimeOnly) != 0;

		public bool isInactiveInEditor
		{
			get
			{
				if (isRuntimeOnly)
				{
					return !Application.isPlaying;
				}
				return false;
			}
		}

		internal virtual void GenerateQueryPath()
		{
			queryPath = displayName.Trim();
			if (m_Parent != null)
			{
				queryPath = m_Parent.queryPath + " -> " + queryPath;
			}
		}

		public override int GetHashCode()
		{
			return queryPath.GetHashCode();
		}
	}

	public interface IContainer
	{
		ObservableList<Widget> children { get; }

		string displayName { get; set; }

		string queryPath { get; }
	}

	public interface IValueField
	{
		object GetValue();

		void SetValue(object value);

		object ValidateValue(object value);
	}

	public class Button : Widget
	{
		public Action action { get; set; }
	}

	public class Value : Widget
	{
		public float refreshRate = 0.1f;

		public Func<object> getter { get; set; }

		public object GetValue()
		{
			return getter();
		}
	}
}
