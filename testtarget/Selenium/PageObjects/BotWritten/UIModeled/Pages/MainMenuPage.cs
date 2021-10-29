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

using OpenQA.Selenium;
using SeleniumTests.Enums;
using SeleniumTests.Utils;
using SeleniumTests.Setup;
// % protected region % [Add further imports here] off begin
// % protected region % [Add further imports here] end

namespace SeleniumTests.PageObjects.BotWritten.UIModeled.Pages
{
	public class MainMenuPage : UIModeledPage
	{
		// % protected region % [Override class properties here] off begin
		public override string Url => baseUrl + "/mainmenu";
		private const string RootSelector = "//div[@class='body-content']";
		private string MainMenuPageVerticalStackLayout1Selector => RootSelector + "//*[@class='layout__vertical']";
		private By MainMenuPageVerticalStackLayout1By => By.XPath(MainMenuPageVerticalStackLayout1Selector);
		private string MainMenuPageVerticalStackLayout1HorizontalLayoutSelector => MainMenuPageVerticalStackLayout1Selector + "//*[@class='layout__horizontal']";
		private By MainMenuPageVerticalStackLayout1HorizontalLayoutBy => By.XPath(MainMenuPageVerticalStackLayout1HorizontalLayoutSelector);
		private By DropdownComponentBy => By.XPath($"{MainMenuPageVerticalStackLayout1HorizontalLayoutSelector}//div[contains(@class, 'input-group__dropdown')]/label[text()='My profile']");
		private By Link1By => By.XPath($"{MainMenuPageVerticalStackLayout1HorizontalLayoutSelector}//a[@href=''][text()='sửa thông tin']");
		private By Link2By => By.XPath($"{MainMenuPageVerticalStackLayout1HorizontalLayoutSelector}//a[@href=''][text()='assignment']");
		private By Link3By => By.XPath($"{MainMenuPageVerticalStackLayout1HorizontalLayoutSelector}//a[@href=''][text()='thông tin học viên']");
		private By ParagraphBy => By.XPath($"{MainMenuPageVerticalStackLayout1HorizontalLayoutSelector}//p[text()='Trang Chủ']");
		private string MainMenuPageVerticalStackLayout2Selector => RootSelector + "//*[@class='layout__vertical']";
		private By MainMenuPageVerticalStackLayout2By => By.XPath(MainMenuPageVerticalStackLayout2Selector);
		private string MainMenuPageVerticalStackLayout2VerticalStackLayoutSelector => MainMenuPageVerticalStackLayout2Selector + "//*[@class='layout__vertical']";
		private By MainMenuPageVerticalStackLayout2VerticalStackLayoutBy => By.XPath(MainMenuPageVerticalStackLayout2VerticalStackLayoutSelector);
		private By ParagraphBy => By.XPath($"{MainMenuPageVerticalStackLayout2VerticalStackLayoutSelector}//p[text()='List assignment']");
		private By ListBy => By.XPath($"{MainMenuPageVerticalStackLayout2VerticalStackLayoutSelector}//ol");
		// % protected region % [Override class properties here] end
		// % protected region % [Override constructor here] off begin
		public MainMenuPage(ContextConfiguration currentContext) : base(currentContext)
		{
		}
		// % protected region % [Override constructor here] end
		// % protected region % [Override ContainsModeledElements here] off begin
		public override bool ContainsModeledElements()
		{
			var validContents = true;
			validContents &= WaitUtils.elementState(DriverWait, MainMenuPageVerticalStackLayout1By, ElementState.VISIBLE);
			validContents &= WaitUtils.elementState(DriverWait, MainMenuPageVerticalStackLayout1HorizontalLayoutBy, ElementState.VISIBLE);
			validContents &= WaitUtils.elementState(DriverWait, DropdownComponentBy, ElementState.VISIBLE);
			validContents &= WaitUtils.elementState(DriverWait, Link1By, ElementState.VISIBLE);
			validContents &= WaitUtils.elementState(DriverWait, Link2By, ElementState.VISIBLE);
			validContents &= WaitUtils.elementState(DriverWait, Link3By, ElementState.VISIBLE);
			validContents &= WaitUtils.elementState(DriverWait, ParagraphBy, ElementState.VISIBLE);
			validContents &= WaitUtils.elementState(DriverWait, MainMenuPageVerticalStackLayout2By, ElementState.VISIBLE);
			validContents &= WaitUtils.elementState(DriverWait, MainMenuPageVerticalStackLayout2VerticalStackLayoutBy, ElementState.VISIBLE);
			validContents &= WaitUtils.elementState(DriverWait, ParagraphBy, ElementState.VISIBLE);
			validContents &= WaitUtils.elementState(DriverWait, ListBy, ElementState.VISIBLE);
			return validContents;
		}
		// % protected region % [Override ContainsModeledElements here] end
	}
}