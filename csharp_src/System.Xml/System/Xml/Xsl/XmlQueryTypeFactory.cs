using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml.Xsl;

internal static class XmlQueryTypeFactory
{
	private sealed class ItemType : XmlQueryType
	{
		public static readonly XmlQueryType UntypedDocument;

		public static readonly XmlQueryType UntypedElement;

		public static readonly XmlQueryType UntypedAttribute;

		public static readonly XmlQueryType NodeNotRtf;

		private static XmlQueryType[] BuiltInItemTypes;

		private static XmlQueryType[] BuiltInItemTypesStrict;

		private static XmlQueryType[] SpecialBuiltInItemTypes;

		private XmlTypeCode code;

		private XmlQualifiedNameTest nameTest;

		private XmlSchemaType schemaType;

		private bool isNillable;

		private XmlNodeKindFlags nodeKinds;

		private bool isStrict;

		private bool isNotRtf;

		public override XmlTypeCode TypeCode => code;

		public override XmlQualifiedNameTest NameTest => nameTest;

		public override XmlSchemaType SchemaType => schemaType;

		public override bool IsNillable => isNillable;

		public override XmlNodeKindFlags NodeKinds => nodeKinds;

		public override bool IsStrict => isStrict;

		public override bool IsNotRtf => isNotRtf;

		public override bool IsDod => false;

		public override XmlQueryCardinality Cardinality => XmlQueryCardinality.One;

		public override XmlQueryType Prime => this;

		public override XmlValueConverter ClrMapping
		{
			get
			{
				if (base.IsAtomicValue)
				{
					return SchemaType.ValueConverter;
				}
				if (base.IsNode)
				{
					return XmlNodeConverter.Node;
				}
				return XmlAnyConverter.Item;
			}
		}

		public override int Count => 1;

