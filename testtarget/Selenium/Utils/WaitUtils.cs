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
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumTests.Enums;
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace SeleniumTests.Utils
{
	internal static class WaitUtils
	{
		public static void waitForPage(IWait<IWebDriver> wait)
		{
			// % protected region % [Override waitForPage here] off begin
			wait.Until(d => ((IJavaScriptExecutor) d).ExecuteScript("return document.readyState").Equals("complete"));
			// % protected region % [Override waitForPage here] end
		}

		public static void waitForPage(IWait<IWebDriver> wait, string url)
		{
			// % protected region % [Override waitForPage overload here] off begin
			wait.Until(d => d.Url == url);
			waitForPage(wait);
			// % protected region % [Override waitForPage overload here] end
		}

		/// <summary>
		/// A wait util method to use when waiting for an element to be in a state
		/// </summary>
		/// <param name="wait">The preconfigured driver wait to use</param>
		/// <param name="elementPath">The OpenQA.Selenium.By used to locate the element</param>
		/// <param name="status">visible, not visible, exists, not exist</param>
		/// <returns> a boolean indicating if the expected status is true or false</returns>
		public static bool elementState(IWait<IWebDriver> wait, By elementPath, ElementState status) =>
			waitForElementRunner(wait, null, elementPath, status);

		/// <summary>
		/// A wait util method to use when waiting for an element to be in a state
		/// </summary>
		/// <param name="wait">The preconfigured driver wait to use</param>
		/// <param name="parentElement">the parent of the element to wait for</param>
		/// <param name="elementPath">The OpenQA.Selenium.By used to locate the element</param>
		/// <param name="status">visible, not visible, exist, not exist</param>
		/// <returns> a boolean indicating if the expected status is true or false</returns>
		public static bool elementState(IWait<IWebDriver> wait, IWebElement parentElement, By elementPath, ElementState status) =>
			waitForElementRunner(wait, parentElement, elementPath, status);

		private static bool waitForElementRunner(IWait<IWebDriver> wait, IWebElement parentElement, By elementPath,
			ElementState status)
		{
			// % protected region % [Override waitForElementRunner here] off begin
			try
			{
				waitForElement(wait, parentElement, elementPath, status);
				return true;
			}
			catch (NoSuchElementException)
			{
				return false;
			}
			// % protected region % [Override waitForElementRunner here] end
		}

		private static void waitForElement(IWait<IWebDriver> wait, IWebElement parentElement, By elementPath, ElementState status)
		{
			switch (status)
			{
				// % protected region % [Override VISIBLE case here] off begin
				case ElementState.VISIBLE:
					wait.Until(driver =>
					{
						try
						{
							return parentElement != null
								? parentElement.FindElement(elementPath).Displayed
								: driver.FindElement(elementPath).Displayed;
						}
						catch (NoSuchElementException)
						{
							return false;
						}
					});
					break;
				// % protected region % [Override VISIBLE case here] end
				// % protected region % [Override NOT_VISIBLE case here] off begin
				case ElementState.NOT_VISIBLE:
					wait.Until(driver =>
					{
						try
						{
							return parentElement != null
								? !parentElement.FindElement(elementPath).Displayed
								: !driver.FindElement(elementPath).Displayed;
						}
						catch (NoSuchElementException)
						{
							return true;
						}
					});
					break;
				// % protected region % [Override NOT_VISIBLE case here] end
				// % protected region % [Override EXISTS case here] off begin
				case ElementState.EXISTS:
					wait.Until(driver =>
					{
						try
						{
							var element = parentElement != null
								? parentElement.FindElement(elementPath)
								: driver.FindElement(elementPath);

							return true;
						}
						catch (NoSuchElementException)
						{
							return false;
						}
					});
					break;
				// % protected region % [Override EXISTS case here] end
				// % protected region % [Override NOT_EXIST case here] off begin
				case ElementState.NOT_EXIST:
					wait.Until(driver =>
					{
						var originalImplicitWait = driver.Manage().Timeouts().ImplicitWait;
						driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
						try
						{
							var element = parentElement != null
								? parentElement.FindElement(elementPath)
								: driver.FindElement(elementPath);

							return false;
						}
						catch (NoSuchElementException)
						{
							return true;
						}
						finally
						{
							driver.Manage().Timeouts().ImplicitWait = originalImplicitWait;
						}
					});
					break;
				// % protected region % [Override NOT_EXIST case here] end
				// % protected region % [Override default case here] off begin
				default:
					throw new Exception($" '{status}' is not a valid status to wait for element");
				// % protected region % [Override default case here] end
			}
		}

		// % protected region % [Add more WaitUtils related functions here] off begin
		// % protected region % [Add more WaitUtils related functions here] end
	}
}