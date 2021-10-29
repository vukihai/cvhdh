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
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Firstapp2257.Services;
using Firstapp2257.Services.Interfaces;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace ServersideTests.Helpers.Services
{
	public class TestEmailService : IEmailService
	{
		// % protected region % [Customise SentEmails here] off begin
		public List<EmailEntity> SentEmails = new();
		// % protected region % [Customise SentEmails here] end

		// % protected region % [Customise SendEmail here] off begin
		public Task<bool> SendEmail(EmailEntity emailToSend, CancellationToken cancellationToken = default)
		{
			SentEmails.Add(emailToSend);
			return Task.FromResult(true);
		}
		// % protected region % [Customise SendEmail here] end

		// % protected region % [Add any additional methods here] off begin
		// % protected region % [Add any additional methods here] end
	}
}