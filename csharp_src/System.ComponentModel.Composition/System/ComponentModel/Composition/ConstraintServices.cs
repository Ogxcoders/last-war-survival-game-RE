using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition;

internal static class ConstraintServices
{
	private static readonly PropertyInfo _exportDefinitionContractNameProperty = typeof(ExportDefinition).GetProperty("ContractName");

	private static readonly PropertyInfo _exportDefinitionMetadataProperty = typeof(ExportDefinition).GetProperty("Metadata");

	private static readonly MethodInfo _metadataContainsKeyMethod = typeof(IDictionary<string, object>).GetMethod("ContainsKey");

	private static readonly MethodInfo _metadataItemMethod = typeof(IDictionary<string, object>).GetMethod("get_Item");

	private static readonly MethodInfo _metadataEqualsMethod = typeof(object).GetMethod("Equals", new Type[1] { typeof(object) });

	private static readonly MethodInfo _typeIsInstanceOfTypeMethod = typeof(Type).GetMethod("IsInstanceOfType");

	public static Expression<Func<ExportDefinition, bool>> CreateConstraint(string contractName, string requiredTypeIdentity, IEnumerable<KeyValuePair<string, Type>> requiredMetadata, CreationPolicy requiredCreationPolicy)
	{
		ParameterExpression parameterExpression = Expression.Parameter(typeof(ExportDefinition), "exportDefinition");
		Expression expression = CreateContractConstraintBody(contractName, parameterExpression);
		if (!string.IsNullOrEmpty(requiredTypeIdentity))
		{
			Expression right = CreateTypeIdentityContraint(requiredTypeIdentity, parameterExpression);
			expression = Expression.AndAlso(expression, right);
		}
		if (requiredMetadata != null)
		{
			Expression expression2 = CreateMetadataConstraintBody(requiredMetadata, parameterExpression);
			if (expression2 != null)
			{
				expression = Expression.AndAlso(expression, expression2);
			}
		}
		if (requiredCreationPolicy != CreationPolicy.Any)
		{
			Expression right2 = CreateCreationPolicyContraint(requiredCreationPolicy, parameterExpression);
			expression = Expression.AndAlso(expression, right2);
		}
		return Expression.Lambda<Func<ExportDefinition, bool>>(expression, new ParameterExpression[1] { parameterExpression });
	}

	private static Expression CreateContractConstraintBody(string contractName, ParameterExpression parameter)
	{
		Assumes.NotNull(parameter);
		return Expression.Equal(Expression.Property(parameter, _exportDefinitionContractNameProperty), Expression.Constant(contractName ?? string.Empty, typeof(string)));
	}

	private static Expression CreateMetadataConstraintBody(IEnumerable<KeyValuePair<string, Type>> requiredMetadata, ParameterExpression parameter)
	{
		Assumes.NotNull(requiredMetadata);
		Assumes.NotNull(parameter);
		Expression expression = null;
		foreach (KeyValuePair<string, Type> requiredMetadatum in requiredMetadata)
		{
			Expression expression2 = CreateMetadataContainsKeyExpression(parameter, requiredMetadatum.Key);
			expression = ((expression != null) ? Expression.AndAlso(expression, expression2) : expression2);
			expression = Expression.AndAlso(expression, CreateMetadataOfTypeExpression(parameter, requiredMetadatum.Key, requiredMetadatum.Value));
		}
		return expression;
	}

	private static Expression CreateCreationPolicyContraint(CreationPolicy policy, ParameterExpression parameter)
	{
		Assumes.IsTrue(policy != CreationPolicy.Any);
		Assumes.NotNull(parameter);
		return Expression.MakeBinary(ExpressionType.OrElse, Expression.MakeBinary(ExpressionType.OrElse, Expression.Not(CreateMetadataContainsKeyExpression(parameter, "System.ComponentModel.Composition.CreationPolicy")), CreateMetadataValueEqualsExpression(parameter, CreationPolicy.Any, "System.ComponentModel.Composition.CreationPolicy")), CreateMetadataValueEqualsExpression(parameter, policy, "System.ComponentModel.Composition.CreationPolicy"));
	}

	private static Expression CreateTypeIdentityContraint(string requiredTypeIdentity, ParameterExpression parameter)
	{
		Assumes.NotNull(requiredTypeIdentity);
		Assumes.NotNull(parameter);
		return Expression.MakeBinary(ExpressionType.AndAlso, CreateMetadataContainsKeyExpression(parameter, "ExportTypeIdentity"), CreateMetadataValueEqualsExpression(parameter, requiredTypeIdentity, "ExportTypeIdentity"));
	}

	private static Expression CreateMetadataContainsKeyExpression(ParameterExpression parameter, string constantKey)
	{
		Assumes.NotNull(parameter, constantKey);
		return Expression.Call(Expression.Property(parameter, _exportDefinitionMetadataProperty), _metadataContainsKeyMethod, Expression.Constant(constantKey));
	}

	private static Expression CreateMetadataOfTypeExpression(ParameterExpression parameter, string constantKey, Type constantType)
	{
		Assumes.NotNull(parameter, constantKey);
		Assumes.NotNull(parameter, constantType);
		return Expression.Call(Expression.Constant(constantType, typeof(Type)), _typeIsInstanceOfTypeMethod, Expression.Call(Expression.Property(parameter, _exportDefinitionMetadataProperty), _metadataItemMethod, Expression.Constant(constantKey)));
	}

	private static Expression CreateMetadataValueEqualsExpression(ParameterExpression parameter, object constantValue, string metadataName)
	{
		Assumes.NotNull(parameter, constantValue);
		return Expression.Call(Expression.Constant(constantValue), _metadataEqualsMethod, Expression.Call(Expression.Property(parameter, _exportDefinitionMetadataProperty), _metadataItemMethod, Expression.Constant(metadataName)));
	}

	public static Expression<Func<ExportDefinition, bool>> CreatePartCreatorConstraint(Expression<Func<ExportDefinition, bool>> baseConstraint, ImportDefinition productImportDefinition)
	{
		ParameterExpression parameterExpression = baseConstraint.Parameters[0];
		Expression instance = Expression.Property(parameterExpression, _exportDefinitionMetadataProperty);
		Expression left = Expression.Call(instance, _metadataContainsKeyMethod, Expression.Constant("ProductDefinition"));
		Expression expression = Expression.Call(instance, _metadataItemMethod, Expression.Constant("ProductDefinition"));
		Expression right = Expression.Invoke(productImportDefinition.Constraint, Expression.Convert(expression, typeof(ExportDefinition)));
		return Expression.Lambda<Func<ExportDefinition, bool>>(Expression.AndAlso(baseConstraint.Body, Expression.AndAlso(left, right)), new ParameterExpression[1] { parameterExpression });
	}
}
