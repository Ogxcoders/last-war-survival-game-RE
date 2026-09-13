namespace System;

internal class TypeNames
{
	internal abstract class ATypeName : TypeName, IEquatable<TypeName>
	{
		public abstract string DisplayName { get; }

		public abstract TypeName NestedName(TypeIdentifier innerName);

		public bool Equals(TypeName other)
		{
			if (other != null)
			{
				return DisplayName == other.DisplayName;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return DisplayName.GetHashCode();
		}

		public override bool Equals(object other)
		{
			return Equals(other as TypeName);
		}
	}

	private class Display : ATypeName
	{
		private string displayName;

		public override string DisplayName => displayName;

		internal Display(string displayName)
		{
			this.displayName = displayName;
		}

		public override TypeName NestedName(TypeIdentifier innerName)
		{
			return new Display(DisplayName + "+" + innerName.DisplayName);
		}
	}

	internal static TypeName FromDisplay(string displayName)
	{
		return new Display(displayName);
	}
}
