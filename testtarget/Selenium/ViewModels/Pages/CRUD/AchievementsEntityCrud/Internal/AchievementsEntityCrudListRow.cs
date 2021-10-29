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
using OpenQA.Selenium.Support.PageObjects;
using SeleniumTests.Setup;
using SeleniumTests.Utils;
using SeleniumTests.ViewModels.Components;
using SeleniumTests.ViewModels.Components.Common;
using SeleniumTests.ViewModels.Pages.Crud.Internal;
// % protected region % [Custom imports] off begin
// % protected region % [Custom imports] end

namespace SeleniumTests.ViewModels.Pages.CRUD.AchievementsEntityCrud.Internal
{
	public class AchievementsEntityCrudListRow : Component, ICrudListRow
	{
		// % protected region % [Override class properties here] off begin
		public Guid? Id => ContextConfiguration.WebDriver.FindElementExt(Selector).GetAttribute("data-id").ToNullableGuid();
		public Checkbox Selected => new(new ByChained(Selector, By.CssSelector("input")), ContextConfiguration);
		public Button ViewButton => new(new ByChained(Selector, By.CssSelector(".icon-look")), ContextConfiguration);
		public Button EditButton => new(new ByChained(Selector, By.CssSelector(".icon-edit")), ContextConfiguration);
		public Button DeleteButton => new(new ByChained(Selector, By.CssSelector(".bin-full")), ContextConfiguration);
		// % protected region % [Override class properties here] end

		// % protected region % [Override constructor here] off begin
		public AchievementsEntityCrudListRow(By selector, ContextConfiguration contextConfiguration) : base(selector, contextConfiguration)
		{
		}
		// % protected region % [Override constructor here] end

		// % protected region % [Add class methods here] off begin
		// % protected region % [Add class methods here] end
	}
}