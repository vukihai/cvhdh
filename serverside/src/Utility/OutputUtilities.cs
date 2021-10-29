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

namespace Firstapp2257.Utility
{
	public static class OutputUtilities
	{
		/// <summary>
		/// Finds all date time fields on an object and sets all the the kind to all of them to
		/// <see cref="DateTimeKind.Unspecified"/>. This will clear the timezone information for the DateTime.
		/// </summary>
		/// <param name="obj">The object to unset the the DateTime kinds for.</param>
		/// <typeparam name="T"></typeparam>
		public static void UnsetDateTimeKinds<T>(T obj)
		{
			foreach (var property in ReflectionCache.GetDateTimeAttributes(obj.GetType()))
			{
				var dateTimeProperty = property.GetValue(obj);
				if (dateTimeProperty is DateTime dateTime)
				{
					property.SetValue(obj, DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified));
				}
			}
		}
	}
}