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
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Npgsql;
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace Firstapp2257.Utility
{
	public static class ExceptionMessageProcessor
	{
		// % protected region % [Override GetProcessedErrors method here] off begin
		/// <summary>
		/// Gets a flat list of errors from the provided exception. This will traverse inner exceptions in some cases
		/// to get more detailed errors from there as well.
		/// </summary>
		/// <param name="exception">The exception to process.</param>
		/// <returns>A list of all errors that are contained inside the exception.</returns>
		public static IEnumerable<string> GetProcessedErrors(Exception exception)
		{
			switch (exception)
			{
				case AggregateException aggregateException:
					foreach (var message in aggregateException.InnerExceptions.SelectMany(GetProcessedErrors))
					{
						yield return message;
					}
					break;
				case DbUpdateException updateException:
					if (updateException.InnerException is PostgresException pgException)
					{
						yield return pgException.MessageText;
					}
					else if (updateException.InnerException != null)
					{
						yield return updateException.InnerException.Message;
					}
					else
					{
						yield return updateException.Message;
					}
					break;
				default:
					yield return exception.Message;
					break;
			}
		}
		// % protected region % [Override GetProcessedErrors method here] end
	}
}