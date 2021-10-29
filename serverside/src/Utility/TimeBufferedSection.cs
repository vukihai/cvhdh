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
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Utility
{
	/// <summary>
	/// Represents a function that must run for at least for a certain amount of time. This can be user to help aid
	/// against preventing timing attacks.
	/// </summary>
	public class TimeBufferedSection : IDisposable, IAsyncDisposable
	{
		private readonly long _duration;
		private readonly Stopwatch _stopwatch;
		// % protected region % [Add any extra class fields here] off begin
		// % protected region % [Add any extra class fields here] end

		/// <summary>
		/// Creates a new time buffered section. When the dispose method is called if the specified duration provided
		/// was not elapsed, then the dispose method will make up the difference. It is recommended to async disposal
		/// if this class is being made in an async context.
		/// </summary>
		/// <param name="durationMilliseconds">The duration in milliseconds that need to be elapsed.</param>
		/// <example>
		/// <code>
		/// public async Task DoSomething()
		/// {
		/// 	await using var timeBuffer = new TimeBufferedSection(100);
		/// 	Console.WriteLine("Hello world");
		/// }
		///
		/// await DoSomething(); // This call will take 100 ms to complete
		/// </code>
		/// </example>
		public TimeBufferedSection(long durationMilliseconds)
		{
			_duration = durationMilliseconds * TimeSpan.TicksPerMillisecond;
			_stopwatch = Stopwatch.StartNew();
			// % protected region % [Add any extra constructor logic here] off begin
			// % protected region % [Add any extra constructor logic here] end
		}

		public void Dispose()
		{
			// % protected region % [Customise Dispose method here] off begin
			_stopwatch.Stop();
			var difference = new TimeSpan(_duration) - new TimeSpan(_stopwatch.ElapsedTicks);
			if (difference.Ticks > 0)
			{
				Thread.Sleep(new TimeSpan(_duration - _stopwatch.ElapsedTicks));
			}
			// % protected region % [Customise Dispose method here] end
		}

		public async ValueTask DisposeAsync()
		{
			// % protected region % [Customise DisposeAsync method here] off begin
			_stopwatch.Stop();
			var difference = new TimeSpan(_duration) - new TimeSpan(_stopwatch.ElapsedTicks);
			if (difference.Ticks > 0)
			{
				await Task.Delay(difference);
			}
			// % protected region % [Customise DisposeAsync method here] end
		}

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}