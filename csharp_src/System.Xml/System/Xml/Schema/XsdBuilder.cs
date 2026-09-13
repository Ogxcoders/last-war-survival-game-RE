using System.Collections;

namespace System.Xml.Schema;

internal sealed class XsdBuilder : SchemaBuilder
{
	private enum State
	{
		Root,
		Schema,
		Annotation,
		Include,
		Import,
		Element,
		Attribute,
		AttributeGroup,
		AttributeGroupRef,
		AnyAttribute,
		Group,
		GroupRef,
		All,
		Choice,
		Sequence,
		Any,
		Notation,
		SimpleType,
		ComplexType,
		ComplexContent,
		ComplexContentRestriction,
		ComplexContentExtension,
		SimpleContent,
		SimpleContentExtension,
		SimpleContentRestriction,
		SimpleTypeUnion,
		SimpleTypeList,
		SimpleTypeRestriction,
		Unique,
		Key,
		KeyRef,
		Selector,
		Field,
		MinExclusive,
		MinInclusive,
		MaxExclusive,
		MaxInclusive,
		TotalDigits,
		FractionDigits,
		Length,
		MinLength,
		MaxLength,
		Enumeration,
		Pattern,
		WhiteSpace,
		AppInfo,
		Documentation,
		Redefine
	}

	private delegate void XsdBuildFunction(XsdBuilder builder, string value);

	private delegate void XsdInitFunction(XsdBuilder builder, string value);

	private delegate void XsdEndChildFunction(XsdBuilder builder);

	private sealed class XsdAttributeEntry
	{
		public SchemaNames.Token Attribute;

		public XsdBuildFunction BuildFunc;

		public XsdAttributeEntry(SchemaNames.Token a, XsdBuildFunction build)
		{
			Attribute = a;
			BuildFunc = build;
		}
	}

	private sealed class XsdEntry
	{
		public SchemaNames.Token Name;

		public State CurrentState;

		public State[] NextStates;

		public XsdAttributeEntry[] Attributes;

		public XsdInitFunction InitFunc;

		public XsdEndChildFunction EndChildFunc;

		public bool ParseContent;

		public XsdEntry(SchemaNames.Token n, State state, State[] nextStates, XsdAttributeEntry[] attributes, XsdInitFunction init, XsdEndChildFunction end, bool parseContent)
		{
			Name = n;
			CurrentState = state;
			NextStates = nextStates;
			Attributes = attributes;
			InitFunc = init;
			EndChildFunc = end;
			ParseContent = parseContent;
		}
	}

	private class BuilderNamespaceManager : XmlNamespaceManager
	{
		private XmlNamespaceManager nsMgr;

		private XmlReader reader;

		public BuilderNamespaceManager(XmlNamespaceManager nsMgr, XmlReader reader)
		{
			this.nsMgr = nsMgr;
			this.reader = reader;
		}

		public override string LookupNamespace(string prefix)
		{
			string text = nsMgr.LookupNamespace(prefix);
			if (text == null)
			{
				text = reader.LookupNamespace(prefix);
			}
			return text;
		}
	}

	private const int STACK_INCREMENT = 10;

	private static readonly State[] SchemaElement = new State[1] { State.Schema };

	private static readonly State[] SchemaSubelements = new State[11]
	{
		State.Annotation,
		State.Include,
		State.Import,
		State.Redefine,
		State.ComplexType,
		State.SimpleType,
		State.Element,
		State.Attribute,
		State.AttributeGroup,
		State.Group,
		State.Notation
	};

	private static readonly State[] AttributeSubelements = new State[2]
	{
		State.Annotation,
		State.SimpleType
	};

	private static readonly State[] ElementSubelements = new State[6]
	{
		State.Annotation,
		State.SimpleType,
		State.ComplexType,
		State.Unique,
		State.Key,
		State.KeyRef
	};

	private static readonly State[] ComplexTypeSubelements = new State[10]
	{
		State.Annotation,
		State.SimpleContent,
		State.ComplexContent,
		State.GroupRef,
		State.All,
		State.Choice,
		State.Sequence,
		State.Attribute,
		State.AttributeGroupRef,
		State.AnyAttribute
	};

	private static readonly State[] SimpleContentSubelements = new State[3]
	{
		State.Annotation,
		State.SimpleContentRestriction,
		State.SimpleContentExtension
	};

	private static readonly State[] SimpleContentExtensionSubelements = new State[4]
	{
		State.Annotation,
		State.Attribute,
		State.AttributeGroupRef,
		State.AnyAttribute
	};

	private static readonly State[] SimpleContentRestrictionSubelements = new State[17]
	{
		State.Annotation,
		State.SimpleType,
		State.Enumeration,
		State.Length,
		State.MaxExclusive,
		State.MaxInclusive,
		State.MaxLength,
		State.MinExclusive,
		State.MinInclusive,
		State.MinLength,
		State.Pattern,
		State.TotalDigits,
		State.FractionDigits,
		State.WhiteSpace,
		State.Attribute,
		State.AttributeGroupRef,
		State.AnyAttribute
	};

	private static readonly State[] ComplexContentSubelements = new State[3]
	{
		State.Annotation,
		State.ComplexContentRestriction,
		State.ComplexContentExtension
	};

	private static readonly State[] ComplexContentExtensionSubelements = new State[8]
	{
		State.Annotation,
		State.GroupRef,
		State.All,
		State.Choice,
		State.Sequence,
		State.Attribute,
		State.AttributeGroupRef,
		State.AnyAttribute
	};

	private static readonly State[] ComplexContentRestrictionSubelements = new State[8]
	{
		State.Annotation,
		State.GroupRef,
		State.All,
		State.Choice,
		State.Sequence,
		State.Attribute,
		State.AttributeGroupRef,
		State.AnyAttribute
	};

	private static readonly State[] SimpleTypeSubelements = new State[4]
	{
		State.Annotation,
		State.SimpleTypeList,
		State.SimpleTypeRestriction,
		State.SimpleTypeUnion
	};

	private static readonly State[] SimpleTypeRestrictionSubelements = new State[14]
	{
		State.Annotation,
		State.SimpleType,
		State.Enumeration,
		State.Length,
		State.MaxExclusive,
		State.MaxInclusive,
		State.MaxLength,
		State.MinExclusive,
		State.MinInclusive,
		State.MinLength,
		State.Pattern,
		State.TotalDigits,
		State.FractionDigits,
		State.WhiteSpace
	};

	private static readonly State[] SimpleTypeListSubelements = new State[2]
	{
		State.Annotation,
		State.SimpleType
	};

	private static readonly State[] SimpleTypeUnionSubelements = new State[2]
	{
		State.Annotation,
		State.SimpleType
	};

	private static readonly State[] RedefineSubelements = new State[5]
	{
		State.Annotation,
		State.AttributeGroup,
		State.ComplexType,
		State.Group,
		State.SimpleType
	};

	private static readonly State[] AttributeGroupSubelements = new State[4]
	{
		State.Annotation,
		State.Attribute,
		State.AttributeGroupRef,
		State.AnyAttribute
	};

	private static readonly State[] GroupSubelements = new State[4]
	{
		State.Annotation,
		State.All,
		State.Choice,
		State.Sequence
	};

	private static readonly State[] AllSubelements = new State[2]
	{
		State.Annotation,
		State.Element
	};

	private static readonly State[] ChoiceSequenceSubelements = new State[6]
	{
		State.Annotation,
		State.Element,
		State.GroupRef,
		State.Choice,
		State.Sequence,
		State.Any
	};

	private static readonly State[] IdentityConstraintSubelements = new State[3]
	{
		State.Annotation,
		State.Selector,
		State.Field
	};

	private static readonly State[] AnnotationSubelements = new State[2]
	{
		State.AppInfo,
		State.Documentation
	};

	private static readonly State[] AnnotatedSubelements = new State[1] { State.Annotation };

