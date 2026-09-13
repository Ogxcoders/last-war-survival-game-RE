using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;

namespace Microsoft.SqlServer.Server;

internal class SmiExtendedMetaData : SmiMetaData
{
	private string _name;

	private string _typeSpecificNamePart1;

	private string _typeSpecificNamePart2;

	private string _typeSpecificNamePart3;

	internal string Name => _name;

	internal string TypeSpecificNamePart1 => _typeSpecificNamePart1;

	internal string TypeSpecificNamePart2 => _typeSpecificNamePart2;

	internal string TypeSpecificNamePart3 => _typeSpecificNamePart3;

	internal SmiExtendedMetaData(SqlDbType dbType, long maxLength, byte precision, byte scale, long localeId, SqlCompareOptions compareOptions, Type userDefinedType, string name, string typeSpecificNamePart1, string typeSpecificNamePart2, string typeSpecificNamePart3)
		: this(dbType, maxLength, precision, scale, localeId, compareOptions, userDefinedType, isMultiValued: false, null, null, name, typeSpecificNamePart1, typeSpecificNamePart2, typeSpecificNamePart3)
	{
	}

	internal SmiExtendedMetaData(SqlDbType dbType, long maxLength, byte precision, byte scale, long localeId, SqlCompareOptions compareOptions, Type userDefinedType, bool isMultiValued, IList<SmiExtendedMetaData> fieldMetaData, SmiMetaDataPropertyCollection extendedProperties, string name, string typeSpecificNamePart1, string typeSpecificNamePart2, string typeSpecificNamePart3)
		: this(dbType, maxLength, precision, scale, localeId, compareOptions, userDefinedType, null, isMultiValued, fieldMetaData, extendedProperties, name, typeSpecificNamePart1, typeSpecificNamePart2, typeSpecificNamePart3)
	{
	}

	internal SmiExtendedMetaData(SqlDbType dbType, long maxLength, byte precision, byte scale, long localeId, SqlCompareOptions compareOptions, Type userDefinedType, string udtAssemblyQualifiedName, bool isMultiValued, IList<SmiExtendedMetaData> fieldMetaData, SmiMetaDataPropertyCollection extendedProperties, string name, string typeSpecificNamePart1, string typeSpecificNamePart2, string typeSpecificNamePart3)
		: base(dbType, maxLength, precision, scale, localeId, compareOptions, userDefinedType, udtAssemblyQualifiedName, isMultiValued, fieldMetaData, extendedProperties)
	{
		_name = name;
		_typeSpecificNamePart1 = typeSpecificNamePart1;
		_typeSpecificNamePart2 = typeSpecificNamePart2;
		_typeSpecificNamePart3 = typeSpecificNamePart3;
	}
}
