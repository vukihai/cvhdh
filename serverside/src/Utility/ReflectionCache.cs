/*
 * @bot-written
 *
 * WARNING AND NOTICE
 * Any access, download, storage, and/or use of this source code is subject to the terms and conditions of the
 * Full Software Licence as accepted by you before being granted access to this source code and other materials,
 * the terms of which can be accessed on the Codebots website at https://codebots.com/full-software-licence. Any
 * commercial use in contravention of the terms of the Full Software Licence may be pursued by Codebots through
 * licence termination and further legal action, and be required to indemnify Codebots for any loss or damage,
 * including interest and costs. You are deemed to have accepted the terms of the Full Software Licence on any
 * access, download, storage, and/or use of this source code.
 *
 * BOT WARNING
 * This file is bot-written.
 * Any changes out side of "protected regions" will be lost next time the bot makes any changes.
 */
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Firstapp2257.Models;
using Firstapp2257.Models.Internal;
using Firstapp2257.Services;
using GraphQL.EntityFramework;
using Microsoft.EntityFrameworkCore;
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace Firstapp2257.Utility
{
	/// <summary>
	/// Helper methods for getting fields reflectively.
	/// </summary>
	public static class ReflectionCache
	{
		private static ConcurrentDictionary<Type, List<PropertyInfo>> FileAttributeCache { get; } = new();
		private static ConcurrentDictionary<Type, List<PropertyInfo>> DateTimeAttributeCache { get; } = new();
		private static ConcurrentDictionary<Type, List<PropertyInfo>> ReferenceCache { get; } = new();

		/// <summary>
		/// Method info for the Postgres ILike method.
		/// </summary>
		// ReSharper disable once InconsistentNaming
		public static readonly MethodInfo ILikeMethod = typeof(NpgsqlDbFunctionsExtensions).GetMethod(
			"ILike",
			new [] { typeof(DbFunctions), typeof(string), typeof(string) });

		/// <summary>
		/// Method info for the Enumerable Any method
		/// </summary>
		public static readonly MethodInfo EnumerableAnyMethod = typeof(Enumerable)
			.GetMethods(BindingFlags.Static | BindingFlags.Public)
			.Where(m => m.Name == "Any")
			.Select(m => new {m, p = m.GetParameters()})
			.Where(t => t.p.Length == 2 && t.p[0].ParameterType.IsGenericType &&
				t.p[0].ParameterType.GetGenericTypeDefinition() == typeof(IEnumerable<>) &&
				t.p[1].ParameterType.IsGenericType &&
				t.p[1].ParameterType.GetGenericTypeDefinition() == typeof(Func<,>))
			.Select(t => t.m)
			.Single();

		/// <summary>
		/// Method info for the Queryable Where method.
		/// </summary>
		public static readonly MethodInfo WhereMethod = typeof(Queryable).GetMethods()
				.Where(x => x.Name == "Where")
				.Select(x => new { M = x, P = x.GetParameters() })
				.Where(x => x.P.Length == 2
					&& x.P[0].ParameterType.IsGenericType
					&& x.P[0].ParameterType.GetGenericTypeDefinition() == typeof(IQueryable<>)
					&& x.P[1].ParameterType.IsGenericType
					&& x.P[1].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>))
				.Select(x => new { x.M, A = x.P[1].ParameterType.GetGenericArguments() })
				.Where(x => x.A[0].IsGenericType
					&& x.A[0].GetGenericTypeDefinition() == typeof(Func<,>))
				.Select(x => new { x.M, A = x.A[0].GetGenericArguments() })
				.Where(x => x.A[0].IsGenericParameter
					&& x.A[1] == typeof(bool))
				.Select(x => x.M)
				.SingleOrDefault();

		/// <summary>
		/// Method info for the Queryable Take method.
		/// </summary>
		public static readonly MethodInfo TakeMethod = typeof(Queryable)
			.GetMethods(BindingFlags.Static | BindingFlags.Public)
			.Where(m => m.Name == "Take")
			.Select(m => new {m, p = m.GetParameters()})
			.Where(t => t.p.Length == 2 && t.p[0].ParameterType.IsGenericType &&
				t.p[0].ParameterType.GetGenericTypeDefinition() == typeof(IQueryable<>) &&
				t.p[1].ParameterType == typeof(int))
			.Select(t => t.m)
			.Single();

		/// <summary>
		/// Method info for the Queryable Skip method.
		/// </summary>
		public static readonly MethodInfo SkipMethod = typeof(Queryable).GetMethods(BindingFlags.Static | BindingFlags.Public)
			.Where(m => m.Name == "Skip")
			.Select(m => new {m, p = m.GetParameters()})
			.Where(t => t.p.Length == 2 && t.p[0].ParameterType.IsGenericType &&
				t.p[0].ParameterType.GetGenericTypeDefinition() == typeof(IQueryable<>) &&
				t.p[1].ParameterType == typeof(int))
			.Select(t => t.m)
			.Single();

		/// <summary>
		/// Method info for the Enumerable Contains method.
		/// </summary>
		public static readonly MethodInfo ContainsMethod = typeof(Enumerable)
			.GetMethods(BindingFlags.Static | BindingFlags.Public)
			.First(m => m.Name == "Contains" && m.GetParameters().Length == 2);

		/// <summary>
		/// Method info for the Queryable OrderBy method.
		/// </summary>
		public static readonly MethodInfo OrderByMethod = typeof(Queryable)
			.GetMethods(BindingFlags.Static | BindingFlags.Public)
			.Single(m => m.Name == "OrderBy" && m.GetParameters().Length == 2);

		/// <summary>
		/// Method info for the Queryable OrderByDescending method.
		/// </summary>
		public static readonly MethodInfo OrderByDescendingMethod = typeof(Queryable)
			.GetMethods(BindingFlags.Static | BindingFlags.Public)
			.Single(m => m.Name == "OrderByDescending" && m.GetParameters().Length == 2);

		/// <summary>
		/// Method info for the Queryable ThenBy method.
		/// </summary>
		public static readonly MethodInfo ThenByMethod = typeof(Queryable)
			.GetMethods(BindingFlags.Static | BindingFlags.Public)
			.Single(m => m.Name == "ThenBy" && m.GetParameters().Length == 2);

		/// <summary>
		/// Method info for the Queryable ThenByDescending method.
		/// </summary>
		public static readonly MethodInfo ThenByDescendingMethod = typeof(Queryable)
			.GetMethods(BindingFlags.Static | BindingFlags.Public)
			.Single(m => m.Name == "ThenByDescending" && m.GetParameters().Length == 2);

		/// <summary>
		/// Method info for the SecurityService CreateReadSecurityFilter method.
		/// </summary>
		public static readonly MethodInfo CreateReadSecurityFilterMethod = typeof(SecurityService).GetMethod(
			"CreateReadSecurityFilter",
			1,
			BindingFlags.Static | BindingFlags.Public,
			null,
			new [] { typeof(IServiceProvider) },
			null);

		/// <summary>
		/// Method info for the ExpressionUtilities BuildConditionalWhereMethod method.
		/// </summary>
		public static readonly MethodInfo BuildConditionalWhereMethod = typeof(ExpressionUtilities).GetMethod(
			"BuildConditionalWhere",
			1,
			BindingFlags.Static | BindingFlags.Public,
			null,
			new [] { typeof(IEnumerable<IEnumerable<WhereExpression>>) },
			null);

		/// <summary>
		/// Method info for the ExpressionUtilities BuildWhereExpression method.
		/// </summary>
		public static readonly MethodInfo BuildWhereExpressionMethod = typeof(ExpressionUtilities).GetMethod(
			"BuildWhereExpression",
			1,
			BindingFlags.Static | BindingFlags.Public,
			null,
			new [] { typeof(WhereExpression), typeof(ParameterExpression) },
			null);

		/// <summary>
		/// Method info for the ExpressionUtilities BuildConditionalHas method.
		/// </summary>
		public static readonly MethodInfo BuildConditionalHasMethod = typeof(ExpressionUtilities).GetMethod(
			"BuildConditionalHas",
			1,
			BindingFlags.Static | BindingFlags.Public,
			null,
			new [] { typeof(IEnumerable<IEnumerable<HasCondition>>), typeof(IServiceProvider) },
			null);

		// % protected region % [Add any further properties here] off begin
		// % protected region % [Add any further properties here] end

		/// <summary>
		/// Gets all the file attributes for this type. The values for this are cached in a static map for fast
		/// repeated lookups.
		/// </summary>
		/// <param name="entityType">The type to get the file attributes from</param>
		/// <returns>A list of property info representing the file attributes</returns>
		public static List<PropertyInfo> GetFileAttributes(Type entityType)
		{
			if (FileAttributeCache.TryGetValue(entityType, out var properties))
			{
				return properties;
			}

			var attributeInfos = entityType.GetProperties()
				.Where(p => p.GetCustomAttributes<FileReference>().Any())
				.ToList();
			FileAttributeCache.TryAdd(entityType, attributeInfos);

			return attributeInfos;
		}

		/// <summary>
		/// Gets all DateTime and DateTime? attributes for this type.
		/// </summary>
		/// <param name="entityType">The type of class to get the DateTimes for.</param>
		/// <returns>A list of property info for each datetime attribute.</returns>
		public static List<PropertyInfo> GetDateTimeAttributes(Type entityType)
		{
			if (DateTimeAttributeCache.TryGetValue(entityType, out var properties))
			{
				return properties;
			}

			var attributeInfos = entityType.GetProperties()
				.Where(p => p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?))
				.ToList();
			DateTimeAttributeCache.TryAdd(entityType, attributeInfos);

			return attributeInfos;
		}

		/// <summary>
		/// Gets all collection references on a type.
		/// </summary>
		/// <param name="entityType">The type of the entity to get the references from</param>
		/// <returns>A list of info on the types</returns>
		public static IEnumerable<PropertyInfo> GetCollectionReferences(Type entityType)
		{
			if (ReferenceCache.TryGetValue(entityType, out var types))
			{
				return types;
			}

			var referenceTypes = entityType.GetProperties()
				.Where(p => p.GetCustomAttributes<EntityForeignKey>().Any()
					&& typeof(IEnumerable).IsAssignableFrom(p.PropertyType))
				.ToList();

			ReferenceCache.TryAdd(entityType, referenceTypes);

			return referenceTypes;
		}

		/// <summary>
		/// Creates an expression to get a member field out of a nested object.
		/// </summary>
		/// <param name="obj">The object to get the member from.</param>
		/// <param name="path">An array of strings to represent the property to get.</param>
		/// <returns>A member expression that represents getting a property from the obj at the path.</returns>
		public static MemberExpression GetPropertyRecursive([NotNull]Expression obj, params string[] path)
		{
			MemberExpression result = null;
			foreach (var segment in path)
			{
				result = Expression.PropertyOrField(result ?? obj, segment);
			}
			return result;
		}

		// % protected region % [Add any further methods here] off begin
		// % protected region % [Add any further methods here] end
	}
}