	private static readonly XsdAttributeEntry[] SchemaAttributes = new XsdAttributeEntry[7]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaAttributeFormDefault, BuildSchema_AttributeFormDefault),
		new XsdAttributeEntry(SchemaNames.Token.SchemaElementFormDefault, BuildSchema_ElementFormDefault),
		new XsdAttributeEntry(SchemaNames.Token.SchemaTargetNamespace, BuildSchema_TargetNamespace),
		new XsdAttributeEntry(SchemaNames.Token.SchemaId, BuildAnnotated_Id),
		new XsdAttributeEntry(SchemaNames.Token.SchemaVersion, BuildSchema_Version),
		new XsdAttributeEntry(SchemaNames.Token.SchemaFinalDefault, BuildSchema_FinalDefault),
		new XsdAttributeEntry(SchemaNames.Token.SchemaBlockDefault, BuildSchema_BlockDefault)
	};

	private static readonly XsdAttributeEntry[] AttributeAttributes = new XsdAttributeEntry[8]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaDefault, BuildAttribute_Default),
		new XsdAttributeEntry(SchemaNames.Token.SchemaFixed, BuildAttribute_Fixed),
		new XsdAttributeEntry(SchemaNames.Token.SchemaForm, BuildAttribute_Form),
		new XsdAttributeEntry(SchemaNames.Token.SchemaId, BuildAnnotated_Id),
		new XsdAttributeEntry(SchemaNames.Token.SchemaName, BuildAttribute_Name),
		new XsdAttributeEntry(SchemaNames.Token.SchemaRef, BuildAttribute_Ref),
		new XsdAttributeEntry(SchemaNames.Token.SchemaType, BuildAttribute_Type),
		new XsdAttributeEntry(SchemaNames.Token.SchemaUse, BuildAttribute_Use)
	};

	private static readonly XsdAttributeEntry[] ElementAttributes = new XsdAttributeEntry[14]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaAbstract, BuildElement_Abstract),
		new XsdAttributeEntry(SchemaNames.Token.SchemaBlock, BuildElement_Block),
		new XsdAttributeEntry(SchemaNames.Token.SchemaDefault, BuildElement_Default),
		new XsdAttributeEntry(SchemaNames.Token.SchemaFinal, BuildElement_Final),
		new XsdAttributeEntry(SchemaNames.Token.SchemaFixed, BuildElement_Fixed),
		new XsdAttributeEntry(SchemaNames.Token.SchemaForm, BuildElement_Form),
		new XsdAttributeEntry(SchemaNames.Token.SchemaId, BuildAnnotated_Id),
		new XsdAttributeEntry(SchemaNames.Token.SchemaMaxOccurs, BuildElement_MaxOccurs),
		new XsdAttributeEntry(SchemaNames.Token.SchemaMinOccurs, BuildElement_MinOccurs),
		new XsdAttributeEntry(SchemaNames.Token.SchemaName, BuildElement_Name),
		new XsdAttributeEntry(SchemaNames.Token.SchemaNillable, BuildElement_Nillable),
		new XsdAttributeEntry(SchemaNames.Token.SchemaRef, BuildElement_Ref),
		new XsdAttributeEntry(SchemaNames.Token.SchemaSubstitutionGroup, BuildElement_SubstitutionGroup),
		new XsdAttributeEntry(SchemaNames.Token.SchemaType, BuildElement_Type)
	};

	private static readonly XsdAttributeEntry[] ComplexTypeAttributes = new XsdAttributeEntry[6]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaAbstract, BuildComplexType_Abstract),
		new XsdAttributeEntry(SchemaNames.Token.SchemaBlock, BuildComplexType_Block),
		new XsdAttributeEntry(SchemaNames.Token.SchemaFinal, BuildComplexType_Final),
		new XsdAttributeEntry(SchemaNames.Token.SchemaId, BuildAnnotated_Id),
		new XsdAttributeEntry(SchemaNames.Token.SchemaMixed, BuildComplexType_Mixed),
		new XsdAttributeEntry(SchemaNames.Token.SchemaName, BuildComplexType_Name)
	};

	private static readonly XsdAttributeEntry[] SimpleContentAttributes = new XsdAttributeEntry[1]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaId, BuildAnnotated_Id)
	};

	private static readonly XsdAttributeEntry[] SimpleContentExtensionAttributes = new XsdAttributeEntry[2]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaBase, BuildSimpleContentExtension_Base),
		new XsdAttributeEntry(SchemaNames.Token.SchemaId, BuildAnnotated_Id)
	};

	private static readonly XsdAttributeEntry[] SimpleContentRestrictionAttributes = new XsdAttributeEntry[2]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaBase, BuildSimpleContentRestriction_Base),
		new XsdAttributeEntry(SchemaNames.Token.SchemaId, BuildAnnotated_Id)
	};

	private static readonly XsdAttributeEntry[] ComplexContentAttributes = new XsdAttributeEntry[2]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaId, BuildAnnotated_Id),
		new XsdAttributeEntry(SchemaNames.Token.SchemaMixed, BuildComplexContent_Mixed)
	};

	private static readonly XsdAttributeEntry[] ComplexContentExtensionAttributes = new XsdAttributeEntry[2]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaBase, BuildComplexContentExtension_Base),
		new XsdAttributeEntry(SchemaNames.Token.SchemaId, BuildAnnotated_Id)
	};

	private static readonly XsdAttributeEntry[] ComplexContentRestrictionAttributes = new XsdAttributeEntry[2]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaBase, BuildComplexContentRestriction_Base),
		new XsdAttributeEntry(SchemaNames.Token.SchemaId, BuildAnnotated_Id)
	};

	private static readonly XsdAttributeEntry[] SimpleTypeAttributes = new XsdAttributeEntry[3]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaId, BuildAnnotated_Id),
		new XsdAttributeEntry(SchemaNames.Token.SchemaFinal, BuildSimpleType_Final),
		new XsdAttributeEntry(SchemaNames.Token.SchemaName, BuildSimpleType_Name)
	};

	private static readonly XsdAttributeEntry[] SimpleTypeRestrictionAttributes = new XsdAttributeEntry[2]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaBase, BuildSimpleTypeRestriction_Base),
		new XsdAttributeEntry(SchemaNames.Token.SchemaId, BuildAnnotated_Id)
	};

	private static readonly XsdAttributeEntry[] SimpleTypeUnionAttributes = new XsdAttributeEntry[2]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaId, BuildAnnotated_Id),
		new XsdAttributeEntry(SchemaNames.Token.SchemaMemberTypes, BuildSimpleTypeUnion_MemberTypes)
	};

	private static readonly XsdAttributeEntry[] SimpleTypeListAttributes = new XsdAttributeEntry[2]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaId, BuildAnnotated_Id),
		new XsdAttributeEntry(SchemaNames.Token.SchemaItemType, BuildSimpleTypeList_ItemType)
	};

	private static readonly XsdAttributeEntry[] AttributeGroupAttributes = new XsdAttributeEntry[2]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaId, BuildAnnotated_Id),
		new XsdAttributeEntry(SchemaNames.Token.SchemaName, BuildAttributeGroup_Name)
	};

	private static readonly XsdAttributeEntry[] AttributeGroupRefAttributes = new XsdAttributeEntry[2]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaId, BuildAnnotated_Id),
		new XsdAttributeEntry(SchemaNames.Token.SchemaRef, BuildAttributeGroupRef_Ref)
	};

	private static readonly XsdAttributeEntry[] GroupAttributes = new XsdAttributeEntry[2]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaId, BuildAnnotated_Id),
		new XsdAttributeEntry(SchemaNames.Token.SchemaName, BuildGroup_Name)
	};

	private static readonly XsdAttributeEntry[] GroupRefAttributes = new XsdAttributeEntry[4]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaId, BuildAnnotated_Id),
		new XsdAttributeEntry(SchemaNames.Token.SchemaMaxOccurs, BuildParticle_MaxOccurs),
		new XsdAttributeEntry(SchemaNames.Token.SchemaMinOccurs, BuildParticle_MinOccurs),
		new XsdAttributeEntry(SchemaNames.Token.SchemaRef, BuildGroupRef_Ref)
	};

	private static readonly XsdAttributeEntry[] ParticleAttributes = new XsdAttributeEntry[3]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaId, BuildAnnotated_Id),
		new XsdAttributeEntry(SchemaNames.Token.SchemaMaxOccurs, BuildParticle_MaxOccurs),
		new XsdAttributeEntry(SchemaNames.Token.SchemaMinOccurs, BuildParticle_MinOccurs)
	};

	private static readonly XsdAttributeEntry[] AnyAttributes = new XsdAttributeEntry[5]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaId, BuildAnnotated_Id),
		new XsdAttributeEntry(SchemaNames.Token.SchemaMaxOccurs, BuildParticle_MaxOccurs),
		new XsdAttributeEntry(SchemaNames.Token.SchemaMinOccurs, BuildParticle_MinOccurs),
		new XsdAttributeEntry(SchemaNames.Token.SchemaNamespace, BuildAny_Namespace),
		new XsdAttributeEntry(SchemaNames.Token.SchemaProcessContents, BuildAny_ProcessContents)
	};

	private static readonly XsdAttributeEntry[] IdentityConstraintAttributes = new XsdAttributeEntry[3]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaId, BuildAnnotated_Id),
		new XsdAttributeEntry(SchemaNames.Token.SchemaName, BuildIdentityConstraint_Name),
		new XsdAttributeEntry(SchemaNames.Token.SchemaRefer, BuildIdentityConstraint_Refer)
	};

	private static readonly XsdAttributeEntry[] SelectorAttributes = new XsdAttributeEntry[2]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaId, BuildAnnotated_Id),
		new XsdAttributeEntry(SchemaNames.Token.SchemaXPath, BuildSelector_XPath)
	};

	private static readonly XsdAttributeEntry[] FieldAttributes = new XsdAttributeEntry[2]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaId, BuildAnnotated_Id),
		new XsdAttributeEntry(SchemaNames.Token.SchemaXPath, BuildField_XPath)
	};

	private static readonly XsdAttributeEntry[] NotationAttributes = new XsdAttributeEntry[4]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaId, BuildAnnotated_Id),
		new XsdAttributeEntry(SchemaNames.Token.SchemaName, BuildNotation_Name),
		new XsdAttributeEntry(SchemaNames.Token.SchemaPublic, BuildNotation_Public),
		new XsdAttributeEntry(SchemaNames.Token.SchemaSystem, BuildNotation_System)
	};

	private static readonly XsdAttributeEntry[] IncludeAttributes = new XsdAttributeEntry[2]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaId, BuildAnnotated_Id),
		new XsdAttributeEntry(SchemaNames.Token.SchemaSchemaLocation, BuildInclude_SchemaLocation)
	};

	private static readonly XsdAttributeEntry[] ImportAttributes = new XsdAttributeEntry[3]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaId, BuildAnnotated_Id),
		new XsdAttributeEntry(SchemaNames.Token.SchemaNamespace, BuildImport_Namespace),
		new XsdAttributeEntry(SchemaNames.Token.SchemaSchemaLocation, BuildImport_SchemaLocation)
	};

	private static readonly XsdAttributeEntry[] FacetAttributes = new XsdAttributeEntry[3]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaId, BuildAnnotated_Id),
		new XsdAttributeEntry(SchemaNames.Token.SchemaFixed, BuildFacet_Fixed),
		new XsdAttributeEntry(SchemaNames.Token.SchemaValue, BuildFacet_Value)
	};

	private static readonly XsdAttributeEntry[] AnyAttributeAttributes = new XsdAttributeEntry[3]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaId, BuildAnnotated_Id),
		new XsdAttributeEntry(SchemaNames.Token.SchemaNamespace, BuildAnyAttribute_Namespace),
		new XsdAttributeEntry(SchemaNames.Token.SchemaProcessContents, BuildAnyAttribute_ProcessContents)
	};

	private static readonly XsdAttributeEntry[] DocumentationAttributes = new XsdAttributeEntry[2]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaSource, BuildDocumentation_Source),
		new XsdAttributeEntry(SchemaNames.Token.XmlLang, BuildDocumentation_XmlLang)
	};

	private static readonly XsdAttributeEntry[] AppinfoAttributes = new XsdAttributeEntry[1]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaSource, BuildAppinfo_Source)
	};

	private static readonly XsdAttributeEntry[] RedefineAttributes = new XsdAttributeEntry[2]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaId, BuildAnnotated_Id),
		new XsdAttributeEntry(SchemaNames.Token.SchemaSchemaLocation, BuildRedefine_SchemaLocation)
	};

	private static readonly XsdAttributeEntry[] AnnotationAttributes = new XsdAttributeEntry[1]
	{
		new XsdAttributeEntry(SchemaNames.Token.SchemaId, BuildAnnotated_Id)
	};

	private static readonly XsdEntry[] SchemaEntries = new XsdEntry[48]
	{
		new XsdEntry(SchemaNames.Token.Empty, State.Root, SchemaElement, null, null, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdSchema, State.Schema, SchemaSubelements, SchemaAttributes, InitSchema, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdAnnotation, State.Annotation, AnnotationSubelements, AnnotationAttributes, InitAnnotation, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdInclude, State.Include, AnnotatedSubelements, IncludeAttributes, InitInclude, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdImport, State.Import, AnnotatedSubelements, ImportAttributes, InitImport, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdElement, State.Element, ElementSubelements, ElementAttributes, InitElement, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdAttribute, State.Attribute, AttributeSubelements, AttributeAttributes, InitAttribute, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.xsdAttributeGroup, State.AttributeGroup, AttributeGroupSubelements, AttributeGroupAttributes, InitAttributeGroup, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.xsdAttributeGroup, State.AttributeGroupRef, AnnotatedSubelements, AttributeGroupRefAttributes, InitAttributeGroupRef, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdAnyAttribute, State.AnyAttribute, AnnotatedSubelements, AnyAttributeAttributes, InitAnyAttribute, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdGroup, State.Group, GroupSubelements, GroupAttributes, InitGroup, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdGroup, State.GroupRef, AnnotatedSubelements, GroupRefAttributes, InitGroupRef, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdAll, State.All, AllSubelements, ParticleAttributes, InitAll, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdChoice, State.Choice, ChoiceSequenceSubelements, ParticleAttributes, InitChoice, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdSequence, State.Sequence, ChoiceSequenceSubelements, ParticleAttributes, InitSequence, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdAny, State.Any, AnnotatedSubelements, AnyAttributes, InitAny, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdNotation, State.Notation, AnnotatedSubelements, NotationAttributes, InitNotation, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdSimpleType, State.SimpleType, SimpleTypeSubelements, SimpleTypeAttributes, InitSimpleType, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdComplexType, State.ComplexType, ComplexTypeSubelements, ComplexTypeAttributes, InitComplexType, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdComplexContent, State.ComplexContent, ComplexContentSubelements, ComplexContentAttributes, InitComplexContent, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdComplexContentRestriction, State.ComplexContentRestriction, ComplexContentRestrictionSubelements, ComplexContentRestrictionAttributes, InitComplexContentRestriction, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdComplexContentExtension, State.ComplexContentExtension, ComplexContentExtensionSubelements, ComplexContentExtensionAttributes, InitComplexContentExtension, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdSimpleContent, State.SimpleContent, SimpleContentSubelements, SimpleContentAttributes, InitSimpleContent, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdSimpleContentExtension, State.SimpleContentExtension, SimpleContentExtensionSubelements, SimpleContentExtensionAttributes, InitSimpleContentExtension, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdSimpleContentRestriction, State.SimpleContentRestriction, SimpleContentRestrictionSubelements, SimpleContentRestrictionAttributes, InitSimpleContentRestriction, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdSimpleTypeUnion, State.SimpleTypeUnion, SimpleTypeUnionSubelements, SimpleTypeUnionAttributes, InitSimpleTypeUnion, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdSimpleTypeList, State.SimpleTypeList, SimpleTypeListSubelements, SimpleTypeListAttributes, InitSimpleTypeList, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdSimpleTypeRestriction, State.SimpleTypeRestriction, SimpleTypeRestrictionSubelements, SimpleTypeRestrictionAttributes, InitSimpleTypeRestriction, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdUnique, State.Unique, IdentityConstraintSubelements, IdentityConstraintAttributes, InitIdentityConstraint, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdKey, State.Key, IdentityConstraintSubelements, IdentityConstraintAttributes, InitIdentityConstraint, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdKeyref, State.KeyRef, IdentityConstraintSubelements, IdentityConstraintAttributes, InitIdentityConstraint, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdSelector, State.Selector, AnnotatedSubelements, SelectorAttributes, InitSelector, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdField, State.Field, AnnotatedSubelements, FieldAttributes, InitField, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdMinExclusive, State.MinExclusive, AnnotatedSubelements, FacetAttributes, InitFacet, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdMinInclusive, State.MinInclusive, AnnotatedSubelements, FacetAttributes, InitFacet, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdMaxExclusive, State.MaxExclusive, AnnotatedSubelements, FacetAttributes, InitFacet, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdMaxInclusive, State.MaxInclusive, AnnotatedSubelements, FacetAttributes, InitFacet, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdTotalDigits, State.TotalDigits, AnnotatedSubelements, FacetAttributes, InitFacet, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdFractionDigits, State.FractionDigits, AnnotatedSubelements, FacetAttributes, InitFacet, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdLength, State.Length, AnnotatedSubelements, FacetAttributes, InitFacet, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdMinLength, State.MinLength, AnnotatedSubelements, FacetAttributes, InitFacet, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdMaxLength, State.MaxLength, AnnotatedSubelements, FacetAttributes, InitFacet, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdEnumeration, State.Enumeration, AnnotatedSubelements, FacetAttributes, InitFacet, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdPattern, State.Pattern, AnnotatedSubelements, FacetAttributes, InitFacet, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdWhitespace, State.WhiteSpace, AnnotatedSubelements, FacetAttributes, InitFacet, null, parseContent: true),
		new XsdEntry(SchemaNames.Token.XsdAppInfo, State.AppInfo, null, AppinfoAttributes, InitAppinfo, EndAppinfo, parseContent: false),
		new XsdEntry(SchemaNames.Token.XsdDocumentation, State.Documentation, null, DocumentationAttributes, InitDocumentation, EndDocumentation, parseContent: false),
		new XsdEntry(SchemaNames.Token.XsdRedefine, State.Redefine, RedefineSubelements, RedefineAttributes, InitRedefine, EndRedefine, parseContent: true)
	};

	private static readonly int[] DerivationMethodValues = new int[6] { 1, 2, 4, 8, 16, 255 };

	private static readonly string[] DerivationMethodStrings = new string[6] { "substitution", "extension", "restriction", "list", "union", "#all" };

	private static readonly string[] FormStringValues = new string[2] { "qualified", "unqualified" };

	private static readonly string[] UseStringValues = new string[3] { "optional", "prohibited", "required" };

	private static readonly string[] ProcessContentsStringValues = new string[3] { "skip", "lax", "strict" };

	private XmlReader reader;

	private PositionInfo positionInfo;

	private XsdEntry currentEntry;

	private XsdEntry nextEntry;

	private bool hasChild;

	private HWStack stateHistory = new HWStack(10);

	private Stack containerStack = new Stack();

	private XmlNameTable nameTable;

	private SchemaNames schemaNames;

	private XmlNamespaceManager namespaceManager;

	private bool canIncludeImport;

	private XmlSchema schema;

	private XmlSchemaObject xso;

	private XmlSchemaElement element;

	private XmlSchemaAny anyElement;

	private XmlSchemaAttribute attribute;

	private XmlSchemaAnyAttribute anyAttribute;

	private XmlSchemaComplexType complexType;

	private XmlSchemaSimpleType simpleType;

	private XmlSchemaComplexContent complexContent;

	private XmlSchemaComplexContentExtension complexContentExtension;

	private XmlSchemaComplexContentRestriction complexContentRestriction;

	private XmlSchemaSimpleContent simpleContent;

	private XmlSchemaSimpleContentExtension simpleContentExtension;

	private XmlSchemaSimpleContentRestriction simpleContentRestriction;

	private XmlSchemaSimpleTypeUnion simpleTypeUnion;

	private XmlSchemaSimpleTypeList simpleTypeList;

	private XmlSchemaSimpleTypeRestriction simpleTypeRestriction;

	private XmlSchemaGroup group;

	private XmlSchemaGroupRef groupRef;

	private XmlSchemaAll all;

	private XmlSchemaChoice choice;

	private XmlSchemaSequence sequence;

	private XmlSchemaParticle particle;

	private XmlSchemaAttributeGroup attributeGroup;

	private XmlSchemaAttributeGroupRef attributeGroupRef;

	private XmlSchemaNotation notation;

	private XmlSchemaIdentityConstraint identityConstraint;

	private XmlSchemaXPath xpath;

	private XmlSchemaInclude include;

	private XmlSchemaImport import;

	private XmlSchemaAnnotation annotation;

	private XmlSchemaAppInfo appInfo;

	private XmlSchemaDocumentation documentation;

	private XmlSchemaFacet facet;

	private XmlNode[] markup;

	private XmlSchemaRedefine redefine;

	private ValidationEventHandler validationEventHandler;

	private ArrayList unhandledAttributes = new ArrayList();

	private Hashtable namespaces;

	private SchemaNames.Token CurrentElement => currentEntry.Name;

	private SchemaNames.Token ParentElement => ((XsdEntry)stateHistory[stateHistory.Length - 1]).Name;

	private XmlSchemaObject ParentContainer => (XmlSchemaObject)containerStack.Peek();

	internal XsdBuilder(XmlReader reader, XmlNamespaceManager curmgr, XmlSchema schema, XmlNameTable nameTable, SchemaNames schemaNames, ValidationEventHandler eventhandler)
	{
		this.reader = reader;
		xso = (this.schema = schema);
		namespaceManager = new BuilderNamespaceManager(curmgr, reader);
		validationEventHandler = eventhandler;
		this.nameTable = nameTable;
		this.schemaNames = schemaNames;
		stateHistory = new HWStack(10);
		currentEntry = SchemaEntries[0];
		positionInfo = PositionInfo.GetPositionInfo(reader);
	}

	internal override bool ProcessElement(string prefix, string name, string ns)
	{
		XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(name, ns);
		if (GetNextState(xmlQualifiedName))
		{
			Push();
			xso = null;
			currentEntry.InitFunc(this, null);
			RecordPosition();
			return true;
		}
		if (!IsSkipableElement(xmlQualifiedName))
		{
			SendValidationEvent("The '{0}' element is not supported in this context.", xmlQualifiedName.ToString());
		}
		return false;
	}

	internal override void ProcessAttribute(string prefix, string name, string ns, string value)
	{
		XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(name, ns);
		if (currentEntry.Attributes != null)
		{
			for (int i = 0; i < currentEntry.Attributes.Length; i++)
			{
				XsdAttributeEntry xsdAttributeEntry = currentEntry.Attributes[i];
				if (schemaNames.TokenToQName[(int)xsdAttributeEntry.Attribute].Equals(xmlQualifiedName))
				{
					try
					{
						xsdAttributeEntry.BuildFunc(this, value);
						return;
					}
					catch (XmlSchemaException ex)
					{
						ex.SetSource(reader.BaseURI, positionInfo.LineNumber, positionInfo.LinePosition);
						SendValidationEvent("The value for the '{0}' attribute is invalid - {1}", new string[2] { name, ex.Message }, XmlSeverityType.Error);
						return;
					}
				}
			}
		}
		if (ns != schemaNames.NsXs && ns.Length != 0)
		{
			if (ns == schemaNames.NsXmlNs)
			{
				if (namespaces == null)
				{
					namespaces = new Hashtable();
				}
				namespaces.Add((name == schemaNames.QnXmlNs.Name) ? string.Empty : name, value);
			}
			else
			{
				XmlAttribute xmlAttribute = new XmlAttribute(prefix, name, ns, schema.Document);
				xmlAttribute.Value = value;
				unhandledAttributes.Add(xmlAttribute);
			}
		}
		else
		{
			SendValidationEvent("The '{0}' attribute is not supported in this context.", xmlQualifiedName.ToString());
		}
	}

	internal override bool IsContentParsed()
	{
		return currentEntry.ParseContent;
	}

	internal override void ProcessMarkup(XmlNode[] markup)
	{
		this.markup = markup;
	}

	internal override void ProcessCData(string value)
	{
		SendValidationEvent("The following text is not allowed in this context: '{0}'.", value);
	}

	internal override void StartChildren()
	{
		if (xso != null)
		{
			if (namespaces != null && namespaces.Count > 0)
			{
				xso.Namespaces.Namespaces = namespaces;
				namespaces = null;
			}
			if (unhandledAttributes.Count != 0)
			{
				xso.SetUnhandledAttributes((XmlAttribute[])unhandledAttributes.ToArray(typeof(XmlAttribute)));
				unhandledAttributes.Clear();
			}
		}
	}

	internal override void EndChildren()
	{
		if (currentEntry.EndChildFunc != null)
		{
			currentEntry.EndChildFunc(this);
		}
		Pop();
	}

	private void Push()
	{
		stateHistory.Push();
		stateHistory[stateHistory.Length - 1] = currentEntry;
		containerStack.Push(GetContainer(currentEntry.CurrentState));
		currentEntry = nextEntry;
		if (currentEntry.Name != SchemaNames.Token.XsdAnnotation)
		{
			hasChild = false;
		}
	}

	private void Pop()
	{
		currentEntry = (XsdEntry)stateHistory.Pop();
		SetContainer(currentEntry.CurrentState, containerStack.Pop());
		hasChild = true;
	}

	private XmlSchemaObject GetContainer(State state)
	{
		XmlSchemaObject result = null;
		switch (state)
		{
		case State.Schema:
			result = schema;
			break;
		case State.Annotation:
			result = annotation;
			break;
		case State.Include:
			result = include;
			break;
		case State.Import:
			result = import;
			break;
		case State.Element:
			result = element;
			break;
		case State.Attribute:
			result = attribute;
			break;
		case State.AttributeGroup:
			result = attributeGroup;
			break;
		case State.AttributeGroupRef:
			result = attributeGroupRef;
			break;
		case State.AnyAttribute:
			result = anyAttribute;
			break;
		case State.Group:
			result = group;
			break;
		case State.GroupRef:
			result = groupRef;
			break;
		case State.All:
			result = all;
			break;
		case State.Choice:
			result = choice;
			break;
		case State.Sequence:
			result = sequence;
			break;
		case State.Any:
			result = anyElement;
			break;
		case State.Notation:
			result = notation;
			break;
		case State.SimpleType:
			result = simpleType;
			break;
		case State.ComplexType:
			result = complexType;
			break;
		case State.ComplexContent:
			result = complexContent;
			break;
		case State.ComplexContentExtension:
			result = complexContentExtension;
			break;
		case State.ComplexContentRestriction:
			result = complexContentRestriction;
			break;
		case State.SimpleContent:
			result = simpleContent;
			break;
		case State.SimpleContentExtension:
			result = simpleContentExtension;
			break;
		case State.SimpleContentRestriction:
			result = simpleContentRestriction;
			break;
		case State.SimpleTypeUnion:
			result = simpleTypeUnion;
			break;
		case State.SimpleTypeList:
			result = simpleTypeList;
			break;
		case State.SimpleTypeRestriction:
			result = simpleTypeRestriction;
			break;
		case State.Unique:
		case State.Key:
		case State.KeyRef:
			result = identityConstraint;
			break;
		case State.Selector:
		case State.Field:
			result = xpath;
			break;
		case State.MinExclusive:
		case State.MinInclusive:
		case State.MaxExclusive:
		case State.MaxInclusive:
		case State.TotalDigits:
		case State.FractionDigits:
		case State.Length:
		case State.MinLength:
		case State.MaxLength:
		case State.Enumeration:
		case State.Pattern:
		case State.WhiteSpace:
			result = facet;
			break;
		case State.AppInfo:
			result = appInfo;
			break;
		case State.Documentation:
			result = documentation;
			break;
		case State.Redefine:
			result = redefine;
			break;
		}
		return result;
	}

	private void SetContainer(State state, object container)
	{
		switch (state)
		{
		case State.Annotation:
			annotation = (XmlSchemaAnnotation)container;
			break;
		case State.Include:
			include = (XmlSchemaInclude)container;
			break;
		case State.Import:
			import = (XmlSchemaImport)container;
			break;
		case State.Element:
			element = (XmlSchemaElement)container;
			break;
		case State.Attribute:
			attribute = (XmlSchemaAttribute)container;
			break;
		case State.AttributeGroup:
			attributeGroup = (XmlSchemaAttributeGroup)container;
			break;
		case State.AttributeGroupRef:
			attributeGroupRef = (XmlSchemaAttributeGroupRef)container;
			break;
		case State.AnyAttribute:
			anyAttribute = (XmlSchemaAnyAttribute)container;
			break;
		case State.Group:
			group = (XmlSchemaGroup)container;
			break;
		case State.GroupRef:
			groupRef = (XmlSchemaGroupRef)container;
			break;
		case State.All:
			all = (XmlSchemaAll)container;
			break;
		case State.Choice:
			choice = (XmlSchemaChoice)container;
			break;
		case State.Sequence:
			sequence = (XmlSchemaSequence)container;
			break;
		case State.Any:
			anyElement = (XmlSchemaAny)container;
			break;
		case State.Notation:
			notation = (XmlSchemaNotation)container;
			break;
		case State.SimpleType:
			simpleType = (XmlSchemaSimpleType)container;
			break;
		case State.ComplexType:
			complexType = (XmlSchemaComplexType)container;
			break;
		case State.ComplexContent:
			complexContent = (XmlSchemaComplexContent)container;
			break;
		case State.ComplexContentExtension:
			complexContentExtension = (XmlSchemaComplexContentExtension)container;
			break;
		case State.ComplexContentRestriction:
			complexContentRestriction = (XmlSchemaComplexContentRestriction)container;
			break;
		case State.SimpleContent:
			simpleContent = (XmlSchemaSimpleContent)container;
			break;
		case State.SimpleContentExtension:
			simpleContentExtension = (XmlSchemaSimpleContentExtension)container;
			break;
		case State.SimpleContentRestriction:
			simpleContentRestriction = (XmlSchemaSimpleContentRestriction)container;
			break;
		case State.SimpleTypeUnion:
			simpleTypeUnion = (XmlSchemaSimpleTypeUnion)container;
			break;
		case State.SimpleTypeList:
			simpleTypeList = (XmlSchemaSimpleTypeList)container;
			break;
		case State.SimpleTypeRestriction:
			simpleTypeRestriction = (XmlSchemaSimpleTypeRestriction)container;
			break;
		case State.Unique:
		case State.Key:
		case State.KeyRef:
			identityConstraint = (XmlSchemaIdentityConstraint)container;
			break;
		case State.Selector:
		case State.Field:
			xpath = (XmlSchemaXPath)container;
			break;
		case State.MinExclusive:
		case State.MinInclusive:
		case State.MaxExclusive:
		case State.MaxInclusive:
		case State.TotalDigits:
		case State.FractionDigits:
		case State.Length:
		case State.MinLength:
		case State.MaxLength:
		case State.Enumeration:
		case State.Pattern:
		case State.WhiteSpace:
			facet = (XmlSchemaFacet)container;
			break;
		case State.AppInfo:
			appInfo = (XmlSchemaAppInfo)container;
			break;
		case State.Documentation:
			documentation = (XmlSchemaDocumentation)container;
			break;
		case State.Redefine:
			redefine = (XmlSchemaRedefine)container;
			break;
		case State.Root:
		case State.Schema:
			break;
		}
	}

	private static void BuildAnnotated_Id(XsdBuilder builder, string value)
	{
		builder.xso.IdAttribute = value;
	}

	private static void BuildSchema_AttributeFormDefault(XsdBuilder builder, string value)
	{
		builder.schema.AttributeFormDefault = (XmlSchemaForm)builder.ParseEnum(value, "attributeFormDefault", FormStringValues);
	}

	private static void BuildSchema_ElementFormDefault(XsdBuilder builder, string value)
	{
		builder.schema.ElementFormDefault = (XmlSchemaForm)builder.ParseEnum(value, "elementFormDefault", FormStringValues);
	}

	private static void BuildSchema_TargetNamespace(XsdBuilder builder, string value)
	{
		builder.schema.TargetNamespace = value;
	}

	private static void BuildSchema_Version(XsdBuilder builder, string value)
	{
		builder.schema.Version = value;
	}

	private static void BuildSchema_FinalDefault(XsdBuilder builder, string value)
	{
		builder.schema.FinalDefault = (XmlSchemaDerivationMethod)builder.ParseBlockFinalEnum(value, "finalDefault");
	}

	private static void BuildSchema_BlockDefault(XsdBuilder builder, string value)
	{
		builder.schema.BlockDefault = (XmlSchemaDerivationMethod)builder.ParseBlockFinalEnum(value, "blockDefault");
	}

	private static void InitSchema(XsdBuilder builder, string value)
	{
		builder.canIncludeImport = true;
		builder.xso = builder.schema;
	}

	private static void InitInclude(XsdBuilder builder, string value)
	{
		if (!builder.canIncludeImport)
		{
			builder.SendValidationEvent("The 'include' element cannot appear at this location.", null);
		}
		builder.xso = (builder.include = new XmlSchemaInclude());
		builder.schema.Includes.Add(builder.include);
	}

	private static void BuildInclude_SchemaLocation(XsdBuilder builder, string value)
	{
		builder.include.SchemaLocation = value;
	}

	private static void InitImport(XsdBuilder builder, string value)
	{
		if (!builder.canIncludeImport)
		{
			builder.SendValidationEvent("The 'import' element cannot appear at this location.", null);
		}
		builder.xso = (builder.import = new XmlSchemaImport());
		builder.schema.Includes.Add(builder.import);
	}

	private static void BuildImport_Namespace(XsdBuilder builder, string value)
	{
		builder.import.Namespace = value;
	}

	private static void BuildImport_SchemaLocation(XsdBuilder builder, string value)
	{
		builder.import.SchemaLocation = value;
	}

	private static void InitRedefine(XsdBuilder builder, string value)
	{
		if (!builder.canIncludeImport)
		{
			builder.SendValidationEvent("The 'redefine' element cannot appear at this location.", null);
		}
		builder.xso = (builder.redefine = new XmlSchemaRedefine());
		builder.schema.Includes.Add(builder.redefine);
	}

	private static void BuildRedefine_SchemaLocation(XsdBuilder builder, string value)
	{
		builder.redefine.SchemaLocation = value;
	}

	private static void EndRedefine(XsdBuilder builder)
	{
		builder.canIncludeImport = true;
	}

	private static void InitAttribute(XsdBuilder builder, string value)
	{
		builder.xso = (builder.attribute = new XmlSchemaAttribute());
		if (builder.ParentElement == SchemaNames.Token.XsdSchema)
		{
			builder.schema.Items.Add(builder.attribute);
		}
		else
		{
			builder.AddAttribute(builder.attribute);
		}
		builder.canIncludeImport = false;
	}

	private static void BuildAttribute_Default(XsdBuilder builder, string value)
	{
		builder.attribute.DefaultValue = value;
	}

	private static void BuildAttribute_Fixed(XsdBuilder builder, string value)
	{
		builder.attribute.FixedValue = value;
	}

	private static void BuildAttribute_Form(XsdBuilder builder, string value)
	{
		builder.attribute.Form = (XmlSchemaForm)builder.ParseEnum(value, "form", FormStringValues);
	}

	private static void BuildAttribute_Use(XsdBuilder builder, string value)
	{
		builder.attribute.Use = (XmlSchemaUse)builder.ParseEnum(value, "use", UseStringValues);
	}

	private static void BuildAttribute_Ref(XsdBuilder builder, string value)
	{
		builder.attribute.RefName = builder.ParseQName(value, "ref");
	}

	private static void BuildAttribute_Name(XsdBuilder builder, string value)
	{
		builder.attribute.Name = value;
	}

	private static void BuildAttribute_Type(XsdBuilder builder, string value)
	{
		builder.attribute.SchemaTypeName = builder.ParseQName(value, "type");
	}

	private static void InitElement(XsdBuilder builder, string value)
	{
		builder.xso = (builder.element = new XmlSchemaElement());
		builder.canIncludeImport = false;
		switch (builder.ParentElement)
		{
		case SchemaNames.Token.XsdSchema:
			builder.schema.Items.Add(builder.element);
			break;
		case SchemaNames.Token.XsdAll:
			builder.all.Items.Add(builder.element);
			break;
		case SchemaNames.Token.XsdChoice:
			builder.choice.Items.Add(builder.element);
			break;
		case SchemaNames.Token.XsdSequence:
			builder.sequence.Items.Add(builder.element);
			break;
		}
	}

	private static void BuildElement_Abstract(XsdBuilder builder, string value)
	{
		builder.element.IsAbstract = builder.ParseBoolean(value, "abstract");
	}

	private static void BuildElement_Block(XsdBuilder builder, string value)
	{
		builder.element.Block = (XmlSchemaDerivationMethod)builder.ParseBlockFinalEnum(value, "block");
	}

	private static void BuildElement_Default(XsdBuilder builder, string value)
	{
		builder.element.DefaultValue = value;
	}

	private static void BuildElement_Form(XsdBuilder builder, string value)
	{
		builder.element.Form = (XmlSchemaForm)builder.ParseEnum(value, "form", FormStringValues);
	}

	private static void BuildElement_SubstitutionGroup(XsdBuilder builder, string value)
	{
		builder.element.SubstitutionGroup = builder.ParseQName(value, "substitutionGroup");
	}

	private static void BuildElement_Final(XsdBuilder builder, string value)
	{
		builder.element.Final = (XmlSchemaDerivationMethod)builder.ParseBlockFinalEnum(value, "final");
	}

	private static void BuildElement_Fixed(XsdBuilder builder, string value)
	{
		builder.element.FixedValue = value;
	}

	private static void BuildElement_MaxOccurs(XsdBuilder builder, string value)
	{
		builder.SetMaxOccurs(builder.element, value);
	}

	private static void BuildElement_MinOccurs(XsdBuilder builder, string value)
	{
		builder.SetMinOccurs(builder.element, value);
	}

	private static void BuildElement_Name(XsdBuilder builder, string value)
	{
		builder.element.Name = value;
	}

	private static void BuildElement_Nillable(XsdBuilder builder, string value)
	{
		builder.element.IsNillable = builder.ParseBoolean(value, "nillable");
	}

	private static void BuildElement_Ref(XsdBuilder builder, string value)
	{
		builder.element.RefName = builder.ParseQName(value, "ref");
	}

	private static void BuildElement_Type(XsdBuilder builder, string value)
	{
		builder.element.SchemaTypeName = builder.ParseQName(value, "type");
	}

	private static void InitSimpleType(XsdBuilder builder, string value)
	{
		builder.xso = (builder.simpleType = new XmlSchemaSimpleType());
		switch (builder.ParentElement)
		{
		case SchemaNames.Token.XsdSchema:
			builder.canIncludeImport = false;
			builder.schema.Items.Add(builder.simpleType);
			break;
		case SchemaNames.Token.XsdRedefine:
			builder.redefine.Items.Add(builder.simpleType);
			break;
		case SchemaNames.Token.XsdAttribute:
			if (builder.attribute.SchemaType != null)
			{
				builder.SendValidationEvent("'{0}' is a duplicate XSD element.", "simpleType");
			}
			builder.attribute.SchemaType = builder.simpleType;
			break;
		case SchemaNames.Token.XsdElement:
			if (builder.element.SchemaType != null)
			{
				builder.SendValidationEvent("'{0}' is a duplicate XSD element.", "simpleType");
			}
			if (builder.element.Constraints.Count != 0)
			{
				builder.SendValidationEvent("'simpleType' or 'complexType' cannot follow 'unique', 'key' or 'keyref'.", null);
			}
			builder.element.SchemaType = builder.simpleType;
			break;
		case SchemaNames.Token.XsdSimpleTypeList:
			if (builder.simpleTypeList.ItemType != null)
			{
				builder.SendValidationEvent("'{0}' is a duplicate XSD element.", "simpleType");
			}
			builder.simpleTypeList.ItemType = builder.simpleType;
			break;
		case SchemaNames.Token.XsdSimpleTypeRestriction:
			if (builder.simpleTypeRestriction.BaseType != null)
			{
				builder.SendValidationEvent("'{0}' is a duplicate XSD element.", "simpleType");
			}
			builder.simpleTypeRestriction.BaseType = builder.simpleType;
			break;
		case SchemaNames.Token.XsdSimpleContentRestriction:
			if (builder.simpleContentRestriction.BaseType != null)
			{
				builder.SendValidationEvent("'{0}' is a duplicate XSD element.", "simpleType");
			}
			if (builder.simpleContentRestriction.Attributes.Count != 0 || builder.simpleContentRestriction.AnyAttribute != null || builder.simpleContentRestriction.Facets.Count != 0)
			{
				builder.SendValidationEvent("'simpleType' should be the first child of restriction.", null);
			}
			builder.simpleContentRestriction.BaseType = builder.simpleType;
			break;
		case SchemaNames.Token.XsdSimpleTypeUnion:
			builder.simpleTypeUnion.BaseTypes.Add(builder.simpleType);
			break;
		}
	}

	private static void BuildSimpleType_Name(XsdBuilder builder, string value)
	{
		builder.simpleType.Name = value;
	}

	private static void BuildSimpleType_Final(XsdBuilder builder, string value)
	{
		builder.simpleType.Final = (XmlSchemaDerivationMethod)builder.ParseBlockFinalEnum(value, "final");
	}

	private static void InitSimpleTypeUnion(XsdBuilder builder, string value)
	{
		if (builder.simpleType.Content != null)
		{
			builder.SendValidationEvent("'simpleType' should have only one child 'union', 'list', or 'restriction'.", null);
		}
		builder.xso = (builder.simpleTypeUnion = new XmlSchemaSimpleTypeUnion());
		builder.simpleType.Content = builder.simpleTypeUnion;
	}

	private static void BuildSimpleTypeUnion_MemberTypes(XsdBuilder builder, string value)
	{
		XmlSchemaDatatype xmlSchemaDatatype = XmlSchemaDatatype.FromXmlTokenizedTypeXsd(XmlTokenizedType.QName).DeriveByList(null);
		try
		{
			builder.simpleTypeUnion.MemberTypes = (XmlQualifiedName[])xmlSchemaDatatype.ParseValue(value, builder.nameTable, builder.namespaceManager);
		}
		catch (XmlSchemaException ex)
		{
			ex.SetSource(builder.reader.BaseURI, builder.positionInfo.LineNumber, builder.positionInfo.LinePosition);
			builder.SendValidationEvent(ex);
		}
	}

	private static void InitSimpleTypeList(XsdBuilder builder, string value)
	{
		if (builder.simpleType.Content != null)
		{
			builder.SendValidationEvent("'simpleType' should have only one child 'union', 'list', or 'restriction'.", null);
		}
		builder.xso = (builder.simpleTypeList = new XmlSchemaSimpleTypeList());
		builder.simpleType.Content = builder.simpleTypeList;
	}

	private static void BuildSimpleTypeList_ItemType(XsdBuilder builder, string value)
	{
		builder.simpleTypeList.ItemTypeName = builder.ParseQName(value, "itemType");
	}

	private static void InitSimpleTypeRestriction(XsdBuilder builder, string value)
	{
		if (builder.simpleType.Content != null)
		{
			builder.SendValidationEvent("'simpleType' should have only one child 'union', 'list', or 'restriction'.", null);
		}
		builder.xso = (builder.simpleTypeRestriction = new XmlSchemaSimpleTypeRestriction());
		builder.simpleType.Content = builder.simpleTypeRestriction;
	}

	private static void BuildSimpleTypeRestriction_Base(XsdBuilder builder, string value)
	{
		builder.simpleTypeRestriction.BaseTypeName = builder.ParseQName(value, "base");
	}

	private static void InitComplexType(XsdBuilder builder, string value)
	{
		builder.xso = (builder.complexType = new XmlSchemaComplexType());
		switch (builder.ParentElement)
		{
		case SchemaNames.Token.XsdSchema:
			builder.canIncludeImport = false;
			builder.schema.Items.Add(builder.complexType);
			break;
		case SchemaNames.Token.XsdRedefine:
			builder.redefine.Items.Add(builder.complexType);
			break;
		case SchemaNames.Token.XsdElement:
			if (builder.element.SchemaType != null)
			{
				builder.SendValidationEvent("The '{0}' element already exists in the content model.", "complexType");
			}
			if (builder.element.Constraints.Count != 0)
			{
				builder.SendValidationEvent("'simpleType' or 'complexType' cannot follow 'unique', 'key' or 'keyref'.", null);
			}
			builder.element.SchemaType = builder.complexType;
			break;
		}
	}

	private static void BuildComplexType_Abstract(XsdBuilder builder, string value)
	{
		builder.complexType.IsAbstract = builder.ParseBoolean(value, "abstract");
	}

	private static void BuildComplexType_Block(XsdBuilder builder, string value)
	{
		builder.complexType.Block = (XmlSchemaDerivationMethod)builder.ParseBlockFinalEnum(value, "block");
	}

	private static void BuildComplexType_Final(XsdBuilder builder, string value)
	{
		builder.complexType.Final = (XmlSchemaDerivationMethod)builder.ParseBlockFinalEnum(value, "final");
	}

	private static void BuildComplexType_Mixed(XsdBuilder builder, string value)
	{
		builder.complexType.IsMixed = builder.ParseBoolean(value, "mixed");
	}

	private static void BuildComplexType_Name(XsdBuilder builder, string value)
	{
		builder.complexType.Name = value;
	}

	private static void InitComplexContent(XsdBuilder builder, string value)
	{
		if (builder.complexType.ContentModel != null || builder.complexType.Particle != null || builder.complexType.Attributes.Count != 0 || builder.complexType.AnyAttribute != null)
		{
			builder.SendValidationEvent("The content model of a complex type must consist of 'annotation' (if present); followed by zero or one of the following: 'simpleContent', 'complexContent', 'group', 'choice', 'sequence', or 'all'; followed by zero or more 'attribute' or 'attributeGroup'; followed by zero or one 'anyAttribute'.", "complexContent");
		}
		builder.xso = (builder.complexContent = new XmlSchemaComplexContent());
		builder.complexType.ContentModel = builder.complexContent;
	}

	private static void BuildComplexContent_Mixed(XsdBuilder builder, string value)
	{
		builder.complexContent.IsMixed = builder.ParseBoolean(value, "mixed");
	}

	private static void InitComplexContentExtension(XsdBuilder builder, string value)
	{
		if (builder.complexContent.Content != null)
		{
			builder.SendValidationEvent("Complex content restriction or extension should consist of zero or one of 'group', 'choice', 'sequence', or 'all'; followed by zero or more 'attribute' or 'attributeGroup'; followed by zero or one 'anyAttribute'.", "extension");
		}
		builder.xso = (builder.complexContentExtension = new XmlSchemaComplexContentExtension());
		builder.complexContent.Content = builder.complexContentExtension;
	}

	private static void BuildComplexContentExtension_Base(XsdBuilder builder, string value)
	{
		builder.complexContentExtension.BaseTypeName = builder.ParseQName(value, "base");
	}

	private static void InitComplexContentRestriction(XsdBuilder builder, string value)
	{
		builder.xso = (builder.complexContentRestriction = new XmlSchemaComplexContentRestriction());
		builder.complexContent.Content = builder.complexContentRestriction;
	}

	private static void BuildComplexContentRestriction_Base(XsdBuilder builder, string value)
	{
		builder.complexContentRestriction.BaseTypeName = builder.ParseQName(value, "base");
	}

	private static void InitSimpleContent(XsdBuilder builder, string value)
	{
		if (builder.complexType.ContentModel != null || builder.complexType.Particle != null || builder.complexType.Attributes.Count != 0 || builder.complexType.AnyAttribute != null)
		{
			builder.SendValidationEvent("The content model of a complex type must consist of 'annotation' (if present); followed by zero or one of the following: 'simpleContent', 'complexContent', 'group', 'choice', 'sequence', or 'all'; followed by zero or more 'attribute' or 'attributeGroup'; followed by zero or one 'anyAttribute'.", "simpleContent");
		}
		builder.xso = (builder.simpleContent = new XmlSchemaSimpleContent());
		builder.complexType.ContentModel = builder.simpleContent;
	}

	private static void InitSimpleContentExtension(XsdBuilder builder, string value)
	{
		if (builder.simpleContent.Content != null)
		{
			builder.SendValidationEvent("The '{0}' element already exists in the content model.", "extension");
		}
		builder.xso = (builder.simpleContentExtension = new XmlSchemaSimpleContentExtension());
		builder.simpleContent.Content = builder.simpleContentExtension;
	}

	private static void BuildSimpleContentExtension_Base(XsdBuilder builder, string value)
	{
		builder.simpleContentExtension.BaseTypeName = builder.ParseQName(value, "base");
	}

	private static void InitSimpleContentRestriction(XsdBuilder builder, string value)
	{
		if (builder.simpleContent.Content != null)
		{
			builder.SendValidationEvent("The '{0}' element already exists in the content model.", "restriction");
		}
		builder.xso = (builder.simpleContentRestriction = new XmlSchemaSimpleContentRestriction());
		builder.simpleContent.Content = builder.simpleContentRestriction;
	}

	private static void BuildSimpleContentRestriction_Base(XsdBuilder builder, string value)
	{
		builder.simpleContentRestriction.BaseTypeName = builder.ParseQName(value, "base");
	}

	private static void InitAttributeGroup(XsdBuilder builder, string value)
	{
		builder.canIncludeImport = false;
		builder.xso = (builder.attributeGroup = new XmlSchemaAttributeGroup());
		switch (builder.ParentElement)
		{
		case SchemaNames.Token.XsdSchema:
			builder.schema.Items.Add(builder.attributeGroup);
			break;
		case SchemaNames.Token.XsdRedefine:
			builder.redefine.Items.Add(builder.attributeGroup);
			break;
		}
	}

	private static void BuildAttributeGroup_Name(XsdBuilder builder, string value)
	{
		builder.attributeGroup.Name = value;
	}

	private static void InitAttributeGroupRef(XsdBuilder builder, string value)
	{
		builder.xso = (builder.attributeGroupRef = new XmlSchemaAttributeGroupRef());
		builder.AddAttribute(builder.attributeGroupRef);
	}

	private static void BuildAttributeGroupRef_Ref(XsdBuilder builder, string value)
	{
		builder.attributeGroupRef.RefName = builder.ParseQName(value, "ref");
	}

	private static void InitAnyAttribute(XsdBuilder builder, string value)
	{
		builder.xso = (builder.anyAttribute = new XmlSchemaAnyAttribute());
		switch (builder.ParentElement)
		{
		case SchemaNames.Token.XsdComplexType:
			if (builder.complexType.ContentModel != null)
			{
				builder.SendValidationEvent("'{0}' and content model are mutually exclusive.", "anyAttribute");
			}
			if (builder.complexType.AnyAttribute != null)
			{
				builder.SendValidationEvent("The '{0}' element already exists in the content model.", "anyAttribute");
			}
			builder.complexType.AnyAttribute = builder.anyAttribute;
			break;
		case SchemaNames.Token.XsdSimpleContentRestriction:
			if (builder.simpleContentRestriction.AnyAttribute != null)
			{
				builder.SendValidationEvent("The '{0}' element already exists in the content model.", "anyAttribute");
			}
			builder.simpleContentRestriction.AnyAttribute = builder.anyAttribute;
			break;
		case SchemaNames.Token.XsdSimpleContentExtension:
			if (builder.simpleContentExtension.AnyAttribute != null)
			{
				builder.SendValidationEvent("The '{0}' element already exists in the content model.", "anyAttribute");
			}
			builder.simpleContentExtension.AnyAttribute = builder.anyAttribute;
			break;
		case SchemaNames.Token.XsdComplexContentExtension:
			if (builder.complexContentExtension.AnyAttribute != null)
			{
				builder.SendValidationEvent("The '{0}' element already exists in the content model.", "anyAttribute");
			}
			builder.complexContentExtension.AnyAttribute = builder.anyAttribute;
			break;
		case SchemaNames.Token.XsdComplexContentRestriction:
			if (builder.complexContentRestriction.AnyAttribute != null)
			{
				builder.SendValidationEvent("The '{0}' element already exists in the content model.", "anyAttribute");
			}
			builder.complexContentRestriction.AnyAttribute = builder.anyAttribute;
			break;
		case SchemaNames.Token.xsdAttributeGroup:
			if (builder.attributeGroup.AnyAttribute != null)
			{
				builder.SendValidationEvent("The '{0}' element already exists in the content model.", "anyAttribute");
			}
			builder.attributeGroup.AnyAttribute = builder.anyAttribute;
			break;
		}
	}

	private static void BuildAnyAttribute_Namespace(XsdBuilder builder, string value)
	{
		builder.anyAttribute.Namespace = value;
	}

	private static void BuildAnyAttribute_ProcessContents(XsdBuilder builder, string value)
	{
		builder.anyAttribute.ProcessContents = (XmlSchemaContentProcessing)builder.ParseEnum(value, "processContents", ProcessContentsStringValues);
	}

	private static void InitGroup(XsdBuilder builder, string value)
	{
		builder.xso = (builder.group = new XmlSchemaGroup());
		builder.canIncludeImport = false;
		switch (builder.ParentElement)
		{
		case SchemaNames.Token.XsdSchema:
			builder.schema.Items.Add(builder.group);
			break;
		case SchemaNames.Token.XsdRedefine:
			builder.redefine.Items.Add(builder.group);
			break;
		}
	}

	private static void BuildGroup_Name(XsdBuilder builder, string value)
	{
		builder.group.Name = value;
	}

	private static void InitGroupRef(XsdBuilder builder, string value)
	{
		builder.xso = (builder.particle = (builder.groupRef = new XmlSchemaGroupRef()));
		builder.AddParticle(builder.groupRef);
	}

	private static void BuildParticle_MaxOccurs(XsdBuilder builder, string value)
	{
		builder.SetMaxOccurs(builder.particle, value);
	}

	private static void BuildParticle_MinOccurs(XsdBuilder builder, string value)
	{
		builder.SetMinOccurs(builder.particle, value);
	}

	private static void BuildGroupRef_Ref(XsdBuilder builder, string value)
	{
		builder.groupRef.RefName = builder.ParseQName(value, "ref");
	}

	private static void InitAll(XsdBuilder builder, string value)
	{
		builder.xso = (builder.particle = (builder.all = new XmlSchemaAll()));
		builder.AddParticle(builder.all);
	}

	private static void InitChoice(XsdBuilder builder, string value)
	{
		builder.xso = (builder.particle = (builder.choice = new XmlSchemaChoice()));
		builder.AddParticle(builder.choice);
	}

	private static void InitSequence(XsdBuilder builder, string value)
	{
		builder.xso = (builder.particle = (builder.sequence = new XmlSchemaSequence()));
		builder.AddParticle(builder.sequence);
	}

	private static void InitAny(XsdBuilder builder, string value)
	{
		builder.xso = (builder.particle = (builder.anyElement = new XmlSchemaAny()));
		builder.AddParticle(builder.anyElement);
	}

	private static void BuildAny_Namespace(XsdBuilder builder, string value)
	{
		builder.anyElement.Namespace = value;
	}

	private static void BuildAny_ProcessContents(XsdBuilder builder, string value)
	{
		builder.anyElement.ProcessContents = (XmlSchemaContentProcessing)builder.ParseEnum(value, "processContents", ProcessContentsStringValues);
	}

	private static void InitNotation(XsdBuilder builder, string value)
	{
		builder.xso = (builder.notation = new XmlSchemaNotation());
		builder.canIncludeImport = false;
		builder.schema.Items.Add(builder.notation);
	}

	private static void BuildNotation_Name(XsdBuilder builder, string value)
	{
		builder.notation.Name = value;
	}

	private static void BuildNotation_Public(XsdBuilder builder, string value)
	{
		builder.notation.Public = value;
	}

	private static void BuildNotation_System(XsdBuilder builder, string value)
	{
		builder.notation.System = value;
	}

	private static void InitFacet(XsdBuilder builder, string value)
	{
		switch (builder.CurrentElement)
		{
		case SchemaNames.Token.XsdEnumeration:
			builder.facet = new XmlSchemaEnumerationFacet();
			break;
		case SchemaNames.Token.XsdLength:
			builder.facet = new XmlSchemaLengthFacet();
			break;
		case SchemaNames.Token.XsdMaxExclusive:
			builder.facet = new XmlSchemaMaxExclusiveFacet();
			break;
		case SchemaNames.Token.XsdMaxInclusive:
			builder.facet = new XmlSchemaMaxInclusiveFacet();
			break;
		case SchemaNames.Token.XsdMaxLength:
			builder.facet = new XmlSchemaMaxLengthFacet();
			break;
		case SchemaNames.Token.XsdMinExclusive:
			builder.facet = new XmlSchemaMinExclusiveFacet();
			break;
		case SchemaNames.Token.XsdMinInclusive:
			builder.facet = new XmlSchemaMinInclusiveFacet();
			break;
		case SchemaNames.Token.XsdMinLength:
			builder.facet = new XmlSchemaMinLengthFacet();
			break;
		case SchemaNames.Token.XsdPattern:
			builder.facet = new XmlSchemaPatternFacet();
			break;
		case SchemaNames.Token.XsdTotalDigits:
			builder.facet = new XmlSchemaTotalDigitsFacet();
			break;
		case SchemaNames.Token.XsdFractionDigits:
			builder.facet = new XmlSchemaFractionDigitsFacet();
			break;
		case SchemaNames.Token.XsdWhitespace:
			builder.facet = new XmlSchemaWhiteSpaceFacet();
			break;
		}
		builder.xso = builder.facet;
		if (SchemaNames.Token.XsdSimpleTypeRestriction == builder.ParentElement)
		{
			builder.simpleTypeRestriction.Facets.Add(builder.facet);
			return;
		}
		if (builder.simpleContentRestriction.Attributes.Count != 0 || builder.simpleContentRestriction.AnyAttribute != null)
		{
			builder.SendValidationEvent("Facet should go before 'attribute', 'attributeGroup', or 'anyAttribute'.", null);
		}
		builder.simpleContentRestriction.Facets.Add(builder.facet);
	}

	private static void BuildFacet_Fixed(XsdBuilder builder, string value)
	{
		builder.facet.IsFixed = builder.ParseBoolean(value, "fixed");
	}

	private static void BuildFacet_Value(XsdBuilder builder, string value)
	{
		builder.facet.Value = value;
	}

	private static void InitIdentityConstraint(XsdBuilder builder, string value)
	{
		if (!builder.element.RefName.IsEmpty)
		{
			builder.SendValidationEvent("When the ref attribute is present, the type attribute and complexType, simpleType, key, keyref, and unique elements cannot be present.", null);
		}
		switch (builder.CurrentElement)
		{
		case SchemaNames.Token.XsdUnique:
			builder.xso = (builder.identityConstraint = new XmlSchemaUnique());
			break;
		case SchemaNames.Token.XsdKey:
			builder.xso = (builder.identityConstraint = new XmlSchemaKey());
			break;
		case SchemaNames.Token.XsdKeyref:
			builder.xso = (builder.identityConstraint = new XmlSchemaKeyref());
			break;
		}
		builder.element.Constraints.Add(builder.identityConstraint);
	}

	private static void BuildIdentityConstraint_Name(XsdBuilder builder, string value)
	{
		builder.identityConstraint.Name = value;
	}

	private static void BuildIdentityConstraint_Refer(XsdBuilder builder, string value)
	{
		if (builder.identityConstraint is XmlSchemaKeyref)
		{
			((XmlSchemaKeyref)builder.identityConstraint).Refer = builder.ParseQName(value, "refer");
		}
		else
		{
			builder.SendValidationEvent("The '{0}' attribute is not supported in this context.", "refer");
		}
	}

	private static void InitSelector(XsdBuilder builder, string value)
	{
		builder.xso = (builder.xpath = new XmlSchemaXPath());
		if (builder.identityConstraint.Selector == null)
		{
			builder.identityConstraint.Selector = builder.xpath;
		}
		else
		{
			builder.SendValidationEvent("Selector cannot appear twice in one identity constraint.", builder.identityConstraint.Name);
		}
	}

	private static void BuildSelector_XPath(XsdBuilder builder, string value)
	{
		builder.xpath.XPath = value;
	}

	private static void InitField(XsdBuilder builder, string value)
	{
		builder.xso = (builder.xpath = new XmlSchemaXPath());
		if (builder.identityConstraint.Selector == null)
		{
			builder.SendValidationEvent("Cannot define fields before selector.", builder.identityConstraint.Name);
		}
		builder.identityConstraint.Fields.Add(builder.xpath);
	}

	private static void BuildField_XPath(XsdBuilder builder, string value)
	{
		builder.xpath.XPath = value;
	}

	private static void InitAnnotation(XsdBuilder builder, string value)
	{
		if (builder.hasChild && builder.ParentElement != SchemaNames.Token.XsdSchema && builder.ParentElement != SchemaNames.Token.XsdRedefine)
		{
			builder.SendValidationEvent("The 'annotation' element cannot appear at this location.", null);
		}
		builder.xso = (builder.annotation = new XmlSchemaAnnotation());
		builder.ParentContainer.AddAnnotation(builder.annotation);
	}

	private static void InitAppinfo(XsdBuilder builder, string value)
	{
		builder.xso = (builder.appInfo = new XmlSchemaAppInfo());
		builder.annotation.Items.Add(builder.appInfo);
		builder.markup = new XmlNode[0];
	}

	private static void BuildAppinfo_Source(XsdBuilder builder, string value)
	{
		builder.appInfo.Source = ParseUriReference(value);
	}

	private static void EndAppinfo(XsdBuilder builder)
	{
		builder.appInfo.Markup = builder.markup;
	}

	private static void InitDocumentation(XsdBuilder builder, string value)
	{
		builder.xso = (builder.documentation = new XmlSchemaDocumentation());
		builder.annotation.Items.Add(builder.documentation);
		builder.markup = new XmlNode[0];
	}

	private static void BuildDocumentation_Source(XsdBuilder builder, string value)
	{
		builder.documentation.Source = ParseUriReference(value);
	}

	private static void BuildDocumentation_XmlLang(XsdBuilder builder, string value)
	{
		try
		{
			builder.documentation.Language = value;
		}
		catch (XmlSchemaException ex)
		{
			ex.SetSource(builder.reader.BaseURI, builder.positionInfo.LineNumber, builder.positionInfo.LinePosition);
			builder.SendValidationEvent(ex);
		}
	}

	private static void EndDocumentation(XsdBuilder builder)
	{
		builder.documentation.Markup = builder.markup;
	}

	private void AddAttribute(XmlSchemaObject value)
	{
		switch (ParentElement)
		{
		case SchemaNames.Token.XsdComplexType:
			if (complexType.ContentModel != null)
			{
				SendValidationEvent("'{0}' and content model are mutually exclusive.", "attribute");
			}
			if (complexType.AnyAttribute != null)
			{
				SendValidationEvent("'anyAttribute' must be the last child.", null);
			}
			complexType.Attributes.Add(value);
			break;
		case SchemaNames.Token.XsdSimpleContentRestriction:
			if (simpleContentRestriction.AnyAttribute != null)
			{
				SendValidationEvent("'anyAttribute' must be the last child.", null);
			}
			simpleContentRestriction.Attributes.Add(value);
			break;
		case SchemaNames.Token.XsdSimpleContentExtension:
			if (simpleContentExtension.AnyAttribute != null)
			{
				SendValidationEvent("'anyAttribute' must be the last child.", null);
			}
			simpleContentExtension.Attributes.Add(value);
			break;
		case SchemaNames.Token.XsdComplexContentExtension:
			if (complexContentExtension.AnyAttribute != null)
			{
				SendValidationEvent("'anyAttribute' must be the last child.", null);
			}
			complexContentExtension.Attributes.Add(value);
			break;
		case SchemaNames.Token.XsdComplexContentRestriction:
			if (complexContentRestriction.AnyAttribute != null)
			{
				SendValidationEvent("'anyAttribute' must be the last child.", null);
			}
			complexContentRestriction.Attributes.Add(value);
			break;
		case SchemaNames.Token.xsdAttributeGroup:
			if (attributeGroup.AnyAttribute != null)
			{
				SendValidationEvent("'anyAttribute' must be the last child.", null);
			}
			attributeGroup.Attributes.Add(value);
			break;
		}
	}

	private void AddParticle(XmlSchemaParticle particle)
	{
		switch (ParentElement)
		{
		case SchemaNames.Token.XsdComplexType:
			if (complexType.ContentModel != null || complexType.Attributes.Count != 0 || complexType.AnyAttribute != null || complexType.Particle != null)
			{
				SendValidationEvent("The content model of a complex type must consist of 'annotation' (if present); followed by zero or one of the following: 'simpleContent', 'complexContent', 'group', 'choice', 'sequence', or 'all'; followed by zero or more 'attribute' or 'attributeGroup'; followed by zero or one 'anyAttribute'.", "complexType");
			}
			complexType.Particle = particle;
			break;
		case SchemaNames.Token.XsdComplexContentExtension:
			if (complexContentExtension.Particle != null || complexContentExtension.Attributes.Count != 0 || complexContentExtension.AnyAttribute != null)
			{
				SendValidationEvent("Complex content restriction or extension should consist of zero or one of 'group', 'choice', 'sequence', or 'all'; followed by zero or more 'attribute' or 'attributeGroup'; followed by zero or one 'anyAttribute'.", "ComplexContentExtension");
			}
			complexContentExtension.Particle = particle;
			break;
		case SchemaNames.Token.XsdComplexContentRestriction:
			if (complexContentRestriction.Particle != null || complexContentRestriction.Attributes.Count != 0 || complexContentRestriction.AnyAttribute != null)
			{
				SendValidationEvent("Complex content restriction or extension should consist of zero or one of 'group', 'choice', 'sequence', or 'all'; followed by zero or more 'attribute' or 'attributeGroup'; followed by zero or one 'anyAttribute'.", "ComplexContentExtension");
			}
			complexContentRestriction.Particle = particle;
			break;
		case SchemaNames.Token.XsdGroup:
			if (group.Particle != null)
			{
				SendValidationEvent("The content model can only have one of the following; 'all', 'choice', or 'sequence'.", "particle");
			}
			group.Particle = (XmlSchemaGroupBase)particle;
			break;
		case SchemaNames.Token.XsdChoice:
		case SchemaNames.Token.XsdSequence:
			((XmlSchemaGroupBase)ParentContainer).Items.Add(particle);
			break;
		}
	}

	private bool GetNextState(XmlQualifiedName qname)
	{
		if (currentEntry.NextStates != null)
		{
			for (int i = 0; i < currentEntry.NextStates.Length; i++)
			{
				int num = (int)currentEntry.NextStates[i];
				if (schemaNames.TokenToQName[(int)SchemaEntries[num].Name].Equals(qname))
				{
					nextEntry = SchemaEntries[num];
					return true;
				}
			}
		}
		return false;
	}

	private bool IsSkipableElement(XmlQualifiedName qname)
	{
		if (CurrentElement != SchemaNames.Token.XsdDocumentation)
		{
			return CurrentElement == SchemaNames.Token.XsdAppInfo;
		}
		return true;
	}

	private void SetMinOccurs(XmlSchemaParticle particle, string value)
	{
		try
		{
			particle.MinOccursString = value;
		}
		catch (Exception)
		{
			SendValidationEvent("The value for the 'minOccurs' attribute must be xsd:nonNegativeInteger.", null);
		}
	}

	private void SetMaxOccurs(XmlSchemaParticle particle, string value)
	{
		try
		{
			particle.MaxOccursString = value;
		}
		catch (Exception)
		{
			SendValidationEvent("The value for the 'maxOccurs' attribute must be xsd:nonNegativeInteger or 'unbounded'.", null);
		}
	}

	private bool ParseBoolean(string value, string attributeName)
	{
		try
		{
			return XmlConvert.ToBoolean(value);
		}
		catch (Exception)
		{
			SendValidationEvent("'{1}' is an invalid value for the '{0}' attribute.", attributeName, value, null);
			return false;
		}
	}

	private int ParseEnum(string value, string attributeName, string[] values)
	{
		string text = value.Trim();
		for (int i = 0; i < values.Length; i++)
		{
			if (values[i] == text)
			{
				return i + 1;
			}
		}
		SendValidationEvent("'{1}' is an invalid value for the '{0}' attribute.", attributeName, text, null);
		return 0;
	}

	private XmlQualifiedName ParseQName(string value, string attributeName)
	{
		try
		{
			value = XmlComplianceUtil.NonCDataNormalize(value);
			string prefix;
			return XmlQualifiedName.Parse(value, namespaceManager, out prefix);
		}
		catch (Exception)
		{
			SendValidationEvent("'{1}' is an invalid value for the '{0}' attribute.", attributeName, value, null);
			return XmlQualifiedName.Empty;
		}
	}

	private int ParseBlockFinalEnum(string value, string attributeName)
	{
		int num = 0;
		string[] array = XmlConvert.SplitString(value);
		for (int i = 0; i < array.Length; i++)
		{
			bool flag = false;
			for (int j = 0; j < DerivationMethodStrings.Length; j++)
			{
				if (array[i] == DerivationMethodStrings[j])
				{
					if ((num & DerivationMethodValues[j]) != 0 && (num & DerivationMethodValues[j]) != DerivationMethodValues[j])
					{
						SendValidationEvent("'{1}' is an invalid value for the '{0}' attribute.", attributeName, value, null);
						return 0;
					}
					num |= DerivationMethodValues[j];
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				SendValidationEvent("'{1}' is an invalid value for the '{0}' attribute.", attributeName, value, null);
				return 0;
			}
			if (num == 255 && value.Length > 4)
			{
				SendValidationEvent("'{1}' is an invalid value for the '{0}' attribute.", attributeName, value, null);
				return 0;
			}
		}
		return num;
	}

	private static string ParseUriReference(string s)
	{
		return s;
	}

	private void SendValidationEvent(string code, string arg0, string arg1, string arg2)
	{
		SendValidationEvent(new XmlSchemaException(code, new string[3] { arg0, arg1, arg2 }, reader.BaseURI, positionInfo.LineNumber, positionInfo.LinePosition));
	}

	private void SendValidationEvent(string code, string msg)
	{
		SendValidationEvent(new XmlSchemaException(code, msg, reader.BaseURI, positionInfo.LineNumber, positionInfo.LinePosition));
	}

	private void SendValidationEvent(string code, string[] args, XmlSeverityType severity)
	{
		SendValidationEvent(new XmlSchemaException(code, args, reader.BaseURI, positionInfo.LineNumber, positionInfo.LinePosition), severity);
	}

	private void SendValidationEvent(XmlSchemaException e, XmlSeverityType severity)
	{
		schema.ErrorCount++;
		e.SetSchemaObject(schema);
		if (validationEventHandler != null)
		{
			validationEventHandler(null, new ValidationEventArgs(e, severity));
		}
		else if (severity == XmlSeverityType.Error)
		{
			throw e;
		}
	}

	private void SendValidationEvent(XmlSchemaException e)
	{
		SendValidationEvent(e, XmlSeverityType.Error);
	}

	private void RecordPosition()
	{
		xso.SourceUri = reader.BaseURI;
		xso.LineNumber = positionInfo.LineNumber;
		xso.LinePosition = positionInfo.LinePosition;
		if (xso != schema)
		{
			xso.Parent = ParentContainer;
		}
	}
}
