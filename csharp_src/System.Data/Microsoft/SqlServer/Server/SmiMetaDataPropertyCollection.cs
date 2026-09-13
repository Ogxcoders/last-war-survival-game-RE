using System.Collections.Generic;
using System.Data.Common;

namespace Microsoft.SqlServer.Server;

internal class SmiMetaDataPropertyCollection
{
	private const int SelectorCount = 3;

	private SmiMetaDataProperty[] _properties;

	private bool _isReadOnly;

	private static readonly SmiDefaultFieldsProperty s_emptyDefaultFields = new SmiDefaultFieldsProperty(new List<bool>());

	private static readonly SmiOrderProperty s_emptySortOrder = new SmiOrderProperty(new List<SmiOrderProperty.SmiColumnOrder>());

	private static readonly SmiUniqueKeyProperty s_emptyUniqueKey = new SmiUniqueKeyProperty(new List<bool>());

	internal static readonly SmiMetaDataPropertyCollection EmptyInstance = CreateEmptyInstance();

	internal SmiMetaDataProperty this[SmiPropertySelector key]
	{
		get
		{
			return _properties[(int)key];
		}
		set
		{
			if (value == null)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			EnsureWritable();
			_properties[(int)key] = value;
		}
	}

	internal bool IsReadOnly => _isReadOnly;

	private static SmiMetaDataPropertyCollection CreateEmptyInstance()
	{
		SmiMetaDataPropertyCollection smiMetaDataPropertyCollection = new SmiMetaDataPropertyCollection();
		smiMetaDataPropertyCollection.SetReadOnly();
		return smiMetaDataPropertyCollection;
	}

	internal SmiMetaDataPropertyCollection()
	{
		_properties = new SmiMetaDataProperty[3];
		_isReadOnly = false;
		_properties[0] = s_emptyDefaultFields;
		_properties[1] = s_emptySortOrder;
		_properties[2] = s_emptyUniqueKey;
	}

	internal void SetReadOnly()
	{
		_isReadOnly = true;
	}

	private void EnsureWritable()
	{
		if (IsReadOnly)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
		}
	}
}
