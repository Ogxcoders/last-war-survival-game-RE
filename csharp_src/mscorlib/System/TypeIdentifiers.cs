namespace System;

internal class TypeIdentifiers
{
	private class Display : TypeNames.ATypeName, TypeIdentifier, TypeName, IEquatable<TypeName>
	{
		private string displayName;

		private string internal_name;

		public override string DisplayName => displayName;

		public string InternalName
		{
			get
			{
				if (internal_name == null)
				{
					internal_name = GetInternalName();
				}
				return internal_name;
			}
		}

		internal Display(string displayName)
		{
			this.displayName = displayName;
			internal_name = null;
		}

		private string GetInternalName()
		{
			return TypeSpec.UnescapeInternalName(displayName);
		}

		public override TypeName NestedName(TypeIdentifier innerName)
		{
			return TypeNames.FromDisplay(DisplayName + "+" + innerName.DisplayName);
		}
	}

	private class Internal : TypeNames.ATypeName, TypeIdentifier, TypeName, IEquatable<TypeName>
	{
		private string internalName;

		private string display_name;

		public override string DisplayName
		{
			get
			{
				if (display_name == null)
				{
					display_name = GetDisplayName();
				}
				return display_name;
			}
		}

		public string InternalName => internalName;

		internal Internal(string internalName)
		{
			this.internalName = internalName;
			display_name = null;
		}

		internal Internal(string nameSpaceInternal, TypeIdentifier typeName)
		{
			internalName = nameSpaceInternal + "." + typeName.InternalName;
			display_name = null;
		}

		private string GetDisplayName()
		{
			return TypeSpec.EscapeDisplayName(internalName);
		}

		public override TypeName NestedName(TypeIdentifier innerName)
		{
			return TypeNames.FromDisplay(DisplayName + "+" + innerName.DisplayName);
		}
	}

	private class NoEscape : TypeNames.ATypeName, TypeIdentifier, TypeName, IEquatable<TypeName>
	{
		private string simpleName;

		public override string DisplayName => simpleName;

		public string InternalName => simpleName;

		internal NoEscape(string simpleName)
		{
			this.simpleName = simpleName;
		}

		public override TypeName NestedName(TypeIdentifier innerName)
		{
			return TypeNames.FromDisplay(DisplayName + "+" + innerName.DisplayName);
		}
	}

	internal static TypeIdentifier FromDisplay(string displayName)
	{
		return new Display(displayName);
	}

	internal static TypeIdentifier FromInternal(string internalName)
	{
		return new Internal(internalName);
	}

	internal static TypeIdentifier FromInternal(string internalNameSpace, TypeIdentifier typeName)
	{
		return new Internal(internalNameSpace, typeName);
	}

	internal static TypeIdentifier WithoutEscape(string simpleName)
	{
		return new NoEscape(simpleName);
	}
}