		public override XmlQueryType this[int index]
		{
			get
			{
				if (index != 0)
				{
					throw new IndexOutOfRangeException();
				}
				return this;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		static ItemType()
		{
			int num = 55;
			BuiltInItemTypes = new XmlQueryType[num];
			BuiltInItemTypesStrict = new XmlQueryType[num];
			for (int i = 0; i < num; i++)
			{
				XmlTypeCode typeCode = (XmlTypeCode)i;
				switch ((XmlTypeCode)i)
				{
				case XmlTypeCode.None:
					BuiltInItemTypes[i] = ChoiceType.None;
					BuiltInItemTypesStrict[i] = ChoiceType.None;
					break;
				case XmlTypeCode.Item:
				case XmlTypeCode.Node:
					BuiltInItemTypes[i] = new ItemType(typeCode, XmlQualifiedNameTest.Wildcard, XmlSchemaComplexType.AnyType, isNillable: false, isStrict: false, isNotRtf: false);
					BuiltInItemTypesStrict[i] = BuiltInItemTypes[i];
					break;
				case XmlTypeCode.Document:
				case XmlTypeCode.Element:
				case XmlTypeCode.Namespace:
				case XmlTypeCode.ProcessingInstruction:
				case XmlTypeCode.Comment:
				case XmlTypeCode.Text:
					BuiltInItemTypes[i] = new ItemType(typeCode, XmlQualifiedNameTest.Wildcard, XmlSchemaComplexType.AnyType, isNillable: false, isStrict: false, isNotRtf: true);
					BuiltInItemTypesStrict[i] = BuiltInItemTypes[i];
					break;
				case XmlTypeCode.Attribute:
					BuiltInItemTypes[i] = new ItemType(typeCode, XmlQualifiedNameTest.Wildcard, DatatypeImplementation.AnySimpleType, isNillable: false, isStrict: false, isNotRtf: true);
					BuiltInItemTypesStrict[i] = BuiltInItemTypes[i];
					break;
				case XmlTypeCode.AnyAtomicType:
					BuiltInItemTypes[i] = new ItemType(typeCode, XmlQualifiedNameTest.Wildcard, DatatypeImplementation.AnyAtomicType, isNillable: false, isStrict: false, isNotRtf: true);
					BuiltInItemTypesStrict[i] = BuiltInItemTypes[i];
					break;
				case XmlTypeCode.UntypedAtomic:
					BuiltInItemTypes[i] = new ItemType(typeCode, XmlQualifiedNameTest.Wildcard, DatatypeImplementation.UntypedAtomicType, isNillable: false, isStrict: true, isNotRtf: true);
					BuiltInItemTypesStrict[i] = BuiltInItemTypes[i];
					break;
				default:
				{
					XmlSchemaType builtInSimpleType = XmlSchemaType.GetBuiltInSimpleType(typeCode);
					BuiltInItemTypes[i] = new ItemType(typeCode, XmlQualifiedNameTest.Wildcard, builtInSimpleType, isNillable: false, isStrict: false, isNotRtf: true);
					BuiltInItemTypesStrict[i] = new ItemType(typeCode, XmlQualifiedNameTest.Wildcard, builtInSimpleType, isNillable: false, isStrict: true, isNotRtf: true);
					break;
				}
				}
			}
			UntypedDocument = new ItemType(XmlTypeCode.Document, XmlQualifiedNameTest.Wildcard, XmlSchemaComplexType.UntypedAnyType, isNillable: false, isStrict: false, isNotRtf: true);
			UntypedElement = new ItemType(XmlTypeCode.Element, XmlQualifiedNameTest.Wildcard, XmlSchemaComplexType.UntypedAnyType, isNillable: false, isStrict: false, isNotRtf: true);
			UntypedAttribute = new ItemType(XmlTypeCode.Attribute, XmlQualifiedNameTest.Wildcard, DatatypeImplementation.UntypedAtomicType, isNillable: false, isStrict: false, isNotRtf: true);
			NodeNotRtf = new ItemType(XmlTypeCode.Node, XmlQualifiedNameTest.Wildcard, XmlSchemaComplexType.AnyType, isNillable: false, isStrict: false, isNotRtf: true);
			SpecialBuiltInItemTypes = new XmlQueryType[4] { UntypedDocument, UntypedElement, UntypedAttribute, NodeNotRtf };
		}

		public static XmlQueryType Create(XmlTypeCode code, bool isStrict)
		{
			if (isStrict)
			{
				return BuiltInItemTypesStrict[(int)code];
			}
			return BuiltInItemTypes[(int)code];
		}

		public static XmlQueryType Create(XmlSchemaSimpleType schemaType, bool isStrict)
		{
			XmlTypeCode typeCode = schemaType.Datatype.TypeCode;
			if (schemaType == XmlSchemaType.GetBuiltInSimpleType(typeCode))
			{
				return Create(typeCode, isStrict);
			}
			return new ItemType(typeCode, XmlQualifiedNameTest.Wildcard, schemaType, isNillable: false, isStrict, isNotRtf: true);
		}

		public static XmlQueryType Create(XmlTypeCode code, XmlQualifiedNameTest nameTest, XmlSchemaType contentType, bool isNillable)
		{
			switch (code)
			{
			case XmlTypeCode.Document:
			case XmlTypeCode.Element:
				if (nameTest.IsWildcard)
				{
					if (contentType == XmlSchemaComplexType.AnyType)
					{
						return Create(code, isStrict: false);
					}
					if (contentType == XmlSchemaComplexType.UntypedAnyType)
					{
						switch (code)
						{
						case XmlTypeCode.Element:
							return UntypedElement;
						case XmlTypeCode.Document:
							return UntypedDocument;
						}
					}
				}
				return new ItemType(code, nameTest, contentType, isNillable, isStrict: false, isNotRtf: true);
			case XmlTypeCode.Attribute:
				if (nameTest.IsWildcard)
				{
					if (contentType == DatatypeImplementation.AnySimpleType)
					{
						return Create(code, isStrict: false);
					}
					if (contentType == DatatypeImplementation.UntypedAtomicType)
					{
						return UntypedAttribute;
					}
				}
				return new ItemType(code, nameTest, contentType, isNillable, isStrict: false, isNotRtf: true);
			default:
				return Create(code, isStrict: false);
			}
		}

		private ItemType(XmlTypeCode code, XmlQualifiedNameTest nameTest, XmlSchemaType schemaType, bool isNillable, bool isStrict, bool isNotRtf)
		{
			this.code = code;
			this.nameTest = nameTest;
			this.schemaType = schemaType;
			this.isNillable = isNillable;
			this.isStrict = isStrict;
			this.isNotRtf = isNotRtf;
			switch (code)
			{
			case XmlTypeCode.Item:
				nodeKinds = XmlNodeKindFlags.Any;
				break;
			case XmlTypeCode.Node:
				nodeKinds = XmlNodeKindFlags.Any;
				break;
			case XmlTypeCode.Document:
				nodeKinds = XmlNodeKindFlags.Document;
				break;
			case XmlTypeCode.Element:
				nodeKinds = XmlNodeKindFlags.Element;
				break;
			case XmlTypeCode.Attribute:
				nodeKinds = XmlNodeKindFlags.Attribute;
				break;
			case XmlTypeCode.Namespace:
				nodeKinds = XmlNodeKindFlags.Namespace;
				break;
			case XmlTypeCode.ProcessingInstruction:
				nodeKinds = XmlNodeKindFlags.PI;
				break;
			case XmlTypeCode.Comment:
				nodeKinds = XmlNodeKindFlags.Comment;
				break;
			case XmlTypeCode.Text:
				nodeKinds = XmlNodeKindFlags.Text;
				break;
			default:
				nodeKinds = XmlNodeKindFlags.None;
				break;
			}
		}

		public override void GetObjectData(BinaryWriter writer)
		{
			sbyte b = (sbyte)code;
			for (int i = 0; i < SpecialBuiltInItemTypes.Length; i++)
			{
				if ((object)this == SpecialBuiltInItemTypes[i])
				{
					b = (sbyte)(~i);
					break;
				}
			}
			writer.Write(b);
			if (0 <= b)
			{
				writer.Write(isStrict);
			}
		}

		public static XmlQueryType Create(BinaryReader reader)
		{
			sbyte b = reader.ReadSByte();
			if (0 <= b)
			{
				return Create((XmlTypeCode)b, reader.ReadBoolean());
			}
			return SpecialBuiltInItemTypes[~b];
		}
	}

	private sealed class ChoiceType : XmlQueryType
	{
		public static readonly XmlQueryType None = new ChoiceType(new List<XmlQueryType>());

		private XmlTypeCode code;

		private XmlSchemaType schemaType;

		private XmlNodeKindFlags nodeKinds;

		private List<XmlQueryType> members;

		private static readonly XmlTypeCode[] NodeKindToTypeCode = new XmlTypeCode[8]
		{
			XmlTypeCode.None,
			XmlTypeCode.Document,
			XmlTypeCode.Element,
			XmlTypeCode.Attribute,
			XmlTypeCode.Text,
			XmlTypeCode.Comment,
			XmlTypeCode.ProcessingInstruction,
			XmlTypeCode.Namespace
		};

		public override XmlTypeCode TypeCode => code;

		public override XmlQualifiedNameTest NameTest => XmlQualifiedNameTest.Wildcard;

		public override XmlSchemaType SchemaType => schemaType;

		public override bool IsNillable => false;

		public override XmlNodeKindFlags NodeKinds => nodeKinds;

		public override bool IsStrict => members.Count == 0;

		public override bool IsNotRtf
		{
			get
			{
				for (int i = 0; i < members.Count; i++)
				{
					if (!members[i].IsNotRtf)
					{
						return false;
					}
				}
				return true;
			}
		}

		public override bool IsDod => false;

		public override XmlQueryCardinality Cardinality
		{
			get
			{
				if (TypeCode != XmlTypeCode.None)
				{
					return XmlQueryCardinality.One;
				}
				return XmlQueryCardinality.None;
			}
		}

		public override XmlQueryType Prime => this;

		public override XmlValueConverter ClrMapping
		{
			get
			{
				if (code == XmlTypeCode.None || code == XmlTypeCode.Item)
				{
					return XmlAnyConverter.Item;
				}
				if (base.IsAtomicValue)
				{
					return SchemaType.ValueConverter;
				}
				return XmlNodeConverter.Node;
			}
		}

		public override int Count => members.Count;

		public override XmlQueryType this[int index]
		{
			get
			{
				return members[index];
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public static XmlQueryType Create(XmlNodeKindFlags nodeKinds)
		{
			if (Bits.ExactlyOne((uint)nodeKinds))
			{
				return ItemType.Create(NodeKindToTypeCode[Bits.LeastPosition((uint)nodeKinds)], isStrict: false);
			}
			List<XmlQueryType> list = new List<XmlQueryType>();
			while (nodeKinds != XmlNodeKindFlags.None)
			{
				list.Add(ItemType.Create(NodeKindToTypeCode[Bits.LeastPosition((uint)nodeKinds)], isStrict: false));
				nodeKinds = (XmlNodeKindFlags)Bits.ClearLeast((uint)nodeKinds);
			}
			return Create(list);
		}

		public static XmlQueryType Create(List<XmlQueryType> members)
		{
			if (members.Count == 0)
			{
				return None;
			}
			if (members.Count == 1)
			{
				return members[0];
			}
			return new ChoiceType(members);
		}

		private ChoiceType(List<XmlQueryType> members)
		{
			this.members = members;
			for (int i = 0; i < members.Count; i++)
			{
				XmlQueryType xmlQueryType = members[i];
				if (code == XmlTypeCode.None)
				{
					code = xmlQueryType.TypeCode;
					schemaType = xmlQueryType.SchemaType;
				}
				else if (base.IsNode && xmlQueryType.IsNode)
				{
					if (code == xmlQueryType.TypeCode)
					{
						if (code == XmlTypeCode.Element)
						{
							schemaType = XmlSchemaComplexType.AnyType;
						}
						else if (code == XmlTypeCode.Attribute)
						{
							schemaType = DatatypeImplementation.AnySimpleType;
						}
					}
					else
					{
						code = XmlTypeCode.Node;
						schemaType = null;
					}
				}
				else if (base.IsAtomicValue && xmlQueryType.IsAtomicValue)
				{
					code = XmlTypeCode.AnyAtomicType;
					schemaType = DatatypeImplementation.AnyAtomicType;
				}
				else
				{
					code = XmlTypeCode.Item;
					schemaType = null;
				}
				nodeKinds |= xmlQueryType.NodeKinds;
			}
		}

		public override void GetObjectData(BinaryWriter writer)
		{
			writer.Write(members.Count);
			for (int i = 0; i < members.Count; i++)
			{
				Serialize(writer, members[i]);
			}
		}

		public static XmlQueryType Create(BinaryReader reader)
		{
			int num = reader.ReadInt32();
			List<XmlQueryType> list = new List<XmlQueryType>(num);
			for (int i = 0; i < num; i++)
			{
				list.Add(Deserialize(reader));
			}
			return Create(list);
		}
	}

	private sealed class SequenceType : XmlQueryType
	{
		public static readonly XmlQueryType Zero = new SequenceType(ChoiceType.None, XmlQueryCardinality.Zero);

		private XmlQueryType prime;

		private XmlQueryCardinality card;

		private XmlValueConverter converter;

		public override XmlTypeCode TypeCode => prime.TypeCode;

		public override XmlQualifiedNameTest NameTest => prime.NameTest;

		public override XmlSchemaType SchemaType => prime.SchemaType;

		public override bool IsNillable => prime.IsNillable;

		public override XmlNodeKindFlags NodeKinds => prime.NodeKinds;

		public override bool IsStrict => prime.IsStrict;

		public override bool IsNotRtf => prime.IsNotRtf;

		public override bool IsDod => (object)this == NodeSDod;

		public override XmlQueryCardinality Cardinality => card;

		public override XmlQueryType Prime => prime;

		public override XmlValueConverter ClrMapping
		{
			get
			{
				if (converter == null)
				{
					converter = XmlListConverter.Create(prime.ClrMapping);
				}
				return converter;
			}
		}

		public override int Count => prime.Count;

		public override XmlQueryType this[int index]
		{
			get
			{
				return prime[index];
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public static XmlQueryType Create(XmlQueryType prime, XmlQueryCardinality card)
		{
			if (prime.TypeCode == XmlTypeCode.None)
			{
				if (!(XmlQueryCardinality.Zero <= card))
				{
					return None;
				}
				return Zero;
			}
			if (card == XmlQueryCardinality.None)
			{
				return None;
			}
			if (card == XmlQueryCardinality.Zero)
			{
				return Zero;
			}
			if (card == XmlQueryCardinality.One)
			{
				return prime;
			}
			return new SequenceType(prime, card);
		}

		private SequenceType(XmlQueryType prime, XmlQueryCardinality card)
		{
			this.prime = prime;
			this.card = card;
		}

		public override void GetObjectData(BinaryWriter writer)
		{
			writer.Write(IsDod);
			if (!IsDod)
			{
				Serialize(writer, prime);
				card.GetObjectData(writer);
			}
		}

		public static XmlQueryType Create(BinaryReader reader)
		{
			if (reader.ReadBoolean())
			{
				return NodeSDod;
			}
			XmlQueryType xmlQueryType = Deserialize(reader);
			XmlQueryCardinality xmlQueryCardinality = new XmlQueryCardinality(reader);
			return Create(xmlQueryType, xmlQueryCardinality);
		}
	}

	public static readonly XmlQueryType None = ChoiceType.None;

	public static readonly XmlQueryType Empty = SequenceType.Zero;

	public static readonly XmlQueryType Item = Type(XmlTypeCode.Item, isStrict: false);

	public static readonly XmlQueryType ItemS = PrimeProduct(Item, XmlQueryCardinality.ZeroOrMore);

	public static readonly XmlQueryType Node = Type(XmlTypeCode.Node, isStrict: false);

	public static readonly XmlQueryType NodeS = PrimeProduct(Node, XmlQueryCardinality.ZeroOrMore);

	public static readonly XmlQueryType Element = Type(XmlTypeCode.Element, isStrict: false);

	public static readonly XmlQueryType ElementS = PrimeProduct(Element, XmlQueryCardinality.ZeroOrMore);

	public static readonly XmlQueryType Document = Type(XmlTypeCode.Document, isStrict: false);

	public static readonly XmlQueryType DocumentS = PrimeProduct(Document, XmlQueryCardinality.ZeroOrMore);

	public static readonly XmlQueryType Attribute = Type(XmlTypeCode.Attribute, isStrict: false);

	public static readonly XmlQueryType AttributeQ = PrimeProduct(Attribute, XmlQueryCardinality.ZeroOrOne);

	public static readonly XmlQueryType AttributeS = PrimeProduct(Attribute, XmlQueryCardinality.ZeroOrMore);

	public static readonly XmlQueryType Namespace = Type(XmlTypeCode.Namespace, isStrict: false);

	public static readonly XmlQueryType NamespaceS = PrimeProduct(Namespace, XmlQueryCardinality.ZeroOrMore);

	public static readonly XmlQueryType Text = Type(XmlTypeCode.Text, isStrict: false);

	public static readonly XmlQueryType TextS = PrimeProduct(Text, XmlQueryCardinality.ZeroOrMore);

	public static readonly XmlQueryType Comment = Type(XmlTypeCode.Comment, isStrict: false);

	public static readonly XmlQueryType CommentS = PrimeProduct(Comment, XmlQueryCardinality.ZeroOrMore);

	public static readonly XmlQueryType PI = Type(XmlTypeCode.ProcessingInstruction, isStrict: false);

	public static readonly XmlQueryType PIS = PrimeProduct(PI, XmlQueryCardinality.ZeroOrMore);

	public static readonly XmlQueryType DocumentOrElement = Choice(Document, Element);

	public static readonly XmlQueryType DocumentOrElementQ = PrimeProduct(DocumentOrElement, XmlQueryCardinality.ZeroOrOne);

	public static readonly XmlQueryType DocumentOrElementS = PrimeProduct(DocumentOrElement, XmlQueryCardinality.ZeroOrMore);

	public static readonly XmlQueryType Content = Choice(Element, Comment, PI, Text);

	public static readonly XmlQueryType ContentS = PrimeProduct(Content, XmlQueryCardinality.ZeroOrMore);

	public static readonly XmlQueryType DocumentOrContent = Choice(Document, Content);

	public static readonly XmlQueryType DocumentOrContentS = PrimeProduct(DocumentOrContent, XmlQueryCardinality.ZeroOrMore);

	public static readonly XmlQueryType AttributeOrContent = Choice(Attribute, Content);

	public static readonly XmlQueryType AttributeOrContentS = PrimeProduct(AttributeOrContent, XmlQueryCardinality.ZeroOrMore);

	public static readonly XmlQueryType AnyAtomicType = Type(XmlTypeCode.AnyAtomicType, isStrict: false);

	public static readonly XmlQueryType AnyAtomicTypeS = PrimeProduct(AnyAtomicType, XmlQueryCardinality.ZeroOrMore);

	public static readonly XmlQueryType String = Type(XmlTypeCode.String, isStrict: false);

	public static readonly XmlQueryType StringX = Type(XmlTypeCode.String, isStrict: true);

	public static readonly XmlQueryType StringXS = PrimeProduct(StringX, XmlQueryCardinality.ZeroOrMore);

	public static readonly XmlQueryType Boolean = Type(XmlTypeCode.Boolean, isStrict: false);

	public static readonly XmlQueryType BooleanX = Type(XmlTypeCode.Boolean, isStrict: true);

	public static readonly XmlQueryType Int = Type(XmlTypeCode.Int, isStrict: false);

	public static readonly XmlQueryType IntX = Type(XmlTypeCode.Int, isStrict: true);

	public static readonly XmlQueryType IntXS = PrimeProduct(IntX, XmlQueryCardinality.ZeroOrMore);

	public static readonly XmlQueryType IntegerX = Type(XmlTypeCode.Integer, isStrict: true);

	public static readonly XmlQueryType LongX = Type(XmlTypeCode.Long, isStrict: true);

	public static readonly XmlQueryType DecimalX = Type(XmlTypeCode.Decimal, isStrict: true);

	public static readonly XmlQueryType FloatX = Type(XmlTypeCode.Float, isStrict: true);

	public static readonly XmlQueryType Double = Type(XmlTypeCode.Double, isStrict: false);

	public static readonly XmlQueryType DoubleX = Type(XmlTypeCode.Double, isStrict: true);

	public static readonly XmlQueryType DateTimeX = Type(XmlTypeCode.DateTime, isStrict: true);

	public static readonly XmlQueryType QNameX = Type(XmlTypeCode.QName, isStrict: true);

	public static readonly XmlQueryType UntypedDocument = ItemType.UntypedDocument;

	public static readonly XmlQueryType UntypedElement = ItemType.UntypedElement;

	public static readonly XmlQueryType UntypedAttribute = ItemType.UntypedAttribute;

	public static readonly XmlQueryType UntypedNode = Choice(UntypedDocument, UntypedElement, UntypedAttribute, Namespace, Text, Comment, PI);

	public static readonly XmlQueryType UntypedNodeS = PrimeProduct(UntypedNode, XmlQueryCardinality.ZeroOrMore);

	public static readonly XmlQueryType NodeNotRtf = ItemType.NodeNotRtf;

	public static readonly XmlQueryType NodeNotRtfQ = PrimeProduct(NodeNotRtf, XmlQueryCardinality.ZeroOrOne);

	public static readonly XmlQueryType NodeNotRtfS = PrimeProduct(NodeNotRtf, XmlQueryCardinality.ZeroOrMore);

	public static readonly XmlQueryType NodeSDod = PrimeProduct(NodeNotRtf, XmlQueryCardinality.ZeroOrMore);

	private static readonly XmlTypeCode[] NodeKindToTypeCode = new XmlTypeCode[10]
	{
		XmlTypeCode.Document,
		XmlTypeCode.Element,
		XmlTypeCode.Attribute,
		XmlTypeCode.Namespace,
		XmlTypeCode.Text,
		XmlTypeCode.Text,
		XmlTypeCode.Text,
		XmlTypeCode.ProcessingInstruction,
		XmlTypeCode.Comment,
		XmlTypeCode.Node
	};

	public static XmlQueryType Type(XmlTypeCode code, bool isStrict)
	{
		return ItemType.Create(code, isStrict);
	}

	public static XmlQueryType Type(XmlSchemaSimpleType schemaType, bool isStrict)
	{
		if (schemaType.Datatype.Variety == XmlSchemaDatatypeVariety.Atomic)
		{
			if (schemaType == DatatypeImplementation.AnySimpleType)
			{
				return AnyAtomicTypeS;
			}
			return ItemType.Create(schemaType, isStrict);
		}
		while (schemaType.DerivedBy == XmlSchemaDerivationMethod.Restriction)
		{
			schemaType = (XmlSchemaSimpleType)schemaType.BaseXmlSchemaType;
		}
		if (schemaType.DerivedBy == XmlSchemaDerivationMethod.List)
		{
			return PrimeProduct(Type(((XmlSchemaSimpleTypeList)schemaType.Content).BaseItemType, isStrict), XmlQueryCardinality.ZeroOrMore);
		}
		XmlSchemaSimpleType[] baseMemberTypes = ((XmlSchemaSimpleTypeUnion)schemaType.Content).BaseMemberTypes;
		XmlQueryType[] array = new XmlQueryType[baseMemberTypes.Length];
		for (int i = 0; i < baseMemberTypes.Length; i++)
		{
			array[i] = Type(baseMemberTypes[i], isStrict);
		}
		return Choice(array);
	}

	public static XmlQueryType Choice(XmlQueryType left, XmlQueryType right)
	{
		return SequenceType.Create(ChoiceType.Create(PrimeChoice(new List<XmlQueryType>(left), right)), left.Cardinality | right.Cardinality);
	}

	public static XmlQueryType Choice(params XmlQueryType[] types)
	{
		if (types.Length == 0)
		{
			return None;
		}
		if (types.Length == 1)
		{
			return types[0];
		}
		List<XmlQueryType> list = new List<XmlQueryType>(types[0]);
		XmlQueryCardinality cardinality = types[0].Cardinality;
		for (int i = 1; i < types.Length; i++)
		{
			PrimeChoice(list, types[i]);
			cardinality |= types[i].Cardinality;
		}
		return SequenceType.Create(ChoiceType.Create(list), cardinality);
	}

	public static XmlQueryType NodeChoice(XmlNodeKindFlags kinds)
	{
		return ChoiceType.Create(kinds);
	}

	public static XmlQueryType Sequence(XmlQueryType left, XmlQueryType right)
	{
		return SequenceType.Create(ChoiceType.Create(PrimeChoice(new List<XmlQueryType>(left), right)), left.Cardinality + right.Cardinality);
	}

	public static XmlQueryType PrimeProduct(XmlQueryType t, XmlQueryCardinality c)
	{
		if (t.Cardinality == c && !t.IsDod)
		{
			return t;
		}
		return SequenceType.Create(t.Prime, c);
	}

	public static XmlQueryType Product(XmlQueryType t, XmlQueryCardinality c)
	{
		return PrimeProduct(t, t.Cardinality * c);
	}

	public static XmlQueryType AtMost(XmlQueryType t, XmlQueryCardinality c)
	{
		return PrimeProduct(t, c.AtMost());
	}

	private static List<XmlQueryType> PrimeChoice(List<XmlQueryType> accumulator, IList<XmlQueryType> types)
	{
		foreach (XmlQueryType type in types)
		{
			AddItemToChoice(accumulator, type);
		}
		return accumulator;
	}

	private static void AddItemToChoice(List<XmlQueryType> accumulator, XmlQueryType itemType)
	{
		bool flag = true;
		for (int i = 0; i < accumulator.Count; i++)
		{
			if (itemType.IsSubtypeOf(accumulator[i]))
			{
				return;
			}
			if (accumulator[i].IsSubtypeOf(itemType))
			{
				if (flag)
				{
					flag = false;
					accumulator[i] = itemType;
				}
				else
				{
					accumulator.RemoveAt(i);
					i--;
				}
			}
		}
		if (flag)
		{
			accumulator.Add(itemType);
		}
	}

	public static XmlQueryType Type(XPathNodeType kind, XmlQualifiedNameTest nameTest, XmlSchemaType contentType, bool isNillable)
	{
		return ItemType.Create(NodeKindToTypeCode[(int)kind], nameTest, contentType, isNillable);
	}

	[Conditional("DEBUG")]
	public static void CheckSerializability(XmlQueryType type)
	{
		type.GetObjectData(new BinaryWriter(Stream.Null));
	}

	public static void Serialize(BinaryWriter writer, XmlQueryType type)
	{
		sbyte value = (sbyte)((!(type.GetType() == typeof(ItemType))) ? ((type.GetType() == typeof(ChoiceType)) ? 1 : ((!(type.GetType() == typeof(SequenceType))) ? (-1) : 2)) : 0);
		writer.Write(value);
		type.GetObjectData(writer);
	}

	public static XmlQueryType Deserialize(BinaryReader reader)
	{
		return reader.ReadByte() switch
		{
			0 => ItemType.Create(reader), 
			1 => ChoiceType.Create(reader), 
			2 => SequenceType.Create(reader), 
			_ => null, 
		};
	}
}
